namespace OisysNew.Helpers
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Values to define different quantity types
    /// </summary>
    public enum QuantityType
    {
        /// <summary>
        /// The quantity that can be ordered.
        /// </summary>
        [Display(Name = "Quantity")]
        Quantity,

        /// <summary>
        /// The actual quantity in stock.
        /// </summary>
        [Display(Name = "Stock Quantity")]
        StockQuantity,

        /// <summary>
        /// Both quantities
        /// </summary>
        [Display(Name = "All Quantities")]
        Both
    }
}
