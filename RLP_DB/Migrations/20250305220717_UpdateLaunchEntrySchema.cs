using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RLP_DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLaunchEntrySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LaunchEntries",
                table: "LaunchEntries");

            migrationBuilder.RenameTable(
                name: "LaunchEntries",
                newName: "launch_entries");

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
                name: "FailedRocketLaunches",
                table: "launch_entries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LaunchMass",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RocketDiameter",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RocketLength",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuccessfulPadLaunches",
                table: "launch_entries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuccessfulRocketLaunches",
                table: "launch_entries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ToThrust",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_launch_entries",
                table: "launch_entries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "prediction_results",
                columns: table => new
                {
                    result_id = table.Column<Guid>(type: "uuid", nullable: false),
                    model_name = table.Column<string>(type: "text", nullable: false),
                    model_prediction = table.Column<string>(type: "text", nullable: true),
                    accuracy = table.Column<double>(type: "double precision", nullable: true),
                    loss = table.Column<double>(type: "double precision", nullable: true),
                    precision = table.Column<double>(type: "double precision", nullable: true),
                    recall = table.Column<double>(type: "double precision", nullable: true),
                    f1_score = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("prediction_results_pkey", x => x.result_id);
                });

            migrationBuilder.CreateTable(
                name: "launch_predictions",
                columns: table => new
                {
                    prediction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    params_weather = table.Column<string>(type: "jsonb", nullable: true),
                    params_rocket = table.Column<string>(type: "jsonb", nullable: true),
                    result_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("launch_predictions_pkey", x => x.prediction_id);
                    table.ForeignKey(
                        name: "launch_predictions_result_id_fkey",
                        column: x => x.result_id,
                        principalTable: "prediction_results",
                        principalColumn: "result_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_launch_predictions_result_id",
                table: "launch_predictions",
                column: "result_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "launch_predictions");

            migrationBuilder.DropTable(
                name: "prediction_results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_launch_entries",
                table: "launch_entries");

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
                name: "FailedRocketLaunches",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "LaunchMass",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "RocketDiameter",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "RocketLength",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "SuccessfulPadLaunches",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "SuccessfulRocketLaunches",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "ToThrust",
                table: "launch_entries");

            migrationBuilder.RenameTable(
                name: "launch_entries",
                newName: "LaunchEntries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaunchEntries",
                table: "LaunchEntries",
                column: "Id");
        }
    }
}
