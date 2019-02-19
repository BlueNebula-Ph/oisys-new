using OisysNew.DTO.Province;

namespace OisysNew.DTO.Customer
{
    public class CustomerDetail : DTOBase
    {
        /// <summary>
        /// Gets or sets property Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets property Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets property Phone Number.
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets property Contact Person.
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets property Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets city of customer.
        /// </summary>
        public CityLookup City { get; set; }

        /// <summary>
        /// Gets or sets province of customer.
        /// </summary>
        public ProvinceLookup Province { get; set; }

        /// <summary>
        /// Gets or sets property Terms.
        /// </summary>
        public string Terms { get; set; }

        /// <summary>
        /// Gets or sets property Discount.
        /// </summary>
        public string Discount { get; set; }

        /// <summary>
        /// Gets or sets price list id.
        /// </summary>
        public int PriceListId { get; set; }

        /// <summary>
        /// Gets or sets property price list.
        /// </summary>
        public string PriceList { get; set; }

        /// <summary>
        /// Gets or sets property price list.
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets concurrency check.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
