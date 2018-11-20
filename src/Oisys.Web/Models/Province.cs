using System.Collections.Generic;

namespace OisysNew.Models
{
    /// <summary>
    /// Entity model for province
    /// </summary>
    public class Province : SoftDeletableModel
    {
        /// <summary>
        /// Gets or sets the province name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the cities under this province.
        /// </summary>
        public virtual ICollection<City> Cities { get; set; }
    }
}
