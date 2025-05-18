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
    [HttpGet("{id:Guid}", Name = "GetPromotion")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var promotionDTO = await _promotionService.GetAsync(id);
            if (promotionDTO == null)
                return NotFound();

            return Ok(promotionDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PromotionDTO>>> GetAll()
    {
        try
        {
            var promotionListDTO = await _promotionService.GetAllAsync();
            if (promotionListDTO == null)
                return NotFound();


            return Ok(promotionListDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PromotionDTO>> Post([FromBody] PromotionDTO promotionDTO)
    {
        try
        {
            if (promotionDTO == null)
                throw new BusinessException("Dados inválidos");

            var newPromotionDTO = await _promotionService.CreateAsync(promotionDTO);

            return new CreatedAtRouteResult("GetPromotion", new { id = newPromotionDTO.Id }, newPromotionDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var promotionDTO = await _promotionService.DeleteAsync(id);
            if (!promotionDTO)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
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
    [HttpPost("add-game")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PromotionDTO>> AddGame([FromBody] GamePromotionDTO gamePromotionDTO)
    {
        try
        {
            if (gamePromotionDTO == null)
                return BadRequest("Dados inválidos");

            var newPromotionDTO = await _promotionService.AddGamePromotionAsync(gamePromotionDTO);

            return new CreatedAtRouteResult("GetPromotion", new { id = newPromotionDTO.Id }, newPromotionDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    [HttpPost("delete-game")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGamePromotion([FromBody] GamePromotionDTO gamePromotionDTO)
    {
        try
        {
            if (gamePromotionDTO == null)
                return BadRequest("Dados inválidos");

            var deletedPromotion = await _promotionService.DeleteGamePromotionAsync(gamePromotionDTO);
            if(!deletedPromotion)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
