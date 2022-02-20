using Binance.Net.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrzonBinanceTradingBot.Models;
using StrzonBinanceTradingBot.Services;

namespace StrzonBinanceTradingBot.Controllers;

[Authorize]
public class DemoWalletController : Controller
{
    private readonly ILogger<DemoWalletController> _logger;
    private readonly IBinanceClient _binanceClient;
    private readonly IBalanceService _balanceService;
    private readonly IDemoWalletService _demoWalletService;

    public DemoWalletController(
        ILogger<DemoWalletController> logger,
        IBinanceClient binanceClient,
        IBalanceService balanceService,
        IDemoWalletService demoWalletService)
    {
        _logger = logger;
        _binanceClient = binanceClient;
        _balanceService = balanceService;
        _demoWalletService = demoWalletService;
    }

    public IActionResult Index()
    {
        var model = new DemoWalletIndexViewModel();
        return View(model);
    }

    public IActionResult GetBalances()
    {
        var response = new BalancesViewModel();
        try
        {
            var username = User.Identity?.Name ?? "";
            var wallet = _demoWalletService.GetOrCreate(username);
            if (wallet == null)
            {
                throw new ArgumentNullException("There is no wallet for this user");
            }
            response.USDTBalance = wallet.USDTBalance;
            response.TotalBalanceInUSDT = wallet.TotalBalanceInUSDT;
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateBalance Error");
            response.Error = new ErrorViewModel
            {
                ErrorCode = "Update Balance",
                Message = ex.Message
            };
        }

        return Json(response);
    }

    [HttpPost]
    public IActionResult UpdateBalance(decimal balance)
    {
        var response = new ResponseViewModel();
        try
        {
            var username = User.Identity?.Name ?? "";
            var wallet = _demoWalletService.GetOrCreate(username);
            if (wallet != null)
            {
                wallet = _demoWalletService.UpdateUsdtBalance(wallet.Id, balance);
            }
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateBalance Error");
            response.Error = new ErrorViewModel
            {
                ErrorCode = "Update Balance",
                Message = ex.Message
            };
        }

        return Json(response);
    }
}