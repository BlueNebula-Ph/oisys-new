﻿using OisysNew.DTO.Province;
using System.Collections.Generic;

namespace OisysNew.DTO.Delivery
{
    public class DeliveryDetail : DTOBase
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
        /// Gets or sets the delivery province.
        /// </summary>
        public ProvinceLookup Province { get; set; }

        /// <summary>
        /// Gets or sets the delivery city.
        /// </summary>
        public CityLookup City { get; set; }

        /// <summary>
        /// Gets or sets property Details.
        /// </summary>
        public IEnumerable<DeliveryDetailLineItem> LineItems { get; set; }
    }
}