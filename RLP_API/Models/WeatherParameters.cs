namespace RLP_API.Models
{
    public class WeatherParameters
    {
        public double? Rain { get; set; } = 0;
        public double? Precipitation { get; set; } = 0;
        public double? Snowfall { get; set; } = 0;
        public double? CloudCover { get; set; } = 0;
        public double? CloudCoverLow { get; set; } = 0;
        public double? CloudCoverMid { get; set; } = 0;
        public double? CloudCoverHigh { get; set; } = 0;
        public double? Visibility { get; set; } = 0;
        public double? Temperature { get; set; } = 0;
        public double? WindSpeed10m { get; set; } = 0;
        public double? WindSpeed80m { get; set; } = 0;
        public double? WindSpeed120m { get; set; } = 0;
        public double? WindSpeed180m { get; set; } = 0;
    }
}