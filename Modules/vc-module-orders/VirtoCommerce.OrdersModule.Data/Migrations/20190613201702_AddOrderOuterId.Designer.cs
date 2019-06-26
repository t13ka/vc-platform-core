﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtoCommerce.OrdersModule.Data.Repositories;

namespace VirtoCommerce.OrdersModule.Data.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    [Migration("20190613201702_AddOrderOuterId")]
    partial class AddOrderOuterId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.AddressEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("AddressType")
                        .HasMaxLength(32);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("CountryCode")
                        .HasMaxLength(3);

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("CustomerOrderId");

                    b.Property<string>("Email")
                        .HasMaxLength(254);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Line1")
                        .HasMaxLength(2048);

                    b.Property<string>("Line2")
                        .HasMaxLength(2048);

                    b.Property<string>("Organization")
                        .HasMaxLength(64);

                    b.Property<string>("PaymentInId");

                    b.Property<string>("Phone")
                        .HasMaxLength(64);

                    b.Property<string>("PostalCode")
                        .HasMaxLength(64);

                    b.Property<string>("RegionId")
                        .HasMaxLength(128);

                    b.Property<string>("RegionName")
                        .HasMaxLength(128);

                    b.Property<string>("ShipmentId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerOrderId");

                    b.HasIndex("PaymentInId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("OrderAddress");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.CustomerOrderEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("CancelReason")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("CancelledDate");

                    b.Property<string>("ChannelId")
                        .HasMaxLength(64);

                    b.Property<string>("Comment")
                        .HasMaxLength(2048);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("CustomerName")
                        .HasMaxLength(255);

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("Money");

                    b.Property<decimal>("DiscountTotal")
                        .HasColumnType("Money");

                    b.Property<decimal>("DiscountTotalWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("EmployeeId")
                        .HasMaxLength(64);

                    b.Property<string>("EmployeeName")
                        .HasMaxLength(255);

                    b.Property<decimal>("HandlingTotal")
                        .HasColumnType("Money");

                    b.Property<decimal>("HandlingTotalWithTax")
                        .HasColumnType("Money");

                    b.Property<bool>("IsApproved");

                    b.Property<bool>("IsCancelled");

                    b.Property<bool>("IsPrototype");

                    b.Property<string>("LanguageCode")
                        .HasMaxLength(16);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("OrganizationId")
                        .HasMaxLength(64);

                    b.Property<string>("OrganizationName")
                        .HasMaxLength(255);

                    b.Property<string>("OuterId")
                        .HasMaxLength(128);

                    b.Property<decimal>("PaymentTotal")
                        .HasColumnType("Money");

                    b.Property<decimal>("PaymentTotalWithTax")
                        .HasColumnType("Money");

                    b.Property<decimal>("ShippingTotal")
                        .HasColumnType("Money");

                    b.Property<decimal>("ShippingTotalWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("ShoppingCartId")
                        .HasMaxLength(128);

                    b.Property<string>("Status")
                        .HasMaxLength(64);

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("StoreName")
                        .HasMaxLength(255);

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("Money");

                    b.Property<decimal>("SubTotalWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("SubscriptionId")
                        .HasMaxLength(64);

                    b.Property<string>("SubscriptionNumber")
                        .HasMaxLength(64);

                    b.Property<decimal>("Sum")
                        .HasColumnType("Money");

                    b.Property<decimal>("TaxPercentRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("TaxTotal")
                        .HasColumnType("Money");

                    b.Property<decimal>("Total")
                        .HasColumnType("Money");

                    b.HasKey("Id");

                    b.ToTable("CustomerOrder");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.DiscountEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("CouponCode")
                        .HasMaxLength(64);

                    b.Property<string>("CouponInvalidDescription")
                        .HasMaxLength(1024);

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("CustomerOrderId");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("Money");

                    b.Property<decimal>("DiscountAmountWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("LineItemId");

                    b.Property<string>("PaymentInId");

                    b.Property<string>("PromotionDescription")
                        .HasMaxLength(1024);

                    b.Property<string>("PromotionId")
                        .HasMaxLength(64);

                    b.Property<string>("ShipmentId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerOrderId");

                    b.HasIndex("LineItemId");

                    b.HasIndex("PaymentInId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("OrderDiscount");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.LineItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("CancelReason")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("CancelledDate");

                    b.Property<string>("CatalogId")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("CategoryId")
                        .HasMaxLength(64);

                    b.Property<string>("Comment")
                        .HasMaxLength(2048);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("CustomerOrderId");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("Money");

                    b.Property<decimal>("DiscountAmountWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("FulfillmentLocationCode")
                        .HasMaxLength(64);

                    b.Property<decimal?>("Height");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(1028);

                    b.Property<bool>("IsCancelled");

                    b.Property<bool>("IsGift");

                    b.Property<bool>("IsReccuring");

                    b.Property<decimal?>("Length");

                    b.Property<string>("MeasureUnit")
                        .HasMaxLength(32);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("OuterId")
                        .HasMaxLength(128);

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<string>("PriceId")
                        .HasMaxLength(128);

                    b.Property<decimal>("PriceWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("ProductType")
                        .HasMaxLength(64);

                    b.Property<int>("Quantity");

                    b.Property<string>("ShippingMethodCode")
                        .HasMaxLength(64);

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<decimal>("TaxPercentRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("TaxTotal")
                        .HasColumnType("Money");

                    b.Property<string>("TaxType")
                        .HasMaxLength(64);

                    b.Property<decimal?>("Weight");

                    b.Property<string>("WeightUnit")
                        .HasMaxLength(32);

                    b.Property<decimal?>("Width");

                    b.HasKey("Id");

                    b.HasIndex("CustomerOrderId");

                    b.ToTable("OrderLineItem");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.PaymentGatewayTransactionEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<decimal>("Amount")
                        .HasColumnType("Money");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Currency")
                        .HasMaxLength(3);

                    b.Property<string>("GatewayIpAddress")
                        .HasMaxLength(128);

                    b.Property<bool>("IsProcessed");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Note")
                        .HasMaxLength(2048);

                    b.Property<string>("OuterId")
                        .HasMaxLength(128);

                    b.Property<string>("PaymentInId")
                        .IsRequired();

                    b.Property<int>("ProcessAttemptCount");

                    b.Property<string>("ProcessError")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("ProcessedDate");

                    b.Property<string>("RequestData");

                    b.Property<string>("ResponseCode")
                        .HasMaxLength(64);

                    b.Property<string>("ResponseData");

                    b.Property<string>("Status")
                        .HasMaxLength(64);

                    b.Property<string>("Type")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("PaymentInId");

                    b.ToTable("OrderPaymentGatewayTransaction");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.PaymentInEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<DateTime?>("AuthorizedDate");

                    b.Property<string>("CancelReason")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("CancelledDate");

                    b.Property<DateTime?>("CapturedDate");

                    b.Property<string>("Comment")
                        .HasMaxLength(2048);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("CustomerName")
                        .HasMaxLength(255);

                    b.Property<string>("CustomerOrderId");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("Money");

                    b.Property<decimal>("DiscountAmountWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("GatewayCode")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("IncomingDate");

                    b.Property<bool>("IsApproved");

                    b.Property<bool>("IsCancelled");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("OrganizationId")
                        .HasMaxLength(64);

                    b.Property<string>("OrganizationName")
                        .HasMaxLength(255);

                    b.Property<string>("OuterId")
                        .HasMaxLength(128);

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<decimal>("PriceWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("Purpose")
                        .HasMaxLength(1024);

                    b.Property<string>("ShipmentId");

                    b.Property<string>("Status")
                        .HasMaxLength(64);

                    b.Property<decimal>("Sum")
                        .HasColumnType("Money");

                    b.Property<decimal>("TaxPercentRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("TaxTotal")
                        .HasColumnType("Money");

                    b.Property<string>("TaxType")
                        .HasMaxLength(64);

                    b.Property<decimal>("Total")
                        .HasColumnType("Money");

                    b.Property<decimal>("TotalWithTax")
                        .HasColumnType("Money");

                    b.Property<DateTime?>("VoidedDate");

                    b.HasKey("Id");

                    b.HasIndex("CustomerOrderId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("OrderPaymentIn");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.ShipmentEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("CancelReason")
                        .HasMaxLength(2048);

                    b.Property<DateTime?>("CancelledDate");

                    b.Property<string>("Comment")
                        .HasMaxLength(2048);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<string>("CustomerOrderId")
                        .IsRequired();

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("Money");

                    b.Property<decimal>("DiscountAmountWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("EmployeeId")
                        .HasMaxLength(64);

                    b.Property<string>("EmployeeName")
                        .HasMaxLength(255);

                    b.Property<string>("FulfillmentCenterId")
                        .HasMaxLength(64);

                    b.Property<string>("FulfillmentCenterName")
                        .HasMaxLength(255);

                    b.Property<decimal?>("Height");

                    b.Property<bool>("IsApproved");

                    b.Property<bool>("IsCancelled");

                    b.Property<decimal?>("Length");

                    b.Property<string>("MeasureUnit")
                        .HasMaxLength(32);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("OrganizationId")
                        .HasMaxLength(64);

                    b.Property<string>("OrganizationName")
                        .HasMaxLength(255);

                    b.Property<string>("OuterId")
                        .HasMaxLength(128);

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<decimal>("PriceWithTax")
                        .HasColumnType("Money");

                    b.Property<string>("ShipmentMethodCode")
                        .HasMaxLength(64);

                    b.Property<string>("ShipmentMethodOption")
                        .HasMaxLength(64);

                    b.Property<string>("Status")
                        .HasMaxLength(64);

                    b.Property<decimal>("Sum")
                        .HasColumnType("Money");

                    b.Property<decimal>("TaxPercentRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("TaxTotal")
                        .HasColumnType("Money");

                    b.Property<string>("TaxType")
                        .HasMaxLength(64);

                    b.Property<decimal>("Total")
                        .HasColumnType("Money");

                    b.Property<decimal>("TotalWithTax")
                        .HasColumnType("Money");

                    b.Property<decimal?>("VolumetricWeight");

                    b.Property<decimal?>("Weight");

                    b.Property<string>("WeightUnit")
                        .HasMaxLength(32);

                    b.Property<decimal?>("Width");

                    b.HasKey("Id");

                    b.HasIndex("CustomerOrderId");

                    b.ToTable("OrderShipment");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.ShipmentItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("BarCode")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("LineItemId")
                        .IsRequired();

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("OuterId")
                        .HasMaxLength(128);

                    b.Property<int>("Quantity");

                    b.Property<string>("ShipmentId")
                        .IsRequired();

                    b.Property<string>("ShipmentPackageId");

                    b.HasKey("Id");

                    b.HasIndex("LineItemId");

                    b.HasIndex("ShipmentId");

                    b.HasIndex("ShipmentPackageId");

                    b.ToTable("OrderShipmentItem");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.ShipmentPackageEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("BarCode")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<decimal?>("Height");

                    b.Property<decimal?>("Length");

                    b.Property<string>("MeasureUnit")
                        .HasMaxLength(32);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("OuterId")
                        .HasMaxLength(128);

                    b.Property<string>("PackageType")
                        .HasMaxLength(64);

                    b.Property<string>("ShipmentId")
                        .IsRequired();

                    b.Property<decimal?>("Weight");

                    b.Property<string>("WeightUnit")
                        .HasMaxLength(32);

                    b.Property<decimal?>("Width");

                    b.HasKey("Id");

                    b.HasIndex("ShipmentId");

                    b.ToTable("OrderShipmentPackage");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.TaxDetailEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<decimal>("Amount")
                        .HasColumnType("Money");

                    b.Property<string>("CustomerOrderId");

                    b.Property<string>("LineItemId");

                    b.Property<string>("Name")
                        .HasMaxLength(1024);

                    b.Property<string>("PaymentInId");

                    b.Property<decimal>("Rate");

                    b.Property<string>("ShipmentId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerOrderId");

                    b.HasIndex("LineItemId");

                    b.HasIndex("PaymentInId");

                    b.HasIndex("ShipmentId");

                    b.ToTable("OrderTaxDetail");
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.AddressEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.CustomerOrderEntity", "CustomerOrder")
                        .WithMany("Addresses")
                        .HasForeignKey("CustomerOrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.PaymentInEntity", "PaymentIn")
                        .WithMany("Addresses")
                        .HasForeignKey("PaymentInId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.ShipmentEntity", "Shipment")
                        .WithMany("Addresses")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.DiscountEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.CustomerOrderEntity", "CustomerOrder")
                        .WithMany("Discounts")
                        .HasForeignKey("CustomerOrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.LineItemEntity", "LineItem")
                        .WithMany("Discounts")
                        .HasForeignKey("LineItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.PaymentInEntity", "PaymentIn")
                        .WithMany("Discounts")
                        .HasForeignKey("PaymentInId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.ShipmentEntity", "Shipment")
                        .WithMany("Discounts")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.LineItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.CustomerOrderEntity", "CustomerOrder")
                        .WithMany("Items")
                        .HasForeignKey("CustomerOrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.PaymentGatewayTransactionEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.PaymentInEntity", "PaymentIn")
                        .WithMany("Transactions")
                        .HasForeignKey("PaymentInId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.PaymentInEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.CustomerOrderEntity", "CustomerOrder")
                        .WithMany("InPayments")
                        .HasForeignKey("CustomerOrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.ShipmentEntity", "Shipment")
                        .WithMany("InPayments")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.ShipmentEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.CustomerOrderEntity", "CustomerOrder")
                        .WithMany("Shipments")
                        .HasForeignKey("CustomerOrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.ShipmentItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.LineItemEntity", "LineItem")
                        .WithMany()
                        .HasForeignKey("LineItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.ShipmentEntity", "Shipment")
                        .WithMany("Items")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.ShipmentPackageEntity", "ShipmentPackage")
                        .WithMany("Items")
                        .HasForeignKey("ShipmentPackageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.ShipmentPackageEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.ShipmentEntity", "Shipment")
                        .WithMany("Packages")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.OrdersModule.Data.Model.TaxDetailEntity", b =>
                {
                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.CustomerOrderEntity", "CustomerOrder")
                        .WithMany("TaxDetails")
                        .HasForeignKey("CustomerOrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.LineItemEntity", "LineItem")
                        .WithMany("TaxDetails")
                        .HasForeignKey("LineItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.PaymentInEntity", "PaymentIn")
                        .WithMany("TaxDetails")
                        .HasForeignKey("PaymentInId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtoCommerce.OrdersModule.Data.Model.ShipmentEntity", "Shipment")
                        .WithMany("TaxDetails")
                        .HasForeignKey("ShipmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
