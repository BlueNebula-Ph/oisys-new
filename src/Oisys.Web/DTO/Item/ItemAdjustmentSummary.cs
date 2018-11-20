using System;

namespace OisysNew.DTO.Item
{
    /// <summary>
    /// <see cref="ItemAdjustmentSummary"/> class for displaying item adjustments.
    /// </summary>
    public class ItemAdjustmentSummary
    {
        /// <summary>
        /// Gets or sets property id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets property item.
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets property adjustment date.
        /// </summary>
        public DateTime AdjustmentDate { get; set; }

        /// <summary>
        /// Gets or sets property quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property adjustment type.
        /// </summary>
        public string AdjustmentType { get; set; }

        /// <summary>
        /// Gets or sets property remarks.
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
    }
}
