namespace OisysNew.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// <see cref="CreditMemoDetail"/> class CreditMemodetail object.
    /// </summary>
    public class CreditMemoDetail : ModelBase
    {
        /// <summary>
        /// Gets or sets property CreditMemoId.
        /// </summary>
        [Required]
        public int CreditMemoId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetailId.
        /// </summary>
        [Required]
        public int OrderDetailId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the item is returned to the inventory.
        /// </summary>
        public bool ReturnedToInventory { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetail.
        /// </summary>
        public OrderDetail OrderDetail { get; set; }

        /// <summary>
        /// Gets or sets property CreditMemo.
        /// </summary>
        public CreditMemo CreditMemo { get; set; }
    }
}
