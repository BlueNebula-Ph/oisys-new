namespace OisysNew.DTO.User
{
    /// <summary>
    /// The view model of the user.
    /// </summary>
    public class UserSummary : DTOBase
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user is an admin.
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user has view access.
        /// </summary>
        public bool CanView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user has write access.
        /// </summary>
        public bool CanWrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user has delete access.
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// Gets or sets the concurrency check.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
