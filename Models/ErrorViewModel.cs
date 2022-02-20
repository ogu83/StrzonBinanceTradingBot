namespace StrzonBinanceTradingBot.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string? ErrorCode { get; set; }
    
    public string? Message { get; set; }
}