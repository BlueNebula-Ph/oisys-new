using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="Customer"/> class Customer object.
    /// </summary>
    public class Customer : SoftDeletableModel
    {
        /// <summary>
        /// Gets or sets property Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets property Phone Number.
        /// </summary>
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
        public long CityId { get; set; }

        /// <summary>
        /// Gets or sets property Province.
        /// </summary>
        public long ProvinceId { get; set; }

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
        /// Gets or sets property City.
        /// </summary>
        public virtual City City { get; set; }

        /// <summary>
        /// Gets or sets property Province.
        /// </summary>
        public virtual Province Province { get; set; }

        /// <summary>
        /// Gets or sets property Customer transactions.
        /// </summary>
        public virtual ICollection<CustomerTransaction> Transactions { get; set; }
    }

    public enum PriceList
    {
        [Display(Name = "Main Price")]
        MainPrice = 1,
        [Display(Name = "Walk-In Price")]
        WalkInPrice = 2,
        [Display(Name = "N.E. Price")]
        NEPrice = 3
    }
}
