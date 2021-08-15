namespace ConformityCheck.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddUserIdForContactFormEntryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ContactFormEntries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactFormEntries_UserId",
                table: "ContactFormEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactFormEntries_AspNetUsers_UserId",
                table: "ContactFormEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactFormEntries_AspNetUsers_UserId",
                table: "ContactFormEntries");

            migrationBuilder.DropIndex(
                name: "IX_ContactFormEntries_UserId",
                table: "ContactFormEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContactFormEntries");
        }
    }
}
