using StrzonBinanceTradingBot.Data;

namespace StrzonBinanceTradingBot.Services;

public interface ITradeService
{
    TradeEntity CreateDemoTrade(
        string symbol, 
        decimal amount, 
        int walletId, 
        string userName, 
        decimal rate, 
        TradeEntity.TradeType tradeType);

    List<TradeEntity>? TradesOfDemoWallet(int walletId);

    Task CheckLockedDemoProfits();
}