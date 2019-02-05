using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// <see cref="SaveDeliveryLineItemRequest"/> is a viewmodel for saving delivery line items
    /// </summary>
    public class SaveDeliveryLineItemRequest
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
