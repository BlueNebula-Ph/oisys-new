namespace OisysNew.DTO.Customer
{
    /// <summary>
    /// <see cref="CustomerLookup"/> class represents data for dropdowns.
    /// </summary>
    public class CustomerLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets property Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets property PriceListId.
        /// </summary>
        public int PriceListId { get; set; }

        /// <summary>
        /// Gets or sets property Discount
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets property Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets property Contact Number
        /// </summary>
        public string ContactNumber { get; set; }
    }
}
