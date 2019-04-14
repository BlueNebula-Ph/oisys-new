using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.Invoice
{
    /// <summary>
    /// <see cref="SaveInvoiceRequest"/> class is used for data transfer for creating and updating invoices.
    /// </summary>
    public class SaveInvoiceRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        public int InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the invoice date.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the total amount due from orders.
        /// </summary>
        public decimal TotalAmountDue { get; set; }

        /// <summary>
        /// Gets or sets the total amount from credit memos.
        /// </summary>
        public decimal TotalCreditAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount percent.
        /// </summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets the discount amount.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the total net amount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a list of invoice details.
        /// </summary>
        public List<SaveInvoiceLineItemRequest> LineItems { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
