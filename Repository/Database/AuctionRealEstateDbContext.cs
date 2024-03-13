using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database
{
    public class AuctionRealEstateDbContext : DbContext
    {
        public AuctionRealEstateDbContext()
        {
        }

        public AuctionRealEstateDbContext(DbContextOptions options) : base(options)
        {
        }

        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<AppImage> Images { get; set; }
        public DbSet<AccountImages> AccountImages { get; set; }
        public DbSet<EstateImages> EstateImages { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionReceipt> AuctionReceipts { get; set; }
        public DbSet<AuctionReceiptPayment> AuctionReceiptPayments { get; set; }
        public DbSet<JoinedAuction> JoinedAuctions { get; set; }
        public DbSet<Bid> BidLogs { get; set; }
        public DbSet<Estate> Estates { get; set; }
        public DbSet<EstateCategories> EstateCategories { get; set; }
        public DbSet<EstateCategoryDetail> EstateCategoryDetails { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
			//optionsBuilder.UseSqlServer("server =(local);uid=sa;pwd=12345;Database=AuctionDatabase;TrustServerCertificate=true;");
			optionsBuilder.UseSqlServer("server=LAPTOP-HUELJUER\\SQLSERVER19;uid=sa;pwd=12345;Database=AuctionDatabase;TrustServerCertificate=true;");
		}

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            configurationBuilder.Properties<Enum>().HaveConversion<string>();
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Account>(a =>
            {
                var roleConverter = new EnumToStringConverter<Role>();
                var statusConverter = new EnumToStringConverter<AccountStatus>();
                a.Property(a => a.Role).HasConversion(roleConverter);
                a.Property(a => a.Status).HasConversion(statusConverter);
                a.ToTable("Account");
            });
            builder.Entity<AccountImages>(a => 
            {
                a.HasKey(k => new { k.ImageId , k.AccountId});
                a.HasOne(a => a.Account).WithMany(a => a.AccountImages).HasForeignKey(k => k.AccountId).OnDelete(DeleteBehavior.Cascade) ;
                a.HasOne(a => a.Image).WithMany(a => a.AccountImages).HasForeignKey(k => k.ImageId).OnDelete(DeleteBehavior.Cascade);
            }); 
            builder.Entity<Transaction>(t => 
            {
                t.HasOne(t => t.Account).WithMany(a => a.Transactions).HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.SetNull);
            });
            builder.Entity<Company>(c =>
            {
                c.ToTable("Company");
                c.Property(c => c.AccountId).HasColumnName("Id");
                //c.HasOne<Account>().WithOne().HasForeignKey<Company>(c => c.AccountId).OnDelete(DeleteBehavior.Cascade);
                //c.HasMany(c => c.OwnedEstate).WithOne(a => a.Company).HasForeignKey(e => e.CompanyId);
            });
            builder.Entity<Auction>(a => 
            {
                a.HasOne(a => a.Estate).WithMany(e => e.Auctions).HasForeignKey(k => k.EstateId); 
				a.HasOne(a => a.AuctionReceipt).WithOne(a => a.Auction).HasForeignKey<AuctionReceipt>(a => a.AuctionId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
			});
            builder.Entity<AuctionReceipt>(a => 
            {
                a.HasOne(a=> a.Buyer).WithMany(a => a.AuctionsReceipt).HasForeignKey(a => a.BuyerId).OnDelete(DeleteBehavior.SetNull);
            });
            builder.Entity<AuctionReceiptPayment>(a => 
            {
                //a.HasKey(k => new { k.AccountId, k.ReceiptId });
                a.HasOne(a => a.Account).WithMany(a => a.ReceiptPayments).HasForeignKey(a => a.AccountId).OnDelete(DeleteBehavior.Cascade); 
				a.HasOne(a => a.Receipt).WithMany(r => r.Payments).HasForeignKey(a => a.ReceiptId).OnDelete(DeleteBehavior.Cascade);
                a.Property(a => a.PayTime).HasDefaultValue(DateTime.Now);
			});
            builder.Entity<JoinedAuction>(j =>
			{
				var statusConverter = new EnumToStringConverter<JoinedAuctionStatus>();
				j.HasKey(k => new { k.AccountId, k.AuctionId });
				j.HasOne(j => j.Auction).WithMany(a => a.JoinedAccounts).HasForeignKey(j => j.AuctionId).OnDelete(DeleteBehavior.Cascade);
				j.HasOne(j => j.Account).WithMany(a => a.JoinedAuctions).HasForeignKey(j => j.AccountId).OnDelete(DeleteBehavior.Cascade);
                //j.HasOne(j => j.Transaction).WithOne(t => t.RegisterAuction).HasForeignKey<JoinedAuction>(t => t.TransactionId).IsRequired(false);
				j.Property(j => j.Status).HasConversion(statusConverter);
			});
            builder.Entity<Bid>(b => 
            {
                b.ToTable("Bids");
                //b.HasAlternateKey(ck => new { ck.BidderId, ck.AuctionId});
                b.HasOne(b =>b.Bidder).WithMany(a => a.Bids).HasForeignKey(b => b.BidderId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(b=> b.Auction).WithMany(a => a.Bids).HasForeignKey(b => b.AuctionId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Estate>(e => 
            {
				var statusConverter = new EnumToStringConverter<EstateStatus>();
                e.Property(e => e.Status).HasConversion(statusConverter);
				e.HasOne(e => e.Company).WithMany(c => c.OwnedEstate).HasForeignKey(k => k.CompanyId).OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<EstateImages>(e => 
            {
                e.HasKey(k => new { k.EstateId, k.ImageId});
                e.HasOne(e => e.Estate).WithMany(e => e.Images).HasForeignKey(k => k.EstateId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(e => e.Image).WithMany(i => i.EstateImages).HasForeignKey(k => k.ImageId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<EstateCategories>(e => 
            {
                e.HasKey(ck => new { ck.EstateId, ck.CategoryId } );
                e.HasOne(e => e.Estate).WithMany( e => e.EstateCategory).HasForeignKey(k => k.EstateId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne( e => e.CategoryDetail).WithMany(e => e.Categories).HasForeignKey(k => k.CategoryId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Notification>(e =>
            {
                var type = new EnumToStringConverter<NotificationType>();
                e.Property(e => e.Type).HasConversion(type);
                e.Property(e => e.IsChecked).HasDefaultValue(false);
                e.Property(e => e.CreatedDate).HasDefaultValue(DateTime.Now);
                e.ToTable("Notifications");
            });
        }
    }
}
