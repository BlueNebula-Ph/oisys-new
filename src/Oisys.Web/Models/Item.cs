using System.Collections.Generic;

namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="Item"/> class Item/Inventory object.
    /// </summary>
    public class Item : SoftDeletableModel
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets property Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets property Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a snapshot of the item's quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets property Unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets property MainPrice.
        /// </summary>
        public decimal MainPrice { get; set; }

        /// <summary>
        /// Gets or sets property NEPrice.
        /// Also known as Nueva Ecija Price.
        /// </summary>
        public decimal NEPrice { get; set; }

        /// <summary>
        /// Gets or sets property WalkInPrice.
        /// </summary>
        public decimal WalkInPrice { get; set; }

        /// <summary>
        /// Gets or sets property Weight.
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// Gets or sets property Thickness.
        /// </summary>
        public string Thickness { get; set; }

        /// <summary>
        /// Gets or sets property CategoryId.
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// Gets or sets property Category.
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets transaction history for the item.
        /// </summary>
        public virtual ICollection<ItemTransactionHistory> TransactionHistory { get; set; }
    }
}
