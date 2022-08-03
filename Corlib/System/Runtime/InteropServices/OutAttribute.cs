using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.InteropServices
{
    /// <summary>
	/// Implementation of the "System.Runtime.InteropServices.OutAttribute" class
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
    public class OutAttribute : Attribute
    {
    }
}
