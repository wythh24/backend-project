﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using productstockingv1.Data;

#nullable disable

namespace productstockingv1.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20230223045423_initialize")]
    partial class initialize
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("productstockingv1.models.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("varchar");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<decimal>("Price")
                        .HasColumnType("dec(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("product", (string)null);
                });

            modelBuilder.Entity("productstockingv1.models.Stocking", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar");

                    b.Property<DateTime?>("DocumentDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("PostingDate")
                        .HasColumnType("datetime");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("WareId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ProductId" }, "stk_pro_fk");

                    b.HasIndex(new[] { "WareId" }, "stk_wre_fk");

                    b.ToTable("stocking", (string)null);
                });

            modelBuilder.Entity("productstockingv1.models.Ware", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("varchar");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ware", (string)null);
                });

            modelBuilder.Entity("productstockingv1.models.Stocking", b =>
                {
                    b.HasOne("productstockingv1.models.Product", "product")
                        .WithMany("Stockings")
                        .HasForeignKey("ProductId")
                        .IsRequired()
                        .HasConstraintName("stk_pro_fk");

                    b.HasOne("productstockingv1.models.Ware", "Ware")
                        .WithMany("Stockings")
                        .HasForeignKey("WareId")
                        .IsRequired()
                        .HasConstraintName("stk_wre_fk");

                    b.Navigation("Ware");

                    b.Navigation("product");
                });

            modelBuilder.Entity("productstockingv1.models.Product", b =>
                {
                    b.Navigation("Stockings");
                });

            modelBuilder.Entity("productstockingv1.models.Ware", b =>
                {
                    b.Navigation("Stockings");
                });
#pragma warning restore 612, 618
        }
    }
}
