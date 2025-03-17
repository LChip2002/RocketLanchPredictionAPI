namespace RLP_API.Models
{
    public class PredictionMaker
    {
        // Weather Params
        public WeatherParameters? WeatherParams { get; set; } = new WeatherParameters();

        // Rocket Params
        public RocketParameters? RocketParams { get; set; } = new RocketParameters();

        // Model Type
        public ModelType? ModelType { get; set; }
    }
}