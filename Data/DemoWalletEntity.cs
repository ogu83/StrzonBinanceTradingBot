using Microsoft.EntityFrameworkCore;

namespace StrzonBinanceTradingBot.Data;

public class DemoWalletEntity : BaseUserEntity
{
    public List<BalanceEntity>? Balances { get; set; }

    public List<TradeEntity>? Trades { get; set; }

    public List<BalanceHistoryEntity>? BalanceHistory { get; set; }

    [Precision(18, 18)]
    public decimal USDTBalance { get; set; }

    public decimal TotalBalanceInUSDT
    {
        get
        {
            if (Balances == null)
            {
                return USDTBalance;
            }

            return USDTBalance + Balances.Sum(x => x.AmountInUSDT);
        }
    }
}