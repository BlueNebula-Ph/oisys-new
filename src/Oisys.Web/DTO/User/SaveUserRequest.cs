namespace OisysNew.DTO.User
{
    /// <summary>
    /// The request for saving a new user.
    /// </summary>
    public class SaveUserRequest : DTOBase
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the firstname of the user.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets the lastname of the user.
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the access rights of the user.
        /// </summary>
        public string AccessRights { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to save a new password.
        /// </summary>
        public bool UpdatePassword { get; set; }

        /// <summary>
        /// Gets or sets the concurrency check.
        /// </summary>
        public string RowVersion { get; set; }
    }
}
