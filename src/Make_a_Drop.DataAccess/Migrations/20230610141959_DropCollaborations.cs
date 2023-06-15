using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Make_a_Drop.DataAccess.Migrations
{
    public partial class DropCollaborations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CollaborationId",
                table: "Drop",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drop_CollaborationId",
                table: "Drop",
                column: "CollaborationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drop_Collaboration_CollaborationId",
                table: "Drop",
                column: "CollaborationId",
                principalTable: "Collaboration",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drop_Collaboration_CollaborationId",
                table: "Drop");

            migrationBuilder.DropIndex(
                name: "IX_Drop_CollaborationId",
                table: "Drop");

            migrationBuilder.DropColumn(
                name: "CollaborationId",
                table: "Drop");
        }
    }
}
