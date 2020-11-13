using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConformityCheck.Data.Migrations
{
    public partial class DbModelsBaseModelAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ProductConformities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductConformities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ProductConformities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ArticleSuppliers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ArticleSuppliers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ArticleSuppliers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ArticleSubstances",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ArticleSubstances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ArticleSubstances",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ArticleProducts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ArticleProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ArticleProducts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ArticleConformities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ArticleConformities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ArticleConformities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ProductConformities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductConformities");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ProductConformities");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ArticleSuppliers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ArticleSuppliers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ArticleSuppliers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ArticleSubstances");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ArticleSubstances");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ArticleSubstances");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ArticleProducts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ArticleProducts");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ArticleProducts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ArticleConformities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ArticleConformities");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ArticleConformities");
        }
    }
}
