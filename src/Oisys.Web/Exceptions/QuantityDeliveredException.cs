using System;

namespace OisysNew.Exceptions
{
    /// <summary>
    /// Exception thrown when quantity delivered is greater than original quantity
    /// </summary>
    public class QuantityDeliveredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityDeliveredException"/> class.
        /// </summary>
        /// <param name="message">The error message</param>
        public QuantityDeliveredException(string message)
            : base(message)
        {
        }
    }
}
