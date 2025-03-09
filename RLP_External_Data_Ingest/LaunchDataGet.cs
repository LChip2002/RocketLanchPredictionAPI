using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using RLP_DB.Models;
using RLP_External_Data_Ingest.Models;
using RLP_DB.Contexts;
using Serilog;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace RLP_External_Data_Ingest;

public class LaunchDataGet
{
    private readonly HttpClient client;
    private readonly WeatherQuery weatherQuery;

    public bool isBusy { get; set; } = false;

    // Offset for the API request
    public int offsetNo { get; set; } = 0;

    // List of Launch Status Response Codes: 3 - Success, 4 - Failure, 7 - Partial Failure
    public List<int> statusCodes { get; set; } = new List<int>() { 3, 4, 7 };

    // Constructor for the class where we initialise API connections
    public LaunchDataGet()
    {
        // Load .env file from project root
        Env.Load("../.env");

        // Read environment variables
        string? apiKey = Environment.GetEnvironmentVariable("LL2_API_KEY");

        // Public API client for the Launch Library 2 API
        client = new HttpClient();
        client.BaseAddress = new Uri("https://ll.thespacedevs.com/");
        client.Timeout = new TimeSpan(0, 0, 30);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        // TODO - DO NOT PUSH and move this to environment variables
        client.DefaultRequestHeaders.Add("Authorization", $"Token {apiKey}");

        // Initialise the weather query class
        weatherQuery = new WeatherQuery();

    }

    // Gets the launch data from the Launch Library 2 API
    // See https://ll.thespacedevs.com/docs/#/launches/launches_previous_list for reference
    public async Task LaunchAPIRetrieval()
    {

        // Checks if the worker is still in use
        while (isBusy)
        {
            Log.Information("API retrieval is busy");
        }

        isBusy = true;

        // Loops through the API to get all the launch data using offset
        while (true)
        {
            // Test number of offset
            Console.WriteLine("Offset No: " + offsetNo);

            // Checks if offset number is greater than 500
            if (offsetNo > 500)
            {
                Log.Information("API retrieval completed");
                break;
            }

            // Iterate through the status codes for the API calls to get more balanced status responses for the dataset
            foreach (int statusCode in statusCodes)
            {
                // Log the status code being used
                Log.Information("Status Code: {StatusCode}", statusCode);

                // Actual API connection
                // TODO - Fix timeout issue with API connection
                HttpResponseMessage response = await client.GetAsync($"/2.3.0/launches/previous/?status={statusCode}&limit=100&offset={offsetNo}");

                // Checks if a successful response code has been outputted for the connection
                if (response.IsSuccessStatusCode)
                {
                    Log.Information("API retrieval successful");

                    // Reads the JSON content of the API response and converts to launch object
                    string res = await response.Content.ReadAsStringAsync();

                    // Set up options for deserialising JSON that avoids case sensitivity
                    JsonSerializerOptions? options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    LaunchObject? launch = System.Text.Json.JsonSerializer.Deserialize<LaunchObject>(res, options);
                    using (var context = new PostgresV1Context())
                    {
                        // Ensure the database is created
                        await context.Database.EnsureCreatedAsync();

                        // Checks if the launch object is null or empty
                        if (launch == null || launch.Results.Count == 0)
                        {
                            Log.Information("No results");
                            continue;
                        }

                        // Loop through each launch in the launch object
                        for (int i = 0; i < launch.Results.Count; i++)
                        {

                            // Get relevant data from the launch object
                            var item = launch.Results[i];

                            // Query weather api with launch data date and location
                            AverageWeatherMetrics? weather = weatherQuery.GetWeather(item.WindowStart, item.WindowEnd, item.Pad.Latitude, item.Pad.Longitude);

                            if (weather != null)
                            {
                                try
                                {
                                    // Get the Rocket Configuration data from stored config url
                                    var confRes = await client.GetAsync(item.Rocket.Configuration.Url);

                                    string confJSON = await confRes.Content.ReadAsStringAsync();

                                    // Deserialise the JSON into a Rocket Configuration object
                                    RocketConfiguration? rocketConfig = JsonConvert.DeserializeObject<RocketConfiguration>(confJSON);

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

                                        // Weather Params
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
                                        Temperature180m = weather.Temperature180m,

                                        // Launch Pad and Rocket Params
                                        CelestialBodyDiameter = item.Pad.Location.CelestialBody.Diameter,
                                        CelestialBodyMass = item.Pad.Location.CelestialBody.Mass,
                                        CelestialBodyGravity = item.Pad.Location.CelestialBody.Gravity,
                                        SuccessfulPadLaunches = item.Pad.Location.CelestialBody.SuccessfulLaunches,
                                        FailedPadLaunches = item.Pad.Location.CelestialBody.FailedLaunches,

                                        // Properties from Rocket Configuration Query
                                        ToThrust = rocketConfig.ToThrust,
                                        LaunchMass = rocketConfig.LaunchMass,
                                        RocketLength = rocketConfig.Length,
                                        RocketDiameter = rocketConfig.Diameter,
                                        SuccessfulRocketLaunches = rocketConfig.SuccessfulLaunches,
                                        FailedRocketLaunches = rocketConfig.FailedLaunches

                                    };

                                    // Attempt to add the launch entry to the database
                                    try
                                    {
                                        Console.WriteLine("Adding new launch entry to database");

                                        // Attempts to add the launch entry to the database
                                        context.Set<LaunchEntry>().Add(launchEntry);
                                        await context.SaveChangesAsync();
                                    }
                                    catch (DbUpdateException ex)
                                    {
                                        // TODO - Edit Logging to be more specific
                                        Log.Error(ex.Message, "Error adding launch entry to database");
                                        throw;
                                    }

                                }
                                catch (Exception e)
                                {
                                    Log.Error(e.Message, "Error while creating launch entry");
                                }
                            }

                        }
                    }

                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Log.Error("API retrieval failed due to not found");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    Log.Error("API retrieval failed due to too many requests");
                    Thread.Sleep(5000);
                    break;
                }
                else
                {
                    Log.Error("API retrieval failed or timed out");
                    Thread.Sleep(10000);
                    break;
                }
            }

            // Increment the offset number for the next API request
            offsetNo += 100;

        }

        // Set the worker to no longer be busy
        isBusy = false;

    }

}