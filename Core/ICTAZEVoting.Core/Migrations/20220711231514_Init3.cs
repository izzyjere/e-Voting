using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICTAZEVoting.Core.Migrations;

public partial class Init3 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "VerificationCipher",
            table: "Voters");

        migrationBuilder.AlterColumn<DateTime>(
            name: "ElectionDate",
            table: "Elections",
            type: "datetime2",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime2");

        migrationBuilder.CreateTable(
            name: "SecreteKeys",
            columns: table => new
            {
                VoterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EncryptedKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IV = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SecreteKeys", x => x.VoterId);
                table.ForeignKey(
                    name: "FK_SecreteKeys_Voters_VoterId",
                    column: x => x.VoterId,
                    principalTable: "Voters",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SecreteKeys");

        migrationBuilder.AddColumn<string>(
            name: "VerificationCipher",
            table: "Voters",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AlterColumn<DateTime>(
            name: "ElectionDate",
            table: "Elections",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "datetime2",
            oldNullable: true);
    }
}
