using OisysNew.DTO.Customer;
using System.Collections.Generic;

namespace OisysNew.DTO.Invoice
{
    public class InvoiceDetail : DTOBase
    {
        /// <summary>
        /// Gets or sets invoice number.
        /// </summary>
        public int InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets invoice customer.
        /// </summary>
        public CustomerLookup Customer { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the total amount from orders.
        /// </summary>
        public decimal TotalAmountDue { get; set; }

        /// <summary>
        /// Gets or sets the total credit amount.
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
        /// Gets or sets property TotalAmount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether invoice is paid.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Gets or sets the invoice line items.
        /// </summary>
        public IEnumerable<InvoiceDetailLineItem> LineItems { get; set; }
    }
}
