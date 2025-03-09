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

        // Reads Launch Library 2 API Key from environment variables
        string? apiKey = Environment.GetEnvironmentVariable("LL2_API_KEY");

        // Public API client for the Launch Library 2 API
        client = new HttpClient();
        client.BaseAddress = new Uri("https://ll.thespacedevs.com/");
        client.Timeout = new TimeSpan(0, 0, 30);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        // Add the API key to the request headers
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

                            // Query weather api with launch data date and location
                            AverageWeatherMetrics? weather = weatherQuery.GetWeather(launch.Results[i].WindowStart, launch.Results[i].WindowEnd, launch.Results[i].Pad.Latitude, launch.Results[i].Pad.Longitude);

                            if (weather != null)
                            {
                                try
                                {
                                    // Get the Rocket Configuration data from stored config url
                                    var confRes = await client.GetAsync(launch.Results[i].Rocket.Configuration.Url);

                                    string confJSON = await confRes.Content.ReadAsStringAsync();

                                    // Deserialise the JSON into a Rocket Configuration object
                                    RocketConfiguration? rocketConfig = System.Text.Json.JsonSerializer.Deserialize<RocketConfiguration>(confJSON, options);

                                    // Combine weather and launch data into new object
                                    LaunchEntry launchEntry = new LaunchEntry()
                                    {
                                        Id = launch.Results[i].Id,
                                        Name = launch.Results[i].Name,
                                        Description = launch.Results[i].Mission?.Description,
                                        Country = launch.Results[i].Pad.Location.Name,
                                        LaunchLatitude = launch.Results[i].Pad.Latitude,
                                        LaunchLongitude = launch.Results[i].Pad.Longitude,
                                        LaunchStart = launch.Results[i].WindowStart,
                                        LaunchEnd = launch.Results[i].WindowEnd,
                                        Status = launch.Results[i].Status.Name,
                                        StatusDescription = launch.Results[i].Status.Description,
                                        Rocket = launch.Results[i].Rocket.Configuration.Name,
                                        Mission = launch.Results[i].Mission.Name,
                                        Image = JsonConvert.SerializeObject(launch.Results[i].Image),

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