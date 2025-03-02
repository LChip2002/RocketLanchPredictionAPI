namespace RLP_API.Models
{
    public class PredictionMakerDto
    {
        public WeatherParameters? WeatherParams { get; set; }
        public RocketParameters? RocketParams { get; set; }
        public string? ModelType { get; set; }
    }
}