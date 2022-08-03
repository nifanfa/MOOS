using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
	///
	/// </summary>
	public interface IAsyncResult
    {
        object AsyncState
        {
            get;
        }

        WaitHandle AsyncWaitHandle
        {
            get;
        }

        bool CompletedSynchronously
        {
            get;
        }

        bool IsCompleted
        {
            get;
        }
    }
}
