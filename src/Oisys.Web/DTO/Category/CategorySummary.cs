namespace OisysNew.DTO.Category
{
    /// <summary>
    /// The category view model
    /// </summary>
    public class CategorySummary : DTOBase
    {
        /// <summary>
        /// Gets or sets the category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category row version
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
