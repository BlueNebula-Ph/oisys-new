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
        /// Gets or sets a value indicating whether item has low quantity.
        /// </summary>
        public bool IsQuantityLow { get; set; }
    }
}
