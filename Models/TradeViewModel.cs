using System.Globalization;

namespace StrzonBinanceTradingBot.Models;

public class TradeViewModel
{
    public DateTime Date { get; set; }
    public string DateStr { get { return  Date.ToLocalTime().ToString(); }}

    public string? Symbol { get; set; }
    public decimal Amount { get; set; }

    public decimal Price { get; set; }
    public string PriceStr { get { return Price.ToString("c", new CultureInfo("en-US")); } }

    public decimal Total { get; set; }
    public string TotalStr { get { return Total.ToString("c", new CultureInfo("en-US")); } }

}
