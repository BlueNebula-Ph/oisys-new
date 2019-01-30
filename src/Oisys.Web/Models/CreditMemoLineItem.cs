namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="CreditMemoLineItem"/> class CreditMemodetail object.
    /// </summary>
    public class CreditMemoLineItem : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the parent credit memo.
        /// </summary>
        public long CreditMemoId { get; set; }

        /// <summary>
        /// Gets or sets the line item id of the order.
        /// </summary>
        public long OrderLineItemId { get; set; }

        /// <summary>
        /// Gets or sets property Item Id.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the item is returned to the inventory.
        /// </summary>
        public bool ReturnedToInventory { get; set; }

        /// <summary>
        /// Gets or sets property Order Line Item.
        /// </summary>
        public virtual OrderLineItem OrderLineItem { get; set; }

        /// <summary>
        /// Gets or sets the parent credit memo.
        /// </summary>
        public virtual CreditMemo CreditMemo { get; set; }

        /// <summary>
        /// Gets or sets the item linked to this credit memo item.
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Gets or sets the item history linked to this credit memo item.
        /// </summary>
        public virtual ItemHistory TransactionHistory { get; set; }
    }
}
