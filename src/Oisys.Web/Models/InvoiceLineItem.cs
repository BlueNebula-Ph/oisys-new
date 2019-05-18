namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="InvoiceLineItem"/> for recording invoice details
    /// </summary>
    public class InvoiceLineItem : ModelBase
    {
        /// <summary>
        /// Gets or sets property invoice id.
        /// </summary>
        public long InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets property order id.
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// Gets or sets property credit memo id.
        /// </summary>
        public long? CreditMemoId { get; set; }

        /// <summary>
        /// Gets or sets the amount invoiced.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the invoice navigation property.
        /// </summary>
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the order navigation property.
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Gets or sets the credit memo nav property.
        /// </summary>
        public virtual CreditMemo CreditMemo { get; set; }
    }
}
