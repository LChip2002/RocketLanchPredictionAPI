using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RLP_DB.Models;

namespace RLP_DB.Contexts;

public partial class PostgresV1Context : DbContext
{
    public PostgresV1Context()
    {
    }

    public PostgresV1Context(DbContextOptions<PostgresV1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<LaunchEntry> LaunchEntries { get; set; }

    public virtual DbSet<LaunchPrediction> LaunchPredictions { get; set; }

    public virtual DbSet<PredictionResult> PredictionResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres; Port= 5432;Username=postgres;Password=postgres;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LaunchEntry>(entity =>
        {
            entity.ToTable("launch_entries");
        });

        modelBuilder.Entity<LaunchPrediction>(entity =>
        {
            entity.HasKey(e => e.PredictionId).HasName("launch_predictions_pkey");

            entity.ToTable("launch_predictions");

            entity.Property(e => e.PredictionId)
                .ValueGeneratedNever()
                .HasColumnName("prediction_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ParamsRocket)
                .HasColumnType("jsonb")
                .HasColumnName("params_rocket");
            entity.Property(e => e.ParamsWeather)
                .HasColumnType("jsonb")
                .HasColumnName("params_weather");
            entity.Property(e => e.ResultId).HasColumnName("result_id");

            entity.HasOne(d => d.Result).WithMany(p => p.LaunchPredictions)
                .HasForeignKey(d => d.ResultId)
                .HasConstraintName("launch_predictions_result_id_fkey");
        });

        modelBuilder.Entity<PredictionResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("prediction_results_pkey");

            entity.ToTable("prediction_results");

            entity.Property(e => e.ResultId)
                .ValueGeneratedNever()
                .HasColumnName("result_id");
            entity.Property(e => e.Accuracy).HasColumnName("accuracy");
            entity.Property(e => e.F1Score).HasColumnName("f1_score");
            entity.Property(e => e.Loss).HasColumnName("loss");
            entity.Property(e => e.ModelName).HasColumnName("model_name");
            entity.Property(e => e.ModelPrediction).HasColumnName("model_prediction");
            entity.Property(e => e.Precision).HasColumnName("precision");
            entity.Property(e => e.Recall).HasColumnName("recall");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
