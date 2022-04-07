using Microsoft.EntityFrameworkCore.Migrations;

namespace Sparky.WebApp.Api.Migrations
{
    public partial class M002_addSlotId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SlotIdentifier",
                table: "PowerbankSlot",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
                table: "PowerBanks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlotIdentifier",
                table: "PowerbankSlot");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "PowerBanks");
        }
    }
}
