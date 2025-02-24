
namespace RLP_API.Models
{
    // TODO - Edit this to match the launch_predictions entity in the DB
    public class PredictionQuery
    {
        public Guid? Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public double? Temperature { get; set; }

        // Weather Params
        public WeatherParameters? WeatherParams { get; set; }

        // Rocket Params
        public RocketParameters? RocketParams { get; set; }

    }
}