namespace OisysNew.Models
{
    /// <summary>
    /// Entity model for city.
    /// </summary>
    public class City : SoftDeletableModel
    {
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the province id of the city.
        /// </summary>
        public long ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the province navigation property.
        /// </summary>
        public virtual Province Province { get; set; }
    }
}
