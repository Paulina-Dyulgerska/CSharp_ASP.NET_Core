using Microsoft.EntityFrameworkCore.Migrations;

namespace ConformityCheck.Data.Migrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Substances",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RegulationLists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Substances_UserId",
                table: "Substances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulationLists_UserId",
                table: "RegulationLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegulationLists_AspNetUsers_UserId",
                table: "RegulationLists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Substances_AspNetUsers_UserId",
                table: "Substances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegulationLists_AspNetUsers_UserId",
                table: "RegulationLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Substances_AspNetUsers_UserId",
                table: "Substances");

            migrationBuilder.DropIndex(
                name: "IX_Substances_UserId",
                table: "Substances");

            migrationBuilder.DropIndex(
                name: "IX_RegulationLists_UserId",
                table: "RegulationLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Substances");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RegulationLists");
        }
    }
}
