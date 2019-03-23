namespace OisysNew.DTO.Report
{
    public class OrderReport
    {
        /// <summary>
        /// Gets or sets order code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets order customer
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets order date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets order due date
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// Gets or sets order total
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
