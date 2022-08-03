using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
	/// Implementation of the "System.InvalidOperationException" class
	/// </summary>
	public class InvalidOperationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationException"/> class.
        /// </summary>
        public InvalidOperationException()
            : this("The requested operation cannot be performed.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidOperationException(string message)
            : base(message)
        { }
    }
}
