namespace OisysNew.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// <see cref="SalesQuote"/> class represents child of SalesQuote created in the application.
    /// </summary>
    public class SalesQuoteDetail : ModelBase
    {
        /// <summary>
        /// Gets or sets property SalesQuoteId.
        /// </summary>
        [Required]
        public int SalesQuoteId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets property ItemId.
        /// </summary>
        [Required]
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets property TotalPrice.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets SalesQuote navigation property.
        /// </summary>
        public SalesQuote SalesQuote { get; set; }

        /// <summary>
        /// Gets or sets Item navigation property.
        /// </summary>
        public Item Item { get; set; }
    }
}
