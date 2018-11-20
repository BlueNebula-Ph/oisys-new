namespace OisysNew.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// <see cref="CreditMemo"/> class CreditMemo object.
    /// </summary>
    public class CreditMemo : ModelBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets property Driver.
        /// </summary>
        public string Driver { get; set; }

        /// <summary>
        /// Gets or sets property CustomerId.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Customer.
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether credit memo is invoiced.
        /// </summary>
        public bool IsInvoiced { get; set; }

        /// <summary>
        /// Gets or sets property Details.
        /// </summary>
        public virtual ICollection<CreditMemoDetail> Details { get; set; }
    }
}
