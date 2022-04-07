using Microsoft.EntityFrameworkCore.Migrations;

namespace Sparky.WebApp.Api.Migrations
{
    public partial class changetodouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Loans",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Loans",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
