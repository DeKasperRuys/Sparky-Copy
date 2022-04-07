using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sparky.WebApp.Api.Migrations
{
    public partial class M001_initializedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChargingStations",
                columns: table => new
                {
                    ChargingStationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Lat = table.Column<long>(nullable: false),
                    Lon = table.Column<long>(nullable: false),
                    NumOfSlots = table.Column<int>(nullable: false),
                    NumOfAvailableSlots = table.Column<int>(nullable: false),
                    IsReturnRequest = table.Column<bool>(nullable: false),
                    ReturnOK = table.Column<bool>(nullable: false),
                    CloseStation = table.Column<bool>(nullable: false),
                    AvailablePowerbank = table.Column<int>(nullable: false),
                    IsLoanRequest = table.Column<bool>(nullable: false),
                    OpenStation = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStations", x => x.ChargingStationId);
                });

            migrationBuilder.CreateTable(
                name: "PowerbankLoanObjs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Powerbank = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerbankLoanObjs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerBanks",
                columns: table => new
                {
                    PowerBankId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<string>(nullable: true),
                    isLent = table.Column<bool>(nullable: false),
                    CurrentStationId = table.Column<int>(nullable: false),
                    CurrentUserId = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerBanks", x => x.PowerBankId);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    UserInfoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    FirebaseUid = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.UserInfoId);
                });

            migrationBuilder.CreateTable(
                name: "PowerbankSlot",
                columns: table => new
                {
                    PowerbankSlotId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsEmpty = table.Column<bool>(nullable: false),
                    LevedCharged = table.Column<int>(nullable: false),
                    StationKey = table.Column<int>(nullable: false),
                    StationChargingStationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerbankSlot", x => x.PowerbankSlotId);
                    table.ForeignKey(
                        name: "FK_PowerbankSlot_ChargingStations_StationChargingStationId",
                        column: x => x.StationChargingStationId,
                        principalTable: "ChargingStations",
                        principalColumn: "ChargingStationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDateTime = table.Column<long>(nullable: false),
                    StopDateTime = table.Column<long>(nullable: false),
                    OnGoing = table.Column<bool>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    PowerbankLoanObjId = table.Column<int>(nullable: false),
                    BorrowerUserInfoId = table.Column<int>(nullable: true),
                    PowerBankId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_UserInfo_BorrowerUserInfoId",
                        column: x => x.BorrowerUserInfoId,
                        principalTable: "UserInfo",
                        principalColumn: "UserInfoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_PowerBanks_PowerBankId",
                        column: x => x.PowerBankId,
                        principalTable: "PowerBanks",
                        principalColumn: "PowerBankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_PowerbankLoanObjs_PowerbankLoanObjId",
                        column: x => x.PowerbankLoanObjId,
                        principalTable: "PowerbankLoanObjs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BorrowerUserInfoId",
                table: "Loans",
                column: "BorrowerUserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_PowerBankId",
                table: "Loans",
                column: "PowerBankId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_PowerbankLoanObjId",
                table: "Loans",
                column: "PowerbankLoanObjId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PowerbankSlot_StationChargingStationId",
                table: "PowerbankSlot",
                column: "StationChargingStationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "PowerbankSlot");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "PowerBanks");

            migrationBuilder.DropTable(
                name: "PowerbankLoanObjs");

            migrationBuilder.DropTable(
                name: "ChargingStations");
        }
    }
}
