namespace RLP_API.Models
{
    public class PredictionMakerDto
    {
        public WeatherParameters? WeatherParams { get; set; } = new WeatherParameters();
        public RocketParameters? RocketParams { get; set; } = new RocketParameters();
        public string? ModelType { get; set; }
    }
}