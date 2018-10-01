namespace OisysNew.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Entity model for province
    /// </summary>
    public class Province : ModelBase
    {
        /// <summary>
        /// Gets or sets the province name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets property IsDeleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the cities under this province.
        /// </summary>
        public ICollection<City> Cities { get; set; }
    }
}
