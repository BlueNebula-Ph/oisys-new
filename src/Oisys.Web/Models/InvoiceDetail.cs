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
        public long InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets property order id.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or sets the invoice navigation property.
        /// </summary>
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the order navigation property.
        /// </summary>
        public virtual Order Order { get; set; }
    }
}
