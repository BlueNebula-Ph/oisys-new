using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.Item
{
    /// <summary>
    /// <see cref="SaveItemRequest"/> class Create/Update Item object.
    /// </summary>
    public class SaveItemRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets property item code.
        /// </summary>
        [Required(ErrorMessage = "Item code is required.")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets property Name.
        /// </summary>
        [Required(ErrorMessage = "Item name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets property Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets property MainPrice.
        /// </summary>
        public decimal? MainPrice { get; set; }

        /// <summary>
        /// Gets or sets property NEPrice.
        /// </summary>
        public decimal? NEPrice { get; set; }

        /// <summary>
        /// Gets or sets property WalkInPrice.
        /// </summary>
        public decimal? WalkInPrice { get; set; }

        /// <summary>
        /// Gets or sets property Weight.
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// Gets or sets property Thickness.
        /// </summary>
        public string Thickness { get; set; }

        /// <summary>
        /// Gets or sets property Unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets property CategoryId.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
