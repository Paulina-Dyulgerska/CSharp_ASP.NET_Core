using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConformityCheck.Data.Migrations
{
    public partial class DbModelsAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Number = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConformityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConformityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Number = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegulationLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    Source = table.Column<string>(nullable: false),
                    SourceURL = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulationLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Substances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CASNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Number = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: true),
                    ContactPersonFirstName = table.Column<string>(maxLength: 20, nullable: true),
                    ContactPersonLastName = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleProducts",
                columns: table => new
                {
                    ArticleId = table.Column<string>(nullable: false),
                    ProductId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleProducts", x => new { x.ArticleId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ArticleProducts_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleSubstances",
                columns: table => new
                {
                    ArticleId = table.Column<string>(nullable: false),
                    SubstanceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleSubstances", x => new { x.ArticleId, x.SubstanceId });
                    table.ForeignKey(
                        name: "FK_ArticleSubstances_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleSubstances_Substances_SubstanceId",
                        column: x => x.SubstanceId,
                        principalTable: "Substances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubstanceRegulationLists",
                columns: table => new
                {
                    SubstanceId = table.Column<int>(nullable: false),
                    RegulationListId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Restriction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstanceRegulationLists", x => new { x.RegulationListId, x.SubstanceId });
                    table.ForeignKey(
                        name: "FK_SubstanceRegulationLists_RegulationLists_RegulationListId",
                        column: x => x.RegulationListId,
                        principalTable: "RegulationLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubstanceRegulationLists_Substances_SubstanceId",
                        column: x => x.SubstanceId,
                        principalTable: "Substances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleSuppliers",
                columns: table => new
                {
                    ArticleId = table.Column<string>(nullable: false),
                    SupplierId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleSuppliers", x => new { x.ArticleId, x.SupplierId });
                    table.ForeignKey(
                        name: "FK_ArticleSuppliers_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleSuppliers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conformities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ConformityTypeId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<string>(nullable: false),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    ConformationAcceptanceDate = table.Column<DateTime>(nullable: true),
                    IsAccepted = table.Column<bool>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conformities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conformities_ConformityTypes_ConformityTypeId",
                        column: x => x.ConformityTypeId,
                        principalTable: "ConformityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conformities_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleConformities",
                columns: table => new
                {
                    ArticleId = table.Column<string>(nullable: false),
                    ConformityId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleConformities", x => new { x.ArticleId, x.ConformityId });
                    table.ForeignKey(
                        name: "FK_ArticleConformities_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleConformities_Conformities_ConformityId",
                        column: x => x.ConformityId,
                        principalTable: "Conformities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductConformities",
                columns: table => new
                {
                    ProductId = table.Column<string>(nullable: false),
                    ConformityId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConformities", x => new { x.ProductId, x.ConformityId });
                    table.ForeignKey(
                        name: "FK_ProductConformities_Conformities_ConformityId",
                        column: x => x.ConformityId,
                        principalTable: "Conformities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductConformities_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleConformities_ConformityId",
                table: "ArticleConformities",
                column: "ConformityId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleProducts_ProductId",
                table: "ArticleProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IsDeleted",
                table: "Articles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Number",
                table: "Articles",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleSubstances_SubstanceId",
                table: "ArticleSubstances",
                column: "SubstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleSuppliers_SupplierId",
                table: "ArticleSuppliers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Conformities_ConformityTypeId",
                table: "Conformities",
                column: "ConformityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Conformities_IsDeleted",
                table: "Conformities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Conformities_SupplierId",
                table: "Conformities",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ConformityTypes_Description",
                table: "ConformityTypes",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConformityTypes_IsDeleted",
                table: "ConformityTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConformities_ConformityId",
                table: "ProductConformities",
                column: "ConformityId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsDeleted",
                table: "Products",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Number",
                table: "Products",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegulationLists_Description",
                table: "RegulationLists",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegulationLists_IsDeleted",
                table: "RegulationLists",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SubstanceRegulationLists_IsDeleted",
                table: "SubstanceRegulationLists",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SubstanceRegulationLists_SubstanceId",
                table: "SubstanceRegulationLists",
                column: "SubstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Substances_CASNumber",
                table: "Substances",
                column: "CASNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Substances_IsDeleted",
                table: "Substances",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_IsDeleted",
                table: "Suppliers",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleConformities");

            migrationBuilder.DropTable(
                name: "ArticleProducts");

            migrationBuilder.DropTable(
                name: "ArticleSubstances");

            migrationBuilder.DropTable(
                name: "ArticleSuppliers");

            migrationBuilder.DropTable(
                name: "ProductConformities");

            migrationBuilder.DropTable(
                name: "SubstanceRegulationLists");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Conformities");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "RegulationLists");

            migrationBuilder.DropTable(
                name: "Substances");

            migrationBuilder.DropTable(
                name: "ConformityTypes");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
