using Microsoft.EntityFrameworkCore.Migrations;

namespace allinfo.Migrations
{
    public partial class isFA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "teamAvi",
                table: "Prospects");

            migrationBuilder.AddColumn<string>(
                name: "contractDetails",
                table: "Players",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contractDetails",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "teamAvi",
                table: "Prospects",
                type: "TEXT",
                nullable: true);
        }
    }
}
