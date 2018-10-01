namespace OisysNew
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using OisysNew.Models;
    using OisysNew.SeedData;

    /// <summary>
    /// <see cref="OisysDbContext"/> class DbContext.
    /// </summary>
    public class OisysDbContext : DbContext, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OisysDbContext"/> class.
        /// </summary>
        /// <param name="options">Context options</param>
        public OisysDbContext(DbContextOptions<OisysDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the adjustments db set.
        /// </summary>
        public DbSet<Adjustment> Adjustments { get; set; }

        /// <summary>
        /// Gets or sets the cash voucher db set.
        /// </summary>
        public DbSet<CashVoucher> CashVouchers { get; set; }

        /// <summary>
        /// Gets or sets the categories db set.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the cities db set.
        /// </summary>
        public DbSet<City> Cities { get; set; }

        /// <summary>
        /// Gets or sets the credit memos db set.
        /// </summary>
        public DbSet<CreditMemo> CreditMemos { get; set; }

        /// <summary>
        /// Gets or sets the credit memo details db set.
        /// </summary>
        public DbSet<CreditMemoDetail> CreditMemoDetails { get; set; }

        /// <summary>
        /// Gets or sets the customers db set.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Gets or sets the customer transactions db set.
        /// </summary>
        public DbSet<CustomerTransaction> CustomerTransactions { get; set; }

        /// <summary>
        /// Gets or sets the deliveries db set.
        /// </summary>
        public DbSet<Delivery> Deliveries { get; set; }

        /// <summary>
        /// Gets or sets the delivery details db set.
        /// </summary>
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }

        /// <summary>
        /// Gets or sets the invoice db set.
        /// </summary>
        public DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// Gets or sets the invoice details db set.
        /// </summary>
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        /// <summary>
        /// Gets or sets the items db set.
        /// </summary>
        public DbSet<Item> Items { get; set; }

        /// <summary>
        /// Gets or sets the orders db set.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the order details db set.
        /// </summary>
        public DbSet<OrderDetail> OrderDetails { get; set; }

        /// <summary>
        /// Gets or sets the provinces db set.
        /// </summary>
        public DbSet<Province> Provinces { get; set; }

        /// <summary>
        /// Gets or sets the sales quotes db set.
        /// </summary>
        public DbSet<SalesQuote> SalesQuotes { get; set; }

        /// <summary>
        /// Gets or sets the sales quote details db set.
        /// </summary>
        public DbSet<SalesQuoteDetail> SalesQuoteDetails { get; set; }

        /// <summary>
        /// Gets or sets users db set.
        /// </summary>
        public DbSet<ApplicationUser> Users { get; set; }

        /// <summary>
        /// This method sets up the foreign keys of entities
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Item Adjustments
            modelBuilder.Entity<Adjustment>()
                .HasOne<Item>(d => d.Item)
                .WithMany(p => p.Adjustments)
                .HasForeignKey(p => p.ItemId);

            // Category
            modelBuilder.Entity<Category>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            // Credit Memo Detail
            modelBuilder.Entity<CreditMemoDetail>()
                .HasOne<CreditMemo>(d => d.CreditMemo)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.CreditMemoId);

            // Customer
            modelBuilder.Entity<CustomerTransaction>()
                .HasOne<Customer>(d => d.Customer)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.CustomerId);

            // Delivery Details
            modelBuilder.Entity<DeliveryDetail>()
                .HasOne<Delivery>(d => d.Delivery)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.DeliveryId);

            // Invoice Details
            modelBuilder.Entity<InvoiceDetail>()
                .HasOne<Invoice>(d => d.Invoice)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.InvoiceId);

            // Order
            modelBuilder.Entity<OrderDetail>()
                .HasOne<Order>(d => d.Order)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.OrderId);

            // Sales Quote
            modelBuilder.Entity<SalesQuoteDetail>()
                .HasOne<SalesQuote>(d => d.SalesQuote)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.SalesQuoteId);

            modelBuilder.HasSequence<int>("CreditMemoCode")
                .StartsAt(100000)
                .IncrementsBy(1);

            modelBuilder.Entity<CreditMemo>()
                .Property(o => o.Code)
                .HasDefaultValueSql("NEXT VALUE FOR CreditMemoCode");

            // TODO: Remove when migrated to sql server
            modelBuilder.Entity<CreditMemo>()
                .Property(o => o.Code)
                .HasValueGenerator(typeof(CreditMemoCodeGenerator));

            modelBuilder.HasSequence<int>("OrderCode")
               .StartsAt(100000)
               .IncrementsBy(1);

            modelBuilder.Entity<Order>()
                .Property(o => o.Code)
                .HasDefaultValueSql("NEXT VALUE FOR OrderCode");

            // TODO: Remove when migrated to sql server
            modelBuilder.Entity<Order>()
                .Property(o => o.Code)
                .HasValueGenerator(typeof(OrderCodeGenerator));

            modelBuilder.HasSequence<int>("VoucherNumber")
               .StartsAt(100000)
               .IncrementsBy(1);

            modelBuilder.Entity<CashVoucher>()
                .Property(o => o.VoucherNumber)
                .HasDefaultValueSql("NEXT VALUE FOR VoucherNumber");

            // TODO: Remove when migrated to sql server
            modelBuilder.Entity<CashVoucher>()
                .Property(o => o.VoucherNumber)
                .HasValueGenerator(typeof(VoucherNumberGenerator));

            modelBuilder.HasSequence<int>("QuotationCode")
                .StartsAt(100000)
                .IncrementsBy(1);

            modelBuilder.Entity<SalesQuote>()
                .Property(o => o.QuoteNumber)
                .HasDefaultValueSql("NEXT VALUE FOR QuotationCode");

            // TODO: Remove when migrated to sql server
            modelBuilder.Entity<SalesQuote>()
                .Property(o => o.QuoteNumber)
                .HasValueGenerator(typeof(SalesQuotationNumberGenerator));

            modelBuilder.HasSequence<int>("DeliveryNumber")
                .StartsAt(100000)
                .IncrementsBy(1);

            modelBuilder.Entity<Delivery>()
                .Property(o => o.DeliveryNumber)
                .HasDefaultValueSql("NEXT VALUE FOR DeliveryNumber");

            // TODO: Remove when migrated to sql server
            modelBuilder.Entity<Delivery>()
                .Property(o => o.DeliveryNumber)
                .HasValueGenerator(typeof(DeliveryNumberCodeGenerator));

            modelBuilder.HasSequence<int>("InvoiceNumber")
                .StartsAt(100000)
                .IncrementsBy(1);

            modelBuilder.Entity<Invoice>()
                .Property(o => o.InvoiceNumber)
                .HasDefaultValueSql("NEXT VALUE FOR InvoiceNumber");

            // TODO: Remove when migrated to sql server
            modelBuilder.Entity<Invoice>()
                .Property(o => o.InvoiceNumber)
                .HasValueGenerator(typeof(InvoiceNumberCodeGenerator));
        }
    }
}
