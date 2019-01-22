using OisysNew.Helpers;
using System;

namespace OisysNew.Models
{
    /// <summary>
    /// The cash voucher model
    /// </summary>
    public class CashVoucher : ModelBase
    {
        /// <summary>
        /// Gets or sets the voucher number
        /// </summary>
        public int VoucherNumber { get; set; }

        /// <summary>
        /// Gets or sets the voucher date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets pay to
        /// </summary>
        public string PayTo { get; set; }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets released by
        /// </summary>
        public string ReleasedBy { get; set; }

        /// <summary>
        /// Gets or sets category
        /// </summary>
        public VoucherCategory Category { get; set; }
    }
}