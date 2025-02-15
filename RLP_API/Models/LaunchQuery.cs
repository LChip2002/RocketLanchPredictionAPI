namespace RLP_API.Models
{
    public class LaunchQuery
    {
        public int? LaunchId { get; set; }
        public string? MissionName { get; set; }
        public DateTime? LaunchStart { get; set; }
        public DateTime? LaunchEnd { get; set; }
        public string? LaunchSite { get; set; }
        public string? RocketType { get; set; }
        public bool? IsSuccessful { get; set; }
    }
}