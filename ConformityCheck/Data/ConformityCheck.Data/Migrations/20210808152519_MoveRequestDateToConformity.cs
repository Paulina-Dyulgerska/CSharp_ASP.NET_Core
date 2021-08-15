namespace ConformityCheck.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class MoveRequestDateToConformity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "ArticleConformityTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "Conformities",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "Conformities");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "ArticleConformityTypes",
                type: "datetime2",
                nullable: true);
        }
    }
}
