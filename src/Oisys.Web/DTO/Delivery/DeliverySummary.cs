﻿namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// <see cref="DeliverySummary"/> class DeliverySummary object.
    /// </summary>
    public class DeliverySummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public int DeliveryNumber { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets property Plate Number.
        /// </summary>
        public string PlateNumber { get; set; }

        /// <summary>
        /// Gets or sets property Province Name.
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// Gets or sets property City Name.
        /// </summary>
        public string CityName { get; set; }
    }
}
