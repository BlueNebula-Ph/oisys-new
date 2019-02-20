namespace OisysNew.DTO.CreditMemo
{
    /// <summary>
    /// View model for the credit memo entity.
    /// </summary>
    public class CreditMemoSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets property credit memo code.
        /// </summary>
        public string Code { get; set; }

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
        /// Gets or sets total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
