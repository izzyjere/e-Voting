using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICTAZEVoting.Core.Migrations
{
    public partial class Init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NRC",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NRC",
                schema: "Identity",
                table: "Users");
        }
    }
}
