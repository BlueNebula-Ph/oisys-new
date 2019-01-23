using OisysNew.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace OisysNew.DTO.CashVoucher
{
    /// <summary>
    /// The request for saving cash vouchers
    /// </summary>
    public class SaveCashVoucherRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets property Voucher Number.
        /// </summary>
        public int VoucherNumber { get; set; }

        /// <summary>
        /// Gets or sets the voucher date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets pay to
        /// </summary>
        [Required(ErrorMessage = "Please input pay to")]
        public string PayTo { get; set; }

        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets category
        /// </summary>
        public VoucherCategory Category { get; set; }

        /// <summary>
        /// Gets or sets amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets released by
        /// </summary>
        public string ReleasedBy { get; set; }
    }
}