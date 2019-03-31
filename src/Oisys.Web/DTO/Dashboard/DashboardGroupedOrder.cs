using System.Collections.Generic;

namespace OisysNew.DTO.Dashboard
{
    public class DashboardGroupedOrder
    {
        /// <summary>
        /// Gets or sets the city grouping
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the province grouping
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// Gets or sets the orders for each city and province.
        /// </summary>
        public IEnumerable<DashboardOrder> Orders { get; set; }
    }
}
