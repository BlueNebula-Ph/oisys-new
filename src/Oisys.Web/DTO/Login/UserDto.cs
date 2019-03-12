namespace OisysNew.DTO.Login
{
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the username of the logged in user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the fullname of the logged in user.
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Gets or sets the jwt token of the logged in user.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an admin.
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can view records.
        /// </summary>
        public bool CanView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can save records.
        /// </summary>
        public bool CanWrite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can delete records.
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// Gets or sets the access rights of the user.
        /// </summary>
        public string AccessRights { get; set; }
    }
}
