namespace OisysNew.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// <see cref="Delivery"/> class Delivery object.
    /// </summary>
    public class Delivery : ModelBase
    {
        /// <summary>
        /// Gets or sets property Delivery Number.
        /// </summary>
        public int DeliveryNumber { get; set; }

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
        /// Gets or sets property City Id.
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Gets or sets the province nav property.
        /// </summary>
        public virtual Province Province { get; set; }

        /// <summary>
        /// Gets or sets the city nav property.
        /// </summary>
        public virtual City City { get; set; }

        /// <summary>
        /// Gets or sets property Details.
        /// </summary>
        public virtual ICollection<DeliveryDetail> Details { get; set; }
    }
}
