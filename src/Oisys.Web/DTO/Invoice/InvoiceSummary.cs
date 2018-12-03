using System;

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
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets customer id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets customer.
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets the invoice date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
