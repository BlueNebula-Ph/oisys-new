namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="InvoiceDetail"/> for recording invoice details
    /// </summary>
    public class InvoiceDetail : ModelBase
    {
        /// <summary>
        /// Gets or sets property invoice id.
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets property order id.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the invoice navigation property.
        /// </summary>
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the order navigation property.
        /// </summary>
        public Order Order { get; set; }
    }
}
