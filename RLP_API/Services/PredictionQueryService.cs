using RLP_API.Models;
using RLP_DB.Models;
using RLP_DB;
using RLP_DB.Contexts;
using RLP_API.Enums;
using Microsoft.EntityFrameworkCore;

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
                    // TODO - Change for prediction table after DB migration
                    // Query the DB
                    DbSet<LaunchPrediction> predictions = context.LaunchPredictions;

                    List<LaunchPrediction> predictionsFiltered = new List<LaunchPrediction>();

                    if (predQuery.Id != null)
                    {
                        // Filter the query based on the launchQuery object
                        predictionsFiltered = await predictions.Where(launch => launch.PredictionId == predQuery.Id).ToListAsync();
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