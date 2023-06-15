using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Make_a_Drop.DataAccess.Migrations
{
    public partial class DropUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Drop");

            migrationBuilder.RenameColumn(
                name: "PathToData",
                table: "Drop",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Drop",
                newName: "DownloadUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Drop",
                newName: "PathToData");

            migrationBuilder.RenameColumn(
                name: "DownloadUrl",
                table: "Drop",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Drop",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
