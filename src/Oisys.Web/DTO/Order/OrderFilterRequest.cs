using System;

namespace OisysNew.DTO.Order
{
    /// <summary>
    /// <see cref="OrderFilterRequest"/> class represents basic Order filter for displaying data.
    /// </summary>
    public class OrderFilterRequest : FilterRequestBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets property Customer Id.
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Order DateFrom.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets property Order DateTo.
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Gets or sets property Item.
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// Gets or sets property Province Id.
        /// </summary>
        public int? ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether orders shown are invoiced.
        /// </summary>
        public bool IsInvoiced { get; set; }
    }
}
