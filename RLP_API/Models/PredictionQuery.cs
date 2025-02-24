

namespace RLP_API.Models
{
    // TODO - Edit this to match the launch_predictions entity in the DB
    public class PredictionQuery
    {
        public Guid? Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public double? Temperature { get; set; }

        // Weather Params
        public double? Rain { get; set; }
        public double? Showers { get; set; }
        public double? Snowfall { get; set; }
        public double? CloudCover { get; set; }
        public double? CloudCoverLow { get; set; }
        public double? CloudCoverMid { get; set; }
        public double? CloudCoverHigh { get; set; }
        public double? Visibility { get; set; }
        public double? WindSpeed10m { get; set; }
        public double? WindSpeed80m { get; set; }
        public double? WindSpeed120m { get; set; }
        public double? WindSpeed180m { get; set; }
        public double? Temperature80m { get; set; }
        public double? Temperature120m { get; set; }
        public double? Temperature180m { get; set; }

        // TODO - Add Rocket Params

    }
}