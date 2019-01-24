namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="SalesQuote"/> class represents child of SalesQuote created in the application.
    /// </summary>
    public class SalesQuoteLineItem : ModelBase
    {
        /// <summary>
        /// Gets or sets property SalesQuoteId.
        /// </summary>
        public long SalesQuoteId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property ItemId.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets property Unit Price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets property TotalPrice.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets SalesQuote navigation property.
        /// </summary>
        public virtual SalesQuote SalesQuote { get; set; }

        /// <summary>
        /// Gets or sets Item navigation property.
        /// </summary>
        public virtual Item Item { get; set; }
    }
}
