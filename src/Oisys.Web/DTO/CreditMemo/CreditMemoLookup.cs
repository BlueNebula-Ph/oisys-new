namespace OisysNew.DTO.CreditMemo
{
    public class CreditMemoLookup : DTOBase
    {
        /// <summary>
        /// Gets or sets property Code.
        /// </summary>
        public string Code { get; set; }

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
