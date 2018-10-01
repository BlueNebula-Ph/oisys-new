namespace OisysNew.Helpers
{
    using System.ComponentModel;

    /// <summary>
    /// Values to define different transaction types
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Debit Transaction
        /// </summary>
        [DisplayName("Debit")]
        Debit,

        /// <summary>
        /// Credit Transaction
        /// </summary>
        [DisplayName("Credit")]
        Credit,
    }
}