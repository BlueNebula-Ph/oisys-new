namespace OisysNew.DTO.Order
{
    /// <summary>
    /// <see cref="OrderDetailLookup"/> represents order details in lookup data.
    /// </summary>
    public class OrderDetailLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets item id property.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets item code and name.
        /// </summary>
        public string ItemCodeName { get; set; }

        /// <summary>
        /// Gets or sets item code and name and order code.
        /// </summary>
        public string ItemCodeNameOrder { get; set; }

        /// <summary>
        /// Gets or sets item name property.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets quantity property.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets unit property.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets price property.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets category name property.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets quantity delivered property.
        /// </summary>
        public decimal QuantityDelivered { get; set; }

        /// <summary>
        /// Gets initial deliver quantity
        /// </summary>
        public decimal DeliverQuantity
        {
            get
            {
                return Quantity - QuantityDelivered;
            }
        }

        /// <summary>
        /// Gets total price
        /// </summary>
        public decimal TotalPrice
        {
            get
            {
                return Quantity * Price;
            }
        }
    }
}
