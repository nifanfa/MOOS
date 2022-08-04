using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Implementation of the "System.Exception" class
    /// </summary>
    [Serializable]
    public class Exception
    {
        private readonly string message;
        private readonly Exception innerException;

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
        /// </summary>
        //public virtual IDictionary Data { get; }

        /// <summary>
        /// Gets the Exception instance that caused the current exception.
        /// </summary>
        public Exception InnerException
        {
            get { return innerException; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class.
        /// </summary>
        public Exception()
            : this("An exception was thrown.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public Exception(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public Exception(string message, Exception innerException)
        {
            this.message = message;
            this.innerException = innerException;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public virtual string Message
        {
            get { return message; }
        }
    }
}
