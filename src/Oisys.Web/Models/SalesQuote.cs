namespace OisysNew.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// <see cref="SalesQuote"/> class represents SalesQuote created in the application.
    /// </summary>
    public class SalesQuote : ModelBase
    {
        /// <summary>
        /// Gets or sets property quote number.
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
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// Gets or sets property TotalAmount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets property IsDeleted.
        /// </summary>
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets Customer navigation property.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets collection of SalesQuoteDetails navigation property.
        /// </summary>
        public ICollection<SalesQuoteDetail> Details { get; set; }
    }
}