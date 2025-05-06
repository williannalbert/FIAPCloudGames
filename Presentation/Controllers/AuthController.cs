using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        try
        {
            string token = await _authService.RegisterAsync(registerUserDTO);

            return Ok(new { token });
        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
    {
        try
        {
            string token = await _authService.LoginAsync(loginUserDTO);
            if (token == null)
                return NotFound("Usuário e senha inválidos.");

            return Ok(new { token });

        }
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpPost("forgot-password")]   
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordUserDto forgotPasswordUserDto)
    {
        try
        {
            var token = await _authService.GenerateResetPasswordTokenAsync(forgotPasswordUserDto.Email);
            if (token == null)
                return NotFound("Usuário não encontrado.");

            return Ok(new { token });
        }
        catch (Exception e)
        {
            throw;
        }        
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordUserDto ResetPasswordUserDto)
    {
        try
        {
            var success = await _authService.ResetPasswordAsync(ResetPasswordUserDto.Email, ResetPasswordUserDto.Token, ResetPasswordUserDto.NewPassword);
            if (!success)
                throw new Exception("Falha ao redefinir a senha.");

            return Ok("Senha redefinida com sucesso.");

        }
        catch (Exception e)
        {
            throw;        
        }
    }
}
