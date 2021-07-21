using Microsoft.EntityFrameworkCore.Migrations;

namespace allinfo.Migrations
{
    public partial class Moderation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isModerator",
                table: "Writers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isImportant",
                table: "Articles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isModerated",
                table: "Articles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isModerator",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "isImportant",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "isModerated",
                table: "Articles");
        }
    }
}
