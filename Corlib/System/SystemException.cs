using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
	/// Implementation of the "System.SystemException" class
	/// </summary>
	[Serializable]
    public class SystemException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemException"/> class.
        /// </summary>
        public SystemException()
            : base("A system exception was thrown.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SystemException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public SystemException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
