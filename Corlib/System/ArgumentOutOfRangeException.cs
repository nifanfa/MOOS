using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
	/// Implementation of the "System.ArgumentOutOfRangeException" class
	/// </summary>
	public class ArgumentOutOfRangeException : ArgumentException
    {
        //private object _actualValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentOutOfRangeException"/> class.
        /// </summary>
        public ArgumentOutOfRangeException()
            : this("Argument is out of range.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentOutOfRangeException"/> class with the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="paramName"></param>
        public ArgumentOutOfRangeException(string paramName)
            : base("Argument is out of range.", paramName)
        { }

        /// <summary>
        /// Initializes an instance of the <see cref="ArgumentOutOfRangeException"/> class with the name of the parameter that causes this exception and a specified error message.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        public ArgumentOutOfRangeException(string paramName, string message)
            : base(message, paramName)
        { }

        //public ArgumentOutOfRangeException(string paramName, object actualValue, string message)
        //: base(message, paramName)
        //{
        //	_actualValue = actualValue;
        //}
    }
}
