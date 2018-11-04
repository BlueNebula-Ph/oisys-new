using Microsoft.EntityFrameworkCore;
using OisysNew.Models;
using OisysNew.SeedData;
using System;

namespace OisysNew
{
    /// <summary>
    /// <see cref="OisysDbContext"/> class DbContext.
    /// </summary>
    public class OisysDbContext : DbContext, IOisysDbContext, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OisysDbContext"/> class.
        /// </summary>
        /// <param name="options">Context options</param>
        public OisysDbContext(DbContextOptions<OisysDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public DbSet<Adjustment> Adjustments { get; set; }

        /// <inheritdoc />
        public DbSet<CashVoucher> CashVouchers { get; set; }

        /// <inheritdoc />
        public DbSet<Category> Categories { get; set; }

        /// <inheritdoc />
        public DbSet<City> Cities { get; set; }

        /// <inheritdoc />
        public DbSet<CreditMemo> CreditMemos { get; set; }

        /// <inheritdoc />
        public DbSet<CreditMemoDetail> CreditMemoDetails { get; set; }

        /// <inheritdoc />
        public DbSet<Customer> Customers { get; set; }

        /// <inheritdoc />
        public DbSet<CustomerTransaction> CustomerTransactions { get; set; }

        /// <inheritdoc />
        public DbSet<Delivery> Deliveries { get; set; }

        /// <inheritdoc />
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }

        /// <inheritdoc />
        public DbSet<Invoice> Invoices { get; set; }

        /// <inheritdoc />
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        /// <inheritdoc />
        public DbSet<Item> Items { get; set; }

        /// <inheritdoc />
        public DbSet<Order> Orders { get; set; }

        /// <inheritdoc />
        public DbSet<OrderDetail> OrderDetails { get; set; }

        /// <inheritdoc />
        public DbSet<Province> Provinces { get; set; }

        /// <inheritdoc />
        public DbSet<SalesQuote> SalesQuotes { get; set; }

        /// <inheritdoc />
        public DbSet<SalesQuoteDetail> SalesQuoteDetails { get; set; }

        /// <inheritdoc />
        public DbSet<ApplicationUser> Users { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Adjustments
            modelBuilder.Entity<Adjustment>()
                .HasOne<Item>(d => d.Item)
                .WithMany(p => p.Adjustments)
                .HasForeignKey(p => p.ItemId);

            // Category
            modelBuilder.Entity<Category>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Category>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            // City
            modelBuilder.Entity<City>()
                .HasQueryFilter(c => !c.IsDeleted);

            // Credit Memo Detail
            modelBuilder.Entity<CreditMemoDetail>()
                .HasOne<CreditMemo>(d => d.CreditMemo)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.CreditMemoId);

            // Customer
            modelBuilder.Entity<Customer>()
                .HasQueryFilter(c => !c.IsDeleted);

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
            modelBuilder.Entity<Order>()
                .HasQueryFilter(o => !o.IsDeleted);

            modelBuilder.Entity<OrderDetail>()
                .HasOne<Order>(d => d.Order)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.OrderId);

            // Province
            modelBuilder.Entity<Province>()
                .HasQueryFilter(p => !p.IsDeleted);

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
