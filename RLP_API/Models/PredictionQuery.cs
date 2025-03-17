
namespace RLP_API.Models
{
    public class PredictionQuery
    {
        public Guid? Id { get; set; }
        public DateTime CreatedDateTime { get; set; }

        // Weather Params
        public WeatherParameters? WeatherParams { get; set; }

        // Rocket Params
        public RocketParameters? RocketParams { get; set; }

    }
}