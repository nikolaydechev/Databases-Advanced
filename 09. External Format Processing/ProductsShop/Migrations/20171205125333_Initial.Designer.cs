﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ProductsShop.Data;
using System;

namespace ProductsShop.Migrations
{
    [DbContext(typeof(ProductsShopContext))]
    [Migration("20171205125333_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductsShop.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(15);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ProductsShop.Models.CategoryProduct", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("CategoryId");

                    b.HasKey("ProductId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("ProductsShop.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BuyerId");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<int>("SellerId");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("SellerId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProductsShop.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Age");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProductsShop.Models.CategoryProduct", b =>
                {
                    b.HasOne("ProductsShop.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProductsShop.Models.Product", "Product")
                        .WithMany("Categories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProductsShop.Models.Product", b =>
                {
                    b.HasOne("ProductsShop.Models.User", "Buyer")
                        .WithMany("BoughtProducts")
                        .HasForeignKey("BuyerId");

                    b.HasOne("ProductsShop.Models.User", "Seller")
                        .WithMany("SoldProducts")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
