using Application.DTOs.Wallet;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WalletController : Controller
{
    private readonly ITokenInformationsServices _tokenInformationsServices;
    private readonly IWalletService _walletService;
    public WalletController(IWalletService walletService, ITokenInformationsServices tokenInformationsServices)
    {
        _walletService = walletService;
        _tokenInformationsServices = tokenInformationsServices;
    }
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
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
    [Authorize(Roles = "Admin,User")]
    [HttpPost("create")]
    public async Task<ActionResult> Post()
    {
        try
        {
            var userId = _tokenInformationsServices.GetUserId();

            var walletDTO = await _walletService.CreateAsync(userId);

            return Ok(walletDTO);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    [Authorize(Roles = "Admin,User")]
    [HttpPost("addMoney")]
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
}
