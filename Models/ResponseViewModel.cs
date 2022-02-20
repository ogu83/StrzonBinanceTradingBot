namespace StrzonBinanceTradingBot.Models;

public class ResponseViewModel
{
    public bool IsSuccess { get; set; }

    public ErrorViewModel? Error { get; set; }

    public bool HasError { get { return Error != null; } }
}