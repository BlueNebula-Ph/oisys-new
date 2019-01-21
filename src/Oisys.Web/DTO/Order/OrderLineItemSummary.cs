using OisysNew.DTO.Item;

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
        /// Gets or sets property ItemId.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets property Item.
        /// </summary>
        public ItemSummary Item { get; set; }

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
