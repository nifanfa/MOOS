using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
	/// Implementation of the "System.ObjectDisposedException" class
	/// </summary>
	public class ObjectDisposedException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisposedException"/> class.
        /// </summary>
        public ObjectDisposedException()
            : this("The object was used after being disposed.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisposedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ObjectDisposedException(string message)
            : base(message)
        { }
    }
}
