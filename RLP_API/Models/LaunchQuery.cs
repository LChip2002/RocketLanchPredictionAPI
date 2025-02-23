using RLP_API.Enums;

namespace RLP_API.Models
{
    public class LaunchQuery
    {
        public Guid? LaunchId { get; set; }
        public string? MissionName { get; set; }
        public DateTime? LaunchStart { get; set; }
        public DateTime? LaunchEnd { get; set; }
        public string? LaunchSiteLongitude { get; set; }
        public string? LaunchSiteLatitude { get; set; }
        public string? RocketType { get; set; }
        public LaunchStatus? IsSuccessful { get; set; }
    }
}