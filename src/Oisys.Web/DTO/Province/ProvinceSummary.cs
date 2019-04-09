using System.Collections.Generic;
using System.Linq;

namespace OisysNew.DTO.Province
{
    /// <see cref="ProvinceSummary"/> class ProvinceSummary object.
    /// </summary>
    public class ProvinceSummary : DTOBase
    {
        private IEnumerable<CitySummary> cities;

        /// <summary>
        /// Gets or sets the province name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the province row version
        /// </summary>
        public string RowVersion { get; set; }

        /// <summary>
        /// Gets or sets collection of cities.
        /// </summary>
        public IEnumerable<CitySummary> Cities
        {
            get { return cities.Where(a => !a.IsDeleted); }
            set { cities = value; }
        }
    }
}
