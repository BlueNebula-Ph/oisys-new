namespace OisysNew.DTO.Order
{
    /// <summary>
    /// <see cref="OrderLineItemSummary"/> class represents the child of OrderSummary object.
    /// </summary>
    public class OrderLineItemSummary : DTOBase
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

        /// <summary>
        /// Gets or sets the quantity returned for this line item.
        /// </summary>
        public int QuantityReturned { get; set; }
    }
}
