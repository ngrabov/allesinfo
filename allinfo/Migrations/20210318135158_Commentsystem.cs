using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace allinfo.Migrations
{
    public partial class Commentsystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSelected",
                table: "Articles");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<string>(nullable: true),
                    ArticleID = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 30000, nullable: false),
                    TimeWritten = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.AddColumn<bool>(
                name: "isSelected",
                table: "Articles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
