using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.Category
{
    /// <summary>
    /// Save request for saving categories
    /// </summary>
    public class SaveCategoryRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }
    }
}