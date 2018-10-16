namespace OisysNew.DTO.Customer
{
    /// <summary>
    /// A lookup class for customers and their corresponding orders.
    /// </summary>
    public class CustomerWithOrdersLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets orders of the customer.
        /// </summary>
        //public List<OrderDetailLookup> OrderDetails { get; set; }
    }
}
