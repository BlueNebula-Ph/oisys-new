namespace OisysNew.DTO.Customer
{
    public class CustomerTransactionSummary
    {
        /// <summary>
        /// Gets or sets the order or credit memo code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets order or credit memo date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets type of transaction
        /// Values: Order, Credit Memo
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the transaction
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether transaction is invoiced
        /// </summary>
        public bool IsInvoiced { get; set; }
    }
}
