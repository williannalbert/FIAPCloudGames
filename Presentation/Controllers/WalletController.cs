using Application.DTOs.User;
using Application.DTOs.Wallet;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WalletController(IWalletService _walletService, ITokenInformationsServices _tokenInformationsServices) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();

            var walletDTO = await _walletService.GetByUserIdAsync(userId);
            if (walletDTO == null)
                return NotFound();

            return Ok(walletDTO);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [Authorize(Roles = "User")]
    [HttpPost("add-money")]
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

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/create")]
    public async Task<ActionResult> Post(UserIdDTO userIdDTO)
    {
        try
        {
            var walletDTO = await _walletService.CreateAsync(userIdDTO.UserId);

            return Ok(walletDTO);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("admin/add-money")]
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

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/deduct-money")]
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
