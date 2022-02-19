using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StrzonBinanceTradingBot.Data;

public class TradeEntity : BaseDemoWalletEntity
{
    public enum TradeType { Buy, Sell }

    public TradeType Type { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? Symbol { get; set; }

    public DateTime Date { get; set; }

    [Precision(18, 18)]
    public decimal Amount { get; set; }

    [Precision(18, 18)]
    public decimal USDTRate { get; set; }

    public decimal AmountInUSDT { get { return USDTRate * Amount; } }

    public override string ToString()
    {
        return $"A {Enum.GetName<TradeType>(Type)} trade for {Symbol} with amount {Amount} ({AmountInUSDT} USDT) at {Date.ToLocalTime().ToString()}";
    }
}
