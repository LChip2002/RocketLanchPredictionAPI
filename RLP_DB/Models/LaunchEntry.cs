namespace RLP_DB.Models;

public class LaunchEntry
{
    public string Id { get; set; }

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