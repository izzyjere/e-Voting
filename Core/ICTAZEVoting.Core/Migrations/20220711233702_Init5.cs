using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICTAZEVoting.Core.Migrations
{
    public partial class Init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voters_PolingStations_PolingStationId",
                table: "Voters");

            migrationBuilder.AlterColumn<Guid>(
                name: "PolingStationId",
                table: "Voters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Voters_PolingStations_PolingStationId",
                table: "Voters",
                column: "PolingStationId",
                principalTable: "PolingStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voters_PolingStations_PolingStationId",
                table: "Voters");

            migrationBuilder.AlterColumn<Guid>(
                name: "PolingStationId",
                table: "Voters",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Voters_PolingStations_PolingStationId",
                table: "Voters",
                column: "PolingStationId",
                principalTable: "PolingStations",
                principalColumn: "Id");
        }
    }
}
