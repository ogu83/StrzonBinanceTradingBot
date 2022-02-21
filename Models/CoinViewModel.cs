using System.Globalization;

namespace StrzonBinanceTradingBot.Models;

public class CoinViewModel
{
    public string? Coin { get; set; }

    public decimal Amount { get; set; }
    public string AmountStr { get { return Amount.ToString(); } }

    public decimal Price { get; set; }
    public string PriceStr { get { return Price.ToString("c", new CultureInfo("en-US")); } }

    public decimal Total { get { return Amount * Price; } }
    public string TotalStr { get { return Total.ToString("c", new CultureInfo("en-US")); } }

    public bool IsLocked { get; set; }
}