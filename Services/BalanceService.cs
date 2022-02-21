// using System.Transactions;
using Binance.Net.Interfaces;
using StrzonBinanceTradingBot.Data;
using StrzonBinanceTradingBot.Helpers;

namespace StrzonBinanceTradingBot.Services;

public class BalanceService : IBalanceService
{
    private readonly ApplicationDbContext _context;
    private readonly IBinanceClient _binanceClient;
    private readonly ILogger<BalanceService> _logger;
    private readonly Settings _settings;

    public BalanceService(
        ApplicationDbContext context,
        IBinanceClient binanceClient,
        ILogger<BalanceService> logger,
        Settings settings)
    {
        _context = context;
        _binanceClient = binanceClient;
        _logger = logger;
        _settings = settings;
    }

    private void addBalanceLog(int walletId, string userName, BalanceHistoryEntity.Action reason)
    {
        var balances = GetBalancesOfWalletQuery(walletId, userName);
        var balanceHistory = new BalanceHistoryEntity
        {
            Date = DateTime.UtcNow,
            DemoWallet_ID = walletId,
            Event = reason,
            IdentityUserName = userName,
        };
        var rows = balances?.Select(x => new BalanceHistoryRow
        {
            Amount = x.Amount,
            USDTRate = x.USDTRate,
        }).ToList();
        balanceHistory.Rows = rows;
        _context.BalancesHistory?.Add(balanceHistory);

        _context.SaveChanges();
    }

    public BalanceEntity Add(string symbol, decimal amount, decimal rate, int walletId, string userName)
    {
        var entity = new BalanceEntity
        {
            Symbol = symbol,
            Amount = amount,
            DemoWallet_ID = walletId,
            IdentityUserName = userName,
            USDTRate = rate
        };

        _context.Balances?.Add(entity);
        _context.SaveChanges();

        addBalanceLog(walletId, userName, BalanceHistoryEntity.Action.Init);

        return entity;
    }

    public BalanceEntity? Get(string symbol, int walletId, string userName)
    {
        var entity = _context.Balances?.SingleOrDefault(x => x.Symbol == symbol
                                                          && x.DemoWallet_ID == walletId
                                                          && x.IdentityUserName == userName);
        return entity;
    }

    public BalanceEntity? Get(int id)
    {
        var entity = _context.Balances?.SingleOrDefault(x => x.Id == id);
        return entity;
    }

    public List<BalanceEntity>? GetBalancesOfWallet(int walletId, string userName)
    {
        var retval = GetBalancesOfWalletQuery(walletId, userName)?.ToList();
        return retval;
    }

    private IQueryable<BalanceEntity>? GetBalancesOfWalletQuery(int walletId, string userName)
    {
        var retval = _context.Balances?.Where(x => x.DemoWallet_ID == walletId && x.IdentityUserName == userName);
        return retval;
    }

    public List<BalanceEntity>? Lock(string[] symbol, int walletId, string userName)
    {
        var balances = _context.Balances?
                               .Where(x => x.DemoWallet_ID == walletId
                                        && x.IdentityUserName == userName
                                        && symbol.Contains(x.Symbol)
                                        && !x.IsLocked)
                               .ToList();
        if (balances != null && balances.Any())
        {
            foreach (var entity in balances)
            {
                entity.LockAmount = entity.Amount;
                entity.LockUSDTRate = entity.USDTRate;
                entity.LockDate = DateTime.UtcNow;
                entity.IsLocked = true;
                _context.Update(entity);
            }
            _context.SaveChanges();
        }

        return balances;
    }

    public List<BalanceEntity>? Unlock(string[] symbol, int walletId, string userName)
    {
        var balances = _context.Balances?
                               .Where(x => x.DemoWallet_ID == walletId
                                        && x.IdentityUserName == userName
                                        && symbol.Contains(x.Symbol)
                                        && x.IsLocked)
                               .ToList();
        if (balances != null && balances.Any())
        {
            foreach (var entity in balances)
            {
                entity.IsLocked = false;
                _context.Update(entity);
            }
            _context.SaveChanges();
        }

        return balances;
    }

