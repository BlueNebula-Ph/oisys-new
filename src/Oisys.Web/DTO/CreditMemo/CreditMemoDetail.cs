using OisysNew.DTO.Customer;
using System.Collections.Generic;

namespace OisysNew.DTO.CreditMemo
{
    public class CreditMemoDetail : DTOBase
    {
        /// <summary>
        /// Gets or sets code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets customer.
        /// </summary>
        public CustomerLookup Customer { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets property Driver.
        /// </summary>
        public string Driver { get; set; }

        /// <summary>
        /// Gets or sets total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets property Details.
        /// </summary>
        public IEnumerable<CreditMemoDetailLineItem> LineItems { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
