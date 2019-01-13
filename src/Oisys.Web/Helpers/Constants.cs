namespace OisysNew.Helpers
{
    /// <summary>
    /// <see cref="Constants"/> class to hold commonly used values object.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Sets the error message key
        /// </summary>
        public const string ErrorMessage = "ErrorMessage";

        /// <summary>
        /// Sets the sort direction ascending value
        /// </summary>
        public const string SortDirectionAscending = "asc";

        /// <summary>
        /// Sets the sort direction descending value
        /// </summary>
        public const string SortDirectionDescending = "desc";

        /// <summary>
        /// Sets the default sort direction
        /// </summary>
        public const string DefaultSortDirection = SortDirectionAscending;

        /// <summary>
        /// Sets the default page size
        /// </summary>
        public const int DefaultPageSize = 20;

        /// <summary>
        /// Sets the default page index
        /// </summary>
        public const int DefaultPageIndex = 1;

        /// <summary>
        /// <see cref="TransactionType"/> class to hold commonly used values object.
        /// </summary>
        public static class TransactionType
        {
            /// <summary>
            /// Sets the customer transaction type value
            /// </summary>
            public const string Payment = "Payment";

            /// <summary>
            /// Sets the customer transaction type value
            /// </summary>
            public const string Delivery = "Delivery";
        }

        /// <summary>
        /// Common remarks for adjustments
        /// </summary>
        public static class AdjustmentRemarks
        {
            /// <summary>
            /// The initial quantity text.
            /// </summary>
            public const string InitialQuantity = "Initial quantity";

            /// <summary>
            /// The order created text.
            /// </summary>
            public const string OrderCreated = "Order created";

            /// <summary>
            /// The order updated text.
            /// </summary>
            public const string OrderUpdated = "Order updated";

            /// <summary>
            /// The order deleted text.
            /// </summary>
            public const string OrderDeleted = "Order deleted";

            /// <summary>
            /// The order detail created text.
            /// </summary>
            public const string OrderDetailCreated = "Order detail added";

            /// <summary>
            /// The order detail created text.
            /// </summary>
            public const string OrderDetailDeleted = "Order detail deleted";

            /// <summary>
            /// The delivery created text.
            /// </summary>
            public const string DeliveryCreated = "Delivery created";

            /// <summary>
            /// The delivery updated text.
            /// </summary>
            public const string DeliveryUpdated = "Delivery updated";

            /// <summary>
            /// The delivery deleted text.
            /// </summary>
            public const string DeliveryDeleted = "Delivery deleted";

            /// <summary>
            /// The creditmemo created text.
            /// </summary>
            public const string CreditMemoCreated = "Credit memo created";

            /// <summary>
            /// The creditmemo detail created text.
            /// </summary>
            public const string CreditMemoDetailCreated = "Credit memo detail created";

            /// <summary>
            /// The creditmemo updated text.
            /// </summary>
            public const string CreditMemoUpdated = "Credit memo updated";

            /// <summary>
            /// The creditmemo deleted text.
            /// </summary>
            public const string CreditMemoDeleted = "Credit memo deleted";

            /// <summary>
            /// The creditmemo detail deleted text.
            /// </summary>
            public const string CreditMemoDetailDeleted = "Credit memo detail deleted";
        }

        /// <summary>
        /// Column name constants
        /// </summary>
        public static class ColumnNames
        {
            /// <summary>
            /// The code column name.
            /// </summary>
            public const string Code = "Code";

            /// <summary>
            /// The invoice number column name.
            /// </summary>
            public const string InvoiceNumber = "InvoiceNumber";

            /// <summary>
            /// The name column name.
            /// </summary>
            public const string Name = "Name";

            /// <summary>
            /// The quote number column name.
            /// </summary>
            public const string QuoteNumber = "QuoteNumber";
        }
    }
}