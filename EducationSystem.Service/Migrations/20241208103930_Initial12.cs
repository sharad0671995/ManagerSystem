using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationSystem.Service.Migrations
{
    /// <inheritdoc />
    public partial class Initial12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Table_Tasks_AspNetUsers_AssignedToId",
                table: "Table_Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Table_Tasks_AspNetUsers_CreatedById",
                table: "Table_Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Table_Tasks",
                table: "Table_Tasks");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Table_Tasks",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Table_Tasks_CreatedById",
                table: "Tasks",
                newName: "IX_Tasks_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Table_Tasks_AssignedToId",
                table: "Tasks",
                newName: "IX_Tasks_AssignedToId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_AssignedToId",
                table: "Tasks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_CreatedById",
                table: "Tasks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_AssignedToId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_CreatedById",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Table_Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_CreatedById",
                table: "Table_Tasks",
                newName: "IX_Table_Tasks_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AssignedToId",
                table: "Table_Tasks",
                newName: "IX_Table_Tasks_AssignedToId");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Table_Tasks",
                table: "Table_Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Table_Tasks_AspNetUsers_AssignedToId",
                table: "Table_Tasks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Table_Tasks_AspNetUsers_CreatedById",
                table: "Table_Tasks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
