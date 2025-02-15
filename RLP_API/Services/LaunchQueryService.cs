using RLP_API.Models;
using RLP_DB.Models;
using RLP_DB;
using Microsoft.EntityFrameworkCore;

namespace RLP_API.Services
{
    public class LaunchQueryService
    {
        // Constructor
        public LaunchQueryService()
        {
            // Initialization code here
        }

        // Example method to get launch data
        public async Task<List<LaunchEntry>> GetLaunchDataAsync(LaunchQuery launchQuery)
        {
            try
            {
                // Fetch the launch entries from the DB
                using (var context = new PostgresV1Context())
                {
                    // TODO - Filter the query based on the launchQuery object
                    // Query the DB
                    var launchEntries = await context.LaunchEntries.ToListAsync();

                    // Return the launch entries
                    return launchEntries;
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