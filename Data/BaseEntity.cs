using System.ComponentModel.DataAnnotations;

namespace StrzonBinanceTradingBot.Data;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
