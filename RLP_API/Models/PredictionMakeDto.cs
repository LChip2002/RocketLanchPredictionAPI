namespace RLP_API.Models
{
    public class PredictionMakeDto
    {
        public WeatherParameters? ParamsWeather { get; set; }
        public RocketParameters? ParamsRocket { get; set; }
        public string? ModelType { get; set; }
        public string? PredictedStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}