using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IRoleService
{
    Task<bool> CreateRoleAsync(string name);
    Task<bool> AddUserRoleAsync(string userId, string roleId);
}
