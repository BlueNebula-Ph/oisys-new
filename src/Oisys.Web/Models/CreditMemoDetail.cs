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
        public long CreditMemoId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetailId.
        /// </summary>
        public long OrderDetailId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the item is returned to the inventory.
        /// </summary>
        public bool ReturnedToInventory { get; set; }

        /// <summary>
        /// Gets or sets property OrderDetail.
        /// </summary>
        public virtual OrderLineItem OrderDetail { get; set; }

        /// <summary>
        /// Gets or sets property CreditMemo.
        /// </summary>
        public virtual CreditMemo CreditMemo { get; set; }
    }
}
