using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.SalesQuote
{
    /// <summary>
    /// <see cref="SalesQuoteDetail"/> class represents the child of SalesQuote object.
    /// </summary>
    public class SaveSalesQuoteLineItemRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets property SalesQuoteId.
        /// </summary>
        [Required]
        public int SalesQuoteId { get; set; }

        /// <summary>
        /// Gets or sets property Quantity.
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets property ItemId.
        /// </summary>
        [Required]
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        [Required]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets property TotalPrice.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
