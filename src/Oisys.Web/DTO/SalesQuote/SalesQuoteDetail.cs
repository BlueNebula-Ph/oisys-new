using OisysNew.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OisysNew.DTO.SalesQuote
{
    public class SalesQuoteDetail : DTOBase
    {
        /// <summary>
        /// Gets or sets property Quote Number.
        /// </summary>
        public int QuoteNumber { get; set; }

        /// <summary>
        /// Gets or sets the selected customer.
        /// </summary>
        public CustomerLookup Customer { get; set; }

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
        public IEnumerable<SalesQuoteDetailLineItem> LineItems { get; set; }

        /// <summary>
        /// Gets or sets the row version for concurrency check.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
