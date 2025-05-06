using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Seeders;

public static class RoleSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleService = serviceProvider.GetRequiredService<IRoleService>();
        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            await roleService.CreateRoleAsync(role);
        }
    }
}
