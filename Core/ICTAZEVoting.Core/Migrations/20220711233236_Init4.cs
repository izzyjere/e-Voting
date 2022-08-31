using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICTAZEVoting.Core.Migrations;

public partial class Init4 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "PolingStationId",
            table: "Voters",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Constituencies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Constituencies", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PolingStations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConstituencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PolingStations", x => x.Id);
                table.ForeignKey(
                    name: "FK_PolingStations_Constituencies_ConstituencyId",
                    column: x => x.ConstituencyId,
                    principalTable: "Constituencies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Voters_PolingStationId",
            table: "Voters",
            column: "PolingStationId");

        migrationBuilder.CreateIndex(
            name: "IX_PolingStations_ConstituencyId",
            table: "PolingStations",
            column: "ConstituencyId");

        migrationBuilder.AddForeignKey(
            name: "FK_Voters_PolingStations_PolingStationId",
            table: "Voters",
            column: "PolingStationId",
            principalTable: "PolingStations",
            principalColumn: "Id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Voters_PolingStations_PolingStationId",
            table: "Voters");

        migrationBuilder.DropTable(
            name: "PolingStations");

        migrationBuilder.DropTable(
            name: "Constituencies");

        migrationBuilder.DropIndex(
            name: "IX_Voters_PolingStationId",
            table: "Voters");

        migrationBuilder.DropColumn(
            name: "PolingStationId",
            table: "Voters");
    }
}
