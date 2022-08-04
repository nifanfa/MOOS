using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
	/// Defines the valid calling conventions for a method.
	/// </summary>
	[Serializable]
    [Flags]
    public enum CallingConventions : byte
    {
        /// <summary>
        /// Specifies the default calling convention as determined by the common language runtime. Use this calling convention for static methods. For instance or virtual methods use HasThis.
        /// </summary>
        Standard = 0x01,

        /// <summary>
        /// Specifies the calling convention for methods with variable arguments.
        /// </summary>
        VarArgs = 0x02,

        /// <summary>
        /// Specifies that either the Standard or the VarArgs calling convention may be used.
        /// </summary>
        Any = 0x03,

        /// <summary>
        /// Specifies an instance or virtual method (not a static method).
        /// At run-time, the called method is passed a pointer to the target object as its first argument (the this pointer).
        /// The signature stored in metadata does not include the type of this first argument, because the method is known and its owner class can be discovered from metadata.
        /// </summary>
        HasThis = 0x20,

        /// <summary>
        /// Specifies that the signature is a function-pointer signature, representing a call to an instance or virtual method (not a static method).
        /// If ExplicitThis is set, HasThis must also be set. The first argument passed to the called method is still a this pointer, but the type of the first argument is now unknown.
        /// Therefore, a token that describes the type (or class) of the this pointer is explicitly stored into its metadata signature.
        /// </summary>
        ExplicitThis = 0x40,
    }
}
