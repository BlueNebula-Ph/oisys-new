namespace OisysNew.DTO.Province
{
    /// <summary>
    /// <see cref="CitySummary"/> class CitySummary object.
    /// </summary>
    public class CitySummary : DTOBase
    {
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets property IsDeleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the city row version
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the province name of the city.
        /// </summary>
        public string ProvinceName { get; set; }
    }
}
