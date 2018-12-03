using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// <see cref="SaveDeliveryDetailRequest"/> is a viewmodel for saving delivery details
    /// </summary>
    public class SaveDeliveryDetailRequest
    {
        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetailId.
        /// </summary>
        [Required]
        public int OrderDetailId { get; set; }
    }
}
