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
        /// Gets or sets property ItemCode.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets property Item.
        /// </summary>
        public string ItemName { get; set; }

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
