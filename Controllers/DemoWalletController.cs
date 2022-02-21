using System.Linq;
using Binance.Net.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StrzonBinanceTradingBot.Helpers;
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
    private readonly Settings _settings;

    public DemoWalletController(
        ILogger<DemoWalletController> logger,
        IBinanceClient binanceClient,
        IBalanceService balanceService,
        IDemoWalletService demoWalletService, 
        Settings settings)
    {
        _logger = logger;
        _binanceClient = binanceClient;
        _balanceService = balanceService;
        _demoWalletService = demoWalletService;
        _settings = settings;
    }

    public IActionResult Index()
    {
        var model = new DemoWalletIndexViewModel();
        ViewBag.UpdateInterval = _settings.scheduledTaskInterval;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddCoinToWallet(string symbol, decimal amount)
    {
        var response = new ResponseViewModel();

        var username = User.Identity?.Name ?? "";

        try
        {
            var wallet = _demoWalletService.GetOrCreate(username);
            if (wallet == null)
            {
                throw new ArgumentNullException("There is no wallet for this user");
            }

            var priceResponse = await _binanceClient.Spot.Market.GetPriceAsync($"{symbol}USDT");
            if (!priceResponse.Success)
            {
                _logger.LogError("UpdateBalance Error");
                response.Error = new ErrorViewModel
                {
                    ErrorCode = priceResponse.Error?.Code.ToString(),
                    Message = priceResponse.Error?.Message
                };
            }
            else
            {
                var price = priceResponse.Data.Price;

                var balance = _balanceService.Get(symbol, wallet.Id, username);
                if (balance == null)
                {
                    _balanceService.Add(symbol, amount, price, wallet.Id, username);
                }
                else
                {
                    _balanceService.Update(symbol, wallet.Id, username, price, amount, Data.BalanceHistoryEntity.Action.Update);
                }
                response.IsSuccess = true;
            }
        }
        catch (Exception ex)
        {
            response.Error = new ErrorViewModel
            {
                ErrorCode = "Update Balance",
                Message = ex.Message
            };
        }

        return Json(response);
    }

    public IActionResult GetCoins()
    {
        var response = new CoinsViewModel();
        var username = User.Identity?.Name ?? "";
        try
        {
            var wallet = _demoWalletService.GetOrCreate(username);
            if (wallet == null)
            {
                throw new ArgumentNullException("There is no wallet for this user");
            }

            var balances = _balanceService.GetBalancesOfWallet(wallet.Id, username);
            response.Coins = balances?.Select(x => new CoinViewModel
            {
                Coin = x.Symbol,
                Amount = x.Amount,
                Price = x.USDTRate,
                IsLocked = x.IsLocked,
            }).ToList();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("GetCoins Error");
            response.Error = new ErrorViewModel
            {
                ErrorCode = "GetCoins",
                Message = ex.Message
            };
        }

        return Json(response);
    }

    public async Task<IActionResult> GetPrices()
    {
        var response = new PricesViewModel();
        var pricesResponse = await _binanceClient.Spot.Market.GetPricesAsync();
        if (pricesResponse.Success)
        {
            response.Prices = pricesResponse.Data
                                            .Where(x => x.Symbol.Contains("USDT"))
                                            .Select(x => new PriceViewModel
                                            {
                                                Price = x.Price,
                                                Symbol = x.Symbol
                                            })
                                            .ToList();
            response.IsSuccess = true;
        }
        else
        {
            _logger.LogError("UpdateBalance Error");
            response.Error = new ErrorViewModel
            {
                ErrorCode = pricesResponse.Error?.Code.ToString(),
                Message = pricesResponse.Error?.Message
            };
        }
        return Json(response);
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
            response.TotalBalanceInUSDT = _demoWalletService.GetTotalCoinBalanceInUSDT(wallet.Id) + wallet.USDTBalance;
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