using Application.DTOs.User;
using Application.Interfaces;
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
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }
    public async Task<string?> LoginAsync(LoginUserDTO loginUserDTO)
    {
        try
        {
            var user = await _userManager.Users.Include(u => u.User).FirstOrDefaultAsync(u => u.Email == loginUserDTO.Email);
            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDTO.Password, false);
            if (!result.Succeeded)
                throw new Exception("Senha inválida.");

            return await GenerateToken(user);
        }
        catch (Exception e)
        {
            throw;
        }
        
    }

    public async Task<string?> RegisterAsync(RegisterUserDTO registerUserDTO)
    {
        try
        {
            var userExists = await _userManager.FindByEmailAsync(registerUserDTO.Email);
            if (userExists != null)
                throw new Exception("Usuário já cadastrado.");

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

            return await GenerateToken(appUser);
        }
        catch (Exception e)
        {
            throw;
        }
        
    }

    private async Task<string?> GenerateToken(ApplicationUser user)
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
                expires: DateTime.UtcNow.AddHours(1),
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
}
