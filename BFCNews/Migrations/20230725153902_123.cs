using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCNews.Migrations
{
    public partial class _123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_Id_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Positions_Id_PositionId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Id_PositionId",
                table: "AspNetUsers",
                newName: "PositionId");

            migrationBuilder.RenameColumn(
                name: "Id_DepartmentId",
                table: "AspNetUsers",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Id_PositionId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Id_DepartmentId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Positions_PositionId",
                table: "AspNetUsers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Positions_PositionId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "AspNetUsers",
                newName: "Id_PositionId");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "AspNetUsers",
                newName: "Id_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_PositionId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Id_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Id_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departments_Id_DepartmentId",
                table: "AspNetUsers",
                column: "Id_DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Positions_Id_PositionId",
                table: "AspNetUsers",
                column: "Id_PositionId",
                principalTable: "Positions",
                principalColumn: "Id");
        }
    }
}
