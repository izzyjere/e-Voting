using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICTAZEVoting.Core.Migrations
{
    public partial class Init14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Elections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Elections");
        }
    }
}
