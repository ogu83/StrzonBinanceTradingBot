using Microsoft.EntityFrameworkCore;

namespace StrzonBinanceTradingBot.Data;

public class BalanceHistoryRow : BaseEntity
{
    [Precision(18, 18)]
    public decimal Amount { get; set; }

    [Precision(18, 18)]
    public decimal USDTRate { get; set; }

    public decimal AmountInUSDT { get { return USDTRate * Amount; } }

    public BalanceHistoryEntity? Parent { get; set; }
    public int? Parent_ID { get; set; }
}
