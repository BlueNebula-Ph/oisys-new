using System.ComponentModel.DataAnnotations;

namespace OisysNew.Helpers
{
    /// <summary>
    /// Values to define different adjustment types
    /// </summary>
    public enum AdjustmentType
    {
        /// <summary>
        /// AdjustmentType Add
        /// </summary>
        [Display(Name = "Add")]
        Add = 1,

        /// <summary>
        /// AdjustmentType Subtract
        /// </summary>
        [Display(Name = "Deduct")]
        Deduct = 2,
    }
}
