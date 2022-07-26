//Copyright © 2022 Contributors of moose-org, This code is licensed under the BSD 3-Clause "New" or "Revised" License.
using System.Runtime.CompilerServices;
using Internal.Runtime;

namespace System
{
	public unsafe struct EETypePtr
	{
		internal unsafe EEType* ToPointer()
		{
			return Value;
		}

		internal EEType* Value;

		public bool IsSzArray => Value->IsSzArray;

		public EETypePtr ArrayElementType => new(Value->RelatedParameterType);

		internal int ArrayRank => Value->ArrayRank;

		public IntPtr RawValue => (IntPtr)Value;

		public EETypePtr(IntPtr value)
		{
			Value = (EEType*)value;
		}

		public EETypePtr(EEType* value)
		{
			Value = value;
		}

		[Intrinsic]
		internal static EETypePtr EETypePtrOf<T>()
		{
			// Compilers are required to provide a low level implementation of this method.
			// This can be achieved by optimizing away the reflection part of this implementation
			// by optimizing typeof(!!0).TypeHandle into "ldtoken !!0", or by
			// completely replacing the body of this method.
			return default;
		}
	}
}