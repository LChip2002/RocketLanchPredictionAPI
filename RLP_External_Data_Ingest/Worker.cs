namespace RLP_External_Data_Ingest;

public class Worker : BackgroundService
{
    // Using ILogger for Console Logging and Debugging
    private readonly ILogger<Worker> _logger;
    private bool _isInUse { get; set; }

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
            if (_logger.IsEnabled(LogLevel.Information) && !_isInUse)
            {
                _isInUse = true;

                // TODO - Max out 15 requests per hour

                // TODO - Checks if the data has been updated recently based on appsettings

                // Calls the LaunchDataGet class to get the launch data and ingest to DB
                LaunchDataGet dataGet = new();
                dataGet.LaunchAPIRetrieval();

            }

            _isInUse = false;
            await Task.Delay(10000, stoppingToken);
        }
    }
}
