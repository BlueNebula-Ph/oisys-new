namespace OisysNew.Models
{
    public class SoftDeletableModel : ModelBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
