using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Make_a_Drop.DataAccess.Migrations
{
    public partial class UpdateDropAndFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Drop_DropId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_Drop_FileName",
                table: "Drop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_File",
                table: "File");

            migrationBuilder.RenameTable(
                name: "File",
                newName: "DropFile");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Drop",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_File_DropId",
                table: "DropFile",
                newName: "IX_DropFile_DropId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DropFile",
                table: "DropFile",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drop_Name",
                table: "Drop",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DropFile_Drop_DropId",
                table: "DropFile",
                column: "DropId",
                principalTable: "Drop",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropFile_Drop_DropId",
                table: "DropFile");

            migrationBuilder.DropIndex(
                name: "IX_Drop_Name",
                table: "Drop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DropFile",
                table: "DropFile");

            migrationBuilder.RenameTable(
                name: "DropFile",
                newName: "File");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Drop",
                newName: "FileName");

            migrationBuilder.RenameIndex(
                name: "IX_DropFile_DropId",
                table: "File",
                newName: "IX_File_DropId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_File",
                table: "File",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drop_FileName",
                table: "Drop",
                column: "FileName",
                unique: true,
                filter: "[FileName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Drop_DropId",
                table: "File",
                column: "DropId",
                principalTable: "Drop",
                principalColumn: "Id");
        }
    }
}
