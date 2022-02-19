// using EntityFrameworkCore.EncryptColumn.Attribute;

namespace StrzonBinanceTradingBot.Data;

public class BinanceCredentialEntity : BaseUserEntity
{
    // [EncryptColumn]
    public string? ApiKey { get; set; }
    // [EncryptColumn]
    public string? ApiSecret { get; set;}
}