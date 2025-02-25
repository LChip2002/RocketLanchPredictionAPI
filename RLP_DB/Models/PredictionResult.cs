using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RLP_DB.Models;

public partial class PredictionResult
{
    public Guid? ResultId { get; set; }

    public string ModelName { get; set; } = null!;

    public string? ModelPrediction { get; set; }

    public double? Accuracy { get; set; }

    public double? Loss { get; set; }

    public double? Precision { get; set; }

    public double? Recall { get; set; }

    public double? F1Score { get; set; }

    public virtual ICollection<LaunchPrediction> LaunchPredictions { get; set; } = new List<LaunchPrediction>();
}
