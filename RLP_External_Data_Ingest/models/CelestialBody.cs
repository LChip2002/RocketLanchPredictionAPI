using System.Text.Json.Serialization;

public class CelestialBody
{
    public string? ResponseMode { get; set; }
    public int? Id { get; set; }
    public string? Name { get; set; }
    public CelestialBodyType? Type { get; set; }

    [JsonPropertyName("diameter")]
    public double? Diameter { get; set; }
    [JsonPropertyName("mass")]
    public double? Mass { get; set; }
    [JsonPropertyName("gravity")]
    public double? Gravity { get; set; }
    public string? LengthOfDay { get; set; }
    public bool? Atmosphere { get; set; }
    public Image? Image { get; set; }
    public string? Description { get; set; }
    public string? WikiUrl { get; set; }
    public int? TotalAttemptedLaunches { get; set; }
    [JsonPropertyName("successful_launches")]
    public int? SuccessfulLaunches { get; set; }
    [JsonPropertyName("failed_launches")]
    public int? FailedLaunches { get; set; }
    [JsonPropertyName("total_attempted_launches")]
    public int? TotalAttemptedLandings { get; set; }
    public int? SuccessfulLandings { get; set; }
    public int? FailedLandings { get; set; }
}