namespace OisysNew.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// <see cref="CustomerTransaction"/> class.
    /// </summary>
    public class CustomerTransaction : ModelBase
    {
        /// <summary>
        /// Gets or sets property Customer Id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets property Debit.
        /// </summary>
        public decimal? Debit { get; set; }

        /// <summary>
        /// Gets or sets property Credit.
        /// </summary>
        public decimal? Credit { get; set; }

        /// <summary>
        /// Gets or sets property Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets property Order Id.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets property CreditMemoId.
        /// </summary>
        public int CreditMemoId { get; set; }

        /// <summary>
        /// Gets or sets property InvoiceId.
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets property Transaction type.
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets property Customer.
        /// </summary>
        public virtual Customer Customer { get; set; }
    }
}
