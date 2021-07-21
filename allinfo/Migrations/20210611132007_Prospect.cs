using Microsoft.EntityFrameworkCore.Migrations;

namespace allinfo.Migrations
{
    public partial class Prospect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prospects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    playingPosition = table.Column<string>(type: "TEXT", nullable: false),
                    age = table.Column<int>(type: "INTEGER", nullable: false),
                    height = table.Column<int>(type: "INTEGER", nullable: false),
                    report = table.Column<string>(type: "TEXT", nullable: false),
                    college = table.Column<string>(type: "TEXT", nullable: false),
                    group = table.Column<int>(type: "INTEGER", nullable: false),
                    rank = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    shirtNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    avatarUrl = table.Column<string>(type: "TEXT", nullable: false),
                    stat1 = table.Column<string>(type: "TEXT", nullable: false),
                    stat2 = table.Column<string>(type: "TEXT", nullable: false),
                    stat3 = table.Column<string>(type: "TEXT", nullable: false),
                    stat4 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prospects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Prospects_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prospects_TeamId",
                table: "Prospects",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prospects");
        }
    }
}
