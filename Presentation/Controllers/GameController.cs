using Application.DTOs.Error;
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
    /// <summary>
    /// Obtem as informações do jogo cadastrado na plataforma
    /// </summary>
    /// <param name="id">Id do jogo</param>
    /// <returns>Jogo cadastrado</returns>
    /// <response code="200">Jogo carregado com sucesso</response>
    /// <response code="404">Jogo não encontrado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpGet("{id:Guid}", Name = "GetGame")]
    [ProducesResponseType(typeof(GameDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var gameDTO = await _gameService.GetAsync(id);
            if (gameDTO == null) 
                throw new NotFoundException("Jogo não localizado");

            return Ok(gameDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Retorna lista com todos os jogos cadastrados na plataforma
    /// </summary>
    /// <returns>Lista de jogos cadastrados</returns>
    /// <response code="200">Jogos carregados com sucesso</response>
    /// <response code="404">Não há jogos cadastrados</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<GameDTO>>> GetAll()
    {
        try
        {
            var games = await _gameService.GetAllAsync();
            if (games == null || games.Count() == 0)
                throw new NotFoundException("Não há jogos cadastrados");
            return Ok(games);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Cadastro de jogo na plataforma
    /// </summary>
    /// <param name="createGameDTO">Objeto com informações do jogo</param>
    /// <returns>Jogo cadastrado</returns>
    /// <response code="201">Cadastro realizado com sucesso</response>
    /// <response code="400">Erro ao criar jogo</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(GameDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
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
    /// <summary>
    /// Exclusão do jogo na plataforma
    /// </summary>
    /// <param name="id">Id do jogo</param>
    /// <returns>Não há retorno</returns>
    /// <response code="204">Exclusão realizada com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Jogo não localizado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var game = await _gameService.DeleteAsync(id);
            if (!game)
                throw new NotFoundException("Jogo não localizado");

            return NoContent();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Atualização do jogo na plataforma
    /// </summary>
    /// <param name="updateGameDTO">Objeto com informações atualizadas do jogo</param>
    /// <returns>Jogo atualizado</returns>
    /// <response code="200">Atualização realizada com sucesso</response>
    /// <response code="400">Erro ao atualizar jogo</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Jogo não localizado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UpdateGameDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
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
