using System;

namespace OisysNew.Models
{
    public class ItemTransactionHistory : ModelBase
    {
        /// <summary>
        /// Gets or sets the item id
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item transaction date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the item transaction quantity
        /// </summary>
        public int Quantity { get; set; }
    }
}
