using Application.BusinessRules.Interfaces;
using Application.DTOs.Library;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class AuthService(
    UserManager<ApplicationUser> _userManager,
    SignInManager<ApplicationUser> _signInManager,
    IConfiguration _configuration,
    AppDbContext _context,
    ILibraryService _libraryService,
    IWalletService _walletService,
    IUserRules _userRules,
    IMapper _mapper) : IAuthService
{
    public async Task<AuthResponseDTO?> LoginAsync(LoginUserDTO loginUserDTO)
    {
        try
        {
            var user = await _userManager.Users.Include(u => u.User).FirstOrDefaultAsync(u => u.Email == loginUserDTO.Email);
            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDTO.Password, false);
            if (!result.Succeeded)
                throw new BusinessException("Senha inválida.");

            string token = await GenerateToken(user);
            string refreshToken = await SetRefreshTokenToUser(user);
            return new AuthResponseDTO
            {
                Token = token, 
                RefreshToken = refreshToken
            };
        }
        catch (Exception e)
        {
            throw;
        }
        
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO registerUserDTO)
    {
        try
        {
            if (!_userRules.ValidatePassword(registerUserDTO.Password))
                throw new BusinessException("Senha não segue os parâmetros mínimos de validação");

            var userExists = await _userManager.FindByEmailAsync(registerUserDTO.Email);
            if (userExists != null)
                throw new BusinessException("Usuário já cadastrado.");

            var appUser = new ApplicationUser
            {
                UserName = registerUserDTO.Email,
                Email = registerUserDTO.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(appUser, registerUserDTO.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors));

            await _userManager.AddToRoleAsync(appUser, "User");

            var user = new User
            {
                ApplicationUserId = appUser.Id,
                Name = registerUserDTO.Name,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _ = await _libraryService.CreateAsync(new CreateLibraryDTO() { UserId = user.Id });

            _ = await _walletService.CreateAsync(user.Id);

            string token = await GenerateToken(appUser);
            string refreshToken = await SetRefreshTokenToUser(appUser);
            return new AuthResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }
        catch (Exception e)
        {
            throw;
        }
        
    }

    private async Task<string> SetRefreshTokenToUser(ApplicationUser applicationUser)
    {
        var refreshToken = GenerateRefreshToken();
        applicationUser.RefreshToken = refreshToken;
        applicationUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(2);

        await _userManager.UpdateAsync(applicationUser);
        return refreshToken;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateToken(ApplicationUser user)
    {
        try
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles.FirstOrDefault() ?? "User";

            var claims = new[]
            {
                new Claim("userId", user.User.Id.ToString()),
                new Claim("applicationUserId", user.Id),
                new Claim("name", user.User.Name),
                new Claim("email", user.Email),
                new Claim("role", role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {

            throw;
        }

    }

    public async Task<string?> GenerateResetPasswordTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return null;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(request.Token);
            var userId = principal.FindFirst("applicationUserId")?.Value;

            if (string.IsNullOrEmpty(userId)) 
                return null;

            var user = await _userManager.Users.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null ||
                user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }
            var token = await GenerateToken(user);
            var refreshToken = await SetRefreshTokenToUser(user);

            return new AuthResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }
        catch (Exception e)
        {

            throw;
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = false, 
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Token inválido");
        }

        return principal;
    }

    public async Task<UserDTO> ChangeEmailAsync(string applicationUserId, string newEmail)
    {
        try
        {
            var applicationUser = await _userManager.FindByIdAsync(applicationUserId);
            if (applicationUser is null)
                throw new NotFoundException("Usuário não encontrado.");

            var emailExists = await _userManager.FindByEmailAsync(newEmail);
            if (emailExists is not null && emailExists.Id != applicationUserId)
                throw new BusinessException("Já consta usuário cadastrado com esse e-mail.");

            var token = await _userManager.GenerateChangeEmailTokenAsync(applicationUser, newEmail);
            var resultEmail = await _userManager.ChangeEmailAsync(applicationUser, newEmail, token);
            if (!resultEmail.Succeeded)
                throw new BusinessException("Ocorreu um erro ao alterar e-mail do usuário.");
            var resultUserName = await _userManager.SetUserNameAsync(applicationUser, newEmail);
            if (!resultUserName.Succeeded)
                throw new BusinessException("Ocorreu um erro ao alterar username do usuário.");

            var user = await _context.Users
                .Include(u => u.Library)
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId);

            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
