using StrzonBinanceTradingBot.Data;

namespace StrzonBinanceTradingBot.Services;

public interface IDemoWalletService
{
    DemoWalletEntity? Create(decimal usdtBalance, string userName);

    DemoWalletEntity? UpdateUsdtBalance(int id, decimal usdtBalance);

    DemoWalletEntity? Get(int id);
}