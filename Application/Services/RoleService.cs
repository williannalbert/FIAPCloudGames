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

public class RoleService : IRoleService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly RoleManager<IdentityRole> _rolermanager;

    public RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolermanager)
    {
        _userManager = userManager;
        _rolermanager = rolermanager;
    }
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
