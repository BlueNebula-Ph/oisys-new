namespace OisysNew.Helpers
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Values to define different quantity types
    /// </summary>
    public enum QuantityType
    {
        /// <summary>
        /// Current quantity
        /// </summary>
        [Display(Name = "Current Quantity")]
        CurrentQuantity,

        /// <summary>
        /// Actual quantity
        /// </summary>
        [Display(Name = "Actual Quantity")]
        ActualQuantity,

        /// <summary>
        /// Both quantities
        /// </summary>
        [Display(Name = "Current & Actual Quantities")]
        Both,
    }
}
