using Serilog;

namespace RLP_External_Data_Ingest;

public class Worker : BackgroundService
{
    private bool _isInUse { get; set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Logger = new LoggerConfiguration()
                            // add console as logging target
                            .WriteTo.Console()
                            // set default minimum level
                            .MinimumLevel.Debug()
                            .CreateLogger();

        Log.Information("Data Ingest App Starting", DateTimeOffset.Now);

        // Use Ctrl + C to stop the Data Ingest
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!_isInUse)
            {
                _isInUse = true;

                // TODO - Max out 15 requests per hour

                // TODO - Checks if the data has been updated recently based on appsettings

                // Calls the LaunchDataGet class to get the launch data and ingest to DB
                LaunchDataGet dataGet = new();
                await dataGet.LaunchAPIRetrieval();

                _isInUse = false;

            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}
