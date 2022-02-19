namespace StrzonBinanceTradingBot.Data;

public class BalanceHistoryEntity : BaseDemoWalletEntity
{
    public enum Action { Init, Trade, Lock, Unlock, Update }

    public Action Event { get; set; }

    public DateTime Date { get; set; }

    public List<BalanceHistoryRow>? Rows { get; set; }

    public decimal USDTBalance { get; set; }

    public decimal TotalBalanceInUSDT { get { return USDTBalance + AmountInUSDT; } }

    public decimal AmountInUSDT
    {
        get
        {
            if (Rows == null)
            {
                return 0;
            }
            else if (!Rows.Any())
            {
                return 0;
            }
            else 
            {
                return Rows.Sum(x => x.AmountInUSDT);
            }
        }
    }
}
