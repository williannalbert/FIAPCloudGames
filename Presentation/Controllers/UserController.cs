using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]/admin")]
public class UserController(IUserService _userService, IAuthService _authService) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpGet("{id:Guid}", Name = "GetUser")]
    public async Task<IActionResult> Get(Guid id)
    {
		try
		{
			var userDTO = await _userService.GetAsync(id);
			if (userDTO is null)
				return NotFound();

			return Ok(userDTO);
		}
		catch (Exception e)
		{
			throw;
		}
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpGet]
	public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
	{
		try
		{
			var usersDTO = await _userService.GetAllAsync();
			if (usersDTO is null || usersDTO.Count() == 0)
				return NotFound();

			return Ok(usersDTO);
		}
		catch (Exception e)
		{
			throw;
		}
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="500"></response>
    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<UserDTO>> Put(Guid id, UpdateUserDTO updateUserDTO)
	{
		try
		{
			if(id != updateUserDTO.Id)
                throw new BusinessException("Ids de Usuários não correspondem");

			var userDTO = await _userService.UpdateAsync(updateUserDTO);
			return Ok(userDTO);
        }
        catch (Exception e)
		{
			throw;
		}
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <response code="204"></response>
    /// <response code="500"></response>
    [HttpDelete("{id:Guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		try
		{
			var userDeleted = await _userService.DeleteAsync(id);
			if (!userDeleted)
				return BadRequest("Ocorreu algum erro ao excluir usuário");

			return NoContent();
		}
		catch (Exception e)
		{
			throw;
		}
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="500"></response>
    [HttpPost("change-email")]
    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailUserAdminDTO changeEmailUserAdminDTO)
    {
        try
        {
            var userDTO = await _authService.ChangeEmailAsync(changeEmailUserAdminDTO.ApplicationUserId, changeEmailUserAdminDTO.NewEmail);
            return Ok(userDTO);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
