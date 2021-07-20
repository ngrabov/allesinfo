using Microsoft.EntityFrameworkCore.Migrations;

namespace allinfo.Migrations
{
    public partial class PlayerStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "stat5",
                table: "Prospects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "stat6",
                table: "Prospects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "stat7",
                table: "Prospects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "stat8",
                table: "Prospects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stat5",
                table: "Prospects");

            migrationBuilder.DropColumn(
                name: "stat6",
                table: "Prospects");

            migrationBuilder.DropColumn(
                name: "stat7",
                table: "Prospects");

            migrationBuilder.DropColumn(
                name: "stat8",
                table: "Prospects");
        }
    }
}
