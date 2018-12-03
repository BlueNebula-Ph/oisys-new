using System;

namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// <see cref="DeliveryFilterRequest"/> class represents basic delivery filter for displaying data.
    /// </summary>
    public class DeliveryFilterRequest : FilterRequestBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets property Customer Id.
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Delivery Date From.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets property Delivery Date To.
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Gets or sets property Item.
        /// </summary>
        public int? ItemId { get; set; }
    }
}
