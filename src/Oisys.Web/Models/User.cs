namespace OisysNew.Models
{
    /// <summary>
    /// <see cref="User"/> class represents User of application.
    /// </summary>
    public class User : ModelBase
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

        /// <summary>
        /// Gets or sets a value indicating whether the user is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}