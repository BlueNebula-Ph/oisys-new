namespace OisysNew.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Base class for models.
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// Gets or sets property Id.
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