    public BalanceEntity? Lock(string symbol, int walletId, string userName)
    {
        var entity = Get(symbol, walletId, userName);
        if (entity != null)
        {
            entity.LockAmount = entity.Amount;
            entity.LockUSDTRate = entity.USDTRate;
            entity.LockDate = DateTime.UtcNow;
            entity.IsLocked = true;
            _context.Update(entity);
            _context.SaveChanges();

            addBalanceLog(walletId, userName, BalanceHistoryEntity.Action.Lock);
        }
        return entity;
    }

    public BalanceEntity? Unlock(string symbol, int walletId, string userName)
    {
        var entity = Get(symbol, walletId, userName);
        if (entity != null)
        {
            entity.IsLocked = false;
            _context.Update(entity);
            _context.SaveChanges();

            addBalanceLog(walletId, userName, BalanceHistoryEntity.Action.Unlock);
        }
        return entity;
    }

    public BalanceEntity? Update(string symbol, int walletId, string userName, decimal rate, decimal amount, BalanceHistoryEntity.Action reason)
    {
        //using (var scope = new TransactionScope())
        {
            var entity = Get(symbol, walletId, userName);
            if (entity != null)
            {
                entity.USDTRate = rate;
                entity.Amount = amount;
                _context.Balances?.Update(entity);
                _context.SaveChanges();

                addBalanceLog(walletId, userName, reason);
                //scope.Complete();
            }
            return entity;
        }
    }

    public BalanceEntity? Update(int id, decimal rate, decimal amount, BalanceHistoryEntity.Action reason)
    {
        var entity = Get(id);
        if (entity != null)
        {
            entity.USDTRate = rate;
            entity.Amount = amount;
            _context.Update(entity);
            _context.SaveChanges();

            addBalanceLog(entity.DemoWallet_ID ?? 0, entity.IdentityUserName ?? "", reason);
        }
        return entity;
    }

    public List<BalanceEntity>? GetLockedBalances(bool onlyDemoWallets)
    {
        var query = _context.Balances?.Where(x => x.IsLocked);
        if (onlyDemoWallets)
        {
            query = query?.Where(x => x.DemoWallet_ID != null);
        }
        var retval = query?.ToList();
        return retval;
    }

    public List<BalanceEntity>? GetBalances(bool onlyDemoWallets)
    {
        var query = _context.Balances?.AsQueryable();
        if (onlyDemoWallets)
        {
            query = query?.Where(x => x.DemoWallet_ID != null);
        }
        var retval = query?.ToList();
        return retval;
    }

    public async Task UpdateBalanceUSDTRates(bool onlyDemoWallets)
    {
        _logger.LogInformation("Updating Balance USDT Rates via Binance");
        var changes = 0;
        var updatedDemowalletIds = new Dictionary<int, string>();
        //using (var scope = new TransactionScope())
        {
            var balances = GetBalances(onlyDemoWallets);
            if (balances == null || !balances.Any())
            {
                _logger.LogInformation("There is no balance in wallets");
                return;
            }

            foreach (var balance in balances)
            {
                var symbol = $"{balance.Symbol}USDT";

                var priceResponse = await _binanceClient.Spot.Market.GetPriceAsync(symbol);
                if (!priceResponse.Success)
                {
                    _logger.LogError($"Binance Spot Market GetPrice Error: {priceResponse.Error?.Code} {priceResponse.Error?.Message}");
                    continue;
                }

                var price = priceResponse.Data;
                _logger.LogInformation($"Price of {balance.Symbol} is {price.Price} at {price.Timestamp?.ToLocalTime().ToString()}");
                if (balance.USDTRate != price.Price)
                {
                    var bbalance = Get(balance.Id);
                    if (bbalance != null)
                    {
                        bbalance.USDTRate = price.Price;
                        _context.Balances?.Update(balance);
                        changes += _context.SaveChanges();
                        if (!updatedDemowalletIds.ContainsKey(balance.DemoWallet_ID ?? 0))
                        {
                            updatedDemowalletIds.TryAdd(balance.DemoWallet_ID ?? 0, balance.IdentityUserName ?? "");
                        }
                    }
                }
            }

            if (changes > 0 && updatedDemowalletIds.Any())
            {
                foreach (var wallet in updatedDemowalletIds)
                {
                    addBalanceLog(wallet.Key, wallet.Value, BalanceHistoryEntity.Action.Update);
                }
            }

            _logger.LogInformation($"{balances.Count} balance prices updated");
            //scope.Complete();
        }
    }
}