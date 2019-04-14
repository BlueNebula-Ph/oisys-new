using System;
using System.Collections.Generic;

namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="Invoice"/> class Invoice object.
    /// </summary>
    public class Invoice : ModelBase
    {
        /// <summary>
        /// Gets or sets property invoice number.
        /// </summary>
        public int InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets property CustomerId.
        /// </summary>
        public long CustomerId { get; set; }
        
        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets total amount due.
        /// This is the total amount of all orders in the invoice.
        /// </summary>
        public decimal TotalAmountDue { get; set; }

        /// <summary>
        /// Gets or sets total credit amount.
        /// This is the total amount of all credit memos in the invoice.
        /// </summary>
        public decimal TotalCreditAmount { get; set; }

        /// <summary>
        /// Gets or sets property discount in percent.
        /// </summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets property discount amount.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets total invoice amount.
        /// Total amount = Total amount due - total credit amount - discount amount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether invoice is paid.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Gets or sets the customer navigation property.
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the invoice details.
        /// </summary>
        public virtual ICollection<InvoiceLineItem> LineItems { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
