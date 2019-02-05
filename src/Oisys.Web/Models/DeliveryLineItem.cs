namespace OisysNew.Models
{
    /// <summary>
    /// The <see cref="DeliveryLineItem"/> models a delivery detail.
    /// </summary>
    public class DeliveryLineItem : ModelBase
    {
        /// <summary>
        /// Gets or sets property Delivery Id.
        /// </summary>
        public long DeliveryId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetailId.
        /// </summary>
        public long OrderLineItemId { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetail.
        /// </summary>
        public virtual OrderLineItem OrderLineItem { get; set; }

        /// <summary>
        /// Gets or sets property Delivery.
        /// </summary>
        public virtual Delivery Delivery { get; set; }
    }
}
