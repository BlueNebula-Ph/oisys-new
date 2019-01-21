﻿using OisysNew.DTO.Customer;
using System.Collections.Generic;

namespace OisysNew.DTO.Order
{
    /// <summary>
    /// View model for the order entity.
    /// </summary>
    public class OrderSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets property CustomerId.
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets property DueDate.
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// Gets or sets property Discount Percent.
        /// </summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>
        /// Gets or sets property Discount Amount.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets property gross amount.
        /// </summary>
        public decimal GrossAmount { get; set; }

        /// <summary>
        /// Gets or sets property total amount.
        /// GrossAmount - DiscountAmount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets Customer navigation property.
        /// </summary>
        public CustomerSummary Customer { get; set; }

        /// <summary>
        /// Gets or sets Details navigation property.
        /// </summary>
        public IEnumerable<OrderLineItemSummary> LineItems { get; set; }
    }
}
