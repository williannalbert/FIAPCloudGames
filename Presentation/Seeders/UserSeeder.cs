using Application.DTOs.Library;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Seeders;

public static class UserSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {

        using var scope = serviceProvider.CreateScope();
        await CreateUser(scope, "Admin", "admin@fiapcloudgames.com", "Admin@123", true);
        await CreateUser(scope, "User", "user@fiapcloudgames.com", "User@123", false);
        await CreateUser(scope, "Willian Test", "willian@fiapcloudgames.com", "Password@123", false);
    }

    private static async Task CreateUser(IServiceScope scope, string nome, string email, string senha, bool adm = false)
    {
        
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenInformationsServices>();
        var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();

        var admToken = await authService.LoginAsync(new LoginUserDTO()
        {
            Email = email,
            Password = senha
        });

        if (admToken is null)
        {
            var newAdm = await authService.RegisterAsync(new RegisterUserDTO()
            {
                Email = email,
                Name = nome,
                Password = senha
            });

            var userClaimsDTO = tokenService.GetUserClaimsByToken(newAdm.Token);
            
            if(adm)
                _ = await roleService.AddUserRoleAsync(userClaimsDTO.ApplicationUserId, "Admin");

        }
    }
}
