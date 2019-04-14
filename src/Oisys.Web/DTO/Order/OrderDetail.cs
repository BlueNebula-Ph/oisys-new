using OisysNew.DTO.Customer;
using System.Collections.Generic;

namespace OisysNew.DTO.Order
{
    public class OrderDetail : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// This property is auto generated.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the selected customer
        /// </summary>
        public CustomerLookup Customer { get; set; }

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
        /// Gets or sets the order line items.
        /// </summary>
        public IEnumerable<OrderDetailLineItem> LineItems { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
