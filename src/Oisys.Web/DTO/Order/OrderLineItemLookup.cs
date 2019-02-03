namespace OisysNew.DTO.Order
{
    /// <summary>
    /// <see cref="OrderLineItemLookup"/> represents order line item in lookup data.
    /// </summary>
    public class OrderLineItemLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets order code.
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// Gets or sets order date.
        /// </summary>
        public string OrderDate { get; set; }

        /// <summary>
        /// Gets or sets item id property.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets item code.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets item name.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets unit property.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets price property.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets category name property.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets quantity returned.
        /// </summary>
        public int QuantityReturned { get; set; }

        /// <summary>
        /// Gets or sets quantity delivered property.
        /// </summary>
        public int QuantityDelivered { get; set; }
    }
}
