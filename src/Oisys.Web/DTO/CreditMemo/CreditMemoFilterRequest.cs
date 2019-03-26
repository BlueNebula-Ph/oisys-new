using System;

namespace OisysNew.DTO.CreditMemo
{
    /// <summary>
    /// <see cref="CreditMemoFilterRequest"/> class represents basic CreditMemo filter for displaying data.
    /// </summary>
    public class CreditMemoFilterRequest : FilterRequestBase
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
        /// Gets or sets a value indicating whether credit memo is invoiced.
        /// </summary>
        public bool IsInvoiced { get; set; }
    }
}
