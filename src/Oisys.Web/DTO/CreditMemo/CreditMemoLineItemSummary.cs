using OisysNew.DTO.Order;

namespace OisysNew.DTO.CreditMemo
{
    /// <summary>
    /// <see cref="CreditMemoLineItemSummary"/> class represents the child of CreditSummary object.
    /// </summary>
    public class CreditMemoLineItemSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property Ordercode.
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetail
        /// </summary>
        //public OrderLineItemSummary OrderDetail { get; set; }

        /// <summary>
        /// Gets or sets property ItemCode.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets property Item.
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// Item was returned to inventory
        /// </summary>
        public bool ShouldAddBackToInventory { get; set; }

        /// <summary>
        /// Gets property total price.
        /// </summary>
        public decimal TotalPrice
        {
            get { return this.Quantity * this.Price; }
        }
    }
}
