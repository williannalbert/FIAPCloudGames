using Application.DTOs.Error;
using Application.DTOs.User;
using Application.DTOs.Wallet;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WalletController(IWalletService _walletService, ITokenInformationsServices _tokenInformationsServices) : ControllerBase
{
    /// <summary>
    /// Retorna saldo da carteira do usuário
    /// </summary>
    /// <returns>Carteira do usuário</returns>
    /// <response code="200">Carteira recuperada com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Carteira não localizada</response>
    /// <response code="500">Erro não mapeado</response>
    [HttpGet]
    [Authorize(Roles = "User")]
    [ProducesResponseType(typeof(WalletDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();

            var walletDTO = await _walletService.GetByUserIdAsync(userId);
            if (walletDTO == null)
                throw new NotFoundException("Carteira não localizada");

            return Ok(walletDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Adiciona créditos à carteira do usuário
    /// </summary>
    /// <param name="addMoneyWalletDTO">Objeto com valor que será adicionado</param>
    /// <returns>Carteira com saldo atualizado</returns>
    /// <response code="200">Créditos adicionados com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Carteira não localizada</response>
    /// <response code="500">Erro não mapeado</response>
    [Authorize(Roles = "User")]
    [HttpPost("add-money")]
    [ProducesResponseType(typeof(WalletDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddMoney([FromBody] AddMoneyWalletDTO addMoneyWalletDTO) 
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();

            var walletDTO = await _walletService.AddMoneyAsync(userId, addMoneyWalletDTO.Value);

            return Ok(walletDTO);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    /// <summary>
    /// Administrador adiciona créditos à carteira de algum usuário
    /// </summary>
    /// <param name="moneyWalletDTO">Objeto com id do usuário e valor do crédito</param>
    /// <returns>Carteira com saldo atualizado</returns>
    /// <response code="200">Crédito adicionado com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Carteira não localizada</response>
    /// <response code="500">Erro não mapeado</response>
    [Authorize(Roles = "Admin")]
    [HttpPost("admin/add-money")]
    [ProducesResponseType(typeof(WalletDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddMoneyAdmin([FromBody] MoneyWalletAdminDTO moneyWalletDTO)
    {
        try
        {
            var walletDTO = await _walletService.AddMoneyAsync(moneyWalletDTO.UserId, moneyWalletDTO.Value);

            return Ok(walletDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    /// <summary>
    /// Administrador retira créditos da carteira de algum usuário
    /// </summary>
    /// <param name="moneyWalletDTO">Objeto com id do usuário e valor do crédito</param>
    /// <returns>Carteira com saldo atualizado</returns>
    /// <response code="200">Crédito debitado com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="404">Carteira não localizada</response>
    /// <response code="500">Erro não mapeado</response>
    [Authorize(Roles = "Admin")]
    [HttpPost("admin/deduct-money")]
    [ProducesResponseType(typeof(WalletDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeductMoneyAdmin([FromBody] MoneyWalletAdminDTO moneyWalletDTO)
    {
        try
        {
            var walletDTO = await _walletService.DeductMoneyAsync(moneyWalletDTO.UserId, moneyWalletDTO.Value, true);

            return Ok(walletDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
