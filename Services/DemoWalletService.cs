using StrzonBinanceTradingBot.Data;
using StrzonBinanceTradingBot.Helpers;

namespace StrzonBinanceTradingBot.Services;

public class DemoWalletService : IDemoWalletService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DemoWalletService> _logger;
    private readonly Settings _settings;

    public DemoWalletService(ApplicationDbContext context, ILogger<DemoWalletService> logger, Settings settings)
    {
        _context = context;
        _logger = logger;
        _settings = settings;
    }

    public DemoWalletEntity? Create(decimal usdtBalance, string userName)
    {
        var wallet = new DemoWalletEntity
        {
            IdentityUserName = userName,
            USDTBalance = usdtBalance
        };
        _context.DemoWallets?.Add(wallet);
        var count = _context.SaveChanges();
        return wallet;
    }

    public DemoWalletEntity? GetOrCreate(string userName)
    {
        var wallet = _context.DemoWallets?.SingleOrDefault(x => x.IdentityUserName == userName);
        if (wallet == null)
        {
            return Create(_settings.demoWalletInitialBalance, userName);
        }
        else
        {
            return wallet;
        }
    }

    public DemoWalletEntity? Get(int id)
    {
        return _context.DemoWallets?.SingleOrDefault(x => x.Id == id);
    }

    public DemoWalletEntity? UpdateUsdtBalance(int id, decimal usdtBalance)
    {
        var wallet = Get(id);
        if (wallet != null)
        {
            wallet.USDTBalance = usdtBalance;
            _context.DemoWallets?.Update(wallet);
            _context.SaveChanges();
        }
        return wallet;
    }

    public decimal GetTotalCoinBalanceInUSDT(int id)
    {
        var retval = _context.Balances?.Where(x => x.DemoWallet_ID == id).Sum(x => (double)x.USDTRate * (double)x.Amount);
        return (decimal)(retval ?? 0d);
    }

    public void DeleteDemoWallet(int id)
    {
        var wallet = _context.DemoWallets?.SingleOrDefault(x => x.Id == id);
        if (wallet != null)
        {
            _context.Remove(wallet);
            _context.SaveChanges();
        }
    }
}