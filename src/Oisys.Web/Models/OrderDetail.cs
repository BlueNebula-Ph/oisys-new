namespace OisysNew.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// <see cref="OrderDetail"/> class represents the child of Order object.
    /// </summary>
    public class OrderDetail : ModelBase
    {
        /// <summary>
        /// Gets or sets property OrderId.
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets property Quanity.
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
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets property TotalPrice.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets property QuantityReturned.
        /// </summary>
        public decimal QuantityReturned { get; set; }

        /// <summary>
        /// Gets or sets property QuantityDelivered.
        /// </summary>
        public decimal QuantityDelivered { get; set; }

        /// <summary>
        /// Gets or sets Order navigation property.
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Gets or sets Item navigation property.
        /// </summary>
        public Item Item { get; set; }
    }
}
