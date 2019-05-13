using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OisysNew.Exceptions
{
    /// <summary>
    /// Exception thrown when quantity goes below zero
    /// </summary>
    public class QuantityBelowZeroException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityBelowZeroException"/> class.
        /// </summary>
        /// <param name="message">The error message</param>
        public QuantityBelowZeroException(string message)
            : base(message)
        {

        }
    }
}
