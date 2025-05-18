using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllAsync();
    Task<UserDTO> GetAsync(Guid userId);
    Task<UserDTO> UpdateAsync(UpdateUserDTO updateUserDTO);
    Task<bool> DeleteAsync(Guid userId);
}
