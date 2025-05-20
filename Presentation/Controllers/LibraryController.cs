using Application.DTOs.Error;
using Application.DTOs.Library;
using Application.DTOs.User;
using Application.Exceptions;
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
    /// <summary>
    /// Retorna biblioteca do usuário com jogos cadastrados
    /// </summary>
    /// <returns>Obtem biblioteca e jogos adquiridos</returns>
    /// <response code="200">Biblioteca obdtida com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Usuário não possui biblioteca cadastrada</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpGet]
    [Authorize(Roles = "User")]
    [ProducesResponseType(typeof(LibraryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> Get()
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();

            var libraryDTO = await _libraryService.GetLibraryByUserIdAsync(userId);
            if (libraryDTO == null) 
                throw new NotFoundException("Usuário não possui biblioteca cadastrada.");

            return Ok(libraryDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Adiciona jogo à biblioteca do usuário
    /// </summary>
    /// <param name="addGamesLibraryDTO">Objeto com informações de jogo e promoção</param>
    /// <returns>Retorna biblioteca atualizada</returns>
    /// <response code="200">Jogo adquirido com sucesso</response>
    /// <response code="400">Erro ao adquirir jogo</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Jogo ou Promoção não localizada</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("add-game")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(typeof(LibraryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]

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
