using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Implementation of the "System.NotSupportedException" class
    /// </summary>
    public class NotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotSupportedException"/> class.
        /// </summary>
        public NotSupportedException() :
            this("Operation is not supported.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSupportedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotSupportedException(string message) :
            base(message)
        {
        }
    }
}
