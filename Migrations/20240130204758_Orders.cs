using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    recipient = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    phone = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    street = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    street_number = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    apt_number = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    neighbourhood = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    city = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    state = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    postal_code = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    comments = table.Column<string>(type: "longtext", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_addresses_users_application_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    card_owner = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    card_number = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    cvv = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    expiry_month = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    expiry_year = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    billing_address_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_methods", x => x.id);
                    table.ForeignKey(
                        name: "fk_payment_methods_addresses_billing_address_id",
                        column: x => x.billing_address_id,
                        principalTable: "Addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payment_methods_users_application_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    payment_method_id = table.Column<int>(type: "int", nullable: true),
                    payment_status = table.Column<int>(type: "int", nullable: false),
                    shipping_address_id = table.Column<int>(type: "int", nullable: true),
                    shipping_status = table.Column<int>(type: "int", nullable: false),
                    shipping_cost = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    tracking_number = table.Column<string>(type: "longtext", nullable: true),
                    total = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_addresses_shipping_address_id",
                        column: x => x.shipping_address_id,
                        principalTable: "Addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_orders_payment_methods_payment_method_id",
                        column: x => x.payment_method_id,
                        principalTable: "PaymentMethods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_orders_users_application_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_products", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "fk_order_products_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_products_products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_addresses_user_id",
                table: "Addresses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_products_product_id",
                table: "OrderProducts",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_payment_method_id",
                table: "Orders",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_shipping_address_id",
                table: "Orders",
                column: "shipping_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_user_id",
                table: "Orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_methods_billing_address_id",
                table: "PaymentMethods",
                column: "billing_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_methods_user_id",
                table: "PaymentMethods",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
