using System.Globalization;

namespace StrzonBinanceTradingBot.Models;

public class PriceViewModel
{
    public string? Symbol { get; set; }
 
    public decimal Price { get; set; }
    public string PriceStr { get { return Price.ToString("c", new CultureInfo("en-US")); }  }

    public string? Value { get { return Symbol?.Replace("USDT", ""); } }

    public string? Text { get { return $"{Value} - {PriceStr} USDT"; } }
}