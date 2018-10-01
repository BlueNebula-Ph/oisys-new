namespace OisysNew.Models
{
    using System;

    /// <summary>
    /// <see cref="Adjustment"/> class.
    /// </summary>
    public class Adjustment
    {
        /// <summary>
        /// Gets or sets the id property
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the item id property
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the quantity property
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the adjustment type property.
        /// </summary>
        public string AdjustmentType { get; set; }

        /// <summary>
        /// Gets or sets the quantity type property.
        /// </summary>
        public string QuantityType { get; set; }

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
        public Item Item { get; set; }
    }
}