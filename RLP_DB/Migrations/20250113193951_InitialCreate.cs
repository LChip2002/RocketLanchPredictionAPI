using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RLP_DB.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LaunchEntries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    LaunchLatitude = table.Column<double>(type: "double precision", nullable: true),
                    LaunchLongitude = table.Column<double>(type: "double precision", nullable: true),
                    LaunchStart = table.Column<string>(type: "text", nullable: true),
                    LaunchEnd = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    StatusDescription = table.Column<string>(type: "text", nullable: true),
                    Rocket = table.Column<string>(type: "text", nullable: true),
                    Mission = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Temperature = table.Column<double>(type: "double precision", nullable: true),
                    Rain = table.Column<double>(type: "double precision", nullable: true),
                    Showers = table.Column<double>(type: "double precision", nullable: true),
                    Snowfall = table.Column<double>(type: "double precision", nullable: true),
                    CloudCover = table.Column<double>(type: "double precision", nullable: true),
                    CloudCoverLow = table.Column<double>(type: "double precision", nullable: true),
                    CloudCoverMid = table.Column<double>(type: "double precision", nullable: true),
                    CloudCoverHigh = table.Column<double>(type: "double precision", nullable: true),
                    Visibility = table.Column<double>(type: "double precision", nullable: true),
                    WindSpeed10m = table.Column<double>(type: "double precision", nullable: true),
                    WindSpeed80m = table.Column<double>(type: "double precision", nullable: true),
                    WindSpeed120m = table.Column<double>(type: "double precision", nullable: true),
                    WindSpeed180m = table.Column<double>(type: "double precision", nullable: true),
                    Temperature80m = table.Column<double>(type: "double precision", nullable: true),
                    Temperature120m = table.Column<double>(type: "double precision", nullable: true),
                    Temperature180m = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaunchEntries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaunchEntries");
        }
    }
}
