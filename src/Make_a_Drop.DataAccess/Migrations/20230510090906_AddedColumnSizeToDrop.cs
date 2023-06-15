using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Make_a_Drop.DataAccess.Migrations
{
    public partial class AddedColumnSizeToDrop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Size",
                table: "Drop");
        }
    }
}
