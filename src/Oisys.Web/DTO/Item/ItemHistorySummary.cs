using System;

namespace OisysNew.DTO.Item
{
    public class ItemHistorySummary : DTOBase
    {
        /// <summary>
        /// Gets or sets the item transaction date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the item transaction quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets remarks for item history
        /// </summary>
        public string Remarks { get; set; }
    }
}
