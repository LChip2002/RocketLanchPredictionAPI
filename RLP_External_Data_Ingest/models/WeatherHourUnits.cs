using System.Text.Json.Serialization;

public class WeatherHourUnits
{
    [JsonPropertyName("time")]
    public string Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public string Temperature2m { get; set; }

    [JsonPropertyName("rain")]
    public string Rain { get; set; }

    [JsonPropertyName("showers")]
    public string Showers { get; set; }

    [JsonPropertyName("snowfall")]
    public string Snowfall { get; set; }

    [JsonPropertyName("cloud_cover")]
    public string CloudCover { get; set; }

    [JsonPropertyName("cloud_cover_low")]
    public string CloudCoverLow { get; set; }

    [JsonPropertyName("cloud_cover_mid")]
    public string CloudCoverMid { get; set; }

    [JsonPropertyName("cloud_cover_high")]
    public string CloudCoverHigh { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public string WindSpeed10m { get; set; }

    [JsonPropertyName("wind_speed_80m")]
    public string WindSpeed80m { get; set; }

    [JsonPropertyName("wind_speed_120m")]
    public string WindSpeed120m { get; set; }

    [JsonPropertyName("wind_speed_180m")]
    public string WindSpeed180m { get; set; }

    [JsonPropertyName("temperature_80m")]
    public string Temperature80m { get; set; }
}