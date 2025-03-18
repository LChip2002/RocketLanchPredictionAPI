using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RLP_DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWeather : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Temperature120m",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "Temperature180m",
                table: "launch_entries");

            migrationBuilder.DropColumn(
                name: "Temperature80m",
                table: "launch_entries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Temperature120m",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Temperature180m",
                table: "launch_entries",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Temperature80m",
                table: "launch_entries",
                type: "double precision",
                nullable: true);
        }
    }
}
