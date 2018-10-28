namespace OisysNew.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum PriceList 
    {
        [Display(Name = "Main Price")]
        MainPrice = 1,
        [Display(Name = "Walk-In Price")]
        WalkInPrice = 2,
        [Display(Name = "N.E. Price")]
        NEPrice = 3
    }

    /// <summary>
    /// <see cref="Customer"/> class Customer object.
    /// </summary>
    public class Customer : ModelBase
    {
        /// <summary>
        /// Gets or sets property Name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets property Phone Number.
        /// </summary>
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets property Contact Person.
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets property Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets property Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets property City.
        /// </summary>
        [Required]
        public int CityId { get; set; }

        /// <summary>
        /// Gets or sets property Province.
        /// </summary>
        [Required]
        public int ProvinceId { get; set; }

        /// <summary>
        /// Gets or sets property Terms.
        /// </summary>
        public string Terms { get; set; }

        /// <summary>
        /// Gets or sets property Discount.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets property Price List.
        /// </summary>
        public PriceList PriceList { get; set; }

        /// <summary>
        /// Gets or sets property Keywords.
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets property IsDeleted.
        /// </summary>
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets property City.
        /// </summary>
        public City City { get; set; }

        /// <summary>
        /// Gets or sets property Province.
        /// </summary>
        public Province Province { get; set; }

        /// <summary>
        /// Gets or sets property Customer transactions.
        /// </summary>
        public ICollection<CustomerTransaction> Transactions { get; set; }
    }
}
