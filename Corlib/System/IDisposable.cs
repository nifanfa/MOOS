using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Interface for "System.IDisposable"
    /// </summary>
    public interface IDisposable
    {
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        void Dispose();
    }
}
