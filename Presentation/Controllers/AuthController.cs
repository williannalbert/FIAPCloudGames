using Application.DTOs.Error;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService _authService, ITokenInformationsServices _tokenInformationsServices) : ControllerBase
{
    /// <summary>
    /// Registra usuário na plataforma
    /// </summary>
    /// <param name="registerUserDTO">Objeto com dados do usuário</param>
    /// <returns>Objeto com Token e RefreshToken do usuário</returns>
    /// <response code="200">Usuário criado com sucesso</response>
    /// <response code="400">Erro ao criar usuário</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
    {
        try
        {
            AuthResponseDTO authResponseDTO = await _authService.RegisterAsync(registerUserDTO);

            return Ok(authResponseDTO);
        }
        catch (Exception e)
        {
            throw;
        }
    }
    /// <summary>
    /// Realiza o acesso do usuário na plataforma
    /// </summary>
    /// <param name="loginUserDTO"></param>
    /// <returns>Objeto com Token e RefreshToken do usuário</returns>
    /// <response code="200">Login realizado com sucesso</response>
    /// <response code="404">Usuário e senha inválidos</response>
    /// <response code="400">Erro ao acessar a plataforma</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
    {
        try
        {
            AuthResponseDTO authResponseDTO = await _authService.LoginAsync(loginUserDTO);
            if (authResponseDTO == null)
                throw new NotFoundException("Usuário e senha inválidos.");

            return Ok(authResponseDTO);

        }
        catch (Exception e)
        {
            throw;
        }
    }
    /// <summary>
    /// Realiza a renovação do token de autorização na plataforma
    /// </summary>
    /// <param name="refreshTokenRequestDTO">Objeto com Token e RefreshToken do usuário</param>
    /// <returns>Objeto com novos Token e RefreshToken do usuário</returns>
    /// <response code="200">Renovação de token realizada com sucesso</response>
    /// <response code="404">Tokens inválidos ou expirados</response>
    /// <response code="400">Erro ao renovar tokens de acesso</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]

    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequestDTO)
    {
        try
        {
            AuthResponseDTO authResponseDTO = await _authService.RefreshTokenAsync(refreshTokenRequestDTO);
            if (authResponseDTO == null)
                throw new NotFoundException("Token expirado ou usuário não encontrado.");

            return Ok(authResponseDTO);

        }
        catch (Exception e)
        {
            throw;
        }
    }
    /// <summary>
    /// Gera token para alteração de senha esquecida
    /// </summary>
    /// <param name="forgotPasswordUserDto">Objeto com e-mail do usuário</param>
    /// <returns>Token para criação da nova senha</returns>
    /// <response code="200">Token gerado com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="400">Erro ao gerar Token</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]

    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordUserDto forgotPasswordUserDto)
    {
        try
        {
            var token = await _authService.GenerateResetPasswordTokenAsync(forgotPasswordUserDto.Email);
            if (token == null)
                throw new NotFoundException("Usuário não encontrado.");

            return Ok(new { token });
        }
        catch (Exception e)
        {
            throw;
        }        
    }
    /// <summary>
    /// Realiza o cadastro da nova senha do usuário
    /// </summary>
    /// <param name="ResetPasswordUserDto">Objeto com dados para cadastro de nova senha</param>
    /// <returns>Mensagem com status da alteração da senha</returns>
    /// <response code="200">Senha alterada com sucesso</response>
    /// <response code="400">Erro ao realizar alteração da senha</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]

    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordUserDto ResetPasswordUserDto)
    {
        try
        {
            var success = await _authService.ResetPasswordAsync(ResetPasswordUserDto.Email, ResetPasswordUserDto.Token, ResetPasswordUserDto.NewPassword);
            if (!success)
                throw new BusinessException("Falha ao redefinir a senha.");

            return Ok("Senha redefinida com sucesso.");

        }
        catch (Exception e)
        {
            throw;        
        }
    }
    /// <summary>
    /// Realiza a troca do E-mail/Username cadastrado
    /// </summary>
    /// <param name="changeEmailUserDTO">Objeto com novo e-mail</param>
    /// <returns>Usuário atualizado</returns>
    /// <response code="200">E-mail e Username alterados com sucesso</response>
    /// <response code="400">Erro ao alterar E-mail e Username</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("change-email")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]

    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailUserDTO changeEmailUserDTO)
    {
        try
        {
            string applicationUserId = _tokenInformationsServices.GetApplicationUserId();
            var userDTO = await _authService.ChangeEmailAsync(applicationUserId, changeEmailUserDTO.NewEmail);

            return Ok(userDTO);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
