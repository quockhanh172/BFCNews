using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCNews.Migrations
{
    public partial class aaaaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "summary",
                table: "Posts",
                newName: "Summary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Posts",
                newName: "summary");
        }
    }
}
