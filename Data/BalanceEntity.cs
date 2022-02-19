using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StrzonBinanceTradingBot.Data;

public class BalanceEntity : BaseDemoWalletEntity
{
    [Column(TypeName = "varchar(20)")]
    public string? Symbol { get; set; }

    [Precision(18, 18)]
    public decimal Amount { get; set; }

    [Precision(18, 18)]
    public decimal USDTRate { get; set; }

    public decimal AmountInUSDT { get { return USDTRate * Amount; } }
    

    public bool IsLocked { get; set; }

    [Precision(18, 18)]
    public decimal LockAmount { get; set; }

    [Precision(18, 18)]
    public decimal LockUSDTRate { get; set; }

    public decimal LockAmountInUSDT { get { return LockUSDTRate * LockAmount; } }

    public DateTime LockDate { get; set; }
}
