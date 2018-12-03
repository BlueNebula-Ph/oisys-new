namespace OisysNew.DTO.Invoice
{
    /// <summary>
    /// <see cref="SaveInvoiceDetailRequest"/> is used for data transfer for creating and updating invoice details.
    /// </summary>
    public class SaveInvoiceDetailRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets order id.
        /// </summary>
        public int OrderId { get; set; }
    }
}
