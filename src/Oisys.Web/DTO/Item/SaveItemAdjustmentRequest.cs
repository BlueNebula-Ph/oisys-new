using OisysNew.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO
{
    /// <summary>
    /// <see cref="SaveItemAdjustmentRequest"/> class Adjust Item object.
    /// </summary>
    public class SaveItemAdjustmentRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets the item id to be adjusted.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets property AdjustmentType.
        /// </summary>
        [Required]
        public AdjustmentType AdjustmentType { get; set; }

        /// <summary>
        /// Gets or sets property AdjustmentQuantity.
        /// </summary>
        [Required]
        public int AdjustmentQuantity { get; set; }

        /// <summary>
        /// Gets or sets property Remarks.
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
