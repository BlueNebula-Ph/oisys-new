using OisysNew.DTO.CreditMemo;
using OisysNew.DTO.Order;

namespace OisysNew.DTO.Invoice
{
    public class InvoiceDetailLineItem : DTOBase
    {
        /// <summary>
        /// Gets or sets the invoice order.
        /// </summary>
        public OrderLookup Order { get; set; }

        /// <summary>
        /// Gets or sets the invoice credit memo.
        /// </summary>
        public CreditMemoLookup CreditMemo { get; set; }
    }
}
