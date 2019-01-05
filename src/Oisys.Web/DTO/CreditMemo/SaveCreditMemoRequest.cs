using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.CreditMemo
{
    /// <summary>
    /// <see cref="SaveCreditMemoRequest"/> class Create/Update CreditMemo object.
    /// </summary>
    public class SaveCreditMemoRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets property CustomerId.
        /// </summary>
        [Required(ErrorMessage = "Please select a customer.")]
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets property Driver.
        /// </summary>
        public string Driver { get; set; }

        /// <summary>
        /// Gets or sets the line items associated to the credit memo.
        /// </summary>
        public IEnumerable<SaveCreditMemoLineItemRequest> LineItems { get; set; }
    }
}