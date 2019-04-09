using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.Province
{
    /// <summary>
    /// <see cref="SaveCityRequest"/> class Create/Update SaveCityRequest object.
    /// </summary>
    public class SaveCityRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets property ProvinceId.
        /// </summary>
        public int? ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the concurrency check.
        /// </summary>
        public string RowVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the city is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
