using System.ComponentModel;

namespace OisysNew.DTO
{
    /// <summary>
    /// <see cref="FilterRequestBase"/> class represents basic filter for displaying data.
    /// </summary>
    public class FilterRequestBase
    {
        /// <summary>
        /// Gets or sets property PageIndex.
        /// </summary>
        [DefaultValue(1)]
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets property PageSize.
        /// </summary>
        [DefaultValue(20)]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets property SortBy.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets property SortDirection.
        /// </summary>
        [DefaultValue("asc")]
        public string SortDirection { get; set; }

        /// <summary>
        /// Gets or sets property SearchTerm.
        /// </summary>
        public string SearchTerm { get; set; }
    }
}