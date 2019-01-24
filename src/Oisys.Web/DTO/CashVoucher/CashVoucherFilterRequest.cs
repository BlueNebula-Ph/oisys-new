using System;

namespace OisysNew.DTO.CashVoucher
{
    /// <summary>
    /// <see cref="CashVoucherFilterRequest"/> class represents basic cash voucher filters for displaying data.
    /// </summary>
    public class CashVoucherFilterRequest : FilterRequestBase
    {
        /// <summary>
        /// Gets or sets cash voucher date from filter.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets cash voucher date to filter.
        /// </summary>
        public DateTime? DateTo { get; set; }
    }
}
