namespace RLP_API.Models
{
    public class PredictionMaker
    {
        // Weather Params
        public WeatherParameters? WeatherParams { get; set; }

        // Rocket Params
        public RocketParameters? RocketParams { get; set; }

        // Model Type
        public ModelType? ModelType { get; set; }
    }
}