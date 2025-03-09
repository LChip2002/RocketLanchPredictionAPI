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
}