using System;

namespace OisysNew.DTO.Invoice
{
    /// <summary>
    /// <see cref="InvoiceFilterRequest"/> class represents basic invoice filter for displaying data.
    /// </summary>
    public class InvoiceFilterRequest : FilterRequestBase
    {
        /// <summary>
        /// Gets or sets customer id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets date from
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets date to
        /// </summary>
        public DateTime? DateTo { get; set; }
    }
}
