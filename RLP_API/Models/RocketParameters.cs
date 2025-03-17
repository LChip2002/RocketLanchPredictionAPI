namespace RLP_API.Models
{
    public class RocketParameters
    {
        public double? ToThrust { get; set; } = 0;
        public double? LaunchMass { get; set; } = 0;
        public double? RocketLength { get; set; } = 0;
        public double? RocketDiameter { get; set; } = 0;
        public int? SuccessfulRocketLaunches { get; set; } = 0;
        public int? FailedRocketLaunches { get; set; } = 0;
    }
}