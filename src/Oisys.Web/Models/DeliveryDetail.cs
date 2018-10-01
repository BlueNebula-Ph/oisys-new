namespace OisysNew.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The <see cref="DeliveryDetail"/> models a delivery detail.
    /// </summary>
    public class DeliveryDetail : ModelBase
    {
        /// <summary>
        /// Gets or sets property Delivery Id.
        /// </summary>
        [Required]
        public int DeliveryId { get; set; }

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

        /// <summary>
        /// Gets or sets property OrderDetail.
        /// </summary>
        public OrderDetail OrderDetail { get; set; }

        /// <summary>
        /// Gets or sets property Delivery.
        /// </summary>
        public Delivery Delivery { get; set; }
    }
}
