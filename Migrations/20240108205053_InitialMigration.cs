using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MediaImages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(type: "varchar(255)", nullable: false),
                    filename = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    mime_type = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    width = table.Column<int>(type: "int", nullable: false),
                    height = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_images", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    thumbnail_id = table.Column<int>(type: "int", nullable: true),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_categories_category_id",
                        column: x => x.parent_id,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_categories_media_images_thumbnail_id",
                        column: x => x.thumbnail_id,
                        principalTable: "MediaImages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sku = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    crossed_out_price = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: true),
                    thumbnail_id = table.Column<int>(type: "int", nullable: true),
                    length = table.Column<double>(type: "double", nullable: true),
                    width = table.Column<double>(type: "double", nullable: true),
                    height = table.Column<double>(type: "double", nullable: true),
                    dimension_units = table.Column<int>(type: "int", nullable: true),
                    weight = table.Column<double>(type: "double", nullable: true),
                    weight_units = table.Column<int>(type: "int", nullable: true),
                    min_purchase_quantity = table.Column<int>(type: "int", nullable: false),
                    max_purchase_quantity = table.Column<int>(type: "int", nullable: false),
                    in_stock = table.Column<int>(type: "int", nullable: false),
                    unlimited = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_media_images_thumbnail_id",
                        column: x => x.thumbnail_id,
                        principalTable: "MediaImages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductAttributes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_attributes", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_attributes_products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_categories", x => new { x.category_id, x.product_id });
                    table.ForeignKey(
                        name: "fk_product_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_categories_products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    image_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_images", x => new { x.product_id, x.image_id });
                    table.ForeignKey(
                        name: "fk_product_images_media_images_image_id",
                        column: x => x.image_id,
                        principalTable: "MediaImages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_images_products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_categories_parent_id",
                table: "Categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_thumbnail_id",
                table: "Categories",
                column: "thumbnail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_media_images_filename",
                table: "MediaImages",
                column: "filename",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_media_images_url",
                table: "MediaImages",
                column: "url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_attributes_product_id",
                table: "ProductAttributes",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_categories_product_id",
                table: "ProductCategories",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_images_image_id",
                table: "ProductImages",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_sku",
                table: "Products",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_thumbnail_id",
                table: "Products",
                column: "thumbnail_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributes");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "MediaImages");
        }
    }
}
