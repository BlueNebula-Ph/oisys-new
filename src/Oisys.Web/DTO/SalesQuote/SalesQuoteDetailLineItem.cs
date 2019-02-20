using OisysNew.DTO.Item;

namespace OisysNew.DTO.SalesQuote
{
    /// <summary>
    /// View model for the <see cref="SalesQuoteDetail"/> entity.
    /// </summary>
    public class SalesQuoteDetailLineItem : DTOBase
    {
        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the item selected.
        /// </summary>
        public ItemLookup Item { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets property TotalPrice.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
