using System;

namespace OisysNew.Models
{
    public class ItemHistory : ModelBase
    {
        /// <summary>
        /// Gets or sets the item id
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item transaction date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the item transaction quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets remarks for item history
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets order id foreign key
        /// </summary>
        public long? OrderLineItemId { get; set; }

        /// <summary>
        /// Gets or sets adjustment id foreign key
        /// </summary>
        public long? AdjustmentId { get; set; }

        /// <summary>
        /// Gets or sets credit memo id foreign key
        /// </summary>
        public long? CreditMemoLineItemId { get; set; }

        /// <summary>
        /// Gets or sets the item nav property
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Gets or sets the order line item nav property
        /// </summary>
        public virtual OrderLineItem OrderLineItem { get; set; }

        /// <summary>
        /// Gets or sets the adjustment nav property
        /// </summary>
        public virtual Adjustment Adjustment { get; set; }
    }
}
