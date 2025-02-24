using System;
using System.Collections.Generic;

namespace RLP_DB.Models;

public partial class LaunchPrediction
{
    public Guid PredictionId { get; set; }

    public string? ParamsWeather { get; set; }

    public string? ParamsRocket { get; set; }

    public Guid? ResultId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual PredictionResult? Result { get; set; }
}
