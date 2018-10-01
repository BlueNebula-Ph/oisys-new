namespace OisysNew.Helpers
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Values to define different adjustment types
    /// </summary>
    public enum AdjustmentType
    {
        /// <summary>
        /// AdjustmentType Add
        /// </summary>
        [Display(Name = "Add")]
        Add,

        /// <summary>
        /// AdjustmentType Subtract
        /// </summary>
        [Display(Name = "Deduct")]
        Deduct,
    }
}
