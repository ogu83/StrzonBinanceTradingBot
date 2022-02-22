using System.Transactions;
using Binance.Net.Interfaces;
using StrzonBinanceTradingBot.Data;
using StrzonBinanceTradingBot.Helpers;

namespace StrzonBinanceTradingBot.Services;

public class TradeService : ITradeService
{
    private readonly ApplicationDbContext _context;
    private readonly IBinanceClient _binanceClient;
    private readonly IBalanceService _balanceService;
    private readonly IDemoWalletService _demoWalletService;
    private readonly ILogger<TradeService> _logger;
    private readonly Settings _settings;

    public TradeService(
        ApplicationDbContext context,
        IBalanceService balanceService,
        IDemoWalletService demoWalletService,
        ILogger<TradeService> logger,
        IBinanceClient binanceClient,
        Settings settings)
    {
        _context = context;
        _balanceService = balanceService;
        _demoWalletService = demoWalletService;
        _logger = logger;
        _binanceClient = binanceClient;
        _settings = settings;
    }

    public async Task CheckLockedDemoProfits()
    {
        _logger.LogInformation("Checking locked profits in demo wallets, search for locked balances.");
        var balances = _balanceService.GetLockedBalances(onlyDemoWallets: true);

        if (balances == null || !balances.Any())
        {
            _logger.LogInformation("no locked balance found.");
            return;
        }

        _logger.LogInformation($"{balances.Count} balances found.");
        foreach (var balance in balances)
        {
            _logger.LogInformation($"Processing {balance.Amount} {balance.Symbol} for user {balance.IdentityUserName} in demo wallet {balance.DemoWallet_ID}");

            var symbol = $"{balance.Symbol}USDT";
            var priceResponse = await _binanceClient.Spot.Market.GetPriceAsync(symbol);
            if (!priceResponse.Success)
            {
                _logger.LogError($"Binance Spot Market GetPrice Error: {priceResponse.Error?.Code} {priceResponse.Error?.Message}");
                continue;
            }

            var price = priceResponse.Data;
            _logger.LogInformation($"Price of {balance.Symbol} is {price.Price} at {price.Timestamp?.ToLocalTime().ToString()}");
            _logger.LogInformation($"Price of {balance.Symbol} is {balance.LockUSDTRate} at {balance.LockDate.ToLocalTime().ToString()}");

            if (price.Price < (1 - _settings.maxAllowedSlipage) * balance.LockUSDTRate)
            {
                _logger.LogInformation($"Current price is LOWER than locked price for {balance.Symbol}, SELL it");

                CreateDemoTrade(
                    balance.Symbol ?? "",
                    balance.Amount,
                    balance.DemoWallet_ID ?? 0,
                    balance.IdentityUserName ?? "",
                    price.Price,
                    TradeEntity.TradeType.Sell);
            }
            else if (price.Price > (1 + _settings.maxAllowedSlipage) * balance.LockUSDTRate)
            {
                _logger.LogInformation($"Current price is HIGHER than locket price for {balance.Symbol}, BUY it");

                var wallet = _demoWalletService.Get(balance.DemoWallet_ID ?? 0);
                if (wallet == null)
                {
                    _logger.LogError($"There is no Demo wallet for this user {balance.IdentityUserName}");
                    continue;
                }

                if (wallet.USDTBalance <= 0)
                {
                    _logger.LogWarning($"There is insufficient funds (${wallet.USDTBalance} USDT) to buy anything in this wallet for this user {balance.IdentityUserName}.");
                    continue;
                }

                _logger.LogInformation($"Locked {balance.Symbol} amount is {balance.LockAmount}.");

                var buyBackAmountInUSDT = balance.LockAmount * price.Price;
                _logger.LogInformation($"{buyBackAmountInUSDT} USDT is neccesary to buy back {balance.LockAmount}.");
                _logger.LogInformation($"demo wallet of {balance.IdentityUserName} has {wallet?.USDTBalance} USDT.");

                var amount = balance.LockAmount;
                if (wallet.USDTBalance >= buyBackAmountInUSDT)
                {
                    _logger.LogInformation($"Demo wallet has sufficient funds to BUY {balance.LockAmount} {balance.Symbol}.");
                }
                else
                {
                    _logger.LogInformation($"Demo wallet has insufficient funds to BUY {balance.LockAmount} {balance.Symbol}.");
                    amount = wallet.USDTBalance / price.Price;
                    _logger.LogInformation($"With {wallet.USDTBalance} only {amount} {balance.Symbol} can be bought with this demo wallet.");
                }

                CreateDemoTrade(
                    balance.Symbol ?? "",
                    amount,
                    balance.DemoWallet_ID ?? 0,
                    balance.IdentityUserName ?? "",
                    price.Price,
                    TradeEntity.TradeType.Buy);
            }
            else
            {
                _logger.LogInformation($"Nothing to BUY OR SELL. Price is between allowed slipage {_settings.maxAllowedSlipage.ToString("P")}");
            }
        }
    }

    public TradeEntity CreateDemoTrade(
        string symbol,
        decimal amount,
        int walletId,
        string userName,
        decimal rate,
        TradeEntity.TradeType tradeType)
    {
        var entity = new TradeEntity
        {
            Amount = amount,
            Date = DateTime.UtcNow,
            DemoWallet_ID = walletId,
            IdentityUserName = userName,
            Symbol = symbol,
            Type = tradeType,
            USDTRate = rate
        };

        //using (var scope = new TransactionScope())
        {
            var wallet = _demoWalletService.Get(walletId);
            if (wallet == null)
            {
                throw new ArgumentNullException($"There is no such wallet {walletId}");
            }

            var balance = _balanceService.Get(entity.Symbol, walletId, userName);
            if (balance == null)
            {
                throw new ArgumentNullException($"No such balance {entity.Symbol} in this wallet {walletId}");
            }

            _context.Trades?.Add(entity);
            _context.SaveChanges();

            if (entity.Type == TradeEntity.TradeType.Buy)
            {
                if (entity.AmountInUSDT > wallet.USDTBalance)
                {
                    throw new ArgumentOutOfRangeException("There not enough USDT to buy!");
                }
                wallet.USDTBalance -= entity.AmountInUSDT;
                _demoWalletService.UpdateUsdtBalance(wallet.Id, wallet.USDTBalance);

                _balanceService.Update(entity.Symbol, walletId, userName, entity.USDTRate, balance.Amount + entity.Amount, BalanceHistoryEntity.Action.Trade);
            }
            else if (entity.Type == TradeEntity.TradeType.Sell)
            {
                wallet.USDTBalance += entity.AmountInUSDT;
                _demoWalletService.UpdateUsdtBalance(wallet.Id, wallet.USDTBalance);

                _balanceService.Update(entity.Symbol, walletId, userName, entity.USDTRate, balance.Amount - entity.Amount, BalanceHistoryEntity.Action.Trade);
            }

            //scope.Complete();

            _logger.LogInformation($"{entity.ToString()} executed.");
        }

        return entity;
    }

    public List<TradeEntity>? TradesOfDemoWallet(int walletId)
    {
        var retval = _context.Trades?
                             .Where(x => x.DemoWallet_ID == walletId)
                             .OrderByDescending(x => x.Date)
                             .ToList();
        return retval;
    }
}