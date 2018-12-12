using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.CreditMemo
{
    /// <summary>
    /// <see cref="SaveCreditMemoLineItemRequest"/> class represents the child of CreditMemo object.
    /// </summary>
    public class SaveCreditMemoLineItemRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets property CreditMemoId.
        /// </summary>
        [Required]
        public int CreditMemoId { get; set; }

        /// <summary>
        /// Gets or sets the order line item id.
        /// </summary>
        [Required]
        public int OrderLineItemId { get; set; }

        /// <summary>
        /// Gets or sets property ItemId.
        /// </summary>
        [Required]
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets property whether to put back the returned item to inventory.
        /// </summary>
        public bool ShouldAddBackToInventory { get; set; }
    }
}