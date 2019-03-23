namespace OisysNew.DTO.Report
{
    public class ProductSales
    {
        /// <summary>
        /// Gets or sets the item name
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets the total quantity sold
        /// </summary>
        public int QuantitySold { get; set; }

        /// <summary>
        /// Gets or sets the total sales
        /// </summary>
        public decimal TotalSales { get; set; }
    }
}
