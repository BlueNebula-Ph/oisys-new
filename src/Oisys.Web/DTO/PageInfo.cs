namespace OisysNew.DTO
{
    /// <summary>
    /// Paging information for server side paging
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// Page number to be fetched.
        /// This is 0 based.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Total size of page
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Total records
        /// </summary>
        public int TotalElements { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }
    }
}
