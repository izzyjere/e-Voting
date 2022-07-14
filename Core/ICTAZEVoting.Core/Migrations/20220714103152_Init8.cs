using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICTAZEVoting.Core.Migrations
{
    public partial class Init8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConstituencyId",
                table: "SystemAdmins",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SystemAdmins_ConstituencyId",
                table: "SystemAdmins",
                column: "ConstituencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemAdmins_Constituencies_ConstituencyId",
                table: "SystemAdmins",
                column: "ConstituencyId",
                principalTable: "Constituencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemAdmins_Constituencies_ConstituencyId",
                table: "SystemAdmins");

            migrationBuilder.DropIndex(
                name: "IX_SystemAdmins_ConstituencyId",
                table: "SystemAdmins");

            migrationBuilder.DropColumn(
                name: "ConstituencyId",
                table: "SystemAdmins");
        }
    }
}
