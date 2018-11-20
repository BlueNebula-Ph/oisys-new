﻿namespace OisysNew.Models
{
    /// <summary>
    /// The <see cref="DeliveryDetail"/> models a delivery detail.
    /// </summary>
    public class DeliveryDetail : ModelBase
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
        public long OrderDetailId { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetail.
        /// </summary>
        public virtual OrderLineItem OrderDetail { get; set; }

        /// <summary>
        /// Gets or sets property Delivery.
        /// </summary>
        public virtual Delivery Delivery { get; set; }
    }
}
