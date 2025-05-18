using Application.DTOs.Game;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GameController(IGameService _gameService) : ControllerBase
{

    [HttpGet("{id:Guid}", Name = "GetGame")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var gameDTO = await _gameService.GetAsync(id);
            if (gameDTO == null) 
                return NotFound();

            return Ok(gameDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDTO>>> GetAll()
    {
        try
        {
            var games = await _gameService.GetAllAsync();
            if (games == null || games.Count() == 0)
                return NotFound();
            return Ok(games);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Post([FromBody] CreateGameDTO createGameDTO)
    {
        try
        {
            if (createGameDTO == null)
                throw new BusinessException("Dados inválidos");

            var newGameDTO = await _gameService.CreateAsync(createGameDTO);

            return CreatedAtRoute("GetGame", new { id = newGameDTO.Id }, newGameDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var game = await _gameService.DeleteAsync(id);
            if (!game)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UpdateGameDTO>> Put(Guid id, UpdateGameDTO updateGameDTO)
    {
        try
        {
            if (id != updateGameDTO.Id)
                throw new BusinessException("Ids de Game não correspondem");

            var updatedGameDTO = await _gameService.UpdateAsync(id, updateGameDTO);
            return Ok(updatedGameDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
