using System.Transactions;
using StrzonBinanceTradingBot.Data;

namespace StrzonBinanceTradingBot.Services;
public class TradeService : ITradeService
{
    private readonly ApplicationDbContext _context;

    private readonly IBalanceService _balanceService;

    private readonly IDemoWalletService _demoWalletService;

    public TradeService(
        ApplicationDbContext context,
        IBalanceService balanceService,
        IDemoWalletService demoWalletService)
    {
        _context = context;
        _balanceService = balanceService;
        _demoWalletService = demoWalletService;
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

        using (var scope = new TransactionScope())
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

            scope.Complete();
        }

        return entity;
    }

    public List<TradeEntity>? TradesOfDemoWallet(int walletId)
    {
        var retval = _context.Trades?.Where(x => x.DemoWallet_ID == walletId).ToList();
        return retval;
    }
}