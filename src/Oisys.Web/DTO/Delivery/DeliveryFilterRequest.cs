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
        /// Gets or sets province id.
        /// </summary>
        public int? ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets city id.
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Gets or sets delivery date from.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets delivery date to.
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Gets or sets item id.
        /// </summary>
        public int? ItemId { get; set; }
    }
}
