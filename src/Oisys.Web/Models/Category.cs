namespace OisysNew.Models
{
    /// <summary>
    /// Entity model for category.
    /// </summary>
    public class Category : SoftDeletableModel
    {
        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
