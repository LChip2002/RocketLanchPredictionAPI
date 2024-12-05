namespace RLP_External_Data_Ingest;

public class Worker : BackgroundService
{
    // Using ILogger for Console Logging and Debugging
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Data Ingest App Starting", DateTimeOffset.Now);

        // Use Ctrl + C to stop the Data Ingest
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                // TODO - Checks if the worker is still in use

                // TODO - Checks if the data has been updated recently based on appsettings
                LaunchDataGet dataGet = new();
                dataGet.LaunchAPIRetrieval();


            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
