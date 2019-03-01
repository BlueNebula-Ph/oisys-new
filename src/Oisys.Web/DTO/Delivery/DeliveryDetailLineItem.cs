using OisysNew.DTO.Customer;
using OisysNew.DTO.Order;

namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// The delivery detail summary view model.
    /// </summary>
    public class DeliveryDetailLineItem : DTOBase
    {
        /// <summary>
        /// Gets or sets order code.
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// Gets or sets order date.
        /// </summary>
        public string OrderDate { get; set; }

        /// <summary>
        /// Gets or sets item id property.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets item code.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets item name.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets unit property.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets price property.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets category name property.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets quantity returned.
        /// </summary>
        public int QuantityReturned { get; set; }

        /// <summary>
        /// Gets or sets quantity delivered property.
        /// </summary>
        public int QuantityDelivered { get; set; }

        /// <summary>
        /// Gets or sets order line item id for delivery line items.
        /// </summary>
        public long OrderLineItemId { get; set; }

        /// <summary>
        /// Gets or sets customer.
        /// </summary>
        public CustomerLookup Customer { get; set; }
    }
}