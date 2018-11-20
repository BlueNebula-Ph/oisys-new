using System;
using System.Collections.Generic;

namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="Order"/> class represents the Order object.
    /// </summary>
    public class Order : ModelBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets property CustomerId.
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets property DueDate.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets property DiscountPercent.
        /// </summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets property DiscountAmount.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets property TotalAmount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether order is invoiced.
        /// </summary>
        public bool IsInvoiced { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether order is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets property Customer navigation property.
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets line items of the order.
        /// </summary>
        public virtual ICollection<OrderLineItem> LineItems { get; set; }
    }
}