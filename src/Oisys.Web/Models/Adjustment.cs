using System;

namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="Adjustment"/> class.
    /// </summary>
    public class Adjustment : ModelBase
    {
        /// <summary>
        /// Gets or sets the item id property
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets the quantity property
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the adjustment type property.
        /// </summary>
        public string AdjustmentType { get; set; }

        /// <summary>
        /// Gets or sets the adjustment date property
        /// </summary>
        public DateTime AdjustmentDate { get; set; }

        /// <summary>
        /// Gets or sets the remarks property
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets property Operator.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets property Machine.
        /// </summary>
        public string Machine { get; set; }

        /// <summary>
        /// Gets or sets the item navigation property
        /// </summary>
        public virtual Item Item { get; set; }

        /// <summary>
        /// Gets or sets the item history linked to this adjustment.
        /// </summary>
        public virtual ItemHistory TransactionHistory { get; set; }
    }
}