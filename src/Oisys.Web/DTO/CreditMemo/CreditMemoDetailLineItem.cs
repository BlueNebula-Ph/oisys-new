using OisysNew.DTO.Order;

namespace OisysNew.DTO.CreditMemo
{
    /// <summary>
    /// <see cref="CreditMemoDetailLineItem"/> class represents the child of CreditSummary object.
    /// </summary>
    public class CreditMemoDetailLineItem : DTOBase
    {
        /// <summary>
        /// Gets or sets the item
        /// </summary>
        public OrderLineItemLookup OrderLineItem { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets total price.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// Item was returned to inventory
        /// </summary>
        public bool ShouldAddBackToInventory { get; set; }
    }
}
