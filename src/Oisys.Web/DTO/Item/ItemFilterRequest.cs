namespace OisysNew.DTO.Item
{
    /// <summary>
    /// <see cref="ItemFilterRequest"/> class represents basic Item filter for displaying data.
    /// </summary>
    public class ItemFilterRequest : FilterRequestBase
    {
        /// <summary>
        /// Gets or sets property CategoryId.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets property Price.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets property Weight.
        /// </summary>
        public decimal? Weight { get; set; }
    }
}
