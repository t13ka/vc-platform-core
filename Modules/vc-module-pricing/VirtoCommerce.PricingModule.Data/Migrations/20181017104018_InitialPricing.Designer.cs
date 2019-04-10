﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtoCommerce.PricingModule.Data.Repositories;

namespace VirtoCommerce.PricingModule.Data.Migrations
{
    [DbContext(typeof(PricingDbContext))]
    [Migration("20181017104018_InitialPricing")]
    partial class InitialPricing
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VirtoCommerce.PricingModule.Data.Model.PriceEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<decimal>("List");

                    b.Property<decimal>("MinQuantity");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("PricelistId")
                        .IsRequired();

                    b.Property<string>("ProductId")
                        .HasMaxLength(128);

                    b.Property<string>("ProductName")
                        .HasMaxLength(1024);

                    b.Property<decimal?>("Sale");

                    b.HasKey("Id");

                    b.HasIndex("PricelistId");

                    b.HasIndex("ProductId", "PricelistId")
                        .HasName("ProductIdAndPricelistId");

                    b.ToTable("Price");
                });

            modelBuilder.Entity("VirtoCommerce.PricingModule.Data.Model.PricelistAssignmentEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CatalogId")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("ConditionExpression");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(512);

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("PredicateVisualTreeSerialized");

                    b.Property<string>("PricelistId")
                        .IsRequired();

                    b.Property<int>("Priority");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("PricelistId");

                    b.ToTable("PricelistAssignment");
                });

            modelBuilder.Entity("VirtoCommerce.PricingModule.Data.Model.PricelistEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Description")
                        .HasMaxLength(512);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Pricelist");
                });

            modelBuilder.Entity("VirtoCommerce.PricingModule.Data.Model.PriceEntity", b =>
                {
                    b.HasOne("VirtoCommerce.PricingModule.Data.Model.PricelistEntity", "Pricelist")
                        .WithMany("Prices")
                        .HasForeignKey("PricelistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.PricingModule.Data.Model.PricelistAssignmentEntity", b =>
                {
                    b.HasOne("VirtoCommerce.PricingModule.Data.Model.PricelistEntity", "Pricelist")
                        .WithMany("Assignments")
                        .HasForeignKey("PricelistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
