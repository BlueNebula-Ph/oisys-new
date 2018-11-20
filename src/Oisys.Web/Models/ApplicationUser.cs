namespace OisysNew.Models
{
    /// <summary>
    /// View model for the user entity.
    /// </summary>
    public class ApplicationUser : SoftDeletableModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the firstname of the user.
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets the lastname of the user.
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the permissions of the user.
        /// ex. CanRead, CanWrite, CanDelete or Admin
        /// </summary>
        public string AccessRights { get; set; }
    }
}
