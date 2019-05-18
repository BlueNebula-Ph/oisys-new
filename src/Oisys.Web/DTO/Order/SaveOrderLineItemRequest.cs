using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.Order
{
    /// <summary>
    /// <see cref="OrderDetail"/> class represents the child of Order object.
    /// </summary>
    public class SaveOrderLineItemRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets property OrderId.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or sets property Quanity.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property ItemId.
        /// </summary>
        [Required]
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets property DeliveryId.
        /// </summary>
        public long? DeliveryId { get; set; }

        /// <summary>
        /// Gets or sets the unit price of an item.
        /// </summary>
        [Required]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the discounted price of an item.
        /// </summary>
        public decimal DiscountedUnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the TotalPrice. Quantity * Price.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
