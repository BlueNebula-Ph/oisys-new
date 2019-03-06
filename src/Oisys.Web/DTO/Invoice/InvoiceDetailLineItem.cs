namespace OisysNew.DTO.Invoice
{
    public class InvoiceDetailLineItem : DTOBase
    {
        /// <summary>
        /// Gets or sets order id.
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// Gets or sets credit memo id.
        /// </summary>
        public long? CreditMemoId { get; set; }

        /// <summary>
        /// Gets or sets code property.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets date property.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets total amount property.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets line item type.
        /// </summary>
        public string Type { get; set; }
    }
}
