using System.Globalization;

namespace StrzonBinanceTradingBot.Models;

public class BalancesViewModel : ResponseViewModel
{
    public decimal USDTBalance { get; set; }
    public decimal TotalBalanceInUSDT { get; set; }

    public string USDTBalanceStr { get { return USDTBalance.ToString("c", new CultureInfo("en-US")); } }

    public string TotalBalanceInUSDTStr { get { return TotalBalanceInUSDT.ToString("c", new CultureInfo("en-US")); } }
}