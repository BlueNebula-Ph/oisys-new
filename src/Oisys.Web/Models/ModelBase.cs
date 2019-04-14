using System.ComponentModel.DataAnnotations;

namespace OisysNew.Models
{
    /// <summary>
    /// Base class for data models.
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// Gets or sets property Id.
        /// </summary>
        [Key]
        public long Id { get; set; }
    }
}
