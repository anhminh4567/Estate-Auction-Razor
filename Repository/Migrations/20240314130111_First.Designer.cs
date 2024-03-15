﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Database;

#nullable disable

namespace Repository.Migrations
{
    [DbContext(typeof(AuctionRealEstateDbContext))]
    [Migration("20240314130111_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Repository.Database.Model.AppAccount.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"), 1L, 1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CMND")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("Repository.Database.Model.AppAccount.AccountImages", b =>
                {
                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.HasKey("ImageId", "AccountId");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountImages");
                });

            modelBuilder.Entity("Repository.Database.Model.AppAccount.JoinedAuction", b =>
                {
                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<int?>("AuctionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId", "AuctionId");

                    b.HasIndex("AuctionId");

                    b.ToTable("JoinedAuctions");
                });

            modelBuilder.Entity("Repository.Database.Model.AppImage", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageId"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImageId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.Auction", b =>
                {
                    b.Property<int>("AuctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuctionId"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndPayDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("EntranceFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("EstateId")
                        .HasColumnType("int");

                    b.Property<decimal>("IncrementPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MaxParticipant")
                        .HasColumnType("int");

                    b.Property<int?>("ReceiptId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("WantedPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("AuctionId");

                    b.HasIndex("EstateId");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.AuctionReceipt", b =>
                {
                    b.Property<int>("ReceiptId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReceiptId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("AuctionId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("BuyerId")
                        .HasColumnType("int");

                    b.Property<decimal>("Commission")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RemainAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ReceiptId");

                    b.HasIndex("AuctionId")
                        .IsUnique();

                    b.HasIndex("BuyerId");

                    b.ToTable("AuctionReceipts");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.AuctionReceiptPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("PayAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PayTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 3, 14, 20, 1, 11, 109, DateTimeKind.Local).AddTicks(3419));

                    b.Property<int?>("ReceiptId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ReceiptId");

                    b.ToTable("AuctionReceiptPayments");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.Bid", b =>
                {
                    b.Property<int>("BidId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BidId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AuctionId")
                        .HasColumnType("int");

                    b.Property<int>("BidderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("BidId");

                    b.HasIndex("AuctionId");

                    b.HasIndex("BidderId");

                    b.ToTable("Bids", (string)null);
                });

            modelBuilder.Entity("Repository.Database.Model.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 3, 14, 20, 1, 11, 112, DateTimeKind.Local).AddTicks(6789));

                    b.Property<bool>("IsChecked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotificationId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.Estate", b =>
                {
                    b.Property<int>("EstateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EstateId"), 1L, 1);

                    b.Property<float>("Area")
                        .HasColumnType("real");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Coordinate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<float>("Length")
                        .HasColumnType("real");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.HasKey("EstateId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Estates");
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.EstateCategories", b =>
                {
                    b.Property<int>("EstateId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("EstateId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("EstateCategories");
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.EstateCategoryDetail", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("EstateCategoryDetails");
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.EstateImages", b =>
                {
                    b.Property<int>("EstateId")
                        .HasColumnType("int");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.HasKey("EstateId", "ImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("EstateImages");
                });

            modelBuilder.Entity("Repository.Database.Model.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"), 1L, 1);

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vnp_Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vnp_OrderInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vnp_PayDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("vnp_TransactionDate")
                        .HasColumnType("bigint");

                    b.Property<string>("vnp_TxnRef")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Repository.Database.Model.AppAccount.Company", b =>
                {
                    b.HasBaseType("Repository.Database.Model.AppAccount.Account");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateStartOnPlatform")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EstablishDate")
                        .HasColumnType("datetime2");

                    b.ToTable("Company", (string)null);
                });

            modelBuilder.Entity("Repository.Database.Model.AppAccount.AccountImages", b =>
                {
                    b.HasOne("Repository.Database.Model.AppAccount.Account", "Account")
                        .WithMany("AccountImages")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repository.Database.Model.AppImage", "Image")
                        .WithMany("AccountImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Repository.Database.Model.AppAccount.JoinedAuction", b =>
                {
                    b.HasOne("Repository.Database.Model.AppAccount.Account", "Account")
                        .WithMany("JoinedAuctions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repository.Database.Model.AuctionRelated.Auction", "Auction")
                        .WithMany("JoinedAccounts")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Auction");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.Auction", b =>
                {
                    b.HasOne("Repository.Database.Model.RealEstate.Estate", "Estate")
                        .WithMany("Auctions")
                        .HasForeignKey("EstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estate");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.AuctionReceipt", b =>
                {
                    b.HasOne("Repository.Database.Model.AuctionRelated.Auction", "Auction")
                        .WithOne("AuctionReceipt")
                        .HasForeignKey("Repository.Database.Model.AuctionRelated.AuctionReceipt", "AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repository.Database.Model.AppAccount.Account", "Buyer")
                        .WithMany("AuctionsReceipt")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Auction");

                    b.Navigation("Buyer");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.AuctionReceiptPayment", b =>
                {
                    b.HasOne("Repository.Database.Model.AppAccount.Account", "Account")
                        .WithMany("ReceiptPayments")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Repository.Database.Model.AuctionRelated.AuctionReceipt", "Receipt")
                        .WithMany("Payments")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Account");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.Bid", b =>
                {
                    b.HasOne("Repository.Database.Model.AuctionRelated.Auction", "Auction")
                        .WithMany("Bids")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repository.Database.Model.AppAccount.Account", "Bidder")
                        .WithMany("Bids")
                        .HasForeignKey("BidderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auction");

                    b.Navigation("Bidder");
                });

            modelBuilder.Entity("Repository.Database.Model.Notification", b =>
                {
                    b.HasOne("Repository.Database.Model.AppAccount.Account", "Receiver")
                        .WithMany("RecievedNotifications")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Repository.Database.Model.AppAccount.Account", "Sender")
                        .WithMany("SentNotifications")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.Estate", b =>
                {
                    b.HasOne("Repository.Database.Model.AppAccount.Company", "Company")
                        .WithMany("OwnedEstate")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.EstateCategories", b =>
                {
                    b.HasOne("Repository.Database.Model.RealEstate.EstateCategoryDetail", "CategoryDetail")
                        .WithMany("Categories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repository.Database.Model.RealEstate.Estate", "Estate")
                        .WithMany("EstateCategory")
                        .HasForeignKey("EstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryDetail");

                    b.Navigation("Estate");
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.EstateImages", b =>
                {
                    b.HasOne("Repository.Database.Model.RealEstate.Estate", "Estate")
                        .WithMany("Images")
                        .HasForeignKey("EstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repository.Database.Model.AppImage", "Image")
                        .WithMany("EstateImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estate");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Repository.Database.Model.Transaction", b =>
                {
                    b.HasOne("Repository.Database.Model.AppAccount.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Repository.Database.Model.AppAccount.Company", b =>
                {
                    b.HasOne("Repository.Database.Model.AppAccount.Account", null)
                        .WithOne()
                        .HasForeignKey("Repository.Database.Model.AppAccount.Company", "AccountId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Repository.Database.Model.AppAccount.Account", b =>
                {
                    b.Navigation("AccountImages");

                    b.Navigation("AuctionsReceipt");

                    b.Navigation("Bids");

                    b.Navigation("JoinedAuctions");

                    b.Navigation("ReceiptPayments");

                    b.Navigation("RecievedNotifications");

                    b.Navigation("SentNotifications");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Repository.Database.Model.AppImage", b =>
                {
                    b.Navigation("AccountImages");

                    b.Navigation("EstateImages");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.Auction", b =>
                {
                    b.Navigation("AuctionReceipt");

                    b.Navigation("Bids");

                    b.Navigation("JoinedAccounts");
                });

            modelBuilder.Entity("Repository.Database.Model.AuctionRelated.AuctionReceipt", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.Estate", b =>
                {
                    b.Navigation("Auctions");

                    b.Navigation("EstateCategory");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("Repository.Database.Model.RealEstate.EstateCategoryDetail", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Repository.Database.Model.AppAccount.Company", b =>
                {
                    b.Navigation("OwnedEstate");
                });
#pragma warning restore 612, 618
        }
    }
}