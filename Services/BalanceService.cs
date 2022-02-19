using StrzonBinanceTradingBot.Data;

namespace StrzonBinanceTradingBot.Services;

public class BalanceService : IBalanceService
{
    private readonly ApplicationDbContext _context;

    public BalanceService(ApplicationDbContext context)
    {
        _context = context;
    }

    private void addBalanceLog(int walletId, string userName, BalanceHistoryEntity.Action reason)
    {
        var balances = GetBalancesOfWallet(walletId, userName) ?? new List<BalanceEntity>();
        var balanceHistory = new BalanceHistoryEntity
        {
            Date = DateTime.UtcNow,
            DemoWallet_ID = walletId,
            Event = reason,
            IdentityUserName = userName,
        };
        var rows = balances.Select(x => new BalanceHistoryRow
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
        var retval = _context.Balances?.Where(x => x.DemoWallet_ID == walletId && x.IdentityUserName == userName).ToList();
        return retval;
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
        var entity = Get(symbol, walletId, userName);
        if (entity != null)
        {
            entity.USDTRate = rate;
            entity.Amount = amount;
            _context.Update(entity);
            _context.SaveChanges();

            addBalanceLog(walletId, userName, reason);
        }
        return entity;
    }

    public BalanceEntity? Update(int id, decimal rate, decimal amount,BalanceHistoryEntity.Action reason)
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
}