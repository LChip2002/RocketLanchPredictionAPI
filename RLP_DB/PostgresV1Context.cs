using Microsoft.EntityFrameworkCore;
using RLP_DB.Models;

namespace RLP_DB;

public class PostgresV1Context : Microsoft.EntityFrameworkCore.DbContext
{
    public PostgresV1Context()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO - Move to appsettings.json
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LaunchEntry>()
            .HasKey(e => e.Id); // Ensure Id is the primary key
    }

    public DbSet<LaunchEntry> LaunchEntries { get; set; }

}
