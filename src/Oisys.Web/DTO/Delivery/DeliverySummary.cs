namespace OisysNew.DTO.Delivery
{
    /// <summary>
    /// <see cref="DeliverySummary"/> class DeliverySummary object.
    /// </summary>
    public class DeliverySummary : DTOBase
    {
        /// <summary>
        /// Gets or sets the delivery number.
        /// </summary>
        public int DeliveryNumber { get; set; }

        /// <summary>
        /// Gets or sets the delivery date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the plate number.
        /// </summary>
        public string PlateNumber { get; set; }

        /// <summary>
        /// Gets or sets areas of the delivery.
        /// Formatted in pipe delimited string.
        /// </summary>
        public string DeliveryAreas { get; set; }
    }
}
