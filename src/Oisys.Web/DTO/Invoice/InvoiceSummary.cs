using System.Collections.Generic;

namespace OisysNew.DTO.Invoice
{
    /// <summary>
    /// View model for the invoice entity.
    /// </summary>
    public class InvoiceSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets invoice number;
        /// </summary>
        public int InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the customer address.
        /// </summary>
        public string CustomerAddress { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets gross amount.
        /// </summary>
        public decimal GrossAmount { get; set; }

        /// <summary>
        /// Gets or sets property discount in percent.
        /// </summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets property discount amount.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets property TotalAmount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether invoice is paid.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Gets or sets invoice line items.
        /// </summary>
        public IEnumerable<InvoiceLineItemSummary> LineItems { get; set; }
    }
}
