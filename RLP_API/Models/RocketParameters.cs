namespace RLP_API.Models
{
    public class RocketParameters
    {
        public double? ToThrust { get; set; } = 0;
        public double? LaunchMass { get; set; } = 0;
        public double? RocketLength { get; set; } = 0;
        public double? RocketDiameter { get; set; } = 0;
        public double? SuccessfulRocketLaunches { get; set; } = 0;
        public double? FailedRocketLaunches { get; set; } = 0;
    }
}