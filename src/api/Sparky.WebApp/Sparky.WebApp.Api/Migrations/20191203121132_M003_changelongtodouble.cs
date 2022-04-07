using Microsoft.EntityFrameworkCore.Migrations;

namespace Sparky.WebApp.Api.Migrations
{
    public partial class M003_changelongtodouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Lon",
                table: "ChargingStations",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<double>(
                name: "Lat",
                table: "ChargingStations",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Lon",
                table: "ChargingStations",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "Lat",
                table: "ChargingStations",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
