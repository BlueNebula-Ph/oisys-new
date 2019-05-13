namespace OisysNew.DTO.Invoice
{
    /// <summary>
    /// <see cref="SaveInvoiceLineItemRequest"/> is used for data transfer for creating and updating invoice details.
    /// </summary>
    public class SaveInvoiceLineItemRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets order id.
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// Gets or sets credit memo id.
        /// </summary>
        public long? CreditMemoId { get; set; }
    }
}
