using StrzonBinanceTradingBot.Data;

namespace StrzonBinanceTradingBot.Services;

public interface IBalanceService
{
    BalanceEntity? Add(string symbol, decimal amount, decimal rate, int walletId, string userName);
    BalanceEntity? Update(string symbol, int walletId, string userName, decimal rate, decimal amount,BalanceHistoryEntity.Action reason);
    BalanceEntity? Update(int id, decimal rate, decimal amount,BalanceHistoryEntity.Action reason);
    BalanceEntity? Get(string symbol, int walletId, string userName);
    BalanceEntity? Get(int id);
    List<BalanceEntity>? GetBalancesOfWallet(int walletId, string userName);
    BalanceEntity? Lock(string symbol, int walletId, string userName);
    BalanceEntity? Unlock(string symbol, int walletId, string userName);
    List<BalanceEntity>? GetLockedBalances(bool onlyDemoWallets);
}