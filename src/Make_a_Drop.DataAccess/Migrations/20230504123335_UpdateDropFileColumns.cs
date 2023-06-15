using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Make_a_Drop.DataAccess.Migrations
{
    public partial class UpdateDropFileColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "DropFile");

            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "Drop");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Drop");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "DropFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "Drop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Drop",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
