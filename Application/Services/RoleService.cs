using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class RoleService(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _rolermanager) : IRoleService
{
    public async Task<bool> AddUserRoleAsync(string userApplicationId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userApplicationId);
        if (user is null)
            return false;

        if (!await _rolermanager.RoleExistsAsync(roleName))
        {
            var roleCreated = await this.CreateRoleAsync(roleName);
            if (!roleCreated)
                return false;

        }
        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<bool> CreateRoleAsync(string name)
    {
        if (await _rolermanager.RoleExistsAsync(name))
            return false;
        var result = await _rolermanager.CreateAsync(new IdentityRole(name));
        return result.Succeeded;
    }
}
