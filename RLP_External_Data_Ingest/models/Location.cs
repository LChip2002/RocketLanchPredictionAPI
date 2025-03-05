using System;
using System.Text.Json.Serialization;

namespace RLP_External_Data_Ingest.Models
{
    public class Location
    {
        public string? ResponseMode { get; set; }
        public int? Id { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }

        [JsonPropertyName("celestial_body")]
        public CelestialBody? CelestialBody { get; set; }
        public bool? Active { get; set; }
        public Country? Country { get; set; }
        public string? Description { get; set; }
        public Image? Image { get; set; }
        public string? MapImage { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? TimezoneName { get; set; }
        public int? TotalLaunchCount { get; set; }
        public int? TotalLandingCount { get; set; }
    }

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

    public class CelestialBodyType
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }

    public class Country
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Alpha2Code { get; set; }
        public string? Alpha3Code { get; set; }
        public string? NationalityName { get; set; }
        public string? NationalityNameComposed { get; set; }
    }

    public class Image
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Credit { get; set; }
        public License? License { get; set; }
        public bool? SingleUse { get; set; }
        public string[]? Variants { get; set; }
    }

    public class License
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? Priority { get; set; }
        public string? Link { get; set; }
    }
}