using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using RLP_DB.Models;
using RLP_DB; // Add this line if PostgresV1Context is defined in this namespace

namespace RLP_External_Data_Ingest;

public class LaunchDataGet
{
    private readonly HttpClient client;
    private readonly WeatherQuery weatherQuery;

    public bool isBusy = false;

    // Constructor for the class where we initialise API connections
    public LaunchDataGet()
    {
        // TODO - Put this in an appsettings
        client = new HttpClient();
        client.BaseAddress = new Uri("https://ll.thespacedevs.com/");
        client.Timeout = new TimeSpan(0, 0, 30);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        // Initialise the weather query class
        weatherQuery = new WeatherQuery();

    }

    // Gets the launch data from the Launch Library 2 API
    // See https://ll.thespacedevs.com/docs/#/launches/launches_previous_list for reference
    public async Task LaunchAPIRetrieval()
    {
        // Checks if the worker is still in use
        if (isBusy)
        {
            Console.WriteLine("API retrieval is busy");
            return;
        }

        isBusy = true;

        // TODO - Actual API connection
        //HttpResponseMessage response = await client.GetAsync("/2.3.0/launches/previous/?limit=10&offset=10");

        // TODO - delete test request output when API is connected and working
        // ---------------------------------------------------------------

        // Fake api response
        HttpResponseMessage response = new HttpResponseMessage();

        // Open JSON file and convert to string
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestLaunchResponse.json");
        string testString = await File.ReadAllTextAsync(jsonFilePath);

        // ---------------------------------------------------------------

        // Checks if a successful response code has been outputted for the connection
        if (response.IsSuccessStatusCode || testString != null)
        {
            Console.WriteLine("Success");

            // Reads the JSON content of the API response and converts to launch object

            // TODO - Bring back when API is connected
            //var res = await response.Content.ReadAsStringAsync();

            var res = testString;

            // Set up options for deserialising JSON that avoids case sensitivity
            JsonSerializerOptions? options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            LaunchObject? launch = System.Text.Json.JsonSerializer.Deserialize<LaunchObject>(res, options);
            using (var context = new PostgresV1Context())
            {
                context.Database.EnsureCreated();
                var launchEntries = new List<LaunchEntry>();

                // Loop through each launch in the launch object
                for (int i = 0; i < launch.Results.Count; i++)
                //{
                //
                //foreach (var item in launch.Results)
                {

                    // Get relevant data from the launch object
                    var item = launch.Results[i];

                    // Query weather api with launch data date and location
                    AverageWeatherMetrics? weather = weatherQuery.GetWeather(item.WindowStart, item.WindowEnd, item.Pad.Latitude, item.Pad.Longitude);

                    try
                    {
                        // Combine weather and launch data into new object
                        LaunchEntry launchEntry = new LaunchEntry()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Mission?.Description,
                            Country = item.Pad.Location.Name,
                            LaunchLatitude = item.Pad.Latitude,
                            LaunchLongitude = item.Pad.Longitude,
                            LaunchStart = item.WindowStart,
                            LaunchEnd = item.WindowEnd,
                            Status = item.Status.Name,
                            StatusDescription = item.Status.Description,
                            Rocket = item.Rocket.Configuration.Name,
                            Mission = item.Mission.Name,
                            Image = JsonConvert.SerializeObject(item.Image),
                            Temperature = weather.Temperature,
                            Rain = weather.Rain,
                            Showers = weather.Showers,
                            Snowfall = weather.Snowfall,
                            CloudCover = weather.CloudCover,
                            CloudCoverLow = weather.CloudCoverLow,
                            CloudCoverMid = weather.CloudCoverMid,
                            CloudCoverHigh = weather.CloudCoverHigh,
                            Visibility = weather.Visibility,
                            WindSpeed10m = weather.WindSpeed10m,
                            WindSpeed80m = weather.WindSpeed80m,
                            WindSpeed120m = weather.WindSpeed120m,
                            WindSpeed180m = weather.WindSpeed180m,
                            Temperature80m = weather.Temperature80m,
                            Temperature120m = weather.Temperature120m,
                            Temperature180m = weather.Temperature180m
                        };

                        launchEntries.Add(launchEntry);
                        Console.WriteLine(launchEntries.Count);

                        // Check if the launch entry already exists in the database
                        // Attempts to ingest launch entry to the database
                        var existingEntry = await context.Set<LaunchEntry>().FindAsync(launchEntry.Id);
                        if (existingEntry == null)
                        {
                            Console.WriteLine("Adding new launch entry to database");
                            // Attempts to add the launch entry to the database
                            context.Set<LaunchEntry>().Add(launchEntry);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            Console.WriteLine("Launch entry already exists in database");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }



            }

            isBusy = false;
            return;
        }

        Console.WriteLine("Fail");
        Thread.Sleep(5000);
    }

}