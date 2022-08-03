using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Implementation of the "System.NotImplementedException" class
    /// </summary>
    [Serializable]
    public class NotImplementedException : SystemException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedException"/> class.
        /// </summary>
        public NotImplementedException() : base("A Not Implemented exception was thrown.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotImplementedException(string message) : base(message)
        { }
    }
}
