using System;
using System.Collections.Generic;

namespace OisysNew.DTO.SalesQuote
{
    /// <summary>
    /// View model for the Sales Quote entity.
    /// </summary>
    public class SalesQuoteSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property Quote Number.
        /// </summary>
        public int QuoteNumber { get; set; }

        /// <summary>
        /// Gets or sets property CustomerId.
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Customer Name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets property Customer Address.
        /// </summary>
        public string CustomerAddress { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets property DeliveryFee.
        /// </summary>
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// Gets or sets property TotalAmount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets collection of SalesQuoteDetailSummary navigation property.
        /// </summary>
        public IEnumerable<SalesQuoteLineItemSummary> LineItems { get; set; }
    }
}
