using System;
using System.ComponentModel.DataAnnotations;

namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="CustomerTransaction"/> class.
    /// </summary>
    public class CustomerTransaction : ModelBase
    {
        /// <summary>
        /// Gets or sets property Customer Id.
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets property Amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets property Transaction Type.
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Gets or sets property Order Id.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or sets property CreditMemoId.
        /// </summary>
        public long CreditMemoId { get; set; }

        /// <summary>
        /// Gets or sets property InvoiceId.
        /// </summary>
        public long InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets property Customer.
        /// </summary>
        public virtual Customer Customer { get; set; }
    }

    public enum TransactionType
    {
        [Display(Name = "Debit")]
        Debit = 1,
        [Display(Name = "Credit")]
        Credit = 2
    }
}
