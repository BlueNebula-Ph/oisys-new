namespace OisysNew.DTO.SalesQuote
{
    /// <summary>
    /// View model for the <see cref="SalesQuoteDetail"/> entity.
    /// </summary>
    public class SalesQuoteLineItemSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the item id of the line item.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item name of the line item.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the item unit of the line item.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the item category of the line item.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets property TotalPrice.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
