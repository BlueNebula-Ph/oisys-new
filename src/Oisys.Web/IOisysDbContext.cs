﻿using Microsoft.EntityFrameworkCore;
using OisysNew.Models;

namespace OisysNew
{
    public interface IOisysDbContext : IDbContext
    {
        /// <summary>
        /// Gets or sets the adjustments db set.
        /// </summary>
        DbSet<Adjustment> Adjustments { get; set; }

        /// <summary>
        /// Gets or sets the cash voucher db set.
        /// </summary>
        DbSet<CashVoucher> CashVouchers { get; set; }

        /// <summary>
        /// Gets or sets the categories db set.
        /// </summary>
        DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the cities db set.
        /// </summary>
        DbSet<City> Cities { get; set; }

        /// <summary>
        /// Gets or sets the credit memos db set.
        /// </summary>
        DbSet<CreditMemo> CreditMemos { get; set; }

        /// <summary>
        /// Gets or sets the credit memo line items db set.
        /// </summary>
        DbSet<CreditMemoLineItem> CreditMemoLineItems { get; set; }

        /// <summary>
        /// Gets or sets the customers db set.
        /// </summary>
        DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Gets or sets the customer transactions db set.
        /// </summary>
        DbSet<CustomerTransaction> CustomerTransactions { get; set; }

        /// <summary>
        /// Gets or sets the deliveries db set.
        /// </summary>
        DbSet<Delivery> Deliveries { get; set; }

        /// <summary>
        /// Gets or sets the delivery line items db set.
        /// </summary>
        DbSet<DeliveryLineItem> DeliveryLineItems { get; set; }

        /// <summary>
        /// Gets or sets the invoice db set.
        /// </summary>
        DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// Gets or sets the invoice line items db set.
        /// </summary>
        DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }

        /// <summary>
        /// Gets or sets the items db set.
        /// </summary>
        DbSet<Item> Items { get; set; }

        /// <summary>
        /// Gets or sets the item transaction history db set.
        /// </summary>
        DbSet<ItemHistory> ItemHistories { get; set; }

        /// <summary>
        /// Gets or sets the orders db set.
        /// </summary>
        DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the order line items db set.
        /// </summary>
        DbSet<OrderLineItem> OrderLineItems { get; set; }

        /// <summary>
        /// Gets or sets the provinces db set.
        /// </summary>
        DbSet<Province> Provinces { get; set; }

        /// <summary>
        /// Gets or sets the sales quotes db set.
        /// </summary>
        DbSet<SalesQuote> SalesQuotes { get; set; }

        /// <summary>
        /// Gets or sets the sales quote line items db set.
        /// </summary>
        DbSet<SalesQuoteLineItem> SalesQuoteLineItems { get; set; }

        /// <summary>
        /// Gets or sets users db set.
        /// </summary>
        DbSet<ApplicationUser> Users { get; set; }
    }
}
