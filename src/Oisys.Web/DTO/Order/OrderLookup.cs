namespace OisysNew.DTO.Order
{
    /// <summary>
    /// <see cref="OrderLookup"/> class represents order data for dropdowns.
    /// </summary>
    public class OrderLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets code property.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets date property.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets total amount property.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}