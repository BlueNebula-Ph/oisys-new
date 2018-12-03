using System;
using System.Collections.Generic;

namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// <see cref="DeliverySummary"/> class DeliverySummary object.
    /// </summary>
    public class DeliverySummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public string DeliveryNumber { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets property Plate Number.
        /// </summary>
        public string PlateNumber { get; set; }

        /// <summary>
        /// Gets or sets property Province Id.
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets property Province Name.
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// Gets or sets property City Id.
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Gets or sets property City Name.
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets property Details.
        /// </summary>
        public IEnumerable<DeliveryDetailSummary> Details { get; set; }
    }
}
