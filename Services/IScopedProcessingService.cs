namespace StrzonBinanceTradingBot.Services;

public interface IScopedProcessingService
{
    Task DoWork(CancellationToken stoppingToken);
}
