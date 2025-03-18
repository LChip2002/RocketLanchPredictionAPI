using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RLP_DB.Migrations
{
    /// <inheritdoc />
    public partial class LaunchWeather : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CelestialBodyDiameter",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "CelestialBodyGravity",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "CelestialBodyMass",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "FailedPadLaunches",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "SuccessfulPadLaunches",
                table: "launch_entries");

            migrationBuilder.RenameColumn(
                name: "Showers",
                table: "launch_entries",
                newName: "Precipitation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Precipitation",
                table: "launch_entries",
                newName: "Showers");

            migrationBuilder.AddColumn<double>(
                name: "CelestialBodyDiameter",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CelestialBodyGravity",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CelestialBodyMass",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailedPadLaunches",
                table: "launch_entries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuccessfulPadLaunches",
                table: "launch_entries",
                type: "integer",
                nullable: true);
        }
    }
}
