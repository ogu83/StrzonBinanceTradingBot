namespace StrzonBinanceTradingBot.Data;

public abstract class BaseDemoWalletEntity : BaseUserEntity
{
    public int? DemoWallet_ID { get; set; }
    public DemoWalletEntity? DemoWallet { get; set; }
}
