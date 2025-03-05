using System.Text.Json.Serialization;

namespace RLP_External_Data_Ingest.Models
{
    public class RocketConfiguration
    {
        public string? ResponseMode { get; set; }
        public int? Id { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public List<Family>? Families { get; set; }
        public string? FullName { get; set; }
        public string? Variant { get; set; }
        public bool? Active { get; set; }
        public bool? IsPlaceholder { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public List<Program>? Program { get; set; }
        public bool? Reusable { get; set; }
        public Image? Image { get; set; }
        public string? InfoUrl { get; set; }
        public string? WikiUrl { get; set; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public int? MinStage { get; set; }
        public int? MaxStage { get; set; }
        [JsonPropertyName("length")]
        public double? Length { get; set; }
        [JsonPropertyName("diameter")]
        public double? Diameter { get; set; }
        public string? MaidenFlight { get; set; }
        public int? LaunchCost { get; set; }
        [JsonPropertyName("launch_mass")]
        public double? LaunchMass { get; set; }
        public double? LeoCapacity { get; set; }
        public double? GtoCapacity { get; set; }
        public double? GeoCapacity { get; set; }
        public double? SsoCapacity { get; set; }
        [JsonPropertyName("to_thrust")]
        public double? ToThrust { get; set; }
        public double? Apogee { get; set; }
        public int? TotalLaunchCount { get; set; }
        public int? ConsecutiveSuccessfulLaunches { get; set; }
        [JsonPropertyName("successful_launches")]
        public int? SuccessfulLaunches { get; set; }
        [JsonPropertyName("failed_launches")]
        public int? FailedLaunches { get; set; }
        public int? PendingLaunches { get; set; }
        public int? AttemptedLandings { get; set; }
        public int? SuccessfulLandings { get; set; }
        public int? FailedLandings { get; set; }
        public int? ConsecutiveSuccessfulLandings { get; set; }
        public string? FastestTurnaround { get; set; }
    }

    public class Family
    {
        public string? ResponseMode { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<Manufacturer>? Manufacturer { get; set; }
        public object? Parent { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }
        public string? MaidenFlight { get; set; }
        public int? TotalLaunchCount { get; set; }
        public int? ConsecutiveSuccessfulLaunches { get; set; }
        public int? SuccessfulLaunches { get; set; }
        public int? FailedLaunches { get; set; }
        public int? PendingLaunches { get; set; }
        public int? AttemptedLandings { get; set; }
        public int? SuccessfulLandings { get; set; }
        public int? FailedLandings { get; set; }
        public int? ConsecutiveSuccessfulLandings { get; set; }
    }

    public class Manufacturer
    {
        public string? ResponseMode { get; set; }
        public int? Id { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Abbrev { get; set; }
        public Type? Type { get; set; }
        public bool? Featured { get; set; }
        public List<Country>? Country { get; set; }
        public string? Description { get; set; }
        public string? Administrator { get; set; }
        public int? FoundingYear { get; set; }
        public string? Launchers { get; set; }
        public string? Spacecraft { get; set; }
        public object? Parent { get; set; }
        public Image? Image { get; set; }
        public Image? Logo { get; set; }
        public Image? SocialLogo { get; set; }
        public int? TotalLaunchCount { get; set; }
        public int? ConsecutiveSuccessfulLaunches { get; set; }
        public int? SuccessfulLaunches { get; set; }
        public int? FailedLaunches { get; set; }
        public int? PendingLaunches { get; set; }
        public int? ConsecutiveSuccessfulLandings { get; set; }
        public int? SuccessfulLandings { get; set; }
        public int? FailedLandings { get; set; }
        public int? AttemptedLandings { get; set; }
        public int? SuccessfulLandingsSpacecraft { get; set; }
        public int? FailedLandingsSpacecraft { get; set; }
        public int? AttemptedLandingsSpacecraft { get; set; }
        public int? SuccessfulLandingsPayload { get; set; }
        public int? FailedLandingsPayload { get; set; }
        public int? AttemptedLandingsPayload { get; set; }
        public string? InfoUrl { get; set; }
        public string? WikiUrl { get; set; }
        public List<object>? SocialMediaLinks { get; set; }
    }

    public class Program
    {
        public string? ResponseMode { get; set; }
        public int? Id { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public Image? Image { get; set; }
        public string? InfoUrl { get; set; }
        public string? WikiUrl { get; set; }
        public string? Description { get; set; }
        public List<Agency>? Agencies { get; set; }
        public string? StartDate { get; set; }
        public object? EndDate { get; set; }
        public List<object>? MissionPatches { get; set; }
        public Type? Type { get; set; }
    }

    public class Agency
    {
        public string? ResponseMode { get; set; }
        public int? Id { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Abbrev { get; set; }
        public Type? Type { get; set; }
    }

    public class Type
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
}