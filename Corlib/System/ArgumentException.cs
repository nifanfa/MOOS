using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
	/// Implementation of the "System.ArgumentException" class
	/// </summary>
	public class ArgumentException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        protected string paramName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentException"/> class.
        /// </summary>
        public ArgumentException()
            : this("Value does not fall within the expected range.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ArgumentException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="paramName">Name of the param.</param>
        public ArgumentException(string message, string paramName)
            : base(message)
        {
            this.paramName = paramName;
        }

        /// <summary>
        /// Gets the name of the param.
        /// </summary>
        /// <value>The name of the param.</value>
        public virtual string ParamName
        {
            get { return paramName; }
        }
    }
}
