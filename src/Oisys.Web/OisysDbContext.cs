﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            this.ChangeTracker.Tracked += ChangeTracker_Tracked;
        }

        private void ChangeTracker_Tracked(object sender, EntityTrackedEventArgs e)
        {
            Console.WriteLine($"Started tracking {e.Entry.Entity.GetType()}");
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
        public DbSet<CreditMemoLineItem> CreditMemoLineItems { get; set; }

        /// <inheritdoc />
        public DbSet<Customer> Customers { get; set; }

        /// <inheritdoc />
        public DbSet<CustomerTransaction> CustomerTransactions { get; set; }

        /// <inheritdoc />
        public DbSet<Delivery> Deliveries { get; set; }

        /// <inheritdoc />
        public DbSet<DeliveryLineItem> DeliveryDetails { get; set; }

        /// <inheritdoc />
        public DbSet<Invoice> Invoices { get; set; }

        /// <inheritdoc />
        public DbSet<InvoiceLineItem> InvoiceDetails { get; set; }

        /// <inheritdoc />
        public DbSet<Item> Items { get; set; }

        /// <inheritdoc />
        public DbSet<ItemHistory> ItemHistories { get; set; }

        /// <inheritdoc />
        public DbSet<Order> Orders { get; set; }

        /// <inheritdoc />
        public DbSet<OrderLineItem> OrderLineItems { get; set; }

        /// <inheritdoc />
        public DbSet<Province> Provinces { get; set; }

        /// <inheritdoc />
        public DbSet<SalesQuote> SalesQuotes { get; set; }

        /// <inheritdoc />
        public DbSet<SalesQuoteLineItem> SalesQuoteLineItems { get; set; }

        /// <inheritdoc />
        public DbSet<ApplicationUser> Users { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CreateCashVoucherModel(modelBuilder);
            CreateCategoryModel(modelBuilder);
            CreateCreditMemoModel(modelBuilder);
            CreateCustomerModel(modelBuilder);
            CreateDeliveryModel(modelBuilder);
            CreateInvoiceModel(modelBuilder);
            CreateItemModel(modelBuilder);
            CreateOrderModel(modelBuilder);
            CreateProvinceAndCityModels(modelBuilder);
            CreateSalesQuoteModels(modelBuilder);
            CreateUserModel(modelBuilder);
        }

        private static void CreateCashVoucherModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("VoucherNumber")
               .StartsAt(100000)
               .IncrementsBy(1);

            modelBuilder.Entity<CashVoucher>(entity =>
            {
                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                entity.Property(o => o.VoucherNumber)
                    .HasDefaultValueSql("NEXT VALUE FOR VoucherNumber");

                // TODO: Remove when migrated to sql server
                entity.Property(o => o.VoucherNumber)
                    .HasValueGenerator(typeof(VoucherNumberGenerator));
            });
        }

        private static void CreateCategoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                // Setup required fields
                entity.Property(p => p.Name).IsRequired();

                // Setup query filters
                entity.HasQueryFilter(c => !c.IsDeleted);

                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();
            });
        }

        private static void CreateCreditMemoModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("CreditMemoCode")
                    .StartsAt(100000)
                    .IncrementsBy(1);

            modelBuilder.Entity<CreditMemo>(entity =>
            {
                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                // Setup auto numbering
                entity.Property(o => o.Code)
                    .HasDefaultValueSql("NEXT VALUE FOR CreditMemoCode");

                // TODO: Remove when migrated to sql server
                entity.Property(o => o.Code)
                    .HasValueGenerator(typeof(CreditMemoCodeGenerator));
            });

            modelBuilder.Entity<CreditMemoLineItem>(entity =>
            {
                entity.HasOne(d => d.CreditMemo)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(p => p.CreditMemoId);

                entity.HasOne(d => d.TransactionHistory)
                     .WithOne(d => d.CreditMemoLineItem)
                     .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void CreateCustomerModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                // Setup required fields
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.CityId).IsRequired();
                entity.Property(p => p.ProvinceId).IsRequired();

                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                // Setup query filters
                entity.HasQueryFilter(c => !c.IsDeleted);

                // Setup value conversions
                entity.Property(a => a.PriceList).HasConversion<string>();
            });

            modelBuilder.Entity<CustomerTransaction>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(d => d.Transactions)
                    .HasForeignKey(p => p.CustomerId);
            });
        }

        private static void CreateDeliveryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("DeliveryNumber")
                .StartsAt(100000)
                .IncrementsBy(1);

            // Delivery
            modelBuilder.Entity<Delivery>(entity =>
            {
                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                entity.Property(o => o.DeliveryNumber)
                    .HasDefaultValueSql("NEXT VALUE FOR DeliveryNumber");

                // TODO: Remove when migrated to sql server
                entity.Property(o => o.DeliveryNumber)
                    .HasValueGenerator(typeof(DeliveryNumberCodeGenerator));
            });

            modelBuilder.Entity<DeliveryLineItem>(entity =>
            {
                entity.HasOne(d => d.Delivery)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(p => p.DeliveryId);
            });
        }

        private static void CreateInvoiceModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("InvoiceNumber")
                .StartsAt(100000)
                .IncrementsBy(1);

            // Invoice
            modelBuilder.Entity<Invoice>(entity =>
            {
                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                entity.Property(o => o.InvoiceNumber)
                    .HasDefaultValueSql("NEXT VALUE FOR InvoiceNumber");

                // TODO: Remove when migrated to sql server
                entity.Property(o => o.InvoiceNumber)
                    .HasValueGenerator(typeof(InvoiceNumberCodeGenerator));
            });

            // Invoice Line Items
            modelBuilder.Entity<InvoiceLineItem>(entity =>
            {
                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(p => p.InvoiceId);
            });
        }

        private static void CreateItemModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                // Setup required fields
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.Code).IsRequired();
                entity.Property(p => p.CategoryId).IsRequired();

                // Setup query filters
                entity.HasQueryFilter(c => !c.IsDeleted);

                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                // Setup default values
                entity.Property(a => a.MainPrice).HasDefaultValue(0);
                entity.Property(a => a.NEPrice).HasDefaultValue(0);
                entity.Property(a => a.WalkInPrice).HasDefaultValue(0);
            });

            modelBuilder.Entity<ItemHistory>(entity =>
            {
                // Setup foreign keys
                entity.HasOne(a => a.Item)
                    .WithMany(a => a.TransactionHistory)
                    .HasForeignKey(a => a.ItemId);
            });
        }

        private static void CreateOrderModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("OrderCode")
              .StartsAt(100000)
              .IncrementsBy(1);

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                // Setup required fields
                entity.Property(p => p.Code).IsRequired();

                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                // Setup auto numbering
                entity.Property(p => p.Code)
                    .HasDefaultValueSql("NEXT VALUE FOR OrderCode");

                // TODO: Remove when migrated to sql server
                entity.Property(o => o.Code)
                    .HasValueGenerator(typeof(OrderCodeGenerator));
            });

            // Order Detail
            modelBuilder.Entity<OrderLineItem>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(p => p.OrderId);

                entity.HasOne(d => d.TransactionHistory)
                    .WithOne(d => d.OrderLineItem)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void CreateProvinceAndCityModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Province>(entity =>
            {
                // Setup required fields
                entity.Property(p => p.Name).IsRequired();

                // Setup query filters
                entity.HasQueryFilter(c => !c.IsDeleted);

                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();
            });

            modelBuilder.Entity<City>(entity =>
            {
                // Setup required fields
                entity.Property(p => p.Name).IsRequired();

                // Setup query filters
                entity.HasQueryFilter(c => !c.IsDeleted);

                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();
            });
        }

        private static void CreateSalesQuoteModels(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("QuotationCode")
                .StartsAt(100000)
                .IncrementsBy(1);

            modelBuilder.Entity<SalesQuote>(entity =>
            {
                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                // Setup autonumbering
                entity.Property(o => o.QuoteNumber)
                    .HasDefaultValueSql("NEXT VALUE FOR QuotationCode");

                // TODO: Remove when migrated to sql server
                entity.Property(o => o.QuoteNumber)
                    .HasValueGenerator(typeof(SalesQuotationNumberGenerator));
            });

            modelBuilder.Entity<SalesQuoteLineItem>(entity =>
            {
                entity.HasOne(d => d.SalesQuote)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(p => p.SalesQuoteId);
            });
        }

        private static void CreateUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(entity => 
            {
                // Setup required fields
                entity.Property(p => p.Username).IsRequired();
                entity.Property(p => p.PasswordHash).IsRequired();
                entity.Property(p => p.Firstname).IsRequired();
                entity.Property(p => p.Lastname).IsRequired();

                // Setup query filters
                entity.HasQueryFilter(c => !c.IsDeleted);

                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();
            });

        }
    }
}
