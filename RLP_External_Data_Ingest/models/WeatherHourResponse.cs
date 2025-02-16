using System.Text.Json.Serialization;

public class WeatherHourResponse
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public List<double?> Temperature2m { get; set; }

    [JsonPropertyName("rain")]
    public List<double?> Rain { get; set; }

    [JsonPropertyName("showers")]
    public List<double?> Showers { get; set; }

    [JsonPropertyName("snowfall")]
    public List<double?> Snowfall { get; set; }

    [JsonPropertyName("cloud_cover")]
    public List<double?> CloudCover { get; set; }

    [JsonPropertyName("cloud_cover_low")]
    public List<double?> CloudCoverLow { get; set; }

    [JsonPropertyName("cloud_cover_mid")]
    public List<double?> CloudCoverMid { get; set; }

    [JsonPropertyName("cloud_cover_high")]
    public List<double?> CloudCoverHigh { get; set; }

    [JsonPropertyName("visibility")]
    public List<double?> Visibility { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public List<double?> WindSpeed10m { get; set; }

    [JsonPropertyName("wind_speed_80m")]
    public List<double?> WindSpeed80m { get; set; }

    [JsonPropertyName("wind_speed_120m")]
    public List<double?> WindSpeed120m { get; set; }

    [JsonPropertyName("wind_speed_180m")]
    public List<double?> WindSpeed180m { get; set; }

    [JsonPropertyName("temperature_80m")]
    public List<double?> Temperature80m { get; set; }

    [JsonPropertyName("temperature_120m")]
    public List<double?> Temperature120m { get; set; }

    [JsonPropertyName("temperature_180m")]
    public List<double?> Temperature180m { get; set; }

}