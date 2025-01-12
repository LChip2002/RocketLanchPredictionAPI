using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace RLP_External_Data_Ingest;

public class LaunchDataGet
{
    private readonly HttpClient client;

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

    }

    // Gets the launch data from the Launch Library 2 API
    // See https://ll.thespacedevs.com/docs/#/launches/launches_previous_list for reference
    public async Task LaunchAPIRetrieval()
    {
        HttpResponseMessage response = await client.GetAsync("/2.3.0/launches/previous/?limit=10&offset=10");

        // Checks if a successful response code has been outputted for the connection
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Success");

            // Reads the JSON content of the API response and converts to launch object
            var res = await response.Content.ReadAsStringAsync();
            var launch = JsonConvert.DeserializeObject<LaunchObject>(res);

            return;
        }

        Console.WriteLine("Fail");
    }

}