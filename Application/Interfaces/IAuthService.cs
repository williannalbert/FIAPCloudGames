using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO registerUserDTO);
    Task<AuthResponseDTO?> LoginAsync(LoginUserDTO loginUserDTO);
    Task<string> GenerateResetPasswordTokenAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);
    Task<UserDTO> ChangeEmailAsync(string applicationUserId, string newEmail);
}
