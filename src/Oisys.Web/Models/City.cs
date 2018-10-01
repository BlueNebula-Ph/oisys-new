namespace OisysNew.Models
{
    /// <summary>
    /// Entity model for city.
    /// </summary>
    public class City : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the province id of the city.
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets property IsDeleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the province navigation property.
        /// </summary>
        public Province Province { get; set; }
    }
}
