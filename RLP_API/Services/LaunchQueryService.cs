using RLP_API.Models;
using RLP_DB.Models;
using RLP_DB.Contexts;
using RLP_API.Enums;
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

        // Get current launch data from the database
        public async Task<List<LaunchEntry>> GetLaunchDataAsync(LaunchQuery launchQuery)
        {
            try
            {
                // Fetch the launch entries from the DB
                using (var context = new PostgresV1Context())
                {

                    // Query the DB
                    DbSet<LaunchEntry> launchEntries = context.LaunchEntries;

                    List<LaunchEntry> launchEntriesFiltered = new List<LaunchEntry>();

                    if (launchQuery.LaunchId != null)
                    {
                        // Filter the query based on the launchQuery object
                        launchEntriesFiltered = await launchEntries.Where(launch => launch.Id == launchQuery.LaunchId.ToString()).ToListAsync();
                    }
                    if (launchQuery.LaunchStart != null && launchQuery.LaunchEnd != null)
                    {
                        // Filter the query based on the launchQuery object
                        launchEntriesFiltered = await launchEntries.Where(launch => DateTime.Parse(launch.LaunchStart) >= launchQuery.LaunchStart && DateTime.Parse(launch.LaunchEnd) <= launchQuery.LaunchEnd).ToListAsync();
                    }
                    if (launchQuery.LaunchSiteLongitude != null && launchQuery.LaunchSiteLatitude != null)
                    {
                        // Filter the query based on the launchQuery object
                        launchEntriesFiltered = await launchEntries.Where(launch => launch.LaunchLongitude.ToString() == launchQuery.LaunchSiteLongitude && launch.LaunchLatitude.ToString() == launchQuery.LaunchSiteLatitude).ToListAsync();
                    }
                    if (launchQuery.IsSuccessful != null)
                    {
                        // Processes status input using static class
                        string status = LaunchStatusFunctions.GetLaunchStatus(launchQuery.IsSuccessful ?? default(LaunchStatus));

                        // Filter the query based on the launchQuery object
                        launchEntriesFiltered = await launchEntries.Where(launch => launch.Status == status).ToListAsync();
                    }

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