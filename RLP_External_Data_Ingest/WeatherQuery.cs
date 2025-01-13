using System.Runtime.InteropServices;
using System.Text.Json;

namespace RLP_External_Data_Ingest
{
    public class WeatherQuery
    {
        public WeatherQuery()
        {
        }

        // Method to get the weather data from the weather API using times and location
        public AverageWeatherMetrics GetWeather(string windowStart, string windowEnd, double latitude, double longitude)
        {
            // Convert datetime strings from zulu time to local time
            string start = DateTime.Parse(windowStart).ToString();
            string end = DateTime.Parse(windowEnd).ToString();

            // Create query for API
            //string queryString = $"https://historical-forecast-api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&start_date={start}&end_date={end}&hourly=temperature_2m";
            string queryString = $"https://historical-forecast-api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&start_date={start}&end_date={end}&hourly=temperature_2m,rain,showers,snowfall,cloud_cover,cloud_cover_low,cloud_cover_mid,cloud_cover_high,visibility,wind_speed_10m,wind_speed_80m,wind_speed_120m,wind_speed_180m,temperature_80m,temperature_120m,temperature_180m";

            // Call API with query from Launch data
            using (HttpClient client = new HttpClient())
            {
                //HttpResponseMessage response = client.GetAsync(queryString).Result;

                // TODO - delete test request output when API is connected and working
                // ---------------------------------------------------------------

                // Fake api response
                HttpResponseMessage response = new HttpResponseMessage();

                // Open JSON file and convert to string
                string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestWeatherResponse.json");
                string testString = File.ReadAllText(jsonFilePath);

                // ----------------------------------------------------------------

                if (response.IsSuccessStatusCode || testString != null)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(responseBody);

                    // Set up options for deserialising JSON that avoids case sensitivity
                    JsonSerializerOptions? options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    // Deserialise JSON response into weather object
                    WeatherResponse weather = System.Text.Json.JsonSerializer.Deserialize<WeatherResponse>(testString, options);
                    Console.WriteLine("test");

                    // Filter Results by time to get exact time for weather
                    var hourlyWeather = weather.Hourly;

                    // Get time indexes are between start and end launch window
                    List<int> timeIndexes = new List<int>();
                    for (int i = 0; i < hourlyWeather.Time.Count; i++)
                    {
                        DateTime time = DateTime.Parse(hourlyWeather.Time[i]);
                        if (time >= DateTime.Parse(windowStart) && time <= DateTime.Parse(windowEnd))
                        {
                            timeIndexes.Add(i);
                        }
                    }

                    // Use indexes to get weather data for launch window and calculate average
                    AverageWeatherMetrics avgWeatherData = new AverageWeatherMetrics()
                    {
                        Temperature = 0,
                        Rain = 0,
                        Showers = 0,
                        Snowfall = 0,
                        CloudCover = 0,
                        CloudCoverLow = 0,
                        CloudCoverMid = 0,
                        CloudCoverHigh = 0,
                        Visibility = 0,
                        WindSpeed10m = 0,
                        WindSpeed80m = 0,
                        WindSpeed120m = 0,
                        WindSpeed180m = 0,
                        Temperature80m = 0,
                        Temperature120m = 0,
                        Temperature180m = 0
                    };


                    // Return the weather object
                    return avgWeatherData;
                }
                else
                {
                    // Handle error response
                }
            }

            return new AverageWeatherMetrics();
        }
    }
}