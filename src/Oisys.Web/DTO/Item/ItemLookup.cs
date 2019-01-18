namespace OisysNew.DTO.Item
{
    /// <summary>
    /// <see cref="ItemLookup"/> class represents data for dropdowns.
    /// </summary>
    public class ItemLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets item code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets item Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets current quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property Unit
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets property category name.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets property Main Price
        /// </summary>
        public decimal MainPrice { get; set; }

        /// <summary>
        /// Gets or sets property N.E. Price
        /// </summary>
        public decimal NEPrice { get; set; }

        /// <summary>
        /// Gets or sets property Walk-In Price
        /// </summary>
        public decimal WalkInPrice { get; set; }
    }
}
