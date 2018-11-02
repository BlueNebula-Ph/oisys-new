namespace OisysNew.DTO.Item
{
    /// <summary>
    /// <see cref="ItemLookup"/> class represents data for dropdowns.
    /// </summary>
    public class ItemLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code - Name.
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Gets or sets property Current Quantity.
        /// </summary>
        public decimal CurrentQuantity { get; set; }

        /// <summary>
        /// Gets or sets property Actual Quantity.
        /// </summary>
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// Gets or sets property Unit
        /// </summary>
        public string Unit { get; set; }

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

        /// <summary>
        /// Gets or sets property Category
        /// </summary>
        public string NameCategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets property category name.
        /// </summary>
        public string CategoryName { get; set; }
    }
}
