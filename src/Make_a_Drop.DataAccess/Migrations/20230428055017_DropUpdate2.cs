using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Make_a_Drop.DataAccess.Migrations
{
    public partial class DropUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "Drop");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Drop");
        }
    }
}
