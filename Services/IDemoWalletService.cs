using StrzonBinanceTradingBot.Data;

namespace StrzonBinanceTradingBot.Services;

public interface IDemoWalletService
{
    DemoWalletEntity? Create(decimal usdtBalance, string userName);
    DemoWalletEntity? UpdateUsdtBalance(int id, decimal usdtBalance);
    DemoWalletEntity? Get(int id);
    DemoWalletEntity? GetOrCreate(string userName);
    public decimal GetTotalCoinBalanceInUSDT(int id);
    public void DeleteDemoWallet(int id);
}