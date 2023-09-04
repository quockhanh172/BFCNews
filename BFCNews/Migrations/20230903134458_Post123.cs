using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCNews.Migrations
{
    public partial class Post123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Posts");
        }
    }
}
