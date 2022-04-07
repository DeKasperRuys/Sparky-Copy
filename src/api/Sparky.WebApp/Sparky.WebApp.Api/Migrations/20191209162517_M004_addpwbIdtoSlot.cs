using Microsoft.EntityFrameworkCore.Migrations;

namespace Sparky.WebApp.Api.Migrations
{
    public partial class M004_addpwbIdtoSlot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PowerbankId",
                table: "PowerbankSlot",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PowerbankId",
                table: "PowerbankSlot");
        }
    }
}
