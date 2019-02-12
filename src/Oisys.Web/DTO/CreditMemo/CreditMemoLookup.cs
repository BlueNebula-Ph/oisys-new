namespace OisysNew.DTO.CreditMemo
{
    public class CreditMemoLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets customer id.
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets customer name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the customer address.
        /// </summary>
        public string CustomerAddress { get; set; }

        /// <summary>
        /// Gets or sets property Date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets property Driver.
        /// </summary>
        public string Driver { get; set; }

        /// <summary>
        /// Gets or sets total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
