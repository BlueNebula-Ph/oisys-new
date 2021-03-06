﻿using OisysNew.DTO.Item;

namespace OisysNew.DTO.Order
{
    /// <summary>
    /// <see cref="OrderDetailLineItem"/> class represents the child of OrderSummary object.
    /// </summary>
    public class OrderDetailLineItem : DTOBase
    {
        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the item selected.
        /// </summary>
        public ItemLookup Item { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets property TotalPrice.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the quantity returned for this line item.
        /// </summary>
        public int QuantityReturned { get; set; }
    }
}
