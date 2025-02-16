using System.Runtime.InteropServices;
using System.Text.Json;
using Serilog;

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
            // Convert datetime strings from zulu time to local date only
            string start = DateTime.Parse(windowStart).ToString("yyyy-MM-dd");
            string end = DateTime.Parse(windowEnd).ToString("yyyy-MM-dd");

            // Create query for API
            string queryString = $"https://historical-forecast-api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&start_date={start}&end_date={end}&hourly=temperature_2m,rain,showers,snowfall,cloud_cover,cloud_cover_low,cloud_cover_mid,cloud_cover_high,visibility,wind_speed_10m,wind_speed_80m,wind_speed_120m,wind_speed_180m,temperature_80m,temperature_120m,temperature_180m";

            // Call API with query from Launch data
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(queryString).Result;

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;

                        // Set up options for deserialising JSON that avoids case sensitivity
                        JsonSerializerOptions? options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        // Deserialise JSON response into weather object
                        WeatherResponse weather = System.Text.Json.JsonSerializer.Deserialize<WeatherResponse>(responseBody, options);

                        // Filter Results by time to get exact time for weather
                        WeatherHourResponse hourlyWeather = weather.Hourly;

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
                        // Assigns default value of 0 if no data is found between date thresholds
                        var avgWeatherData = new AverageWeatherMetrics
                        {
                            Temperature = timeIndexes.Select(i => hourlyWeather.Temperature2m[i]).DefaultIfEmpty(0).Average(),
                            Rain = timeIndexes.Select(i => hourlyWeather.Rain[i]).DefaultIfEmpty(0).Average(),
                            Showers = timeIndexes.Select(i => hourlyWeather.Showers[i]).DefaultIfEmpty(0).Average(),
                            Snowfall = timeIndexes.Select(i => hourlyWeather.Snowfall[i]).DefaultIfEmpty(0).Average(),
                            CloudCover = timeIndexes.Select(i => hourlyWeather.CloudCover[i]).DefaultIfEmpty(0).Average(),
                            CloudCoverLow = timeIndexes.Select(i => hourlyWeather.CloudCoverLow[i]).DefaultIfEmpty(0).Average(),
                            CloudCoverMid = timeIndexes.Select(i => hourlyWeather.CloudCoverMid[i]).DefaultIfEmpty(0).Average(),
                            CloudCoverHigh = timeIndexes.Select(i => hourlyWeather.CloudCoverHigh[i]).DefaultIfEmpty(0).Average(),
                            Visibility = timeIndexes.Select(i => hourlyWeather.Visibility[i]).DefaultIfEmpty(0).Average(),
                            WindSpeed10m = timeIndexes.Select(i => hourlyWeather.WindSpeed10m[i]).DefaultIfEmpty(0).Average(),
                            WindSpeed80m = timeIndexes.Select(i => hourlyWeather.WindSpeed80m[i]).DefaultIfEmpty(0).Average(),
                            WindSpeed120m = timeIndexes.Select(i => hourlyWeather.WindSpeed120m[i]).DefaultIfEmpty(0).Average(),
                            WindSpeed180m = timeIndexes.Select(i => hourlyWeather.WindSpeed180m[i]).DefaultIfEmpty(0).Average(),
                            Temperature80m = timeIndexes.Select(i => hourlyWeather.Temperature80m[i]).DefaultIfEmpty(0).Average(),
                            Temperature120m = timeIndexes.Select(i => hourlyWeather.Temperature120m[i]).DefaultIfEmpty(0).Average(),
                            Temperature180m = timeIndexes.Select(i => hourlyWeather.Temperature180m[i]).DefaultIfEmpty(0).Average()
                        };

                        // Return the weather object
                        return avgWeatherData;
                    }
                    catch (Exception e)
                    {
                        Log.Information("Error: " + e.Message);
                        Console.WriteLine("Error: " + e.Message);
                    }

                }
                else
                {
                    // Handle error response
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }

            return new AverageWeatherMetrics();
        }
    }
}