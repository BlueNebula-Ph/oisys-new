namespace OisysNew.DTO.Dashboard
{
    public class DashboardOrder
    {
        /// <summary>
        /// Gets or sets the order code.
        /// </summary>
        public int OrderCode { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the order due date.
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// Gets or sets the quantity ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit of the item ordered.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the item code ordered.
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the item name ordered.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the category of the item ordered.
        /// </summary>
        public string Category { get; set; }
    }
}
