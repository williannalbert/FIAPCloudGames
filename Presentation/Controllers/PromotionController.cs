using Application.DTOs.Error;
using Application.DTOs.Game;
using Application.DTOs.GamePromotion;
using Application.DTOs.Promotion;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PromotionController(IPromotionService _promotionService) : ControllerBase
{
    /// <summary>
    /// Retorna promoção cadastrada na plataforma
    /// </summary>
    /// <param name="id">Id da promoção</param>
    /// <returns>Promoção cadastrada</returns>
    /// <response code="200">Promoção obtida com sucesso</response>
    /// <response code="404">Promoção não localizada</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpGet("{id:Guid}", Name = "GetPromotion")]
    [ProducesResponseType(typeof(PromotionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var promotionDTO = await _promotionService.GetAsync(id);
            if (promotionDTO == null)
                throw new NotFoundException("Promoção não localizada");

            return Ok(promotionDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Retorna lista com todas as promoções cadastradas
    /// </summary>
    /// <returns>Promoções cadastradas</returns>
    /// <response code="200">Lista com promoções obtida com sucesso</response>
    /// <response code="404">Não há promoções cadastradas</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PromotionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PromotionDTO>>> GetAll()
    {
        try
        {
            var promotionListDTO = await _promotionService.GetAllAsync();
            if (promotionListDTO == null)
                throw new NotFoundException("Não há promoções cadastradas");

            return Ok(promotionListDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Cadastrar nova promoção na plataforma
    /// </summary>
    /// <param name="promotionDTO">Objeto com informações de cadastro de promoção</param>
    /// <returns>Promoção cadastrada</returns>
    /// <response code="201">Promoção cadastrada com sucesso</response>
    /// <response code="400">Erro ao cadastrar promoção</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PromotionDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PromotionDTO>> Post([FromBody] CreatePromotionDTO createPromotionDTO)
    {
        try
        {
            if (createPromotionDTO == null)
                throw new BusinessException("Dados inválidos");

            var newPromotionDTO = await _promotionService.CreateAsync(createPromotionDTO);

            return new CreatedAtRouteResult("GetPromotion", new { id = newPromotionDTO.Id }, newPromotionDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Exclusão de promoção cadastrada na plataforma
    /// </summary>
    /// <param name="id">Id da promoção</param>
    /// <returns>Não há retorno</returns>
    /// <response code="204">Exclusão realizada com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Promoção não localizada</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var promotionDTO = await _promotionService.DeleteAsync(id);
            if (!promotionDTO)
                throw new NotFoundException("Promoção não localizada");

            return NoContent();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Atualização de promoção na plataforma
    /// </summary>
    /// <param name="id">Id da promoção</param>
    /// <param name="promotionDTO">Objeto com informações atualizadas da promoção</param>
    /// <returns>Promoção atualizada</returns>
    /// <response code="200">Promoção atualizada com sucesso</response>
    /// <response code="400">Erro ao atualizar promoção</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PromotionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PromotionDTO>> Put(Guid id, PromotionDTO promotionDTO)
    {
        try
        {
            if(id != promotionDTO.Id)
                throw new BusinessException("Ids de Promotion não correspondem");

            var promotionUpdatedDTO = await _promotionService.UpdateAsync(id, promotionDTO);
            return Ok(promotionUpdatedDTO);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Inclusão de promoção à um jogo
    /// </summary>
    /// <param name="gamePromotionDTO">Objeto com informação do jogo e da promoção</param>
    /// <returns>Retorna promoção com os jogos correspondentes</returns>
    /// <response code="201">Promoção adicionada ao jogo com sucesso</response>
    /// <response code="400">Erro ao adicionar jogo à promoção</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Promoção ou Jogo não localizado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("add-game")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PromotionDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PromotionDTO>> AddGame([FromBody] GamePromotionDTO gamePromotionDTO)
    {
        try
        {
            if (gamePromotionDTO == null)
                throw new BusinessException("Dados inválidos");

            var newPromotionDTO = await _promotionService.AddGamePromotionAsync(gamePromotionDTO);

            return new CreatedAtRouteResult("GetPromotion", new { id = newPromotionDTO.Id }, newPromotionDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Retirar jogo de promoção
    /// </summary>
    /// <param name="gamePromotionDTO">Objeto com informações de jogo e promoção</param>
    /// <returns></returns>
    /// <response code="204">Jogo retirado de promoção com sucesso</response>
    /// <response code="400">Erro ao adicionar jogo à promoção</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Promoção ou Jogo não localizado</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpPost("delete-game")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PromotionDTO), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteGamePromotion([FromBody] GamePromotionDTO gamePromotionDTO)
    {
        try
        {
            if (gamePromotionDTO == null)
                throw new BusinessException("Dados inválidos");

            var deletedPromotion = await _promotionService.DeleteGamePromotionAsync(gamePromotionDTO);
            if (!deletedPromotion)
                throw new BusinessException("Ocorreu um erro ao retirar jogo da promoção");

            return NoContent();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
