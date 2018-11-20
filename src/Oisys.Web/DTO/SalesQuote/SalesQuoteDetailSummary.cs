namespace OisysNew.DTO.SalesQuote
{
    /// <summary>
    /// View model for the <see cref="SalesQuoteDetail"/> entity.
    /// </summary>
    public class SalesQuoteDetailSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property SalesQuoteId.
        /// </summary>
        public int SalesQuoteId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets property Unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets property ItemId.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets property Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets property Category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets property Total Price.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets property Item.
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets property ItemCode.
        /// </summary>
        public string ItemCode { get; set; }
    }
}
