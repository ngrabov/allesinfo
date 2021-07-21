using Microsoft.EntityFrameworkCore.Migrations;

namespace allinfo.Migrations
{
    public partial class AvatarAge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarURL",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarURL",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "age",
                table: "Players");
        }
    }
}
