using System;

namespace OisysNew.Exceptions
{
    /// <summary>
    /// Exception thrown when quantity returned is greater than original quantity
    /// </summary>
    public class QuantityReturnedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityReturnedException"/> class.
        /// </summary>
        /// <param name="message">The error message</param>
        public QuantityReturnedException(string message)
            : base(message)
        {
        }
    }
}
