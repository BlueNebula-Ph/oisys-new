using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// <see cref="SaveDeliveryLineItemRequest"/> is a viewmodel for saving delivery line items
    /// </summary>
    public class SaveDeliveryLineItemRequest
    {
        /// <summary>
        /// Gets or sets the quantity delivered.
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the order line item id.
        /// </summary>
        [Required]
        public int OrderLineItemId { get; set; }
    }
}
