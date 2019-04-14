using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.SalesQuote
{
    /// <summary>
    /// <see cref="SaveSalesQuoteRequest"/> class Create/Update SalesQuote object.
    /// </summary>
    public class SaveSalesQuoteRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets property QuoteNumber.
        /// </summary>
        public int QuoteNumber { get; set; }

        /// <summary>
        /// Gets or sets property CustomerId.
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets property DeliveryFee.
        /// </summary>
        [Required]
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// Gets or sets property TotalAmount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets collection of SalesQuoteDetails navigation property.
        /// </summary>
        public IEnumerable<SaveSalesQuoteLineItemRequest> LineItems { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
