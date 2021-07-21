using Microsoft.EntityFrameworkCore.Migrations;

namespace allinfo.Migrations
{
    public partial class age : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "age",
                table: "Players");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
