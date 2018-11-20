namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="OrderLineItem"/> class represents the child of Order object.
    /// </summary>
    public class OrderLineItem : ModelBase
    {
        /// <summary>
        /// Gets or sets property OrderId.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or sets property Quanity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property ItemId.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets property QuantityReturned.
        /// </summary>
        public int QuantityReturned { get; set; }

        /// <summary>
        /// Gets or sets property QuantityDelivered.
        /// </summary>
        public int QuantityDelivered { get; set; }

        /// <summary>
        /// Gets or sets Order navigation property.
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Gets or sets Item navigation property.
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Gets or sets the transaction history navigation property.
        /// </summary>
        public virtual ItemTransactionHistoryOrder TransactionHistory { get; set; }
    }
}
