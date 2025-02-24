using RLP_API.Models;
using RLP_DB.Models;
using RLP_DB;
using RLP_DB.Contexts;
using RLP_API.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace RLP_API.Services
{
    public class PredictionQueryService
    {
        // Constructor
        public PredictionQueryService()
        {
            // Initialization code here
        }

        /// <summary>
        /// Get existing launch prediction data from the database
        /// </summary>
        /// <param name="predQuery"></param>
        /// <returns></returns>
        public async Task<List<LaunchPrediction>> GetPredictionDataAsync(PredictionQuery predQuery)
        {
            try
            {
                // Fetch the launch entries from the DB
                using (var context = new PostgresV1Context())
                {
                    // Query the DB
                    DbSet<LaunchPrediction> predictions = context.LaunchPredictions;

                    List<LaunchPrediction> predictionsFiltered = new List<LaunchPrediction>();

                    if (predQuery.Id != null)
                    {
                        // Filter the query based on the launchQuery object
                        predictionsFiltered = await predictions.Where(launch => launch.PredictionId == predQuery.Id).ToListAsync();
                    }

                    // Filter for weather parameters
                    if (predQuery.WeatherParams != null)
                    {
                        if (predQuery.WeatherParams.Rain != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Rain == predQuery.WeatherParams.Rain).ToListAsync();
                        }
                        if (predQuery.WeatherParams.Showers != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Showers == predQuery.WeatherParams.Showers).ToListAsync();
                        }
                        if (predQuery.WeatherParams.Snowfall != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Snowfall == predQuery.WeatherParams.Snowfall).ToListAsync();
                        }
                        if (predQuery.WeatherParams.CloudCover != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).CloudCover == predQuery.WeatherParams.CloudCover).ToListAsync();
                        }
                        if (predQuery.WeatherParams.CloudCoverLow != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).CloudCoverLow == predQuery.WeatherParams.CloudCoverLow).ToListAsync();
                        }
                        if (predQuery.WeatherParams.CloudCoverMid != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).CloudCoverMid == predQuery.WeatherParams.CloudCoverMid).ToListAsync();
                        }
                        if (predQuery.WeatherParams.CloudCoverHigh != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).CloudCoverHigh == predQuery.WeatherParams.CloudCoverHigh).ToListAsync();
                        }
                        if (predQuery.WeatherParams.Visibility != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Visibility == predQuery.WeatherParams.Visibility).ToListAsync();
                        }
                        if (predQuery.WeatherParams.WindSpeed10m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).WindSpeed10m == predQuery.WeatherParams.WindSpeed10m).ToListAsync();
                        }
                        if (predQuery.WeatherParams.WindSpeed80m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).WindSpeed80m == predQuery.WeatherParams.WindSpeed80m).ToListAsync();
                        }
                        if (predQuery.WeatherParams.WindSpeed120m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).WindSpeed120m == predQuery.WeatherParams.WindSpeed120m).ToListAsync();
                        }
                        if (predQuery.WeatherParams.WindSpeed180m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).WindSpeed180m == predQuery.WeatherParams.WindSpeed180m).ToListAsync();
                        }
                        if (predQuery.WeatherParams.Temperature80m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Temperature80m == predQuery.WeatherParams.Temperature80m).ToListAsync();
                        }
                        if (predQuery.WeatherParams.Temperature120m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Temperature120m == predQuery.WeatherParams.Temperature120m).ToListAsync();
                        }
                        if (predQuery.WeatherParams.Temperature180m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = await predictions.Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Temperature180m == predQuery.WeatherParams.Temperature180m).ToListAsync();
                        }

                    }

                    // Filter for rocket parameters
                    if (predQuery.RocketParams != null)
                    {
                        // TODO - Add rocket parameters
                        Console.WriteLine("Rocket parameters not yet implemented");
                    }

                    // Return the launch entries
                    return predictionsFiltered;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<LaunchPrediction>();
            }
        }

        /// <summary>
        /// Make a prediction using the trained ML model
        /// </summary>
        /// <param name="predQuery"></param>
        /// <returns></returns>
        public async Task<List<LaunchEntry>> MakePredictionAsync(PredictionQuery predQuery)
        {
            try
            {
                // Fetch the launch entries from the DB
                using (var context = new PostgresV1Context())
                {
                    // TODO - Change for prediction table
                    // Query the DB
                    DbSet<LaunchEntry> launchEntries = context.LaunchEntries;

                    List<LaunchEntry> launchEntriesFiltered = new List<LaunchEntry>();



                    // Return the launch entries
                    return launchEntriesFiltered;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<LaunchEntry>();
            }
        }


    }
}