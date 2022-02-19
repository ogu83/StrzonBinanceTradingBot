using StrzonBinanceTradingBot.Helpers;

namespace StrzonBinanceTradingBot.Services;

public class ScopedProcessingService : IScopedProcessingService
{
    private int executionCount = 0;
    private readonly ILogger<ScopedProcessingService> _logger;
    private readonly ITradeService _tradeService;
    private readonly Settings _settings;
    private const int s2ms = 1000;

    public ScopedProcessingService(
        ILogger<ScopedProcessingService> logger,
        ITradeService tradeService,
        Settings settings)
    {
        _logger = logger;
        _tradeService = tradeService;
        _settings = settings;
    }

    public async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            executionCount++;

            _logger.LogInformation("Scoped Processing Service is working. Count: {Count}", executionCount);

            try
            {
                await _tradeService.CheckLockedDemoProfits();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Scoped Processing Service error");
            }
            finally
            {
                await Task.Delay(_settings.scheduledTaskInterval * s2ms, stoppingToken);
            }
        }
    }
}