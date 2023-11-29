﻿// <auto-generated />
using System;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ecommerce.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasPrecision(0)
                        .HasColumnType("datetime(0)")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("description");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("name");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int")
                        .HasColumnName("parent_id");

                    b.Property<int?>("ThumbnailId")
                        .HasColumnType("int")
                        .HasColumnName("thumbnail_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasPrecision(0)
                        .HasColumnType("datetime(0)")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("ix_categories_parent_id");

                    b.HasIndex("ThumbnailId")
                        .IsUnique()
                        .HasDatabaseName("ix_categories_thumbnail_id");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.MediaImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasPrecision(0)
                        .HasColumnType("datetime(0)")
                        .HasColumnName("created_at");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("file_id");

                    b.Property<int>("Height")
                        .HasColumnType("int")
                        .HasColumnName("height");

                    b.Property<DateTime>("UpdatedAt")
                        .HasPrecision(0)
                        .HasColumnType("datetime(0)")
                        .HasColumnName("updated_at");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.Property<int>("Width")
                        .HasColumnType("int")
                        .HasColumnName("width");

                    b.HasKey("Id")
                        .HasName("pk_media_images");

                    b.HasIndex("FileId")
                        .IsUnique()
                        .HasDatabaseName("ix_media_images_file_id");

                    b.ToTable("media_images", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasPrecision(0)
                        .HasColumnType("datetime(0)")
                        .HasColumnName("created_at");

                    b.Property<decimal?>("CrossedOutPrice")
                        .HasPrecision(10, 4)
                        .HasColumnType("decimal(10,4)")
                        .HasColumnName("crossed_out_price");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("description");

                    b.Property<int?>("DimensionUnits")
                        .HasColumnType("int")
                        .HasColumnName("dimension_units");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("enabled");

                    b.Property<double?>("Height")
                        .HasColumnType("double")
                        .HasColumnName("height");

                    b.Property<int?>("InStock")
                        .HasColumnType("int")
                        .HasColumnName("in_stock");

                    b.Property<double?>("Length")
                        .HasColumnType("double")
                        .HasColumnName("length");

                    b.Property<int>("MaxPurchaseQuantity")
                        .HasColumnType("int")
                        .HasColumnName("max_purchase_quantity");

                    b.Property<int>("MinPurchaseQuantity")
                        .HasColumnType("int")
                        .HasColumnName("min_purchase_quantity");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 4)
                        .HasColumnType("decimal(10,4)")
                        .HasColumnName("price");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)")
                        .HasColumnName("sku");

                    b.Property<int?>("ThumbnailId")
                        .HasColumnType("int")
                        .HasColumnName("thumbnail_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasPrecision(0)
                        .HasColumnType("datetime(0)")
                        .HasColumnName("updated_at");

                    b.Property<double?>("Weight")
                        .HasColumnType("double")
                        .HasColumnName("weight");

                    b.Property<int?>("WeightUnits")
                        .HasColumnType("int")
                        .HasColumnName("weight_units");

                    b.Property<double?>("Width")
                        .HasColumnType("double")
                        .HasColumnName("width");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("Sku")
                        .IsUnique()
                        .HasDatabaseName("ix_products_sku");

                    b.HasIndex("ThumbnailId")
                        .IsUnique()
                        .HasDatabaseName("ix_products_thumbnail_id");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.ProductAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasPrecision(0)
                        .HasColumnType("datetime(0)")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("name");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasPrecision(0)
                        .HasColumnType("datetime(0)")
                        .HasColumnName("updated_at");

                    b.Property<string>("Value")
                        .HasColumnType("longtext")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_product_attributes");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_product_attributes_product_id");

                    b.ToTable("product_attributes", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.ProductCategories", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.HasKey("CategoryId", "ProductId")
                        .HasName("pk_product_categories");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_product_categories_product_id");

                    b.ToTable("product_categories", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.ProductImages", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int>("ImageId")
                        .HasColumnType("int")
                        .HasColumnName("image_id");

                    b.HasKey("ProductId", "ImageId")
                        .HasName("pk_product_images");

                    b.HasIndex("ImageId")
                        .HasDatabaseName("ix_product_images_image_id");

                    b.ToTable("product_images", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.Category", b =>
                {
                    b.HasOne("Ecommerce.Common.Models.Schema.Category", null)
                        .WithMany("Subcategories")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_categories_categories_category_id");

                    b.HasOne("Ecommerce.Common.Models.Schema.MediaImage", "Thumbnail")
                        .WithOne()
                        .HasForeignKey("Ecommerce.Common.Models.Schema.Category", "ThumbnailId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_categories_media_images_thumbnail_id");

                    b.Navigation("Thumbnail");
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.Product", b =>
                {
                    b.HasOne("Ecommerce.Common.Models.Schema.MediaImage", "Thumbnail")
                        .WithOne()
                        .HasForeignKey("Ecommerce.Common.Models.Schema.Product", "ThumbnailId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_products_media_images_thumbnail_id");

                    b.Navigation("Thumbnail");
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.ProductAttribute", b =>
                {
                    b.HasOne("Ecommerce.Common.Models.Schema.Product", null)
                        .WithMany("Attributes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_attributes_products_product_id");
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.ProductCategories", b =>
                {
                    b.HasOne("Ecommerce.Common.Models.Schema.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_categories_categories_category_id");

                    b.HasOne("Ecommerce.Common.Models.Schema.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_categories_products_product_id");
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.ProductImages", b =>
                {
                    b.HasOne("Ecommerce.Common.Models.Schema.MediaImage", null)
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_images_media_images_image_id");

                    b.HasOne("Ecommerce.Common.Models.Schema.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_images_products_product_id");
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.Category", b =>
                {
                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("Ecommerce.Common.Models.Schema.Product", b =>
                {
                    b.Navigation("Attributes");
                });
#pragma warning restore 612, 618
        }
    }
}
