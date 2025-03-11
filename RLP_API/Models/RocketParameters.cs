namespace RLP_API.Models
{
    public class RocketParameters
    {
        public double? ToThrust { get; set; }
        public double? LaunchMass { get; set; }
        public double? RocketLength { get; set; }
        public double? RocketDiameter { get; set; }
        public int? SuccessfulRocketLaunches { get; set; }
        public int? FailedRocketLaunches { get; set; }
    }
}