using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace RLP_External_Data_Ingest;

public class LaunchDataGet
{
    private readonly HttpClient client;
    private readonly WeatherQuery weatherQuery;

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

            // Loop through each launch in the launch object
            foreach (var item in launch.Results)
            {

                // Get relevant data from the launch object

                // Query weather api with launch data date and location
                var weather = weatherQuery.GetWeather(item.WindowStart, item.WindowEnd, item.Pad.Latitude, item.Pad.Longitude);

                // Combine weather and launch data into new object

                // Injest the data into the database

                // TODO - Set up EF and DB first
            }

            return;
        }

        Console.WriteLine("Fail");
        Thread.Sleep(5000);
    }

}