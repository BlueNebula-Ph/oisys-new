namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// The delivery detail summary view model.
    /// </summary>
    public class DeliveryLineItemSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property Unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets property Order Line Item Id.
        /// </summary>
        public long OrderLineItemId { get; set; }

        /// <summary>
        /// Gets or sets property Customer Name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets property Item Code.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets property Item Name.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets property Item Code - Item Name.
        /// </summary>
        public string ItemCodeName { get; set; }

        /// <summary>
        /// Gets or sets property Category Name.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets property Order Number.
        /// </summary>
        public string OrderNumber { get; set; }
    }
}