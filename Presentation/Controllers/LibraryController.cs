using Application.DTOs.Library;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LibraryController(ITokenInformationsServices _tokenInformationsServices,
    ILibraryService _libraryService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();

            var libraryDTO = await _libraryService.GetLibraryByUserIdAsync(userId);
            if (libraryDTO == null) 
                return NoContent();

            return Ok(libraryDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost("create")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Create()
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();
            var createLibraryDTO = new CreateLibraryDTO() { UserId = userId };
            var libraryDTO = await _libraryService.CreateAsync(createLibraryDTO);

            return Ok(libraryDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    [HttpDelete]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Delete()
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();
            var delete = await _libraryService.DeleteAsync(userId);

            return NoContent();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost("add-game")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> AddGame([FromBody] AddGameLibraryDTO addGamesLibraryDTO)
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();
            var libraryDTO = await _libraryService.AddGamesLibraryAsync(userId, addGamesLibraryDTO);
            return Ok(libraryDTO);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
