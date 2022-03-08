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
    private readonly ITradeService _tradeService;
    private readonly Settings _settings;

    public DemoWalletController(
        ILogger<DemoWalletController> logger,
        IBinanceClient binanceClient,
        IBalanceService balanceService,
        IDemoWalletService demoWalletService,
        Settings settings,
        ITradeService tradeService)
    {
        _logger = logger;
        _binanceClient = binanceClient;
        _balanceService = balanceService;
        _demoWalletService = demoWalletService;
        _settings = settings;
        _tradeService = tradeService;
    }

    public IActionResult Index()
    {
        var model = new DemoWalletIndexViewModel();
        ViewBag.UpdateInterval = _settings.scheduledTaskInterval;
        return View(model);
    }

    public IActionResult ResetWallet()
    {
        var username = User.Identity?.Name ?? "";
        var wallet = _demoWalletService.GetOrCreate(username);
        if (wallet == null)
        {
            throw new ArgumentNullException("There is no wallet for this user");
        }
        _demoWalletService.DeleteDemoWallet(wallet.Id);
        _demoWalletService.Create(0, username);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult GetPerformanceChart()
    {
        var response = new PerformanceGraphData();
        try
        {
            var username = User.Identity?.Name ?? "";
            var wallet = _demoWalletService.GetOrCreate(username);
            if (wallet == null)
            {
                throw new ArgumentNullException("There is no wallet for this user");
            }

            var history = _balanceService.GetBalanceHistory(wallet.Id);
            var reducedHistory = new List<Data.BalanceHistoryEntity>();

            if (history != null)
            {
                for (int i = 0; i < history.Count(); i++)
                {
                    if (i <= 0 || (history[i].TotalBalanceInUSDT != history[i - 1].TotalBalanceInUSDT))
                    {
                        reducedHistory.Add(history[i]);
                    }
                }

                response.labels = reducedHistory.Select(x => x.Date.ToString("yyyy-MM-dd HH:mm")).ToArray();

                var uds = new ChartDecimalDataSet
                {
                    label = "USDT",
                    data = reducedHistory.Select(x => x.TotalBalanceInUSDT).ToArray()
                };
                response.datasets = new ChartDecimalDataSet[] { uds };
            }
        }
        catch (Exception ex)
        {
            response.Error = new ErrorViewModel
            {
                ErrorCode = "GetPerformanceChart",
                Message = ex.Message
            };
        }

        return Json(response);
    }

    public IActionResult GetTrades()
    {
        var response = new TradesViewModel();

        try
        {
            var username = User.Identity?.Name ?? "";
            var wallet = _demoWalletService.GetOrCreate(username);
            if (wallet == null)
            {
                throw new ArgumentNullException("There is no wallet for this user");
            }

            var trades = _tradeService.TradesOfDemoWallet(wallet.Id);
            response.Trades = trades?.Select(x => new TradeViewModel
            {
                Date = x.Date,
                Symbol = x.Symbol,
                Amount = x.Amount,
                Price = x.USDTRate,
                Total = x.AmountInUSDT,
                Type = Enum.GetName(x.Type.GetType(), x.Type)
            }).ToList();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.Error = new ErrorViewModel
            {
                ErrorCode = "GetTrades",
                Message = ex.Message
            };
        }

        return Json(response);
    }

    [HttpPost]
    public IActionResult StopLockInProfits(string[] symbols)
    {
        var response = new ResponseViewModel();

        if (symbols == null || symbols.Length <= 0)
        {
            return Json(response);
        }

        try
        {
            var username = User.Identity?.Name ?? "";
            var wallet = _demoWalletService.GetOrCreate(username);
            if (wallet == null)
            {
                throw new ArgumentNullException("There is no wallet for this user");
            }

            _balanceService.Unlock(symbols, wallet.Id, username);
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.Error = new ErrorViewModel
            {
                ErrorCode = "StopLockInProfits",
                Message = ex.Message
            };
        }

        return Json(response);
    }

    [HttpPost]
    public IActionResult LockInProfits(string[] symbols)
    {
        var response = new ResponseViewModel();

        if (symbols == null || symbols.Length <= 0)
        {
            return Json(response);
        }

        try
        {
            var username = User.Identity?.Name ?? "";
            var wallet = _demoWalletService.GetOrCreate(username);
            if (wallet == null)
            {
                throw new ArgumentNullException("There is no wallet for this user");
            }

            _balanceService.Lock(symbols, wallet.Id, username);
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.Error = new ErrorViewModel
            {
                ErrorCode = "LockInProfits",
                Message = ex.Message
            };
        }

        return Json(response);
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
                ErrorCode = "AddCoinToWallet",
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
                LockedPrice = x.LockUSDTRate,
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
            _logger.LogError("GetPrices");
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
                ErrorCode = "GetBalances",
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
                ErrorCode = "UpdateBalance",
                Message = ex.Message
            };
        }

        return Json(response);
    }
}