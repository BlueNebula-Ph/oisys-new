using Microsoft.EntityFrameworkCore;
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
        public DbSet<DeliveryLineItem> DeliveryLineItems { get; set; }

        /// <inheritdoc />
        public DbSet<Invoice> Invoices { get; set; }

        /// <inheritdoc />
        public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }

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

                // Setup decimal precision
                entity.Property(p => p.Amount).HasColumnType("decimal(18, 2)");

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

                // Setup decimal precision
                entity.Property(p => p.TotalAmount).HasColumnType("decimal(18, 2)");

                // Setup auto numbering
                entity.Property(o => o.Code)
                    .HasDefaultValueSql("NEXT VALUE FOR CreditMemoCode");

                // TODO: Remove when migrated to sql server
                entity.Property(o => o.Code)
                    .HasValueGenerator(typeof(CreditMemoCodeGenerator));
            });

            modelBuilder.Entity<CreditMemoLineItem>(entity =>
            {
                // Setup decimal precision
                entity.Property(p => p.UnitPrice).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.TotalPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.CreditMemo)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(p => p.CreditMemoId);

                entity.HasOne(d => d.OrderLineItem)
                    .WithMany()
                    .HasForeignKey(d => d.OrderLineItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Item)
                    .WithMany()
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void CreateCustomerModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                // Setup required fields
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.ProvinceId).IsRequired();
                entity.Property(p => p.CityId).IsRequired();

                // Setup decimal precision
                entity.Property(p => p.Discount).HasColumnType("decimal(18, 2)");

                // Setup concurrency checks
                entity.Property(p => p.RowVersion).IsRowVersion();

                // Setup query filters
                entity.HasQueryFilter(c => !c.IsDeleted);

                // Setup value conversions
                entity.Property(a => a.PriceList).HasConversion<string>();

                // Setup foreign keys
                entity.HasOne(p => p.Province)
                    .WithMany()
                    .HasForeignKey(a => a.ProvinceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.City)
                    .WithMany()
                    .HasForeignKey(a => a.CityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CustomerTransaction>(entity =>
            {
                // Setup decimal precision
                entity.Property(p => p.Amount).HasColumnType("decimal(18, 2)");

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

                // Setup decimal precision
                entity.Property(p => p.DiscountPercent).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.DiscountAmount).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.TotalAmount).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.TotalAmountDue).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.TotalCreditAmount).HasColumnType("decimal(18, 2)");

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

                entity.HasOne(d => d.CreditMemo)
                    .WithMany()
                    .HasForeignKey(d => d.CreditMemoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
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

                // Setup decimal precision
                entity.Property(p => p.MainPrice).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.NEPrice).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.WalkInPrice).HasColumnType("decimal(18, 2)");

                // Setup foreign keys
                entity.HasOne(p => p.Category)
                    .WithMany()
                    .HasForeignKey(a => a.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ItemHistory>(entity =>
            {
                // Setup foreign keys
                entity.HasOne(a => a.Item)
                    .WithMany(a => a.TransactionHistory)
                    .HasForeignKey(a => a.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.OrderLineItem)
                    .WithOne(a => a.TransactionHistory)
                    .HasForeignKey<ItemHistory>(a => a.OrderLineItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.CreditMemoLineItem)
                    .WithOne(a => a.TransactionHistory)
                    .HasForeignKey<ItemHistory>(a => a.CreditMemoLineItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Adjustment)
                    .WithOne(a => a.TransactionHistory)
                    .HasForeignKey<ItemHistory>(a => a.AdjustmentId)
                    .OnDelete(DeleteBehavior.Cascade);
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

                // Setup decimal precision
                entity.Property(p => p.DiscountPercent).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.DiscountAmount).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.GrossAmount).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.TotalAmount).HasColumnType("decimal(18, 2)");

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
                // Setup decimal precision
                entity.Property(p => p.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Item)
                    .WithMany()
                    .HasForeignKey(a => a.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
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

                // Setup foreign key
                entity.HasOne(p => p.Province)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(p => p.ProvinceId)
                    .OnDelete(DeleteBehavior.Restrict);
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

                // Setup decimal precision
                entity.Property(p => p.DeliveryFee).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.TotalAmount).HasColumnType("decimal(18, 2)");

                // Setup autonumbering
                entity.Property(o => o.QuoteNumber)
                    .HasDefaultValueSql("NEXT VALUE FOR QuotationCode");

                // TODO: Remove when migrated to sql server
                entity.Property(o => o.QuoteNumber)
                    .HasValueGenerator(typeof(SalesQuotationNumberGenerator));
            });

            modelBuilder.Entity<SalesQuoteLineItem>(entity =>
            {
                // Setup decimal precision
                entity.Property(p => p.TotalPrice).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.SalesQuote)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(p => p.SalesQuoteId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Item)
                    .WithMany()
                    .HasForeignKey(p => p.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
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
