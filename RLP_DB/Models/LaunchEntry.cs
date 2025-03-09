using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RLP_DB.Models;

public partial class LaunchEntry
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Country { get; set; }

    public double? LaunchLatitude { get; set; }

    public double? LaunchLongitude { get; set; }

    public string? LaunchStart { get; set; }

    public string? LaunchEnd { get; set; }

    public string? Status { get; set; }

    public string? StatusDescription { get; set; }

    public string? Rocket { get; set; }

    public string? Mission { get; set; }

    public string? Image { get; set; }

    // --------------------------------
    // Rocket Params that are retrieved from the Rocket Configuration
    // --------------------------------
    public double? ToThrust { get; set; }
    public double? LaunchMass { get; set; }
    public double? RocketLength { get; set; }
    public double? RocketDiameter { get; set; }
    public int? SuccessfulRocketLaunches { get; set; }
    public int? FailedRocketLaunches { get; set; }

    // --------------------------------
    // Weather Params
    // --------------------------------
    public double? Temperature { get; set; }

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
}
