﻿using System;
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
                name: "images",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    file_id = table.Column<string>(type: "varchar(255)", nullable: false),
                    url = table.Column<string>(type: "longtext", nullable: false),
                    width = table.Column<int>(type: "int", nullable: false),
                    height = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "measure_units",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    symbol = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: true),
                    type = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_measure_units", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    thumbnail_id = table.Column<int>(type: "int", nullable: true),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                    table.ForeignKey(
                        name: "fk_categories_categories_category_id",
                        column: x => x.parent_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_categories_image_thumbnail_id",
                        column: x => x.thumbnail_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sku = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    crossed_out_price = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: true),
                    thumbnail_id = table.Column<int>(type: "int", nullable: true),
                    width = table.Column<double>(type: "double", nullable: true),
                    height = table.Column<double>(type: "double", nullable: true),
                    length = table.Column<double>(type: "double", nullable: true),
                    dimension_units_id = table.Column<int>(type: "int", nullable: true),
                    weight = table.Column<double>(type: "double", nullable: true),
                    weight_units_id = table.Column<int>(type: "int", nullable: true),
                    volume = table.Column<double>(type: "double", nullable: true),
                    volume_units_id = table.Column<int>(type: "int", nullable: true),
                    min_purchase_quantity = table.Column<int>(type: "int", nullable: true),
                    max_purchase_quantity = table.Column<int>(type: "int", nullable: true),
                    in_stock = table.Column<int>(type: "int", nullable: true),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_image_thumbnail_id",
                        column: x => x.thumbnail_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_products_measure_unit_dimension_units_id",
                        column: x => x.dimension_units_id,
                        principalTable: "measure_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_products_measure_unit_volume_units_id",
                        column: x => x.volume_units_id,
                        principalTable: "measure_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_products_measure_unit_weight_units_id",
                        column: x => x.weight_units_id,
                        principalTable: "measure_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_attributes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "longtext", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_attributes", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_attributes_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_categories",
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
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_categories_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_images",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false),
                    image_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_images", x => new { x.product_id, x.image_id });
                    table.ForeignKey(
                        name: "fk_product_images_image_image_id",
                        column: x => x.image_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_images_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_categories_parent_id",
                table: "categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_categories_thumbnail_id",
                table: "categories",
                column: "thumbnail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_images_file_id",
                table: "images",
                column: "file_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_measure_units_symbol",
                table: "measure_units",
                column: "symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_attributes_product_id",
                table: "product_attributes",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_categories_product_id",
                table: "product_categories",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_images_image_id",
                table: "product_images",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_dimension_units_id",
                table: "products",
                column: "dimension_units_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_sku",
                table: "products",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_thumbnail_id",
                table: "products",
                column: "thumbnail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_volume_units_id",
                table: "products",
                column: "volume_units_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_weight_units_id",
                table: "products",
                column: "weight_units_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_attributes");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "measure_units");
        }
    }
}
