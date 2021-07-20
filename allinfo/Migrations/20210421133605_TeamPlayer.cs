using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace allinfo.Migrations
{
    public partial class TeamPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    playingPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    ppg = table.Column<double>(type: "REAL", nullable: false),
                    nationality = table.Column<string>(type: "TEXT", nullable: true),
                    height = table.Column<int>(type: "INTEGER", nullable: false),
                    NBA2KRating = table.Column<int>(type: "INTEGER", nullable: false),
                    shirtNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    salary1 = table.Column<double>(type: "REAL", nullable: false),
                    salary2 = table.Column<double>(type: "REAL", nullable: false),
                    salary3 = table.Column<double>(type: "REAL", nullable: false),
                    salary4 = table.Column<double>(type: "REAL", nullable: false),
                    salary5 = table.Column<double>(type: "REAL", nullable: false),
                    contractOption = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AvatarURL = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
