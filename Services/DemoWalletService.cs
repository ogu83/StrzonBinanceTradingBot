using StrzonBinanceTradingBot.Data;

namespace StrzonBinanceTradingBot.Services;

public class DemoWalletService : IDemoWalletService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DemoWalletService> _logger;

    public DemoWalletService(ApplicationDbContext context, ILogger<DemoWalletService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public DemoWalletEntity? Create(decimal usdtBalance, string userName)
    {
        var wallet = new DemoWalletEntity
        {
            IdentityUserName = userName,
            USDTBalance = usdtBalance
        };
        _context.DemoWallets?.Add(wallet);
        _context.SaveChanges();
        return wallet;
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
}