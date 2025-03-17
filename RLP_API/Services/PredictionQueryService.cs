using RLP_API.Models;
using RLP_DB.Models;
using RLP_DB.Contexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;

namespace RLP_API.Services
{
    public class PredictionQueryService
    {
        // Constructor
        public PredictionQueryService() { }

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
                    var predictions = context.LaunchPredictions;
                    var predictionResults = context.PredictionResults;

                    List<LaunchPrediction> predictionsFiltered = new List<LaunchPrediction>();

                    if (predQuery.Id != null)
                    {
                        // Filter the query based on the launchQuery object
                        predictionsFiltered = await predictions.Where(launch => launch.PredictionId == predQuery.Id).ToListAsync();

                        // Get the results using the returned result IDs
                        foreach (var prediction in predictionsFiltered)
                        {
                            prediction.Result = await predictionResults.Where(result => result.ResultId == prediction.ResultId).FirstOrDefaultAsync();
                        }
                    }

                    // Filter for weather parameters
                    if (predQuery.WeatherParams != null)
                    {
                        if (predQuery.WeatherParams.Rain != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Rain == predQuery.WeatherParams.Rain).ToList();
                        }
                        if (predQuery.WeatherParams.Showers != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Showers == predQuery.WeatherParams.Showers).ToList();
                        }
                        if (predQuery.WeatherParams.Snowfall != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Snowfall == predQuery.WeatherParams.Snowfall).ToList();
                        }
                        if (predQuery.WeatherParams.CloudCover != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).CloudCover == predQuery.WeatherParams.CloudCover).ToList();
                        }
                        if (predQuery.WeatherParams.CloudCoverLow != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).CloudCoverLow == predQuery.WeatherParams.CloudCoverLow).ToList();
                        }
                        if (predQuery.WeatherParams.CloudCoverMid != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).CloudCoverMid == predQuery.WeatherParams.CloudCoverMid).ToList();
                        }
                        if (predQuery.WeatherParams.CloudCoverHigh != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).CloudCoverHigh == predQuery.WeatherParams.CloudCoverHigh).ToList();
                        }
                        if (predQuery.WeatherParams.Visibility != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Visibility == predQuery.WeatherParams.Visibility).ToList();
                        }
                        if (predQuery.WeatherParams.Temperature != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Temperature == predQuery.WeatherParams.Temperature).ToList();
                        }
                        if (predQuery.WeatherParams.WindSpeed10m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).WindSpeed10m == predQuery.WeatherParams.WindSpeed10m).ToList();
                        }
                        if (predQuery.WeatherParams.WindSpeed80m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).WindSpeed80m == predQuery.WeatherParams.WindSpeed80m).ToList();
                        }
                        if (predQuery.WeatherParams.WindSpeed120m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).WindSpeed120m == predQuery.WeatherParams.WindSpeed120m).ToList();
                        }
                        if (predQuery.WeatherParams.WindSpeed180m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).WindSpeed180m == predQuery.WeatherParams.WindSpeed180m).ToList();
                        }
                        if (predQuery.WeatherParams.Temperature80m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Temperature80m == predQuery.WeatherParams.Temperature80m).ToList();
                        }
                        if (predQuery.WeatherParams.Temperature120m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Temperature120m == predQuery.WeatherParams.Temperature120m).ToList();
                        }
                        if (predQuery.WeatherParams.Temperature180m != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<WeatherParameters>(launch.ParamsWeather).Temperature180m == predQuery.WeatherParams.Temperature180m).ToList();
                        }

                        // Get the results using the returned result IDs
                        foreach (var prediction in predictionsFiltered)
                        {
                            prediction.Result = await predictionResults.Where(result => result.ResultId == prediction.ResultId).FirstOrDefaultAsync();
                        }

                    }

                    // Filter for rocket parameters
                    if (predQuery.RocketParams != null)
                    {
                        if (predQuery.RocketParams.ToThrust != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<RocketParameters>(launch.ParamsRocket).ToThrust == predQuery.RocketParams.ToThrust).ToList();
                        }
                        if (predQuery.RocketParams.LaunchMass != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<RocketParameters>(launch.ParamsRocket).LaunchMass == predQuery.RocketParams.LaunchMass).ToList();
                        }
                        if (predQuery.RocketParams.RocketLength != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<RocketParameters>(launch.ParamsRocket).RocketLength == predQuery.RocketParams.RocketLength).ToList();
                        }
                        if (predQuery.RocketParams.RocketDiameter != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<RocketParameters>(launch.ParamsRocket).RocketDiameter == predQuery.RocketParams.RocketDiameter).ToList();
                        }
                        if (predQuery.RocketParams.SuccessfulRocketLaunches != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<RocketParameters>(launch.ParamsRocket).SuccessfulRocketLaunches == predQuery.RocketParams.SuccessfulRocketLaunches).ToList();
                        }
                        if (predQuery.RocketParams.FailedRocketLaunches != null)
                        {
                            // Filter the query based on the launchQuery object
                            predictionsFiltered = predictions.AsEnumerable().Where(launch => JsonConvert.DeserializeObject<RocketParameters>(launch.ParamsRocket).FailedRocketLaunches == predQuery.RocketParams.FailedRocketLaunches).ToList();
                        }

                        // Get the results using the returned result IDs
                        foreach (var prediction in predictionsFiltered)
                        {
                            prediction.Result = await predictionResults.Where(result => result.ResultId == prediction.ResultId).FirstOrDefaultAsync();
                        }
                    }

                    // Return the predictions
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
        /// Make a prediction using the trained ML model and parsed inputs
        /// </summary>
        /// <param name="predQuery"></param>
        /// <returns></returns>
        public async Task<PredictionMakeDto> MakePredictionAsync(PredictionMaker predQuery)
        {
            try
            {

                // Load the model based on the model type
                if (predQuery.ModelType != null)
                {
                    // Attempts to return values from the stored model using a Python Script
                    try
                    {
                        // Processes status input using static class
                        string modelType = ModelTypeFunctions.GetModelType(predQuery.ModelType ?? default(ModelType));

                        // Create dto object to parse to python script
                        PredictionMakerDto dto = new()
                        {
                            RocketParams = predQuery.RocketParams,
                            WeatherParams = predQuery.WeatherParams,
                            ModelType = modelType
                        };

                        // Serialize the dto object
                        string json = JsonConvert.SerializeObject(dto);
                        string escapedJson = "\"" + json.Replace("\"", "\\\"") + "\"";

                        var startInfo = new ProcessStartInfo
                        {
                            FileName = "python3",
                            Arguments = $"\"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ModelLoader.py")}\" {escapedJson}",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        using (var process = new Process { StartInfo = startInfo })
                        {
                            process.Start();
                            string result = await process.StandardOutput.ReadToEndAsync();
                            string error = await process.StandardError.ReadToEndAsync();
                            process.WaitForExit();

                            if (process.ExitCode != 0)
                            {
                                throw new Exception($"Error running ModelLoader.py: {error}");
                            }

                            // Process the result as needed
                            Log.Information(result);

                            ResultDto? resultDto = JsonConvert.DeserializeObject<ResultDto>(result);

                            // Outputs the result to a dto object
                            PredictionMakeDto predictionMakeDto = new()
                            {
                                PredictedStatus = resultDto.Prediction,
                                CreatedAt = DateTime.Now,
                                ParamsRocket = predQuery.RocketParams,
                                ParamsWeather = predQuery.WeatherParams,
                                ModelType = modelType
                            };

                            // Returns the outputted prediction
                            return predictionMakeDto;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                    }
                }

                // Return empty prediction if no model is provided
                return new PredictionMakeDto()
                {
                    PredictedStatus = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                // Return empty prediction if an error occurs
                return new PredictionMakeDto()
                {
                    PredictedStatus = null
                };
            }
        }


    }
}