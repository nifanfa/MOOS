namespace System.Private.CoreLib
{
	internal static class Strings { }
}
namespace System
{
	internal static partial class SR
	{
		/// <summary>Cannot create an abstract class.</summary>
		internal static string @Acc_CreateAbst = "Cannot create an abstract class.";
		/// <summary>Cannot create an instance of {0} because it is an abstract class.</summary>
		internal static string @Acc_CreateAbstEx = "Cannot create an instance of {0} because it is an abstract class.";
		/// <summary>Cannot dynamically create an instance of ArgIterator.</summary>
		internal static string @Acc_CreateArgIterator = "Cannot dynamically create an instance of ArgIterator.";
		/// <summary>Cannot create a type for which Type.ContainsGenericParameters is true.</summary>
		internal static string @Acc_CreateGeneric = "Cannot create a type for which Type.ContainsGenericParameters is true.";
		/// <summary>Cannot create an instance of {0} because Type.ContainsGenericParameters is true.</summary>
		internal static string @Acc_CreateGenericEx = "Cannot create an instance of {0} because Type.ContainsGenericParameters is true.";
		/// <summary>Cannot create an instance of an interface.</summary>
		internal static string @Acc_CreateInterface = "Cannot create an instance of an interface.";
		/// <summary>Cannot create an instance of {0} because it is an interface.</summary>
		internal static string @Acc_CreateInterfaceEx = "Cannot create an instance of {0} because it is an interface.";
		/// <summary>Cannot dynamically create an instance of System.Void.</summary>
		internal static string @Acc_CreateVoid = "Cannot dynamically create an instance of System.Void.";
		/// <summary>Type initializer was not callable.</summary>
		internal static string @Acc_NotClassInit = "Type initializer was not callable.";
		/// <summary>Cannot set a constant field.</summary>
		internal static string @Acc_ReadOnly = "Cannot set a constant field.";
		/// <summary>Cannot create an instance of void.</summary>
		internal static string @Access_Void = "Cannot create an instance of void.";
		/// <summary>One or more errors occurred.</summary>
		internal static string @AggregateException_ctor_DefaultMessage = "One or more errors occurred.";
		/// <summary>An element of innerExceptions was null.</summary>
		internal static string @AggregateException_ctor_InnerExceptionNull = "An element of innerExceptions was null.";
		/// <summary>The serialization stream contains no inner exceptions.</summary>
		internal static string @AggregateException_DeserializationFailure = "The serialization stream contains no inner exceptions.";
		/// <summary>(Inner Exception #{0}) This text is prepended to each inner exception description during aggregate exception formatting</summary>
		internal static string @AggregateException_InnerException = "(Inner Exception #{0}) This text is prepended to each inner exception description during aggregate exception formatting";
		/// <summary>Name:</summary>
		internal static string @AppDomain_Name = "Name:";
		/// <summary>There are no context policies.</summary>
		internal static string @AppDomain_NoContextPolicies = "There are no context policies.";
		/// <summary>Default principal object cannot be set twice.</summary>
		internal static string @AppDomain_Policy_PrincipalTwice = "Default principal object cannot be set twice.";
		/// <summary>Ambiguous implementation found.</summary>
		internal static string @AmbiguousImplementationException_NullMessage = "Ambiguous implementation found.";
		/// <summary>Cannot access member.</summary>
		internal static string @Arg_AccessException = "Cannot access member.";
		/// <summary>Attempted to read or write protected memory. This is often an indication that other memory is corrupt.</summary>
		internal static string @Arg_AccessViolationException = "Attempted to read or write protected memory. This is often an indication that other memory is corrupt.";
		/// <summary>Ambiguous match found.</summary>
		internal static string @Arg_AmbiguousMatchException = "Ambiguous match found.";
		/// <summary>Error in the application.</summary>
		internal static string @Arg_ApplicationException = "Error in the application.";
		/// <summary>Value does not fall within the expected range.</summary>
		internal static string @Arg_ArgumentException = "Value does not fall within the expected range.";
		/// <summary>Specified argument was out of the range of valid values.</summary>
		internal static string @Arg_ArgumentOutOfRangeException = "Specified argument was out of the range of valid values.";
		/// <summary>Overflow or underflow in the arithmetic operation.</summary>
		internal static string @Arg_ArithmeticException = "Overflow or underflow in the arithmetic operation.";
		/// <summary>Array lengths must be the same.</summary>
		internal static string @Arg_ArrayLengthsDiffer = "Array lengths must be the same.";
		/// <summary>Destination array is not long enough to copy all the items in the collection. Check array index and length.</summary>
		internal static string @Arg_ArrayPlusOffTooSmall = "Destination array is not long enough to copy all the items in the collection. Check array index and length.";
		/// <summary>Attempted to access an element as a type incompatible with the array.</summary>
		internal static string @Arg_ArrayTypeMismatchException = "Attempted to access an element as a type incompatible with the array.";
		/// <summary>Array must not be of length zero.</summary>
		internal static string @Arg_ArrayZeroError = "Array must not be of length zero.";
		/// <summary>Read an invalid decimal value from the buffer.</summary>
		internal static string @Arg_BadDecimal = "Read an invalid decimal value from the buffer.";
		/// <summary>Format of the executable (.exe) or library (.dll) is invalid.</summary>
		internal static string @Arg_BadImageFormatException = "Format of the executable (.exe) or library (.dll) is invalid.";
		/// <summary>Encountered an invalid type for a default value.</summary>
		internal static string @Arg_BadLiteralFormat = "Encountered an invalid type for a default value.";
		/// <summary>Unable to sort because the IComparer.Compare() method returns inconsistent results. Either a value does not compare equal to itself, or one value repeatedly compared to another value yields different results. IComparer: '{0}'.</summary>
		internal static string @Arg_BogusIComparer = "Unable to sort because the IComparer.Compare() method returns inconsistent results. Either a value does not compare equal to itself, or one value repeatedly compared to another value yields different results. IComparer: '{0}'.";
		/// <summary>Not enough space available in the buffer.</summary>
		internal static string @Arg_BufferTooSmall = "Not enough space available in the buffer.";
		/// <summary>TimeSpan does not accept floating point Not-a-Number values.</summary>
		internal static string @Arg_CannotBeNaN = "TimeSpan does not accept floating point Not-a-Number values.";
		/// <summary>String cannot contain a minus sign if the base is not 10.</summary>
		internal static string @Arg_CannotHaveNegativeValue = "String cannot contain a minus sign if the base is not 10.";
		/// <summary>The usage of IKeyComparer and IHashCodeProvider/IComparer interfaces cannot be mixed; use one or the other.</summary>
		internal static string @Arg_CannotMixComparisonInfrastructure = "The usage of IKeyComparer and IHashCodeProvider/IComparer interfaces cannot be mixed; use one or the other.";
		/// <summary>Attempt to unload the AppDomain failed.</summary>
		internal static string @Arg_CannotUnloadAppDomainException = "Attempt to unload the AppDomain failed.";
		/// <summary>Failed to resolve type from string "{0}" which was embedded in custom attribute blob.</summary>
		internal static string @Arg_CATypeResolutionFailed = "Failed to resolve type from string '{0}' which was embedded in custom attribute blob.";
		/// <summary>Must specify property Set or Get or method call for a COM Object.</summary>
		internal static string @Arg_COMAccess = "Must specify property Set or Get or method call for a COM Object.";
		/// <summary>Error HRESULT E_FAIL has been returned from a call to a COM component.</summary>
		internal static string @Arg_COMException = "Error HRESULT E_FAIL has been returned from a call to a COM component.";
		/// <summary>Only one of the following binding flags can be set: BindingFlags.SetProperty, BindingFlags.PutDispProperty, BindingFlags.PutRefDispProperty.</summary>
		internal static string @Arg_COMPropSetPut = "Only one of the following binding flags can be set: BindingFlags.SetProperty, BindingFlags.PutDispProperty, BindingFlags.PutRefDispProperty.";
		/// <summary>Cannot specify both CreateInstance and another access type.</summary>
		internal static string @Arg_CreatInstAccess = "Cannot specify both CreateInstance and another access type.";
		/// <summary>Error occurred during a cryptographic operation.</summary>
		internal static string @Arg_CryptographyException = "Error occurred during a cryptographic operation.";
		/// <summary>Binary format of the specified custom attribute was invalid.</summary>
		internal static string @Arg_CustomAttributeFormatException = "Binary format of the specified custom attribute was invalid.";
		/// <summary>A datatype misalignment was detected in a load or store instruction.</summary>
		internal static string @Arg_DataMisalignedException = "A datatype misalignment was detected in a load or store instruction.";
		/// <summary>Decimal constructor requires an array or span of four valid decimal bytes.</summary>
		internal static string @Arg_DecBitCtor = "Decimal constructor requires an array or span of four valid decimal bytes.";
		/// <summary>Attempted to access a path that is not on the disk.</summary>
		internal static string @Arg_DirectoryNotFoundException = "Attempted to access a path that is not on the disk.";
		/// <summary>Attempted to divide by zero.</summary>
		internal static string @Arg_DivideByZero = "Attempted to divide by zero.";
		/// <summary>Delegate to an instance method cannot have null 'this'.</summary>
		internal static string @Arg_DlgtNullInst = "Delegate to an instance method cannot have null 'this'.";
		/// <summary>Cannot bind to the target method because its signature is not compatible with that of the delegate type.</summary>
		internal static string @Arg_DlgtTargMeth = "Cannot bind to the target method because its signature is not compatible with that of the delegate type.";
		/// <summary>Delegates must be of the same type.</summary>
		internal static string @Arg_DlgtTypeMis = "Delegates must be of the same type.";
		/// <summary>Dll was not found.</summary>
		internal static string @Arg_DllNotFoundException = "Dll was not found.";
		/// <summary>Duplicate objects in argument.</summary>
		internal static string @Arg_DuplicateWaitObjectException = "Duplicate objects in argument.";
		/// <summary>This ExceptionHandlingClause is not a clause.</summary>
		internal static string @Arg_EHClauseNotClause = "This ExceptionHandlingClause is not a clause.";
		/// <summary>This ExceptionHandlingClause is not a filter.</summary>
		internal static string @Arg_EHClauseNotFilter = "This ExceptionHandlingClause is not a filter.";
		/// <summary>Array may not be empty.</summary>
		internal static string @Arg_EmptyArray = "Array may not be empty.";
		/// <summary>Attempted to read past the end of the stream.</summary>
		internal static string @Arg_EndOfStreamException = "Attempted to read past the end of the stream.";
		/// <summary>Entry point was not found.</summary>
		internal static string @Arg_EntryPointNotFoundException = "Entry point was not found.";
		/// <summary>Object must be the same type as the enum. The type passed in was '{0}'; the enum type was '{1}'.</summary>
		internal static string @Arg_EnumAndObjectMustBeSameType = "Object must be the same type as the enum. The type passed in was '{0}'; the enum type was '{1}'.";
		/// <summary>Enum underlying type and the object must be same type or object. Type passed in was '{0}'; the enum underlying type was '{1}'.</summary>
		internal static string @Arg_EnumFormatUnderlyingTypeAndObjectMustBeSameType = "Enum underlying type and the object must be same type or object. Type passed in was '{0}'; the enum underlying type was '{1}'.";
		/// <summary>Illegal enum value: {0}.</summary>
		internal static string @Arg_EnumIllegalVal = "Illegal enum value: {0}.";
		/// <summary>Literal value was not found.</summary>
		internal static string @Arg_EnumLitValueNotFound = "Literal value was not found.";
		/// <summary>Enum underlying type and the object must be same type or object must be a String. Type passed in was '{0}'; the enum underlying type was '{1}'.</summary>
		internal static string @Arg_EnumUnderlyingTypeAndObjectMustBeSameType = "Enum underlying type and the object must be same type or object must be a String. Type passed in was '{0}'; the enum underlying type was '{1}'.";
		/// <summary>Requested value '{0}' was not found.</summary>
		internal static string @Arg_EnumValueNotFound = "Requested value '{0}' was not found.";
		/// <summary>Internal error in the runtime.</summary>
		internal static string @Arg_ExecutionEngineException = "Internal error in the runtime.";
		/// <summary>External component has thrown an exception.</summary>
		internal static string @Arg_ExternalException = "External component has thrown an exception.";
		/// <summary>Attempted to access a field that is not accessible by the caller.</summary>
		internal static string @Arg_FieldAccessException = "Attempted to access a field that is not accessible by the caller.";
		/// <summary>Field '{0}' defined on type '{1}' is not a field on the target object which is of type '{2}'.</summary>
		internal static string @Arg_FieldDeclTarget = "Field '{0}' defined on type '{1}' is not a field on the target object which is of type '{2}'.";
		/// <summary>No arguments can be provided to Get a field value.</summary>
		internal static string @Arg_FldGetArgErr = "No arguments can be provided to Get a field value.";
		/// <summary>Cannot specify both GetField and SetProperty.</summary>
		internal static string @Arg_FldGetPropSet = "Cannot specify both GetField and SetProperty.";
		/// <summary>Only the field value can be specified to set a field value.</summary>
		internal static string @Arg_FldSetArgErr = "Only the field value can be specified to set a field value.";
		/// <summary>Cannot specify both Get and Set on a field.</summary>
		internal static string @Arg_FldSetGet = "Cannot specify both Get and Set on a field.";
		/// <summary>Cannot specify Set on a Field and Invoke on a method.</summary>
		internal static string @Arg_FldSetInvoke = "Cannot specify Set on a Field and Invoke on a method.";
		/// <summary>Cannot specify both SetField and GetProperty.</summary>
		internal static string @Arg_FldSetPropGet = "Cannot specify both SetField and GetProperty.";
		/// <summary>One of the identified items was in an invalid format.</summary>
		internal static string @Arg_FormatException = "One of the identified items was in an invalid format.";
		/// <summary>Method must be called on a Type for which Type.IsGenericParameter is false.</summary>
		internal static string @Arg_GenericParameter = "Method must be called on a Type for which Type.IsGenericParameter is false.";
		/// <summary>Property Get method was not found.</summary>
		internal static string @Arg_GetMethNotFnd = "Property Get method was not found.";
		/// <summary>Byte array for Guid must be exactly {0} bytes long.</summary>
		internal static string @Arg_GuidArrayCtor = "Byte array for Guid must be exactly {0} bytes long.";
		/// <summary>Handle does not support asynchronous operations. The parameters to the FileStream constructor may need to be changed to indicate that the handle was opened synchronously (that is, it was not opened for overlapped I/O).</summary>
		internal static string @Arg_HandleNotAsync = "Handle does not support asynchronous operations. The parameters to the FileStream constructor may need to be changed to indicate that the handle was opened synchronously (that is, it was not opened for overlapped I/O).";
		/// <summary>Handle does not support synchronous operations. The parameters to the FileStream constructor may need to be changed to indicate that the handle was opened asynchronously (that is, it was opened explicitly for overlapped I/O).</summary>
		internal static string @Arg_HandleNotSync = "Handle does not support synchronous operations. The parameters to the FileStream constructor may need to be changed to indicate that the handle was opened asynchronously (that is, it was opened explicitly for overlapped I/O).";
		/// <summary>The number style AllowHexSpecifier is not supported on floating point data types.</summary>
		internal static string @Arg_HexStyleNotSupported = "The number style AllowHexSpecifier is not supported on floating point data types.";
		/// <summary>Hashtable's capacity overflowed and went negative. Check load factor, capacity and the current size of the table.</summary>
		internal static string @Arg_HTCapacityOverflow = "Hashtable's capacity overflowed and went negative. Check load factor, capacity and the current size of the table.";
		/// <summary>All indexes must be of type Int32.</summary>
		internal static string @Arg_IndexMustBeInt = "All indexes must be of type Int32.";
		/// <summary>Index was outside the bounds of the array.</summary>
		internal static string @Arg_IndexOutOfRangeException = "Index was outside the bounds of the array.";
		/// <summary>Insufficient stack to continue executing the program safely. This can happen from having too many functions on the call stack or function on the stack using too much stack space.</summary>
		internal static string @Arg_InsufficientExecutionStackException = "Insufficient stack to continue executing the program safely. This can happen from having too many functions on the call stack or function on the stack using too much stack space.";
		/// <summary>The ANSI string passed in could not be converted from the default ANSI code page to Unicode.</summary>
		internal static string @Arg_InvalidANSIString = "The ANSI string passed in could not be converted from the default ANSI code page to Unicode.";
		/// <summary>Invalid Base.</summary>
		internal static string @Arg_InvalidBase = "Invalid Base.";
		/// <summary>Specified cast is not valid.</summary>
		internal static string @Arg_InvalidCastException = "Specified cast is not valid.";
		/// <summary>Attempt has been made to use a COM object that does not have a backing class factory.</summary>
		internal static string @Arg_InvalidComObjectException = "Attempt has been made to use a COM object that does not have a backing class factory.";
		/// <summary>Specified filter criteria was invalid.</summary>
		internal static string @Arg_InvalidFilterCriteriaException = "Specified filter criteria was invalid.";
		/// <summary>Invalid handle.</summary>
		internal static string @Arg_InvalidHandle = "Invalid handle.";
		/// <summary>With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of those in HexNumber.</summary>
		internal static string @Arg_InvalidHexStyle = "With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of those in HexNumber.";
		/// <summary>The NeutralResourcesLanguageAttribute on the assembly "{0}" specifies an invalid culture name: "{1}".</summary>
		internal static string @Arg_InvalidNeutralResourcesLanguage_Asm_Culture = "The NeutralResourcesLanguageAttribute on the assembly '{0}' specifies an invalid culture name: '{1}'.";
		/// <summary>The NeutralResourcesLanguageAttribute specifies an invalid or unrecognized ultimate resource fallback location: "{0}".</summary>
		internal static string @Arg_InvalidNeutralResourcesLanguage_FallbackLoc = "The NeutralResourcesLanguageAttribute specifies an invalid or unrecognized ultimate resource fallback location: '{0}'.";
		/// <summary>Satellite contract version attribute on the assembly '{0}' specifies an invalid version: {1}.</summary>
		internal static string @Arg_InvalidSatelliteContract_Asm_Ver = "Satellite contract version attribute on the assembly '{0}' specifies an invalid version: {1}.";
		/// <summary>Specified OLE variant was invalid.</summary>
		internal static string @Arg_InvalidOleVariantTypeException = "Specified OLE variant was invalid.";
		/// <summary>Operation is not valid due to the current state of the object.</summary>
		internal static string @Arg_InvalidOperationException = "Operation is not valid due to the current state of the object.";
		/// <summary>The return Type must be a type provided by the runtime.</summary>
		internal static string @Arg_InvalidTypeInRetType = "The return Type must be a type provided by the runtime.";
		/// <summary>The signature Type array contains some invalid type (i.e. null, void)</summary>
		internal static string @Arg_InvalidTypeInSignature = "The signature Type array contains some invalid type (i.e. null, void)";
		/// <summary>The UTF8 string passed in could not be converted to Unicode.</summary>
		internal static string @Arg_InvalidUTF8String = "The UTF8 string passed in could not be converted to Unicode.";
		/// <summary>I/O error occurred.</summary>
		internal static string @Arg_IOException = "I/O error occurred.";
		/// <summary>The given key was not present in the dictionary.</summary>
		internal static string @Arg_KeyNotFound = "The given key was not present in the dictionary.";
		/// <summary>The given key '{0}' was not present in the dictionary.</summary>
		internal static string @Arg_KeyNotFoundWithKey = "The given key '{0}' was not present in the dictionary.";
		/// <summary>Destination array was not long enough. Check the destination index, length, and the array's lower bounds.</summary>
		internal static string @Arg_LongerThanDestArray = "Destination array was not long enough. Check the destination index, length, and the array's lower bounds.";
		/// <summary>Source array was not long enough. Check the source index, length, and the array's lower bounds.</summary>
		internal static string @Arg_LongerThanSrcArray = "Source array was not long enough. Check the source index, length, and the array's lower bounds.";
		/// <summary>Source string was not long enough. Check sourceIndex and count.</summary>
		internal static string @Arg_LongerThanSrcString = "Source string was not long enough. Check sourceIndex and count.";
		/// <summary>The arrays' lower bounds must be identical.</summary>
		internal static string @Arg_LowerBoundsMustMatch = "The arrays' lower bounds must be identical.";
		/// <summary>AsAny cannot be used on return types, ByRef parameters, ArrayWithOffset, or parameters passed from unmanaged to managed.</summary>
		internal static string @Arg_MarshalAsAnyRestriction = "AsAny cannot be used on return types, ByRef parameters, ArrayWithOffset, or parameters passed from unmanaged to managed.";
		/// <summary>Marshaling directives are invalid.</summary>
		internal static string @Arg_MarshalDirectiveException = "Marshaling directives are invalid.";
		/// <summary>Attempt to access the method failed.</summary>
		internal static string @Arg_MethodAccessException = "Attempt to access the method failed.";
		/// <summary>Attempted to access a non-existing field.</summary>
		internal static string @Arg_MissingFieldException = "Attempted to access a non-existing field.";
		/// <summary>Unable to find manifest resource.</summary>
		internal static string @Arg_MissingManifestResourceException = "Unable to find manifest resource.";
		/// <summary>Attempted to access a missing member.</summary>
		internal static string @Arg_MissingMemberException = "Attempted to access a missing member.";
		/// <summary>Attempted to access a missing method.</summary>
		internal static string @Arg_MissingMethodException = "Attempted to access a missing method.";
		/// <summary>Attempted to add multiple callbacks to a delegate that does not support multicast.</summary>
		internal static string @Arg_MulticastNotSupportedException = "Attempted to add multiple callbacks to a delegate that does not support multicast.";
		/// <summary>Object must be of type Boolean.</summary>
		internal static string @Arg_MustBeBoolean = "Object must be of type Boolean.";
		/// <summary>Object must be of type Byte.</summary>
		internal static string @Arg_MustBeByte = "Object must be of type Byte.";
		/// <summary>Object must be of type Char.</summary>
		internal static string @Arg_MustBeChar = "Object must be of type Char.";
		/// <summary>Object must be of type DateOnly. {Locked="DateOnly"}</summary>
		internal static string @Arg_MustBeDateOnly = "Object must be of type DateOnly. {Locked='DateOnly'}";
		/// <summary>Object must be of type TimeOnly. {Locked="TimeOnly"}</summary>
		internal static string @Arg_MustBeTimeOnly = "Object must be of type TimeOnly. {Locked='TimeOnly'}";
		/// <summary>Object must be of type DateTime.</summary>
		internal static string @Arg_MustBeDateTime = "Object must be of type DateTime.";
		/// <summary>Object must be of type DateTimeOffset.</summary>
		internal static string @Arg_MustBeDateTimeOffset = "Object must be of type DateTimeOffset.";
		/// <summary>Object must be of type Decimal.</summary>
		internal static string @Arg_MustBeDecimal = "Object must be of type Decimal.";
		/// <summary>Type must derive from Delegate.</summary>
		internal static string @Arg_MustBeDelegate = "Type must derive from Delegate.";
		/// <summary>Object must be of type Double.</summary>
		internal static string @Arg_MustBeDouble = "Object must be of type Double.";
		/// <summary>Drive name must be a root directory (i.e. 'C:\') or a drive letter ('C').</summary>
		internal static string @Arg_MustBeDriveLetterOrRootDir = "Drive name must be a root directory (i.e. 'C:\') or a drive letter ('C').";
		/// <summary>Type provided must be an Enum.</summary>
		internal static string @Arg_MustBeEnum = "Type provided must be an Enum.";
		/// <summary>The value passed in must be an enum base or an underlying type for an enum, such as an Int32.</summary>
		internal static string @Arg_MustBeEnumBaseTypeOrEnum = "The value passed in must be an enum base or an underlying type for an enum, such as an Int32.";
		/// <summary>Object must be of type GUID.</summary>
		internal static string @Arg_MustBeGuid = "Object must be of type GUID.";
		/// <summary>Object must be of type Int16.</summary>
		internal static string @Arg_MustBeInt16 = "Object must be of type Int16.";
		/// <summary>Object must be of type Int32.</summary>
		internal static string @Arg_MustBeInt32 = "Object must be of type Int32.";
		/// <summary>Object must be of type Int64.</summary>
		internal static string @Arg_MustBeInt64 = "Object must be of type Int64.";
		/// <summary>Object must be of type IntPtr.</summary>
		internal static string @Arg_MustBeIntPtr = "Object must be of type IntPtr.";
		/// <summary>Type passed must be an interface.</summary>
		internal static string @Arg_MustBeInterface = "Type passed must be an interface.";
		/// <summary>Type must be a Pointer.</summary>
		internal static string @Arg_MustBePointer = "Type must be a Pointer.";
		/// <summary>Object must be an array of primitives.</summary>
		internal static string @Arg_MustBePrimArray = "Object must be an array of primitives.";
		/// <summary>Object must be of type RuntimeAssembly.</summary>
		internal static string @Arg_MustBeRuntimeAssembly = "Object must be of type RuntimeAssembly.";
		/// <summary>Object must be of type SByte.</summary>
		internal static string @Arg_MustBeSByte = "Object must be of type SByte.";
		/// <summary>Object must be of type Single.</summary>
		internal static string @Arg_MustBeSingle = "Object must be of type Single.";
		/// <summary>Object must be of type String.</summary>
		internal static string @Arg_MustBeString = "Object must be of type String.";
		/// <summary>Object must be of type TimeSpan.</summary>
		internal static string @Arg_MustBeTimeSpan = "Object must be of type TimeSpan.";
		/// <summary>Type must be a type provided by the runtime.</summary>
		internal static string @Arg_MustBeType = "Type must be a type provided by the runtime.";
		/// <summary>Argument must be true.</summary>
		internal static string @Arg_MustBeTrue = "Argument must be true.";
		/// <summary>Object must be of type UInt16.</summary>
		internal static string @Arg_MustBeUInt16 = "Object must be of type UInt16.";
		/// <summary>Object must be of type UInt32.</summary>
		internal static string @Arg_MustBeUInt32 = "Object must be of type UInt32.";
		/// <summary>Object must be of type UInt64.</summary>
		internal static string @Arg_MustBeUInt64 = "Object must be of type UInt64.";
		/// <summary>Object must be of type UIntPtr.</summary>
		internal static string @Arg_MustBeUIntPtr = "Object must be of type UIntPtr.";
		/// <summary>Object must be of type Version.</summary>
		internal static string @Arg_MustBeVersion = "Object must be of type Version.";
		/// <summary>Must specify valid information for parsing in the string.</summary>
		internal static string @Arg_MustContainEnumInfo = "Must specify valid information for parsing in the string.";
		/// <summary>Named parameter value must not be null.</summary>
		internal static string @Arg_NamedParamNull = "Named parameter value must not be null.";
		/// <summary>Named parameter array cannot be bigger than argument array.</summary>
		internal static string @Arg_NamedParamTooBig = "Named parameter array cannot be bigger than argument array.";
		/// <summary>No PInvoke conversion exists for value passed to Object-typed parameter.</summary>
		internal static string @Arg_NDirectBadObject = "No PInvoke conversion exists for value passed to Object-typed parameter.";
		/// <summary>Array was not a one-dimensional array.</summary>
		internal static string @Arg_Need1DArray = "Array was not a one-dimensional array.";
		/// <summary>Array was not a two-dimensional array.</summary>
		internal static string @Arg_Need2DArray = "Array was not a two-dimensional array.";
		/// <summary>Array was not a three-dimensional array.</summary>
		internal static string @Arg_Need3DArray = "Array was not a three-dimensional array.";
		/// <summary>Must provide at least one rank.</summary>
		internal static string @Arg_NeedAtLeast1Rank = "Must provide at least one rank.";
		/// <summary>Argument count must not be negative.</summary>
		internal static string @Arg_NegativeArgCount = "Argument count must not be negative.";
		/// <summary>Must specify binding flags describing the invoke operation required (BindingFlags.InvokeMethod CreateInstance GetField SetField GetProperty SetProperty).</summary>
		internal static string @Arg_NoAccessSpec = "Must specify binding flags describing the invoke operation required (BindingFlags.InvokeMethod CreateInstance GetField SetField GetProperty SetProperty).";
		/// <summary>No parameterless constructor defined.</summary>
		internal static string @Arg_NoDefCTorWithoutTypeName = "No parameterless constructor defined.";
		/// <summary>No parameterless constructor defined for type '{0}'.</summary>
		internal static string @Arg_NoDefCTor = "No parameterless constructor defined for type '{0}'.";
		/// <summary>The lower bound of target array must be zero.</summary>
		internal static string @Arg_NonZeroLowerBound = "The lower bound of target array must be zero.";
		/// <summary>Method cannot be both static and virtual.</summary>
		internal static string @Arg_NoStaticVirtual = "Method cannot be both static and virtual.";
		/// <summary>Number encountered was not a finite quantity.</summary>
		internal static string @Arg_NotFiniteNumberException = "Number encountered was not a finite quantity.";
		/// <summary>Interface not found.</summary>
		internal static string @Arg_NotFoundIFace = "Interface not found.";
		/// <summary>{0} is not a GenericMethodDefinition. MakeGenericMethod may only be called on a method for which MethodBase.IsGenericMethodDefinition is true.</summary>
		internal static string @Arg_NotGenericMethodDefinition = "{0} is not a GenericMethodDefinition. MakeGenericMethod may only be called on a method for which MethodBase.IsGenericMethodDefinition is true.";
		/// <summary>Method may only be called on a Type for which Type.IsGenericParameter is true.</summary>
		internal static string @Arg_NotGenericParameter = "Method may only be called on a Type for which Type.IsGenericParameter is true.";
		/// <summary>{0} is not a GenericTypeDefinition. MakeGenericType may only be called on a type for which Type.IsGenericTypeDefinition is true.</summary>
		internal static string @Arg_NotGenericTypeDefinition = "{0} is not a GenericTypeDefinition. MakeGenericType may only be called on a type for which Type.IsGenericTypeDefinition is true.";
		/// <summary>The method or operation is not implemented.</summary>
		internal static string @Arg_NotImplementedException = "The method or operation is not implemented.";
		/// <summary>Specified method is not supported.</summary>
		internal static string @Arg_NotSupportedException = "Specified method is not supported.";
		/// <summary>Arrays indexes must be set to an object instance.</summary>
		internal static string @Arg_NullIndex = "Arrays indexes must be set to an object instance.";
		/// <summary>Object reference not set to an instance of an object.</summary>
		internal static string @Arg_NullReferenceException = "Object reference not set to an instance of an object.";
		/// <summary>Object type cannot be converted to target type.</summary>
		internal static string @Arg_ObjObj = "Object type cannot be converted to target type.";
		/// <summary>Object of type '{0}' cannot be converted to type '{1}'.</summary>
		internal static string @Arg_ObjObjEx = "Object of type '{0}' cannot be converted to type '{1}'.";
		/// <summary>Not a legal OleAut date.</summary>
		internal static string @Arg_OleAutDateInvalid = "Not a legal OleAut date.";
		/// <summary>OleAut date did not convert to a DateTime correctly.</summary>
		internal static string @Arg_OleAutDateScale = "OleAut date did not convert to a DateTime correctly.";
		/// <summary>Arithmetic operation resulted in an overflow.</summary>
		internal static string @Arg_OverflowException = "Arithmetic operation resulted in an overflow.";
		/// <summary>Insufficient memory to continue the execution of the program.</summary>
		internal static string @Arg_OutOfMemoryException = "Insufficient memory to continue the execution of the program.";
		/// <summary>(Parameter '{0}')</summary>
		internal static string @Arg_ParamName_Name = "(Parameter '{0}')";
		/// <summary>Must specify one or more parameters.</summary>
		internal static string @Arg_ParmArraySize = "Must specify one or more parameters.";
		/// <summary>Parameter count mismatch.</summary>
		internal static string @Arg_ParmCnt = "Parameter count mismatch.";
		/// <summary>The path is empty.</summary>
		internal static string @Arg_PathEmpty = "The path is empty.";
		/// <summary>Operation is not supported on this platform.</summary>
		internal static string @Arg_PlatformNotSupported = "Operation is not supported on this platform.";
		/// <summary>Cannot widen from source type to target type either because the source type is a not a primitive type or the conversion cannot be accomplished.</summary>
		internal static string @Arg_PrimWiden = "Cannot widen from source type to target type either because the source type is a not a primitive type or the conversion cannot be accomplished.";
		/// <summary>Cannot specify both Get and Set on a property.</summary>
		internal static string @Arg_PropSetGet = "Cannot specify both Get and Set on a property.";
		/// <summary>Cannot specify Set on a property and Invoke on a method.</summary>
		internal static string @Arg_PropSetInvoke = "Cannot specify Set on a property and Invoke on a method.";
		/// <summary>Attempted to operate on an array with the incorrect number of dimensions.</summary>
		internal static string @Arg_RankException = "Attempted to operate on an array with the incorrect number of dimensions.";
		/// <summary>Indices length does not match the array rank.</summary>
		internal static string @Arg_RankIndices = "Indices length does not match the array rank.";
		/// <summary>Only single dimensional arrays are supported for the requested action.</summary>
		internal static string @Arg_RankMultiDimNotSupported = "Only single dimensional arrays are supported for the requested action.";
		/// <summary>Number of lengths and lowerBounds must match.</summary>
		internal static string @Arg_RanksAndBounds = "Number of lengths and lowerBounds must match.";
		/// <summary>RegistryKey.GetValue does not allow a String that has a length greater than Int32.MaxValue.</summary>
		internal static string @Arg_RegGetOverflowBug = "RegistryKey.GetValue does not allow a String that has a length greater than Int32.MaxValue.";
		/// <summary>The specified registry key does not exist.</summary>
		internal static string @Arg_RegKeyNotFound = "The specified registry key does not exist.";
		/// <summary>No value exists with that name.</summary>
		internal static string @Arg_RegSubKeyValueAbsent = "No value exists with that name.";
		/// <summary>Registry value names should not be greater than 16,383 characters.</summary>
		internal static string @Arg_RegValStrLenBug = "Registry value names should not be greater than 16,383 characters.";
		/// <summary>Type parameter must refer to a subclass of ResourceSet.</summary>
		internal static string @Arg_ResMgrNotResSet = "Type parameter must refer to a subclass of ResourceSet.";
		/// <summary>The ResourceReader class does not know how to read this version of .resources files. Expected version: {0} This file: {1}</summary>
		internal static string @Arg_ResourceFileUnsupportedVersion = "The ResourceReader class does not know how to read this version of .resources files. Expected version: {0} This file: {1}";
		/// <summary>The specified resource name "{0}" does not exist in the resource file.</summary>
		internal static string @Arg_ResourceNameNotExist = "The specified resource name '{0}' does not exist in the resource file.";
		/// <summary>Specified array was not of the expected rank.</summary>
		internal static string @Arg_SafeArrayRankMismatchException = "Specified array was not of the expected rank.";
		/// <summary>Specified array was not of the expected type.</summary>
		internal static string @Arg_SafeArrayTypeMismatchException = "Specified array was not of the expected type.";
		/// <summary>Security error.</summary>
		internal static string @Arg_SecurityException = "Security error.";
		/// <summary>Serialization error.</summary>
		internal static string @SerializationException = "Serialization error.";
		/// <summary>Property set method not found.</summary>
		internal static string @Arg_SetMethNotFnd = "Property set method not found.";
		/// <summary>Operation caused a stack overflow.</summary>
		internal static string @Arg_StackOverflowException = "Operation caused a stack overflow.";
		/// <summary>Unicode surrogate characters must be written out as pairs together in the same call, not individually. Consider passing in a character array instead.</summary>
		internal static string @Arg_SurrogatesNotAllowedAsSingleChar = "Unicode surrogate characters must be written out as pairs together in the same call, not individually. Consider passing in a character array instead.";
		/// <summary>Object synchronization method was called from an unsynchronized block of code.</summary>
		internal static string @Arg_SynchronizationLockException = "Object synchronization method was called from an unsynchronized block of code.";
		/// <summary>System error.</summary>
		internal static string @Arg_SystemException = "System error.";
		/// <summary>Exception has been thrown by the target of an invocation.</summary>
		internal static string @Arg_TargetInvocationException = "Exception has been thrown by the target of an invocation.";
		/// <summary>Number of parameters specified does not match the expected number.</summary>
		internal static string @Arg_TargetParameterCountException = "Number of parameters specified does not match the expected number.";
		/// <summary>Thread failed to start.</summary>
		internal static string @Arg_ThreadStartException = "Thread failed to start.";
		/// <summary>Thread was in an invalid state for the operation being executed.</summary>
		internal static string @Arg_ThreadStateException = "Thread was in an invalid state for the operation being executed.";
		/// <summary>The operation has timed out.</summary>
		internal static string @Arg_TimeoutException = "The operation has timed out.";
		/// <summary>Attempt to access the type failed.</summary>
		internal static string @Arg_TypeAccessException = "Attempt to access the type failed.";
		/// <summary>The TypedReference must be initialized.</summary>
		internal static string @Arg_TypedReference_Null = "The TypedReference must be initialized.";
		/// <summary>Failure has occurred while loading a type.</summary>
		internal static string @Arg_TypeLoadException = "Failure has occurred while loading a type.";
		/// <summary>A null or zero length string does not represent a valid Type.</summary>
		internal static string @Arg_TypeLoadNullStr = "A null or zero length string does not represent a valid Type.";
		/// <summary>TypedReferences cannot be redefined as primitives. Field name '{0}'.</summary>
		internal static string @Arg_TypeRefPrimitve = "TypedReferences cannot be redefined as primitives. Field name '{0}'.";
		/// <summary>Type had been unloaded.</summary>
		internal static string @Arg_TypeUnloadedException = "Type had been unloaded.";
		/// <summary>Attempted to perform an unauthorized operation.</summary>
		internal static string @Arg_UnauthorizedAccessException = "Attempted to perform an unauthorized operation.";
		/// <summary>Late bound operations cannot be performed on fields with types for which Type.ContainsGenericParameters is true.</summary>
		internal static string @Arg_UnboundGenField = "Late bound operations cannot be performed on fields with types for which Type.ContainsGenericParameters is true.";
		/// <summary>Late bound operations cannot be performed on types or methods for which ContainsGenericParameters is true.</summary>
		internal static string @Arg_UnboundGenParam = "Late bound operations cannot be performed on types or methods for which ContainsGenericParameters is true.";
		/// <summary>Unknown TypeCode value.</summary>
		internal static string @Arg_UnknownTypeCode = "Unknown TypeCode value.";
		/// <summary>Missing parameter does not have a default value.</summary>
		internal static string @Arg_VarMissNull = "Missing parameter does not have a default value.";
		/// <summary>Version string portion was too short or too long.</summary>
		internal static string @Arg_VersionString = "Version string portion was too short or too long.";
		/// <summary>The value "{0}" is not of type "{1}" and cannot be used in this generic collection.</summary>
		internal static string @Arg_WrongType = "The value '{0}' is not of type '{1}' and cannot be used in this generic collection.";
		/// <summary>Path "{0}" is not an absolute path.</summary>
		internal static string @Argument_AbsolutePathRequired = "Path '{0}' is not an absolute path.";
		/// <summary>An item with the same key has already been added.</summary>
		internal static string @Argument_AddingDuplicate = "An item with the same key has already been added.";
		/// <summary>Item has already been added. Key in dictionary: '{0}' Key being added: '{1}'</summary>
		internal static string @Argument_AddingDuplicate__ = "Item has already been added. Key in dictionary: '{0}' Key being added: '{1}'";
		/// <summary>An item with the same key has already been added. Key: {0}</summary>
		internal static string @Argument_AddingDuplicateWithKey = "An item with the same key has already been added. Key: {0}";
		/// <summary>The AdjustmentRule array cannot contain null elements.</summary>
		internal static string @Argument_AdjustmentRulesNoNulls = "The AdjustmentRule array cannot contain null elements.";
		/// <summary>The elements of the AdjustmentRule array must be in chronological order and must not overlap.</summary>
		internal static string @Argument_AdjustmentRulesOutOfOrder = "The elements of the AdjustmentRule array must be in chronological order and must not overlap.";
		/// <summary>The alignment must be a power of two.</summary>
		internal static string @Argument_AlignmentMustBePow2 = "The alignment must be a power of two.";
		/// <summary>The object already has a CCW associated with it.</summary>
		internal static string @Argument_AlreadyACCW = "The object already has a CCW associated with it.";
		/// <summary>'handle' has already been bound to the thread pool, or was not opened for asynchronous I/O.</summary>
		internal static string @Argument_AlreadyBoundOrSyncHandle = "'handle' has already been bound to the thread pool, or was not opened for asynchronous I/O.";
		/// <summary>Interface maps for generic interfaces on arrays cannot be retrieved.</summary>
		internal static string @Argument_ArrayGetInterfaceMap = "Interface maps for generic interfaces on arrays cannot be retrieved.";
		/// <summary>Array or pointer types are not valid.</summary>
		internal static string @Argument_ArraysInvalid = "Array or pointer types are not valid.";
		/// <summary>Attribute names must be unique.</summary>
		internal static string @Argument_AttributeNamesMustBeUnique = "Attribute names must be unique.";
		/// <summary>Bad default value.</summary>
		internal static string @Argument_BadConstantValue = "Bad default value.";
		/// <summary>Cannot have private or static constructor.</summary>
		internal static string @Argument_BadConstructor = "Cannot have private or static constructor.";
		/// <summary>Constructor must have standard calling convention.</summary>
		internal static string @Argument_BadConstructorCallConv = "Constructor must have standard calling convention.";
		/// <summary>Incorrect code generation for exception block.</summary>
		internal static string @Argument_BadExceptionCodeGen = "Incorrect code generation for exception block.";
		/// <summary>Field must be on the same type of the given ConstructorInfo.</summary>
		internal static string @Argument_BadFieldForConstructorBuilder = "Field must be on the same type of the given ConstructorInfo.";
		/// <summary>Field signatures do not have return types.</summary>
		internal static string @Argument_BadFieldSig = "Field signatures do not have return types.";
		/// <summary>Bad field type in defining field.</summary>
		internal static string @Argument_BadFieldType = "Bad field type in defining field.";
		/// <summary>Format specifier was invalid.</summary>
		internal static string @Argument_BadFormatSpecifier = "Format specifier was invalid.";
		/// <summary>A BadImageFormatException has been thrown while parsing the signature. This is likely due to lack of a generic context. Ensure genericTypeArguments and genericMethodArguments are provided and contain enough context.</summary>
		internal static string @Argument_BadImageFormatExceptionResolve = "A BadImageFormatException has been thrown while parsing the signature. This is likely due to lack of a generic context. Ensure genericTypeArguments and genericMethodArguments are provided and contain enough context.";
		/// <summary>Bad label in ILGenerator.</summary>
		internal static string @Argument_BadLabel = "Bad label in ILGenerator.";
		/// <summary>Bad label content in ILGenerator.</summary>
		internal static string @Argument_BadLabelContent = "Bad label content in ILGenerator.";
		/// <summary>Visibility of interfaces must be one of the following: NestedAssembly, NestedFamANDAssem, NestedFamily, NestedFamORAssem, NestedPrivate or NestedPublic.</summary>
		internal static string @Argument_BadNestedTypeFlags = "Visibility of interfaces must be one of the following: NestedAssembly, NestedFamANDAssem, NestedFamily, NestedFamORAssem, NestedPrivate or NestedPublic.";
		/// <summary>Invalid ObjRef provided to '{0}'.</summary>
		internal static string @Argument_BadObjRef = "Invalid ObjRef provided to '{0}'.";
		/// <summary>Parameter count does not match passed in argument value count.</summary>
		internal static string @Argument_BadParameterCountsForConstructor = "Parameter count does not match passed in argument value count.";
		/// <summary>Cannot emit a CustomAttribute with argument of type {0}.</summary>
		internal static string @Argument_BadParameterTypeForCAB = "Cannot emit a CustomAttribute with argument of type {0}.";
		/// <summary>Property must be on the same type of the given ConstructorInfo.</summary>
		internal static string @Argument_BadPropertyForConstructorBuilder = "Property must be on the same type of the given ConstructorInfo.";
		/// <summary>Incorrect signature format.</summary>
		internal static string @Argument_BadSigFormat = "Incorrect signature format.";
		/// <summary>Data size must be > 1 and < 0x3f0000</summary>
		internal static string @Argument_BadSizeForData = "Data size must be > 1 and < 0x3f0000";
		/// <summary>Bad type attributes. Invalid layout attribute specified.</summary>
		internal static string @Argument_BadTypeAttrInvalidLayout = "Bad type attributes. Invalid layout attribute specified.";
		/// <summary>Bad type attributes. Nested visibility flag set on a non-nested type.</summary>
		internal static string @Argument_BadTypeAttrNestedVisibilityOnNonNestedType = "Bad type attributes. Nested visibility flag set on a non-nested type.";
		/// <summary>Bad type attributes. Non-nested visibility flag set on a nested type.</summary>
		internal static string @Argument_BadTypeAttrNonNestedVisibilityNestedType = "Bad type attributes. Non-nested visibility flag set on a nested type.";
		/// <summary>Bad type attributes. Reserved bits set on the type.</summary>
		internal static string @Argument_BadTypeAttrReservedBitsSet = "Bad type attributes. Reserved bits set on the type.";
		/// <summary>An invalid type was used as a custom attribute constructor argument, field or property.</summary>
		internal static string @Argument_BadTypeInCustomAttribute = "An invalid type was used as a custom attribute constructor argument, field or property.";
		/// <summary>Cannot use function evaluation to create a TypedReference object.</summary>
		internal static string @Argument_CannotCreateTypedReference = "Cannot use function evaluation to create a TypedReference object.";
		/// <summary>Cannot set parent to an interface.</summary>
		internal static string @Argument_CannotSetParentToInterface = "Cannot set parent to an interface.";
		/// <summary>{0} is not a supported code page.</summary>
		internal static string @Argument_CodepageNotSupported = "{0} is not a supported code page.";
		/// <summary>CompareOption.Ordinal cannot be used with other options.</summary>
		internal static string @Argument_CompareOptionOrdinal = "CompareOption.Ordinal cannot be used with other options.";
		/// <summary>The DateTimeStyles value RoundtripKind cannot be used with the values AssumeLocal, AssumeUniversal or AdjustToUniversal.</summary>
		internal static string @Argument_ConflictingDateTimeRoundtripStyles = "The DateTimeStyles value RoundtripKind cannot be used with the values AssumeLocal, AssumeUniversal or AdjustToUniversal.";
		/// <summary>The DateTimeStyles values AssumeLocal and AssumeUniversal cannot be used together.</summary>
		internal static string @Argument_ConflictingDateTimeStyles = "The DateTimeStyles values AssumeLocal and AssumeUniversal cannot be used together.";
		/// <summary>Constant does not match the defined type.</summary>
		internal static string @Argument_ConstantDoesntMatch = "Constant does not match the defined type.";
		/// <summary>{0} is not a supported constant type.</summary>
		internal static string @Argument_ConstantNotSupported = "{0} is not a supported constant type.";
		/// <summary>Null is not a valid constant value for this type.</summary>
		internal static string @Argument_ConstantNull = "Null is not a valid constant value for this type.";
		/// <summary>The specified constructor must be declared on a generic type definition.</summary>
		internal static string @Argument_ConstructorNeedGenericDeclaringType = "The specified constructor must be declared on a generic type definition.";
		/// <summary>Conversion buffer overflow.</summary>
		internal static string @Argument_ConversionOverflow = "Conversion buffer overflow.";
		/// <summary>The conversion could not be completed because the supplied DateTime did not have the Kind property set correctly. For example, when the Kind property is DateTimeKind.Local, the source time zone must be TimeZoneInfo.Local.</summary>
		internal static string @Argument_ConvertMismatch = "The conversion could not be completed because the supplied DateTime did not have the Kind property set correctly. For example, when the Kind property is DateTimeKind.Local, the source time zone must be TimeZoneInfo.Local.";
		/// <summary>Cannot find the method on the object instance.</summary>
		internal static string @Argument_CORDBBadMethod = "Cannot find the method on the object instance.";
		/// <summary>Cannot evaluate a VarArgs function.</summary>
		internal static string @Argument_CORDBBadVarArgCallConv = "Cannot evaluate a VarArgs function.";
		/// <summary>Culture IETF Name {0} is not a recognized IETF name.</summary>
		internal static string @Argument_CultureIetfNotSupported = "Culture IETF Name {0} is not a recognized IETF name.";
		/// <summary>{0} is an invalid culture identifier.</summary>
		internal static string @Argument_CultureInvalidIdentifier = "{0} is an invalid culture identifier.";
		/// <summary>Culture ID {0} (0x{0:X4}) is a neutral culture; a region cannot be created from it.</summary>
		internal static string @Argument_CultureIsNeutral = "Culture ID {0} (0x{0:X4}) is a neutral culture; a region cannot be created from it.";
		/// <summary>Culture is not supported.</summary>
		internal static string @Argument_CultureNotSupported = "Culture is not supported.";
		/// <summary>Only the invariant culture is supported in globalization-invariant mode. See https://aka.ms/GlobalizationInvariantMode for more information.</summary>
		internal static string @Argument_CultureNotSupportedInInvariantMode = "Only the invariant culture is supported in globalization-invariant mode. See https://aka.ms/GlobalizationInvariantMode for more information.";
		/// <summary>Resolved assembly's simple name should be the same as of the requested assembly.</summary>
		internal static string @Argument_CustomAssemblyLoadContextRequestedNameMismatch = "Resolved assembly's simple name should be the same as of the requested assembly.";
		/// <summary>Customized cultures cannot be passed by LCID, only by name.</summary>
		internal static string @Argument_CustomCultureCannotBePassedByNumber = "Customized cultures cannot be passed by LCID, only by name.";
		/// <summary>The binary data must result in a DateTime with ticks between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.</summary>
		internal static string @Argument_DateTimeBadBinaryData = "The binary data must result in a DateTime with ticks between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.";
		/// <summary>The supplied DateTime must have the Year, Month, and Day properties set to 1. The time cannot be specified more precisely than whole milliseconds.</summary>
		internal static string @Argument_DateTimeHasTicks = "The supplied DateTime must have the Year, Month, and Day properties set to 1. The time cannot be specified more precisely than whole milliseconds.";
		/// <summary>The supplied DateTime includes a TimeOfDay setting. This is not supported.</summary>
		internal static string @Argument_DateTimeHasTimeOfDay = "The supplied DateTime includes a TimeOfDay setting. This is not supported.";
		/// <summary>The supplied DateTime represents an invalid time. For example, when the clock is adjusted forward, any time in the period that is skipped is invalid.</summary>
		internal static string @Argument_DateTimeIsInvalid = "The supplied DateTime represents an invalid time. For example, when the clock is adjusted forward, any time in the period that is skipped is invalid.";
		/// <summary>The supplied DateTime is not in an ambiguous time range.</summary>
		internal static string @Argument_DateTimeIsNotAmbiguous = "The supplied DateTime is not in an ambiguous time range.";
		/// <summary>The supplied DateTime must have the Kind property set to DateTimeKind.Unspecified.</summary>
		internal static string @Argument_DateTimeKindMustBeUnspecified = "The supplied DateTime must have the Kind property set to DateTimeKind.Unspecified.";
		/// <summary>The supplied DateTime must have the Kind property set to DateTimeKind.Unspecified or DateTimeKind.Utc.</summary>
		internal static string @Argument_DateTimeKindMustBeUnspecifiedOrUtc = "The supplied DateTime must have the Kind property set to DateTimeKind.Unspecified or DateTimeKind.Utc.";
		/// <summary>The DateTimeStyles value 'NoCurrentDateDefault' is not allowed when parsing DateTimeOffset.</summary>
		internal static string @Argument_DateTimeOffsetInvalidDateTimeStyles = "The DateTimeStyles value 'NoCurrentDateDefault' is not allowed when parsing DateTimeOffset.";
		/// <summary>The supplied DateTimeOffset is not in an ambiguous time range.</summary>
		internal static string @Argument_DateTimeOffsetIsNotAmbiguous = "The supplied DateTimeOffset is not in an ambiguous time range.";
		/// <summary>Destination is too short.</summary>
		internal static string @Argument_DestinationTooShort = "Destination is too short.";
		/// <summary>Duplicate type name within an assembly.</summary>
		internal static string @Argument_DuplicateTypeName = "Duplicate type name within an assembly.";
		/// <summary>EmitWriteLine does not support this field or local type.</summary>
		internal static string @Argument_EmitWriteLineType = "EmitWriteLine does not support this field or local type.";
		/// <summary>Decimal separator cannot be the empty string.</summary>
		internal static string @Argument_EmptyDecString = "Decimal separator cannot be the empty string.";
		/// <summary>Empty file name is not legal.</summary>
		internal static string @Argument_EmptyFileName = "Empty file name is not legal.";
		/// <summary>Empty name is not legal.</summary>
		internal static string @Argument_EmptyName = "Empty name is not legal.";
		/// <summary>Empty path name is not legal.</summary>
		internal static string @Argument_EmptyPath = "Empty path name is not legal.";
		/// <summary>Waithandle array may not be empty.</summary>
		internal static string @Argument_EmptyWaithandleArray = "Waithandle array may not be empty.";
		/// <summary>Must complete Convert() operation or call Encoder.Reset() before calling GetBytes() or GetByteCount(). Encoder '{0}' fallback '{1}'.</summary>
		internal static string @Argument_EncoderFallbackNotEmpty = "Must complete Convert() operation or call Encoder.Reset() before calling GetBytes() or GetByteCount(). Encoder '{0}' fallback '{1}'.";
		/// <summary>The output byte buffer is too small to contain the encoded data, encoding codepage '{0}' and fallback '{1}'.</summary>
		internal static string @Argument_EncodingConversionOverflowBytes = "The output byte buffer is too small to contain the encoded data, encoding codepage '{0}' and fallback '{1}'.";
		/// <summary>The output char buffer is too small to contain the decoded characters, encoding codepage '{0}' and fallback '{1}'.</summary>
		internal static string @Argument_EncodingConversionOverflowChars = "The output char buffer is too small to contain the decoded characters, encoding codepage '{0}' and fallback '{1}'.";
		/// <summary>'{0}' is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.</summary>
		internal static string @Argument_EncodingNotSupported = "'{0}' is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.";
		/// <summary>The argument type, '{0}', is not the same as the enum type '{1}'.</summary>
		internal static string @Argument_EnumTypeDoesNotMatch = "The argument type, '{0}', is not the same as the enum type '{1}'.";
		/// <summary>Cannot change fallback when buffer is not empty. Previous Convert() call left data in the fallback buffer.</summary>
		internal static string @Argument_FallbackBufferNotEmpty = "Cannot change fallback when buffer is not empty. Previous Convert() call left data in the fallback buffer.";
		/// <summary>Cannot resolve field {0} because the declaring type of the field handle {1} is generic. Explicitly provide the declaring type to GetFieldFromHandle.</summary>
		internal static string @Argument_FieldDeclaringTypeGeneric = "Cannot resolve field {0} because the declaring type of the field handle {1} is generic. Explicitly provide the declaring type to GetFieldFromHandle.";
		/// <summary>The specified field must be declared on a generic type definition.</summary>
		internal static string @Argument_FieldNeedGenericDeclaringType = "The specified field must be declared on a generic type definition.";
		/// <summary>GenericArguments[{0}], '{1}', on '{2}' violates the constraint of type '{3}'.</summary>
		internal static string @Argument_GenConstraintViolation = "GenericArguments[{0}], '{1}', on '{2}' violates the constraint of type '{3}'.";
		/// <summary>The number of generic arguments provided doesn't equal the arity of the generic type definition.</summary>
		internal static string @Argument_GenericArgsCount = "The number of generic arguments provided doesn't equal the arity of the generic type definition.";
		/// <summary>Generic types are not valid.</summary>
		internal static string @Argument_GenericsInvalid = "Generic types are not valid.";
		/// <summary>Global members must be static.</summary>
		internal static string @Argument_GlobalFunctionHasToBeStatic = "Global members must be static.";
		/// <summary>Must be an array type.</summary>
		internal static string @Argument_HasToBeArrayClass = "Must be an array type.";
		/// <summary>Left to right characters may not be mixed with right to left characters in IDN labels.</summary>
		internal static string @Argument_IdnBadBidi = "Left to right characters may not be mixed with right to left characters in IDN labels.";
		/// <summary>IDN labels must be between 1 and 63 characters long.</summary>
		internal static string @Argument_IdnBadLabelSize = "IDN labels must be between 1 and 63 characters long.";
		/// <summary>IDN names must be between 1 and {0} characters long.</summary>
		internal static string @Argument_IdnBadNameSize = "IDN names must be between 1 and {0} characters long.";
		/// <summary>Invalid IDN encoded string.</summary>
		internal static string @Argument_IdnBadPunycode = "Invalid IDN encoded string.";
		/// <summary>Label contains character '{0}' not allowed with UseStd3AsciiRules</summary>
		internal static string @Argument_IdnBadStd3 = "Label contains character '{0}' not allowed with UseStd3AsciiRules";
		/// <summary>Decoded string is not a valid IDN name.</summary>
		internal static string @Argument_IdnIllegalName = "Decoded string is not a valid IDN name.";
		/// <summary>Environment variable name cannot contain equal character.</summary>
		internal static string @Argument_IllegalEnvVarName = "Environment variable name cannot contain equal character.";
		/// <summary>Illegal name.</summary>
		internal static string @Argument_IllegalName = "Illegal name.";
		/// <summary>At least one object must implement IComparable.</summary>
		internal static string @Argument_ImplementIComparable = "At least one object must implement IComparable.";
		/// <summary>The specified index is out of bounds of the specified array.</summary>
		internal static string @Argument_IndexOutOfArrayBounds = "The specified index is out of bounds of the specified array.";
		/// <summary>'this' type cannot be an interface itself.</summary>
		internal static string @Argument_InterfaceMap = "'this' type cannot be an interface itself.";
		/// <summary>Append access can be requested only in write-only mode.</summary>
		internal static string @Argument_InvalidAppendMode = "Append access can be requested only in write-only mode.";
		/// <summary>Preallocation size can be requested only in write mode.</summary>
		internal static string @Argument_InvalidPreallocateAccess = "Preallocation size can be requested only in write mode.";
		/// <summary>Preallocation size can be requested only for new files.</summary>
		internal static string @Argument_InvalidPreallocateMode = "Preallocation size can be requested only for new files.";
		/// <summary>Type of argument is not compatible with the generic comparer.</summary>
		internal static string @Argument_InvalidArgumentForComparison = "Type of argument is not compatible with the generic comparer.";
		/// <summary>Length of the array must be {0}.</summary>
		internal static string @Argument_InvalidArrayLength = "Length of the array must be {0}.";
		/// <summary>Target array type is not compatible with the type of items in the collection.</summary>
		internal static string @Argument_InvalidArrayType = "Target array type is not compatible with the type of items in the collection.";
		/// <summary>Assembly names may not begin with whitespace or contain the characters '/', or '\\' or ':'.</summary>
		internal static string @Argument_InvalidAssemblyName = "Assembly names may not begin with whitespace or contain the characters '/', or '\\' or ':'.";
		/// <summary>Not a valid calendar for the given culture.</summary>
		internal static string @Argument_InvalidCalendar = "Not a valid calendar for the given culture.";
		/// <summary>Invalid Unicode code point found at index {0}.</summary>
		internal static string @Argument_InvalidCharSequence = "Invalid Unicode code point found at index {0}.";
		/// <summary>String contains invalid Unicode code points.</summary>
		internal static string @Argument_InvalidCharSequenceNoIndex = "String contains invalid Unicode code points.";
		/// <summary>Unable to translate bytes {0} at index {1} from specified code page to Unicode.</summary>
		internal static string @Argument_InvalidCodePageBytesIndex = "Unable to translate bytes {0} at index {1} from specified code page to Unicode.";
		/// <summary>Unable to translate Unicode character \\u{0:X4} at index {1} to specified code page.</summary>
		internal static string @Argument_InvalidCodePageConversionIndex = "Unable to translate Unicode character \\u{0:X4} at index {1} to specified code page.";
		/// <summary>The specified constructor must be declared on the generic type definition of the specified type.</summary>
		internal static string @Argument_InvalidConstructorDeclaringType = "The specified constructor must be declared on the generic type definition of the specified type.";
		/// <summary>The ConstructorInfo object is not valid.</summary>
		internal static string @Argument_InvalidConstructorInfo = "The ConstructorInfo object is not valid.";
		/// <summary>Culture name '{0}' is not supported.</summary>
		internal static string @Argument_InvalidCultureName = "Culture name '{0}' is not supported.";
		/// <summary>Culture name '{0}' is not a predefined culture.</summary>
		internal static string @Argument_InvalidPredefinedCultureName = "Culture name '{0}' is not a predefined culture.";
		/// <summary>Invalid DateTimeKind value.</summary>
		internal static string @Argument_InvalidDateTimeKind = "Invalid DateTimeKind value.";
		/// <summary>An undefined DateTimeStyles value is being used.</summary>
		internal static string @Argument_InvalidDateTimeStyles = "An undefined DateTimeStyles value is being used.";
		/// <summary>The only allowed values for the styles are AllowWhiteSpaces, AllowTrailingWhite, AllowLeadingWhite, and AllowInnerWhite. {Locked="AllowWhiteSpaces, AllowTrailingWhite, AllowLeadingWhite, and AllowInnerWhite"}</summary>
		internal static string @Argument_InvalidDateStyles = "The only allowed values for the styles are AllowWhiteSpaces, AllowTrailingWhite, AllowLeadingWhite, and AllowInnerWhite. {Locked='AllowWhiteSpaces, AllowTrailingWhite, AllowLeadingWhite, and AllowInnerWhite'}";
		/// <summary>The DigitSubstitution property must be of a valid member of the DigitShapes enumeration. Valid entries include Context, NativeNational or None.</summary>
		internal static string @Argument_InvalidDigitSubstitution = "The DigitSubstitution property must be of a valid member of the DigitShapes enumeration. Valid entries include Context, NativeNational or None.";
		/// <summary>Invalid element name '{0}'.</summary>
		internal static string @Argument_InvalidElementName = "Invalid element name '{0}'.";
		/// <summary>Invalid element tag '{0}'.</summary>
		internal static string @Argument_InvalidElementTag = "Invalid element tag '{0}'.";
		/// <summary>Invalid element text '{0}'.</summary>
		internal static string @Argument_InvalidElementText = "Invalid element text '{0}'.";
		/// <summary>Invalid element value '{0}'.</summary>
		internal static string @Argument_InvalidElementValue = "Invalid element value '{0}'.";
		/// <summary>The Enum type should contain one and only one instance field.</summary>
		internal static string @Argument_InvalidEnum = "The Enum type should contain one and only one instance field.";
		/// <summary>The value '{0}' is not valid for this usage of the type {1}.</summary>
		internal static string @Argument_InvalidEnumValue = "The value '{0}' is not valid for this usage of the type {1}.";
		/// <summary>The specified field must be declared on the generic type definition of the specified type.</summary>
		internal static string @Argument_InvalidFieldDeclaringType = "The specified field must be declared on the generic type definition of the specified type.";
		/// <summary>Combining FileMode: {0} with FileAccess: {1} is invalid.</summary>
		internal static string @Argument_InvalidFileModeAndAccessCombo = "Combining FileMode: {0} with FileAccess: {1} is invalid.";
		/// <summary>Value of flags is invalid.</summary>
		internal static string @Argument_InvalidFlag = "Value of flags is invalid.";
		/// <summary>The generic type parameter was not valid</summary>
		internal static string @Argument_InvalidGenericArg = "The generic type parameter was not valid";
		/// <summary>Generic arguments must be provided for each generic parameter and each generic argument must be a RuntimeType.</summary>
		internal static string @Argument_InvalidGenericInstArray = "Generic arguments must be provided for each generic parameter and each generic argument must be a RuntimeType.";
		/// <summary>Every element in the value array should be between one and nine, except for the last element, which can be zero.</summary>
		internal static string @Argument_InvalidGroupSize = "Every element in the value array should be between one and nine, except for the last element, which can be zero.";
		/// <summary>The handle is invalid.</summary>
		internal static string @Argument_InvalidHandle = "The handle is invalid.";
		/// <summary>Found a high surrogate char without a following low surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.</summary>
		internal static string @Argument_InvalidHighSurrogate = "Found a high surrogate char without a following low surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.";
		/// <summary>The specified ID parameter '{0}' is not supported.</summary>
		internal static string @Argument_InvalidId = "The specified ID parameter '{0}' is not supported.";
		/// <summary>This type cannot be represented as a custom attribute.</summary>
		internal static string @Argument_InvalidKindOfTypeForCA = "This type cannot be represented as a custom attribute.";
		/// <summary>Invalid Label.</summary>
		internal static string @Argument_InvalidLabel = "Invalid Label.";
		/// <summary>Found a low surrogate char without a preceding high surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.</summary>
		internal static string @Argument_InvalidLowSurrogate = "Found a low surrogate char without a preceding high surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.";
		/// <summary>The member must be either a field or a property.</summary>
		internal static string @Argument_InvalidMemberForNamedArgument = "The member must be either a field or a property.";
		/// <summary>The specified method must be declared on the generic type definition of the specified type.</summary>
		internal static string @Argument_InvalidMethodDeclaringType = "The specified method must be declared on the generic type definition of the specified type.";
		/// <summary>Invalid name.</summary>
		internal static string @Argument_InvalidName = "Invalid name.";
		/// <summary>The NativeDigits array must contain exactly ten members.</summary>
		internal static string @Argument_InvalidNativeDigitCount = "The NativeDigits array must contain exactly ten members.";
		/// <summary>Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit.</summary>
		internal static string @Argument_InvalidNativeDigitValue = "Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit.";
		/// <summary>The region name {0} should not correspond to neutral culture; a specific culture name is required.</summary>
		internal static string @Argument_InvalidNeutralRegionName = "The region name {0} should not correspond to neutral culture; a specific culture name is required.";
		/// <summary>Invalid or unsupported normalization form.</summary>
		internal static string @Argument_InvalidNormalizationForm = "Invalid or unsupported normalization form.";
		/// <summary>An undefined NumberStyles value is being used.</summary>
		internal static string @Argument_InvalidNumberStyles = "An undefined NumberStyles value is being used.";
		/// <summary>Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.</summary>
		internal static string @Argument_InvalidOffLen = "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.";
		/// <summary>Ldtoken, Ldftn and Ldvirtftn OpCodes cannot target DynamicMethods.</summary>
		internal static string @Argument_InvalidOpCodeOnDynamicMethod = "Ldtoken, Ldftn and Ldvirtftn OpCodes cannot target DynamicMethods.";
		/// <summary>The ParameterInfo object is not valid.</summary>
		internal static string @Argument_InvalidParameterInfo = "The ParameterInfo object is not valid.";
		/// <summary>Invalid type for ParameterInfo member in Attribute class.</summary>
		internal static string @Argument_InvalidParamInfo = "Invalid type for ParameterInfo member in Attribute class.";
		/// <summary>Illegal characters in path.</summary>
		internal static string @Argument_InvalidPathChars = "Illegal characters in path.";
		/// <summary>The given culture name '{0}' cannot be used to locate a resource file. Resource filenames must consist of only letters, numbers, hyphens or underscores.</summary>
		internal static string @Argument_InvalidResourceCultureName = "The given culture name '{0}' cannot be used to locate a resource file. Resource filenames must consist of only letters, numbers, hyphens or underscores.";
		/// <summary>Offset and length were greater than the size of the SafeBuffer.</summary>
		internal static string @Argument_InvalidSafeBufferOffLen = "Offset and length were greater than the size of the SafeBuffer.";
		/// <summary>Invalid seek origin.</summary>
		internal static string @Argument_InvalidSeekOrigin = "Invalid seek origin.";
		/// <summary>The specified serialized string '{0}' is not supported.</summary>
		internal static string @Argument_InvalidSerializedString = "The specified serialized string '{0}' is not supported.";
		/// <summary>The signature of the startup hook '{0}' in assembly '{1}' was invalid. It must be 'public static void Initialize()'.</summary>
		internal static string @Argument_InvalidStartupHookSignature = "The signature of the startup hook '{0}' in assembly '{1}' was invalid. It must be 'public static void Initialize()'.";
		/// <summary>An undefined TimeSpanStyles value is being used.</summary>
		internal static string @Argument_InvalidTimeSpanStyles = "An undefined TimeSpanStyles value is being used.";
		/// <summary>Token {0:x} is not valid in the scope of module {1}.</summary>
		internal static string @Argument_InvalidToken = "Token {0:x} is not valid in the scope of module {1}.";
		/// <summary>Cannot build type parameter for custom attribute with a type that does not support the AssemblyQualifiedName property. The type instance supplied was of type '{0}'.</summary>
		internal static string @Argument_InvalidTypeForCA = "Cannot build type parameter for custom attribute with a type that does not support the AssemblyQualifiedName property. The type instance supplied was of type '{0}'.";
		/// <summary>Invalid type owner for DynamicMethod.</summary>
		internal static string @Argument_InvalidTypeForDynamicMethod = "Invalid type owner for DynamicMethod.";
		/// <summary>The name of the type is invalid.</summary>
		internal static string @Argument_InvalidTypeName = "The name of the type is invalid.";
		/// <summary>Cannot use type '{0}'. Only value types without pointers or references are supported.</summary>
		internal static string @Argument_InvalidTypeWithPointersNotSupported = "Cannot use type '{0}'. Only value types without pointers or references are supported.";
		/// <summary>Type '{0}' is not deserializable.</summary>
		internal static string @Argument_InvalidUnity = "Type '{0}' is not deserializable.";
		/// <summary>Value was invalid.</summary>
		internal static string @Argument_InvalidValue = "Value was invalid.";
		/// <summary>Integer or token was too large to be encoded.</summary>
		internal static string @Argument_LargeInteger = "Integer or token was too large to be encoded.";
		/// <summary>Environment variable name or value is too long.</summary>
		internal static string @Argument_LongEnvVarValue = "Environment variable name or value is too long.";
		/// <summary>Cannot resolve method {0} because the declaring type of the method handle {1} is generic. Explicitly provide the declaring type to GetMethodFromHandle.</summary>
		internal static string @Argument_MethodDeclaringTypeGeneric = "Cannot resolve method {0} because the declaring type of the method handle {1} is generic. Explicitly provide the declaring type to GetMethodFromHandle.";
		/// <summary>Method '{0}' has a generic declaring type '{1}'. Explicitly provide the declaring type to GetTokenFor.</summary>
		internal static string @Argument_MethodDeclaringTypeGenericLcg = "Method '{0}' has a generic declaring type '{1}'. Explicitly provide the declaring type to GetTokenFor.";
		/// <summary>The specified method cannot be dynamic or global and must be declared on a generic type definition.</summary>
		internal static string @Argument_MethodNeedGenericDeclaringType = "The specified method cannot be dynamic or global and must be declared on a generic type definition.";
		/// <summary>'{0}' cannot be greater than {1}.</summary>
		internal static string @Argument_MinMaxValue = "'{0}' cannot be greater than {1}.";
		/// <summary>Two arrays, {0} and {1}, must be of the same size.</summary>
		internal static string @Argument_MismatchedArrays = "Two arrays, {0} and {1}, must be of the same size.";
		/// <summary>was missing default constructor.</summary>
		internal static string @Argument_MissingDefaultConstructor = "was missing default constructor.";
		/// <summary>Argument must be initialized to false</summary>
		internal static string @Argument_MustBeFalse = "Argument must be initialized to false";
		/// <summary>Assembly must be a runtime Assembly object.</summary>
		internal static string @Argument_MustBeRuntimeAssembly = "Assembly must be a runtime Assembly object.";
		/// <summary>FieldInfo must be a runtime FieldInfo object.</summary>
		internal static string @Argument_MustBeRuntimeFieldInfo = "FieldInfo must be a runtime FieldInfo object.";
		/// <summary>MethodInfo must be a runtime MethodInfo object.</summary>
		internal static string @Argument_MustBeRuntimeMethodInfo = "MethodInfo must be a runtime MethodInfo object.";
		/// <summary>The object must be a runtime Reflection object.</summary>
		internal static string @Argument_MustBeRuntimeReflectionObject = "The object must be a runtime Reflection object.";
		/// <summary>Type must be a runtime Type object.</summary>
		internal static string @Argument_MustBeRuntimeType = "Type must be a runtime Type object.";
		/// <summary>'type' must contain a TypeBuilder as a generic argument.</summary>
		internal static string @Argument_MustBeTypeBuilder = "'type' must contain a TypeBuilder as a generic argument.";
		/// <summary>Type passed in must be derived from System.Attribute or System.Attribute itself.</summary>
		internal static string @Argument_MustHaveAttributeBaseClass = "Type passed in must be derived from System.Attribute or System.Attribute itself.";
		/// <summary>The specified structure must be blittable or have layout information.</summary>
		internal static string @Argument_MustHaveLayoutOrBeBlittable = "The specified structure must be blittable or have layout information.";
		/// <summary>'overlapped' has already been freed.</summary>
		internal static string @Argument_NativeOverlappedAlreadyFree = "'overlapped' has already been freed.";
		/// <summary>'overlapped' was not allocated by this ThreadPoolBoundHandle instance.</summary>
		internal static string @Argument_NativeOverlappedWrongBoundHandle = "'overlapped' was not allocated by this ThreadPoolBoundHandle instance.";
		/// <summary>Method must represent a generic method definition on a generic type definition.</summary>
		internal static string @Argument_NeedGenericMethodDefinition = "Method must represent a generic method definition on a generic type definition.";
		/// <summary>The specified object must not be an instance of a generic type.</summary>
		internal static string @Argument_NeedNonGenericObject = "The specified object must not be an instance of a generic type.";
		/// <summary>The specified Type must not be a generic type.</summary>
		internal static string @Argument_NeedNonGenericType = "The specified Type must not be a generic type.";
		/// <summary>The specified Type must be a struct containing no references.</summary>
		internal static string @Argument_NeedStructWithNoRefs = "The specified Type must be a struct containing no references.";
		/// <summary>The type '{0}' may not be used as a type argument.</summary>
		internal static string @Argument_NeverValidGenericArgument = "The type '{0}' may not be used as a type argument.";
		/// <summary>No Era was supplied.</summary>
		internal static string @Argument_NoEra = "No Era was supplied.";
		/// <summary>There is no region associated with the Invariant Culture (Culture ID: 0x7F).</summary>
		internal static string @Argument_NoRegionInvariantCulture = "There is no region associated with the Invariant Culture (Culture ID: 0x7F).";
		/// <summary>Not a writable property.</summary>
		internal static string @Argument_NotAWritableProperty = "Not a writable property.";
		/// <summary>There are not enough bytes remaining in the accessor to read at this position.</summary>
		internal static string @Argument_NotEnoughBytesToRead = "There are not enough bytes remaining in the accessor to read at this position.";
		/// <summary>There are not enough bytes remaining in the accessor to write at this position.</summary>
		internal static string @Argument_NotEnoughBytesToWrite = "There are not enough bytes remaining in the accessor to write at this position.";
		/// <summary>The type or method has {1} generic parameter(s), but {0} generic argument(s) were provided. A generic argument must be provided for each generic parameter.</summary>
		internal static string @Argument_NotEnoughGenArguments = "The type or method has {1} generic parameter(s), but {0} generic argument(s) were provided. A generic argument must be provided for each generic parameter.";
		/// <summary>Does not extend Exception.</summary>
		internal static string @Argument_NotExceptionType = "Does not extend Exception.";
		/// <summary>Not currently in an exception block.</summary>
		internal static string @Argument_NotInExceptionBlock = "Not currently in an exception block.";
		/// <summary>The specified opcode cannot be passed to EmitCall.</summary>
		internal static string @Argument_NotMethodCallOpcode = "The specified opcode cannot be passed to EmitCall.";
		/// <summary>Argument passed in is not serializable.</summary>
		internal static string @Argument_NotSerializable = "Argument passed in is not serializable.";
		/// <summary>Uninitialized Strings cannot be created.</summary>
		internal static string @Argument_NoUninitializedStrings = "Uninitialized Strings cannot be created.";
		/// <summary>The object's type must be __ComObject or derived from __ComObject.</summary>
		internal static string @Argument_ObjNotComObject = "The object's type must be __ComObject or derived from __ComObject.";
		/// <summary>Offset and capacity were greater than the size of the view.</summary>
		internal static string @Argument_OffsetAndCapacityOutOfBounds = "Offset and capacity were greater than the size of the view.";
		/// <summary>The UTC Offset of the local dateTime parameter does not match the offset argument.</summary>
		internal static string @Argument_OffsetLocalMismatch = "The UTC Offset of the local dateTime parameter does not match the offset argument.";
		/// <summary>Field passed in is not a marshaled member of the type '{0}'.</summary>
		internal static string @Argument_OffsetOfFieldNotFound = "Field passed in is not a marshaled member of the type '{0}'.";
		/// <summary>Offset must be within plus or minus 14 hours.</summary>
		internal static string @Argument_OffsetOutOfRange = "Offset must be within plus or minus 14 hours.";
		/// <summary>Offset must be specified in whole minutes.</summary>
		internal static string @Argument_OffsetPrecision = "Offset must be specified in whole minutes.";
		/// <summary>The UTC Offset for Utc DateTime instances must be 0.</summary>
		internal static string @Argument_OffsetUtcMismatch = "The UTC Offset for Utc DateTime instances must be 0.";
		/// <summary>Culture name {0} or {1} is not supported.</summary>
		internal static string @Argument_OneOfCulturesNotSupported = "Culture name {0} or {1} is not supported.";
		/// <summary>Only mscorlib's assembly is valid.</summary>
		internal static string @Argument_OnlyMscorlib = "Only mscorlib's assembly is valid.";
		/// <summary>The DateStart property must come before the DateEnd property.</summary>
		internal static string @Argument_OutOfOrderDateTimes = "The DateStart property must come before the DateEnd property.";
		/// <summary>Path cannot be the empty string or all whitespace.</summary>
		internal static string @Argument_PathEmpty = "Path cannot be the empty string or all whitespace.";
		/// <summary>'preAllocated' is already in use.</summary>
		internal static string @Argument_PreAllocatedAlreadyAllocated = "'preAllocated' is already in use.";
		/// <summary>Recursive fallback not allowed for character \\u{0:X4}.</summary>
		internal static string @Argument_RecursiveFallback = "Recursive fallback not allowed for character \\u{0:X4}.";
		/// <summary>Recursive fallback not allowed for bytes {0}.</summary>
		internal static string @Argument_RecursiveFallbackBytes = "Recursive fallback not allowed for bytes {0}.";
		/// <summary>Label multiply defined.</summary>
		internal static string @Argument_RedefinedLabel = "Label multiply defined.";
		/// <summary>Token {0:x} is not a valid FieldInfo token in the scope of module {1}.</summary>
		internal static string @Argument_ResolveField = "Token {0:x} is not a valid FieldInfo token in the scope of module {1}.";
		/// <summary>Type handle '{0}' and field handle with declaring type '{1}' are incompatible. Get RuntimeFieldHandle and declaring RuntimeTypeHandle off the same FieldInfo.</summary>
		internal static string @Argument_ResolveFieldHandle = "Type handle '{0}' and field handle with declaring type '{1}' are incompatible. Get RuntimeFieldHandle and declaring RuntimeTypeHandle off the same FieldInfo.";
		/// <summary>Token {0:x} is not a valid MemberInfo token in the scope of module {1}.</summary>
		internal static string @Argument_ResolveMember = "Token {0:x} is not a valid MemberInfo token in the scope of module {1}.";
		/// <summary>Token {0:x} is not a valid MethodBase token in the scope of module {1}.</summary>
		internal static string @Argument_ResolveMethod = "Token {0:x} is not a valid MethodBase token in the scope of module {1}.";
		/// <summary>Type handle '{0}' and method handle with declaring type '{1}' are incompatible. Get RuntimeMethodHandle and declaring RuntimeTypeHandle off the same MethodBase.</summary>
		internal static string @Argument_ResolveMethodHandle = "Type handle '{0}' and method handle with declaring type '{1}' are incompatible. Get RuntimeMethodHandle and declaring RuntimeTypeHandle off the same MethodBase.";
		/// <summary>Token {0} resolves to the special module type representing this module.</summary>
		internal static string @Argument_ResolveModuleType = "Token {0} resolves to the special module type representing this module.";
		/// <summary>Token {0:x} is not a valid string token in the scope of module {1}.</summary>
		internal static string @Argument_ResolveString = "Token {0:x} is not a valid string token in the scope of module {1}.";
		/// <summary>Token {0:x} is not a valid Type token in the scope of module {1}.</summary>
		internal static string @Argument_ResolveType = "Token {0:x} is not a valid Type token in the scope of module {1}.";
		/// <summary>The result is out of the supported range for this calendar. The result should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive.</summary>
		internal static string @Argument_ResultCalendarRange = "The result is out of the supported range for this calendar. The result should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive.";
		/// <summary>The initial count for the semaphore must be greater than or equal to zero and less than the maximum count.</summary>
		internal static string @Argument_SemaphoreInitialMaximum = "The initial count for the semaphore must be greater than or equal to zero and less than the maximum count.";
		/// <summary>Should not specify exception type for catch clause for filter block.</summary>
		internal static string @Argument_ShouldNotSpecifyExceptionType = "Should not specify exception type for catch clause for filter block.";
		/// <summary>Should only set visibility flags when creating EnumBuilder.</summary>
		internal static string @Argument_ShouldOnlySetVisibilityFlags = "Should only set visibility flags when creating EnumBuilder.";
		/// <summary>Completed signature cannot be modified.</summary>
		internal static string @Argument_SigIsFinalized = "Completed signature cannot be modified.";
		/// <summary>Stream was not readable.</summary>
		internal static string @Argument_StreamNotReadable = "Stream was not readable.";
		/// <summary>Stream was not writable.</summary>
		internal static string @Argument_StreamNotWritable = "Stream was not writable.";
		/// <summary>The first char in the string is the null character.</summary>
		internal static string @Argument_StringFirstCharIsZero = "The first char in the string is the null character.";
		/// <summary>String cannot be of zero length.</summary>
		internal static string @Argument_StringZeroLength = "String cannot be of zero length.";
		/// <summary>The structure must not be a value class.</summary>
		internal static string @Argument_StructMustNotBeValueClass = "The structure must not be a value class.";
		/// <summary>The TimeSpan parameter cannot be specified more precisely than whole minutes.</summary>
		internal static string @Argument_TimeSpanHasSeconds = "The TimeSpan parameter cannot be specified more precisely than whole minutes.";
		/// <summary>The tzfile does not begin with the magic characters 'TZif'. Please verify that the file is not corrupt.</summary>
		internal static string @Argument_TimeZoneInfoBadTZif = "The tzfile does not begin with the magic characters 'TZif'. Please verify that the file is not corrupt.";
		/// <summary>The TZif data structure is corrupt.</summary>
		internal static string @Argument_TimeZoneInfoInvalidTZif = "The TZif data structure is corrupt.";
		/// <summary>fromInclusive must be less than or equal to toExclusive.</summary>
		internal static string @Argument_ToExclusiveLessThanFromExclusive = "fromInclusive must be less than or equal to toExclusive.";
		/// <summary>Exception blocks may have at most one finally clause.</summary>
		internal static string @Argument_TooManyFinallyClause = "Exception blocks may have at most one finally clause.";
		/// <summary>The DaylightTransitionStart property must not equal the DaylightTransitionEnd property.</summary>
		internal static string @Argument_TransitionTimesAreIdentical = "The DaylightTransitionStart property must not equal the DaylightTransitionEnd property.";
		/// <summary>Field '{0}' in TypedReferences cannot be static.</summary>
		internal static string @Argument_TypedReferenceInvalidField = "Field '{0}' in TypedReferences cannot be static.";
		/// <summary>The specified type must be visible from COM.</summary>
		internal static string @Argument_TypeMustBeVisibleFromCom = "The specified type must be visible from COM.";
		/// <summary>The type must not be imported from COM.</summary>
		internal static string @Argument_TypeMustNotBeComImport = "The type must not be imported from COM.";
		/// <summary>Type name was too long. The fully qualified type name must be less than 1,024 characters.</summary>
		internal static string @Argument_TypeNameTooLong = "Type name was too long. The fully qualified type name must be less than 1,024 characters.";
		/// <summary>The type must be __ComObject or be derived from __ComObject.</summary>
		internal static string @Argument_TypeNotComObject = "The type must be __ComObject or be derived from __ComObject.";
		/// <summary>The Type object is not valid.</summary>
		internal static string @Argument_TypeNotValid = "The Type object is not valid.";
		/// <summary>The IL Generator cannot be used while there are unclosed exceptions.</summary>
		internal static string @Argument_UnclosedExceptionBlock = "The IL Generator cannot be used while there are unclosed exceptions.";
		/// <summary>Unknown unmanaged calling convention for function signature.</summary>
		internal static string @Argument_UnknownUnmanagedCallConv = "Unknown unmanaged calling convention for function signature.";
		/// <summary>The UnmanagedMemoryAccessor capacity and offset would wrap around the high end of the address space.</summary>
		internal static string @Argument_UnmanagedMemAccessorWrapAround = "The UnmanagedMemoryAccessor capacity and offset would wrap around the high end of the address space.";
		/// <summary>Local passed in does not belong to this ILGenerator.</summary>
		internal static string @Argument_UnmatchedMethodForLocal = "Local passed in does not belong to this ILGenerator.";
		/// <summary>Non-matching symbol scope.</summary>
		internal static string @Argument_UnmatchingSymScope = "Non-matching symbol scope.";
		/// <summary>The UTC time represented when the offset is applied must be between year 0 and 10,000.</summary>
		internal static string @Argument_UTCOutOfRange = "The UTC time represented when the offset is applied must be between year 0 and 10,000.";
		/// <summary>The length of the name exceeds the maximum limit.</summary>
		internal static string @Argument_WaitHandleNameTooLong = "The length of the name exceeds the maximum limit.";
		/// <summary>MethodOverride's body must be from this type.</summary>
		internal static string @ArgumentException_BadMethodImplBody = "MethodOverride's body must be from this type.";
		/// <summary>The buffer is not associated with this pool and may not be returned to it.</summary>
		internal static string @ArgumentException_BufferNotFromPool = "The buffer is not associated with this pool and may not be returned to it.";
		/// <summary>The object is not an array with the same number of elements as the array to compare it to.</summary>
		internal static string @ArgumentException_OtherNotArrayOfCorrectLength = "The object is not an array with the same number of elements as the array to compare it to.";
		/// <summary>Object contains non-primitive or non-blittable data.</summary>
		internal static string @ArgumentException_NotIsomorphic = "Object contains non-primitive or non-blittable data.";
		/// <summary>Argument must be of type {0}.</summary>
		internal static string @ArgumentException_TupleIncorrectType = "Argument must be of type {0}.";
		/// <summary>The last element of an eight element tuple must be a Tuple.</summary>
		internal static string @ArgumentException_TupleLastArgumentNotATuple = "The last element of an eight element tuple must be a Tuple.";
		/// <summary>Argument must be of type {0}.</summary>
		internal static string @ArgumentException_ValueTupleIncorrectType = "Argument must be of type {0}.";
		/// <summary>The last element of an eight element ValueTuple must be a ValueTuple.</summary>
		internal static string @ArgumentException_ValueTupleLastArgumentNotAValueTuple = "The last element of an eight element ValueTuple must be a ValueTuple.";
		/// <summary>Array cannot be null.</summary>
		internal static string @ArgumentNull_Array = "Array cannot be null.";
		/// <summary>At least one element in the specified array was null.</summary>
		internal static string @ArgumentNull_ArrayElement = "At least one element in the specified array was null.";
		/// <summary>Found a null value within an array.</summary>
		internal static string @ArgumentNull_ArrayValue = "Found a null value within an array.";
		/// <summary>Assembly cannot be null.</summary>
		internal static string @ArgumentNull_Assembly = "Assembly cannot be null.";
		/// <summary>AssemblyName cannot be null.</summary>
		internal static string @ArgumentNull_AssemblyName = "AssemblyName cannot be null.";
		/// <summary>AssemblyName.Name cannot be null or an empty string.</summary>
		internal static string @ArgumentNull_AssemblyNameName = "AssemblyName.Name cannot be null or an empty string.";
		/// <summary>Buffer cannot be null.</summary>
		internal static string @ArgumentNull_Buffer = "Buffer cannot be null.";
		/// <summary>Cannot have a null child.</summary>
		internal static string @ArgumentNull_Child = "Cannot have a null child.";
		/// <summary>Collection cannot be null.</summary>
		internal static string @ArgumentNull_Collection = "Collection cannot be null.";
		/// <summary>Dictionary cannot be null.</summary>
		internal static string @ArgumentNull_Dictionary = "Dictionary cannot be null.";
		/// <summary>File name cannot be null.</summary>
		internal static string @ArgumentNull_FileName = "File name cannot be null.";
		/// <summary>Value cannot be null.</summary>
		internal static string @ArgumentNull_Generic = "Value cannot be null.";
		/// <summary>Key cannot be null.</summary>
		internal static string @ArgumentNull_Key = "Key cannot be null.";
		/// <summary>Path cannot be null.</summary>
		internal static string @ArgumentNull_Path = "Path cannot be null.";
		/// <summary>SafeHandle cannot be null.</summary>
		internal static string @ArgumentNull_SafeHandle = "SafeHandle cannot be null.";
		/// <summary>Stream cannot be null.</summary>
		internal static string @ArgumentNull_Stream = "Stream cannot be null.";
		/// <summary>String reference not set to an instance of a String.</summary>
		internal static string @ArgumentNull_String = "String reference not set to an instance of a String.";
		/// <summary>Type cannot be null.</summary>
		internal static string @ArgumentNull_Type = "Type cannot be null.";
		/// <summary>Type in TypedReference cannot be null.</summary>
		internal static string @ArgumentNull_TypedRefType = "Type in TypedReference cannot be null.";
		/// <summary>The waitHandles parameter cannot be null.</summary>
		internal static string @ArgumentNull_Waithandles = "The waitHandles parameter cannot be null.";
		/// <summary>Actual value was {0}.</summary>
		internal static string @ArgumentOutOfRange_ActualValue = "Actual value was {0}.";
		/// <summary>The number of bytes cannot exceed the virtual address space on a 32 bit machine.</summary>
		internal static string @ArgumentOutOfRange_AddressSpace = "The number of bytes cannot exceed the virtual address space on a 32 bit machine.";
		/// <summary>Value to add was out of range.</summary>
		internal static string @ArgumentOutOfRange_AddValue = "Value to add was out of range.";
		/// <summary>Number was less than the array's lower bound in the first dimension.</summary>
		internal static string @ArgumentOutOfRange_ArrayLB = "Number was less than the array's lower bound in the first dimension.";
		/// <summary>Higher indices will exceed Int32.MaxValue because of large lower bound and/or length.</summary>
		internal static string @ArgumentOutOfRange_ArrayLBAndLength = "Higher indices will exceed Int32.MaxValue because of large lower bound and/or length.";
		/// <summary>Hour, Minute, and Second parameters describe an un-representable DateTime.</summary>
		internal static string @ArgumentOutOfRange_BadHourMinuteSecond = "Hour, Minute, and Second parameters describe an un-representable DateTime.";
		/// <summary>Year, Month, and Day parameters describe an un-representable DateTime.</summary>
		internal static string @ArgumentOutOfRange_BadYearMonthDay = "Year, Month, and Day parameters describe an un-representable DateTime.";
		/// <summary>Larger than collection size.</summary>
		internal static string @ArgumentOutOfRange_BiggerThanCollection = "Larger than collection size.";
		/// <summary>The number of bytes requested does not fit into BinaryReader's internal buffer.</summary>
		internal static string @ArgumentOutOfRange_BinaryReaderFillBuffer = "The number of bytes requested does not fit into BinaryReader's internal buffer.";
		/// <summary>Argument must be between {0} and {1}.</summary>
		internal static string @ArgumentOutOfRange_Bounds_Lower_Upper = "Argument must be between {0} and {1}.";
		/// <summary>Specified time is not supported in this calendar. It should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive.</summary>
		internal static string @ArgumentOutOfRange_CalendarRange = "Specified time is not supported in this calendar. It should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive.";
		/// <summary>Capacity exceeds maximum capacity.</summary>
		internal static string @ArgumentOutOfRange_Capacity = "Capacity exceeds maximum capacity.";
		/// <summary>Count must be positive and count must refer to a location within the string/array/collection.</summary>
		internal static string @ArgumentOutOfRange_Count = "Count must be positive and count must refer to a location within the string/array/collection.";
		/// <summary>The added or subtracted value results in an un-representable DateTime.</summary>
		internal static string @ArgumentOutOfRange_DateArithmetic = "The added or subtracted value results in an un-representable DateTime.";
		/// <summary>Months value must be between +/-120000.</summary>
		internal static string @ArgumentOutOfRange_DateTimeBadMonths = "Months value must be between +/-120000.";
		/// <summary>Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.</summary>
		internal static string @ArgumentOutOfRange_DateTimeBadTicks = "Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.";
		/// <summary>Ticks must be between 0 and and TimeOnly.MaxValue.Ticks.</summary>
		internal static string @ArgumentOutOfRange_TimeOnlyBadTicks = "Ticks must be between 0 and and TimeOnly.MaxValue.Ticks.";
		/// <summary>Years value must be between +/-10000.</summary>
		internal static string @ArgumentOutOfRange_DateTimeBadYears = "Years value must be between +/-10000.";
		/// <summary>Day must be between 1 and {0} for month {1}.</summary>
		internal static string @ArgumentOutOfRange_Day = "Day must be between 1 and {0} for month {1}.";
		/// <summary>The DayOfWeek enumeration must be in the range 0 through 6.</summary>
		internal static string @ArgumentOutOfRange_DayOfWeek = "The DayOfWeek enumeration must be in the range 0 through 6.";
		/// <summary>The Day parameter must be in the range 1 through 31.</summary>
		internal static string @ArgumentOutOfRange_DayParam = "The Day parameter must be in the range 1 through 31.";
		/// <summary>Decimal can only round to between 0 and 28 digits of precision.</summary>
		internal static string @ArgumentOutOfRange_DecimalRound = "Decimal can only round to between 0 and 28 digits of precision.";
		/// <summary>Decimal's scale value must be between 0 and 28, inclusive.</summary>
		internal static string @ArgumentOutOfRange_DecimalScale = "Decimal's scale value must be between 0 and 28, inclusive.";
		/// <summary>endIndex cannot be greater than startIndex.</summary>
		internal static string @ArgumentOutOfRange_EndIndexStartIndex = "endIndex cannot be greater than startIndex.";
		/// <summary>Enum value was out of legal range.</summary>
		internal static string @ArgumentOutOfRange_Enum = "Enum value was out of legal range.";
		/// <summary>Time value was out of era range.</summary>
		internal static string @ArgumentOutOfRange_Era = "Time value was out of era range.";
		/// <summary>Specified file length was too large for the file system.</summary>
		internal static string @ArgumentOutOfRange_FileLengthTooBig = "Specified file length was too large for the file system.";
		/// <summary>Not a valid Win32 FileTime.</summary>
		internal static string @ArgumentOutOfRange_FileTimeInvalid = "Not a valid Win32 FileTime.";
		/// <summary>Value must be positive.</summary>
		internal static string @ArgumentOutOfRange_GenericPositive = "Value must be positive.";
		/// <summary>Too many characters. The resulting number of bytes is larger than what can be returned as an int.</summary>
		internal static string @ArgumentOutOfRange_GetByteCountOverflow = "Too many characters. The resulting number of bytes is larger than what can be returned as an int.";
		/// <summary>Too many bytes. The resulting number of chars is larger than what can be returned as an int.</summary>
		internal static string @ArgumentOutOfRange_GetCharCountOverflow = "Too many bytes. The resulting number of chars is larger than what can be returned as an int.";
		/// <summary>Load factor needs to be between 0.1 and 1.0.</summary>
		internal static string @ArgumentOutOfRange_HashtableLoadFactor = "Load factor needs to be between 0.1 and 1.0.";
		/// <summary>Arrays larger than 2GB are not supported.</summary>
		internal static string @ArgumentOutOfRange_HugeArrayNotSupported = "Arrays larger than 2GB are not supported.";
		/// <summary>Index was out of range. Must be non-negative and less than the size of the collection.</summary>
		internal static string @ArgumentOutOfRange_Index = "Index was out of range. Must be non-negative and less than the size of the collection.";
		/// <summary>Index and count must refer to a location within the string.</summary>
		internal static string @ArgumentOutOfRange_IndexCount = "Index and count must refer to a location within the string.";
		/// <summary>Index and count must refer to a location within the buffer.</summary>
		internal static string @ArgumentOutOfRange_IndexCountBuffer = "Index and count must refer to a location within the buffer.";
		/// <summary>Index and length must refer to a location within the string.</summary>
		internal static string @ArgumentOutOfRange_IndexLength = "Index and length must refer to a location within the string.";
		/// <summary>Index was out of range. Must be non-negative and less than the length of the string.</summary>
		internal static string @ArgumentOutOfRange_IndexString = "Index was out of range. Must be non-negative and less than the length of the string.";
		/// <summary>Input is too large to be processed.</summary>
		internal static string @ArgumentOutOfRange_InputTooLarge = "Input is too large to be processed.";
		/// <summary>Era value was not valid.</summary>
		internal static string @ArgumentOutOfRange_InvalidEraValue = "Era value was not valid.";
		/// <summary>A valid high surrogate character is between 0xd800 and 0xdbff, inclusive.</summary>
		internal static string @ArgumentOutOfRange_InvalidHighSurrogate = "A valid high surrogate character is between 0xd800 and 0xdbff, inclusive.";
		/// <summary>A valid low surrogate character is between 0xdc00 and 0xdfff, inclusive.</summary>
		internal static string @ArgumentOutOfRange_InvalidLowSurrogate = "A valid low surrogate character is between 0xdc00 and 0xdfff, inclusive.";
		/// <summary>A valid UTF32 value is between 0x000000 and 0x10ffff, inclusive, and should not include surrogate codepoint values (0x00d800 ~ 0x00dfff).</summary>
		internal static string @ArgumentOutOfRange_InvalidUTF32 = "A valid UTF32 value is between 0x000000 and 0x10ffff, inclusive, and should not include surrogate codepoint values (0x00d800 ~ 0x00dfff).";
		/// <summary>The specified length exceeds maximum capacity of SecureString.</summary>
		internal static string @ArgumentOutOfRange_Length = "The specified length exceeds maximum capacity of SecureString.";
		/// <summary>The length cannot be greater than the capacity.</summary>
		internal static string @ArgumentOutOfRange_LengthGreaterThanCapacity = "The length cannot be greater than the capacity.";
		/// <summary>The specified length exceeds the maximum value of {0}.</summary>
		internal static string @ArgumentOutOfRange_LengthTooLarge = "The specified length exceeds the maximum value of {0}.";
		/// <summary>Argument must be less than or equal to 2^31 - 1 milliseconds.</summary>
		internal static string @ArgumentOutOfRange_LessEqualToIntegerMaxVal = "Argument must be less than or equal to 2^31 - 1 milliseconds.";
		/// <summary>Index must be within the bounds of the List.</summary>
		internal static string @ArgumentOutOfRange_ListInsert = "Index must be within the bounds of the List.";
		/// <summary>Month must be between one and twelve.</summary>
		internal static string @ArgumentOutOfRange_Month = "Month must be between one and twelve.";
		/// <summary>Day number must be between 0 and DateOnly.MaxValue.DayNumber. {Locked="DateOnly.MaxValue.DayNumber"}</summary>
		internal static string @ArgumentOutOfRange_DayNumber = "Day number must be between 0 and DateOnly.MaxValue.DayNumber. {Locked='DateOnly.MaxValue.DayNumber'}";
		/// <summary>The Month parameter must be in the range 1 through 12.</summary>
		internal static string @ArgumentOutOfRange_MonthParam = "The Month parameter must be in the range 1 through 12.";
		/// <summary>Value must be non-negative and less than or equal to Int32.MaxValue.</summary>
		internal static string @ArgumentOutOfRange_MustBeNonNegInt32 = "Value must be non-negative and less than or equal to Int32.MaxValue.";
		/// <summary>'{0}' must be non-negative.</summary>
		internal static string @ArgumentOutOfRange_MustBeNonNegNum = "'{0}' must be non-negative.";
		/// <summary>'{0}' must be greater than zero.</summary>
		internal static string @ArgumentOutOfRange_MustBePositive = "'{0}' must be greater than zero.";
		/// <summary>Non-negative number required.</summary>
		internal static string @ArgumentOutOfRange_NeedNonNegNum = "Non-negative number required.";
		/// <summary>Number must be either non-negative and less than or equal to Int32.MaxValue or -1.</summary>
		internal static string @ArgumentOutOfRange_NeedNonNegOrNegative1 = "Number must be either non-negative and less than or equal to Int32.MaxValue or -1.";
		/// <summary>Positive number required.</summary>
		internal static string @ArgumentOutOfRange_NeedPosNum = "Positive number required.";
		/// <summary>The ID parameter must be in the range {0} through {1}.</summary>
		internal static string @ArgumentOutOfRange_NeedValidId = "The ID parameter must be in the range {0} through {1}.";
		/// <summary>Capacity must be positive.</summary>
		internal static string @ArgumentOutOfRange_NegativeCapacity = "Capacity must be positive.";
		/// <summary>Count cannot be less than zero.</summary>
		internal static string @ArgumentOutOfRange_NegativeCount = "Count cannot be less than zero.";
		/// <summary>Length cannot be less than zero.</summary>
		internal static string @ArgumentOutOfRange_NegativeLength = "Length cannot be less than zero.";
		/// <summary>Offset and length must refer to a position in the string.</summary>
		internal static string @ArgumentOutOfRange_OffsetLength = "Offset and length must refer to a position in the string.";
		/// <summary>Either offset did not refer to a position in the string, or there is an insufficient length of destination character array.</summary>
		internal static string @ArgumentOutOfRange_OffsetOut = "Either offset did not refer to a position in the string, or there is an insufficient length of destination character array.";
		/// <summary>The specified parameter index is not in range.</summary>
		internal static string @ArgumentOutOfRange_ParamSequence = "The specified parameter index is not in range.";
		/// <summary>Pointer startIndex and length do not refer to a valid string.</summary>
		internal static string @ArgumentOutOfRange_PartialWCHAR = "Pointer startIndex and length do not refer to a valid string.";
		/// <summary>Period must be less than 2^32-1.</summary>
		internal static string @ArgumentOutOfRange_PeriodTooLarge = "Period must be less than 2^32-1.";
		/// <summary>The position may not be greater or equal to the capacity of the accessor.</summary>
		internal static string @ArgumentOutOfRange_PositionLessThanCapacityRequired = "The position may not be greater or equal to the capacity of the accessor.";
		/// <summary>Valid values are between {0} and {1}, inclusive.</summary>
		internal static string @ArgumentOutOfRange_Range = "Valid values are between {0} and {1}, inclusive.";
		/// <summary>Rounding digits must be between 0 and 15, inclusive.</summary>
		internal static string @ArgumentOutOfRange_RoundingDigits = "Rounding digits must be between 0 and 15, inclusive.";
		/// <summary>Rounding digits must be between 0 and 6, inclusive.</summary>
		internal static string @ArgumentOutOfRange_RoundingDigits_MathF = "Rounding digits must be between 0 and 6, inclusive.";
		/// <summary>capacity was less than the current size.</summary>
		internal static string @ArgumentOutOfRange_SmallCapacity = "capacity was less than the current size.";
		/// <summary>MaxCapacity must be one or greater.</summary>
		internal static string @ArgumentOutOfRange_SmallMaxCapacity = "MaxCapacity must be one or greater.";
		/// <summary>StartIndex cannot be less than zero.</summary>
		internal static string @ArgumentOutOfRange_StartIndex = "StartIndex cannot be less than zero.";
		/// <summary>startIndex cannot be larger than length of string.</summary>
		internal static string @ArgumentOutOfRange_StartIndexLargerThanLength = "startIndex cannot be larger than length of string.";
		/// <summary>Stream length must be non-negative and less than 2^31 - 1 - origin.</summary>
		internal static string @ArgumentOutOfRange_StreamLength = "Stream length must be non-negative and less than 2^31 - 1 - origin.";
		/// <summary>Time-out interval must be less than 2^32-1.</summary>
		internal static string @ArgumentOutOfRange_TimeoutTooLarge = "Time-out interval must be less than 2^32-1.";
		/// <summary>The length of the buffer must be less than the maximum UIntPtr value for your platform.</summary>
		internal static string @ArgumentOutOfRange_UIntPtrMax = "The length of the buffer must be less than the maximum UIntPtr value for your platform.";
		/// <summary>UnmanagedMemoryStream length must be non-negative and less than 2^63 - 1 - baseAddress.</summary>
		internal static string @ArgumentOutOfRange_UnmanagedMemStreamLength = "UnmanagedMemoryStream length must be non-negative and less than 2^63 - 1 - baseAddress.";
		/// <summary>The UnmanagedMemoryStream capacity would wrap around the high end of the address space.</summary>
		internal static string @ArgumentOutOfRange_UnmanagedMemStreamWrapAround = "The UnmanagedMemoryStream capacity would wrap around the high end of the address space.";
		/// <summary>The TimeSpan parameter must be within plus or minus 14.0 hours.</summary>
		internal static string @ArgumentOutOfRange_UtcOffset = "The TimeSpan parameter must be within plus or minus 14.0 hours.";
		/// <summary>The sum of the BaseUtcOffset and DaylightDelta properties must within plus or minus 14.0 hours.</summary>
		internal static string @ArgumentOutOfRange_UtcOffsetAndDaylightDelta = "The sum of the BaseUtcOffset and DaylightDelta properties must within plus or minus 14.0 hours.";
		/// <summary>Version's parameters must be greater than or equal to zero.</summary>
		internal static string @ArgumentOutOfRange_Version = "Version's parameters must be greater than or equal to zero.";
		/// <summary>The Week parameter must be in the range 1 through 5.</summary>
		internal static string @ArgumentOutOfRange_Week = "The Week parameter must be in the range 1 through 5.";
		/// <summary>Year must be between 1 and 9999.</summary>
		internal static string @ArgumentOutOfRange_Year = "Year must be between 1 and 9999.";
		/// <summary>Function does not accept floating point Not-a-Number values.</summary>
		internal static string @Arithmetic_NaN = "Function does not accept floating point Not-a-Number values.";
		/// <summary>Source array type cannot be assigned to destination array type.</summary>
		internal static string @ArrayTypeMismatch_CantAssignType = "Source array type cannot be assigned to destination array type.";
		/// <summary>Array.ConstrainedCopy will only work on array types that are provably compatible, without any form of boxing, unboxing, widening, or casting of each array element. Change the array types (i.e., copy a Derived[] to a Base[]), or use a mitigation strategy in the CER for Array.Copy's less powerful reliability contract, such as cloning the array or throwing away the potentially corrupt destination array.</summary>
		internal static string @ArrayTypeMismatch_ConstrainedCopy = "Array.ConstrainedCopy will only work on array types that are provably compatible, without any form of boxing, unboxing, widening, or casting of each array element. Change the array types (i.e., copy a Derived[] to a Base[]), or use a mitigation strategy in the CER for Array.Copy's less powerful reliability contract, such as cloning the array or throwing away the potentially corrupt destination array.";
		/// <summary>Cannot unload non-collectible AssemblyLoadContext.</summary>
		internal static string @AssemblyLoadContext_Unload_CannotUnloadIfNotCollectible = "Cannot unload non-collectible AssemblyLoadContext.";
		/// <summary>AssemblyLoadContext is unloading or was already unloaded.</summary>
		internal static string @AssemblyLoadContext_Verify_NotUnloading = "AssemblyLoadContext is unloading or was already unloaded.";
		/// <summary>Assertion failed.</summary>
		internal static string @AssertionFailed = "Assertion failed.";
		/// <summary>Assertion failed: {0}</summary>
		internal static string @AssertionFailed_Cnd = "Assertion failed: {0}";
		/// <summary>Assumption failed.</summary>
		internal static string @AssumptionFailed = "Assumption failed.";
		/// <summary>Assumption failed: {0}</summary>
		internal static string @AssumptionFailed_Cnd = "Assumption failed: {0}";
		/// <summary>The builder was not properly initialized.</summary>
		internal static string @AsyncMethodBuilder_InstanceNotInitialized = "The builder was not properly initialized.";
		/// <summary>Bad IL format.</summary>
		internal static string @BadImageFormat_BadILFormat = "Bad IL format.";
		/// <summary>Corrupt .resources file. The specified type doesn't exist.</summary>
		internal static string @BadImageFormat_InvalidType = "Corrupt .resources file. The specified type doesn't exist.";
		/// <summary>Corrupt .resources file. String length must be non-negative.</summary>
		internal static string @BadImageFormat_NegativeStringLength = "Corrupt .resources file. String length must be non-negative.";
		/// <summary>The parameters and the signature of the method don't match.</summary>
		internal static string @BadImageFormat_ParameterSignatureMismatch = "The parameters and the signature of the method don't match.";
		/// <summary>The type serialized in the .resources file was not the same type that the .resources file said it contained. Expected '{0}' but read '{1}'.</summary>
		internal static string @BadImageFormat_ResType_SerBlobMismatch = "The type serialized in the .resources file was not the same type that the .resources file said it contained. Expected '{0}' but read '{1}'.";
		/// <summary>Corrupt .resources file. The specified data length '{0}' is not a valid position in the stream.</summary>
		internal static string @BadImageFormat_ResourceDataLengthInvalid = "Corrupt .resources file. The specified data length '{0}' is not a valid position in the stream.";
		/// <summary>Corrupt .resources file. A resource name extends past the end of the stream.</summary>
		internal static string @BadImageFormat_ResourceNameCorrupted = "Corrupt .resources file. A resource name extends past the end of the stream.";
		/// <summary>Corrupt .resources file. The resource name for name index {0} extends past the end of the stream.</summary>
		internal static string @BadImageFormat_ResourceNameCorrupted_NameIndex = "Corrupt .resources file. The resource name for name index {0} extends past the end of the stream.";
		/// <summary>Corrupt .resources file. Invalid offset '{0}' into data section.</summary>
		internal static string @BadImageFormat_ResourcesDataInvalidOffset = "Corrupt .resources file. Invalid offset '{0}' into data section.";
		/// <summary>Corrupt .resources file. Unable to read resources from this file because of invalid header information. Try regenerating the .resources file.</summary>
		internal static string @BadImageFormat_ResourcesHeaderCorrupted = "Corrupt .resources file. Unable to read resources from this file because of invalid header information. Try regenerating the .resources file.";
		/// <summary>Corrupt .resources file. String for name index '{0}' extends past the end of the file.</summary>
		internal static string @BadImageFormat_ResourcesIndexTooLong = "Corrupt .resources file. String for name index '{0}' extends past the end of the file.";
		/// <summary>Corrupt .resources file. Invalid offset '{0}' into name section.</summary>
		internal static string @BadImageFormat_ResourcesNameInvalidOffset = "Corrupt .resources file. Invalid offset '{0}' into name section.";
		/// <summary>Corrupt .resources file. Resource name extends past the end of the file.</summary>
		internal static string @BadImageFormat_ResourcesNameTooLong = "Corrupt .resources file. Resource name extends past the end of the file.";
		/// <summary>Corrupt .resources file. The specified type doesn't match the available data in the stream.</summary>
		internal static string @BadImageFormat_TypeMismatch = "Corrupt .resources file. The specified type doesn't match the available data in the stream.";
		/// <summary>No tokens were supplied.</summary>
		internal static string @CancellationToken_CreateLinkedToken_TokensIsEmpty = "No tokens were supplied.";
		/// <summary>The CancellationTokenSource has been disposed.</summary>
		internal static string @CancellationTokenSource_Disposed = "The CancellationTokenSource has been disposed.";
		/// <summary>The SyncRoot property may not be used for the synchronization of concurrent collections.</summary>
		internal static string @ConcurrentCollection_SyncRoot_NotSupported = "The SyncRoot property may not be used for the synchronization of concurrent collections.";
		/// <summary>Task {2} completed.</summary>
		internal static string @event_TaskCompleted = "Task {2} completed.";
		/// <summary>Task {2} scheduled to TaskScheduler {0}.</summary>
		internal static string @event_TaskScheduled = "Task {2} scheduled to TaskScheduler {0}.";
		/// <summary>Task {2} executing.</summary>
		internal static string @event_TaskStarted = "Task {2} executing.";
		/// <summary>Beginning wait ({3}) on Task {2}.</summary>
		internal static string @event_TaskWaitBegin = "Beginning wait ({3}) on Task {2}.";
		/// <summary>Ending wait on Task {2}.</summary>
		internal static string @event_TaskWaitEnd = "Ending wait on Task {2}.";
		/// <summary>Abstract event source must not declare event methods ({0} with ID {1}).</summary>
		internal static string @EventSource_AbstractMustNotDeclareEventMethods = "Abstract event source must not declare event methods ({0} with ID {1}).";
		/// <summary>Abstract event source must not declare {0} nested type.</summary>
		internal static string @EventSource_AbstractMustNotDeclareKTOC = "Abstract event source must not declare {0} nested type.";
		/// <summary>Getting out of bounds during scalar addition.</summary>
		internal static string @EventSource_AddScalarOutOfRange = "Getting out of bounds during scalar addition.";
		/// <summary>Bad Hexidecimal digit "{0}".</summary>
		internal static string @EventSource_BadHexDigit = "Bad Hexidecimal digit '{0}'.";
		/// <summary>Channel {0} does not match event channel value {1}.</summary>
		internal static string @EventSource_ChannelTypeDoesNotMatchEventChannelValue = "Channel {0} does not match event channel value {1}.";
		/// <summary>Data descriptors are out of range.</summary>
		internal static string @EventSource_DataDescriptorsOutOfRange = "Data descriptors are out of range.";
		/// <summary>Multiple definitions for string "{0}".</summary>
		internal static string @EventSource_DuplicateStringKey = "Multiple definitions for string '{0}'.";
		/// <summary>The type of {0} is not expected in {1}.</summary>
		internal static string @EventSource_EnumKindMismatch = "The type of {0} is not expected in {1}.";
		/// <summary>Must have an even number of Hexidecimal digits.</summary>
		internal static string @EventSource_EvenHexDigits = "Must have an even number of Hexidecimal digits.";
		/// <summary>Channel {0} has a value of {1} which is outside the legal range (16-254).</summary>
		internal static string @EventSource_EventChannelOutOfRange = "Channel {0} has a value of {1} which is outside the legal range (16-254).";
		/// <summary>Event {0} has ID {1} which is already in use.</summary>
		internal static string @EventSource_EventIdReused = "Event {0} has ID {1} which is already in use.";
		/// <summary>Event {0} (with ID {1}) has a non-default opcode but not a task.</summary>
		internal static string @EventSource_EventMustHaveTaskIfNonDefaultOpcode = "Event {0} (with ID {1}) has a non-default opcode but not a task.";
		/// <summary>Event method {0} (with ID {1}) is an explicit interface method implementation. Re-write method as implicit implementation.</summary>
		internal static string @EventSource_EventMustNotBeExplicitImplementation = "Event method {0} (with ID {1}) is an explicit interface method implementation. Re-write method as implicit implementation.";
		/// <summary>Event {0} (with ID {1}) has a name that is not the concatenation of its task name and opcode.</summary>
		internal static string @EventSource_EventNameDoesNotEqualTaskPlusOpcode = "Event {0} (with ID {1}) has a name that is not the concatenation of its task name and opcode.";
		/// <summary>Event name {0} used more than once. If you wish to overload a method, the overloaded method should have a NonEvent attribute.</summary>
		internal static string @EventSource_EventNameReused = "Event name {0} used more than once. If you wish to overload a method, the overloaded method should have a NonEvent attribute.";
		/// <summary>Event {0} was called with {1} argument(s), but it is defined with {2} parameter(s).</summary>
		internal static string @EventSource_EventParametersMismatch = "Event {0} was called with {1} argument(s), but it is defined with {2} parameter(s).";
		/// <summary>An instance of EventSource with Guid {0} already exists.</summary>
		internal static string @EventSource_EventSourceGuidInUse = "An instance of EventSource with Guid {0} already exists.";
		/// <summary>The payload for a single event is too large.</summary>
		internal static string @EventSource_EventTooBig = "The payload for a single event is too large.";
		/// <summary>Event {0} specifies an Admin channel {1}. It must specify a Message property.</summary>
		internal static string @EventSource_EventWithAdminChannelMustHaveMessage = "Event {0} specifies an Admin channel {1}. It must specify a Message property.";
		/// <summary>Keyword {0} has a value of {1} which is outside the legal range (0-0x0000080000000000).</summary>
		internal static string @EventSource_IllegalKeywordsValue = "Keyword {0} has a value of {1} which is outside the legal range (0-0x0000080000000000).";
		/// <summary>Opcode {0} has a value of {1} which is outside the legal range (11-238).</summary>
		internal static string @EventSource_IllegalOpcodeValue = "Opcode {0} has a value of {1} which is outside the legal range (11-238).";
		/// <summary>Task {0} has a value of {1} which is outside the legal range (1-65535).</summary>
		internal static string @EventSource_IllegalTaskValue = "Task {0} has a value of {1} which is outside the legal range (1-65535).";
		/// <summary>Illegal value "{0}" (prefix strings with @ to indicate a literal string).</summary>
		internal static string @EventSource_IllegalValue = "Illegal value '{0}' (prefix strings with @ to indicate a literal string).";
		/// <summary>Incorrectly-authored TypeInfo - a type should be serialized as one field or as one group</summary>
		internal static string @EventSource_IncorrentlyAuthoredTypeInfo = "Incorrectly-authored TypeInfo - a type should be serialized as one field or as one group";
		/// <summary>Invalid command value.</summary>
		internal static string @EventSource_InvalidCommand = "Invalid command value.";
		/// <summary>Can't specify both etw event format flags.</summary>
		internal static string @EventSource_InvalidEventFormat = "Can't specify both etw event format flags.";
		/// <summary>Keywords {0} and {1} are defined with the same value ({2}).</summary>
		internal static string @EventSource_KeywordCollision = "Keywords {0} and {1} are defined with the same value ({2}).";
		/// <summary>Value {0} for keyword {1} needs to be a power of 2.</summary>
		internal static string @EventSource_KeywordNeedPowerOfTwo = "Value {0} for keyword {1} needs to be a power of 2.";
		/// <summary>Creating an EventListener inside a EventListener callback.</summary>
		internal static string @EventSource_ListenerCreatedInsideCallback = "Creating an EventListener inside a EventListener callback.";
		/// <summary>Listener not found.</summary>
		internal static string @EventSource_ListenerNotFound = "Listener not found.";
		/// <summary>An error occurred when writing to a listener.</summary>
		internal static string @EventSource_ListenerWriteFailure = "An error occurred when writing to a listener.";
		/// <summary>Attempt to define more than the maximum limit of 8 channels for a provider.</summary>
		internal static string @EventSource_MaxChannelExceeded = "Attempt to define more than the maximum limit of 8 channels for a provider.";
		/// <summary>Event {0} was assigned event ID {1} but {2} was passed to WriteEvent.</summary>
		internal static string @EventSource_MismatchIdToWriteEvent = "Event {0} was assigned event ID {1} but {2} was passed to WriteEvent.";
		/// <summary>The Guid of an EventSource must be non zero.</summary>
		internal static string @EventSource_NeedGuid = "The Guid of an EventSource must be non zero.";
		/// <summary>The name of an EventSource must not be null.</summary>
		internal static string @EventSource_NeedName = "The name of an EventSource must not be null.";
		/// <summary>Event IDs must be positive integers.</summary>
		internal static string @EventSource_NeedPositiveId = "Event IDs must be positive integers.";
		/// <summary>No Free Buffers available from the operating system (e.g. event rate too fast).</summary>
		internal static string @EventSource_NoFreeBuffers = "No Free Buffers available from the operating system (e.g. event rate too fast).";
		/// <summary>The API supports only anonymous types or types decorated with the EventDataAttribute. Non-compliant type: {0} dataType.</summary>
		internal static string @EventSource_NonCompliantTypeError = "The API supports only anonymous types or types decorated with the EventDataAttribute. Non-compliant type: {0} dataType.";
		/// <summary>EventSource expects the first parameter of the Event method to be of type Guid and to be named "relatedActivityId" when calling WriteEventWithRelatedActivityId.</summary>
		internal static string @EventSource_NoRelatedActivityId = "EventSource expects the first parameter of the Event method to be of type Guid and to be named 'relatedActivityId' when calling WriteEventWithRelatedActivityId.";
		/// <summary>Arrays of Binary are not supported.</summary>
		internal static string @EventSource_NotSupportedArrayOfBinary = "Arrays of Binary are not supported.";
		/// <summary>Arrays of Nil are not supported.</summary>
		internal static string @EventSource_NotSupportedArrayOfNil = "Arrays of Nil are not supported.";
		/// <summary>Arrays of null-terminated string are not supported.</summary>
		internal static string @EventSource_NotSupportedArrayOfNullTerminatedString = "Arrays of null-terminated string are not supported.";
		/// <summary>Enumerables of custom-serialized data are not supported</summary>
		internal static string @EventSource_NotSupportedCustomSerializedData = "Enumerables of custom-serialized data are not supported";
		/// <summary>Nested arrays/enumerables are not supported.</summary>
		internal static string @EventSource_NotSupportedNestedArraysEnums = "Nested arrays/enumerables are not supported.";
		/// <summary>Null passed as a event argument.</summary>
		internal static string @EventSource_NullInput = "Null passed as a event argument.";
		/// <summary>Opcodes {0} and {1} are defined with the same value ({2}).</summary>
		internal static string @EventSource_OpcodeCollision = "Opcodes {0} and {1} are defined with the same value ({2}).";
		/// <summary>Pins are out of range.</summary>
		internal static string @EventSource_PinArrayOutOfRange = "Pins are out of range.";
		/// <summary>Recursive type definition is not supported.</summary>
		internal static string @EventSource_RecursiveTypeDefinition = "Recursive type definition is not supported.";
		/// <summary>An event with stop suffix must follow a corresponding event with a start suffix.</summary>
		internal static string @EventSource_StopsFollowStarts = "An event with stop suffix must follow a corresponding event with a start suffix.";
		/// <summary>Tasks {0} and {1} are defined with the same value ({2}).</summary>
		internal static string @EventSource_TaskCollision = "Tasks {0} and {1} are defined with the same value ({2}).";
		/// <summary>Event {0} (with ID {1}) has the same task/opcode pair as event {2} (with ID {3}).</summary>
		internal static string @EventSource_TaskOpcodePairReused = "Event {0} (with ID {1}) has the same task/opcode pair as event {2} (with ID {3}).";
		/// <summary>Too many arguments.</summary>
		internal static string @EventSource_TooManyArgs = "Too many arguments.";
		/// <summary>Too many fields in structure.</summary>
		internal static string @EventSource_TooManyFields = "Too many fields in structure.";
		/// <summary>EventSource({0}, {1})</summary>
		internal static string @EventSource_ToString = "EventSource({0}, {1})";
		/// <summary>There must be an even number of trait strings (they are key-value pairs).</summary>
		internal static string @EventSource_TraitEven = "There must be an even number of trait strings (they are key-value pairs).";
		/// <summary>Event source types must be sealed or abstract.</summary>
		internal static string @EventSource_TypeMustBeSealedOrAbstract = "Event source types must be sealed or abstract.";
		/// <summary>Event source types must derive from EventSource.</summary>
		internal static string @EventSource_TypeMustDeriveFromEventSource = "Event source types must derive from EventSource.";
		/// <summary>Use of undefined channel value {0} for event {1}.</summary>
		internal static string @EventSource_UndefinedChannel = "Use of undefined channel value {0} for event {1}.";
		/// <summary>Use of undefined keyword value {0} for event {1}.</summary>
		internal static string @EventSource_UndefinedKeyword = "Use of undefined keyword value {0} for event {1}.";
		/// <summary>Use of undefined opcode value {0} for event {1}.</summary>
		internal static string @EventSource_UndefinedOpcode = "Use of undefined opcode value {0} for event {1}.";
		/// <summary>Unknown ETW trait "{0}".</summary>
		internal static string @EventSource_UnknownEtwTrait = "Unknown ETW trait '{0}'.";
		/// <summary>Unsupported type {0} in event source.</summary>
		internal static string @EventSource_UnsupportedEventTypeInManifest = "Unsupported type {0} in event source.";
		/// <summary>Event {0} specifies an illegal or unsupported formatting message ("{1}").</summary>
		internal static string @EventSource_UnsupportedMessageProperty = "Event {0} specifies an illegal or unsupported formatting message ('{1}').";
		/// <summary>Event {0} was called with a different type as defined (argument "{1}"). This may cause the event to be displayed incorrectly.</summary>
		internal static string @EventSource_VarArgsParameterMismatch = "Event {0} was called with a different type as defined (argument '{1}'). This may cause the event to be displayed incorrectly.";
		/// <summary>--- End of inner exception stack trace ---</summary>
		internal static string @Exception_EndOfInnerExceptionStack = "--- End of inner exception stack trace ---";
		/// <summary>--- End of stack trace from previous location ---</summary>
		internal static string @Exception_EndStackTraceFromPreviousThrow = "--- End of stack trace from previous location ---";
		/// <summary>Exception of type '{0}' was thrown.</summary>
		internal static string @Exception_WasThrown = "Exception of type '{0}' was thrown.";
		/// <summary>An exception was not handled in an AsyncLocal<T> notification callback.</summary>
		internal static string @ExecutionContext_ExceptionInAsyncLocalNotification = "An exception was not handled in an AsyncLocal<T> notification callback.";
		/// <summary>Could not resolve assembly '{0}'.</summary>
		internal static string @FileNotFound_ResolveAssembly = "Could not resolve assembly '{0}'.";
		/// <summary>Duplicate AttributeUsageAttribute found on attribute type {0}.</summary>
		internal static string @Format_AttributeUsage = "Duplicate AttributeUsageAttribute found on attribute type {0}.";
		/// <summary>Too many bytes in what should have been a 7-bit encoded integer.</summary>
		internal static string @Format_Bad7BitInt = "Too many bytes in what should have been a 7-bit encoded integer.";
		/// <summary>Invalid digits for the specified base.</summary>
		internal static string @Format_BadBase = "Invalid digits for the specified base.";
		/// <summary>The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.</summary>
		internal static string @Format_BadBase64Char = "The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.";
		/// <summary>String '{0}' was not recognized as a valid Boolean.</summary>
		internal static string @Format_BadBoolean = "String '{0}' was not recognized as a valid Boolean.";
		/// <summary>Could not determine the order of year, month, and date from '{0}'.</summary>
		internal static string @Format_BadDatePattern = "Could not determine the order of year, month, and date from '{0}'.";
		/// <summary>String '{0}' was not recognized as a valid DateTime.</summary>
		internal static string @Format_BadDateTime = "String '{0}' was not recognized as a valid DateTime.";
		/// <summary>String '{0}' was not recognized as a valid DateOnly. {Locked="DateOnly"}</summary>
		internal static string @Format_BadDateOnly = "String '{0}' was not recognized as a valid DateOnly. {Locked='DateOnly'}";
		/// <summary>String '{0}' was not recognized as a valid TimeOnly. {Locked="TimeOnly"}</summary>
		internal static string @Format_BadTimeOnly = "String '{0}' was not recognized as a valid TimeOnly. {Locked='TimeOnly'}";
		/// <summary>String '{0}' contains parts which are not specific to the {1}.</summary>
		internal static string @Format_DateTimeOnlyContainsNoneDateParts = "String '{0}' contains parts which are not specific to the {1}.";
		/// <summary>The DateTime represented by the string '{0}' is not supported in calendar '{1}'.</summary>
		internal static string @Format_BadDateTimeCalendar = "The DateTime represented by the string '{0}' is not supported in calendar '{1}'.";
		/// <summary>String '{0}' was not recognized as a valid DateTime because the day of week was incorrect.</summary>
		internal static string @Format_BadDayOfWeek = "String '{0}' was not recognized as a valid DateTime because the day of week was incorrect.";
		/// <summary>Format specifier '{0}' was invalid.</summary>
		internal static string @Format_BadFormatSpecifier = "Format specifier '{0}' was invalid.";
		/// <summary>No format specifiers were provided.</summary>
		internal static string @Format_NoFormatSpecifier = "No format specifiers were provided.";
		/// <summary>The input is not a valid hex string as it contains a non-hex character.</summary>
		internal static string @Format_BadHexChar = "The input is not a valid hex string as it contains a non-hex character.";
		/// <summary>The input is not a valid hex string as its length is not a multiple of 2.</summary>
		internal static string @Format_BadHexLength = "The input is not a valid hex string as its length is not a multiple of 2.";
		/// <summary>Cannot find a matching quote character for the character '{0}'.</summary>
		internal static string @Format_BadQuote = "Cannot find a matching quote character for the character '{0}'.";
		/// <summary>String '{0}' was not recognized as a valid TimeSpan.</summary>
		internal static string @Format_BadTimeSpan = "String '{0}' was not recognized as a valid TimeSpan.";
		/// <summary>The DateTime represented by the string '{0}' is out of range.</summary>
		internal static string @Format_DateOutOfRange = "The DateTime represented by the string '{0}' is out of range.";
		/// <summary>Input string was either empty or contained only whitespace.</summary>
		internal static string @Format_EmptyInputString = "Input string was either empty or contained only whitespace.";
		/// <summary>Additional non-parsable characters are at the end of the string.</summary>
		internal static string @Format_ExtraJunkAtEnd = "Additional non-parsable characters are at the end of the string.";
		/// <summary>Expected {0xdddddddd, etc}.</summary>
		internal static string @Format_GuidBrace = "Expected {0xdddddddd, etc}.";
		/// <summary>Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).</summary>
		internal static string @Format_GuidBraceAfterLastNumber = "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).";
		/// <summary>Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).</summary>
		internal static string @Format_GuidComma = "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).";
		/// <summary>Dashes are in the wrong position for GUID parsing.</summary>
		internal static string @Format_GuidDashes = "Dashes are in the wrong position for GUID parsing.";
		/// <summary>Could not find the ending brace.</summary>
		internal static string @Format_GuidEndBrace = "Could not find the ending brace.";
		/// <summary>Expected 0x prefix.</summary>
		internal static string @Format_GuidHexPrefix = "Expected 0x prefix.";
		/// <summary>Guid string should only contain hexadecimal characters.</summary>
		internal static string @Format_GuidInvalidChar = "Guid string should only contain hexadecimal characters.";
		/// <summary>Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).</summary>
		internal static string @Format_GuidInvLen = "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).";
		/// <summary>Unrecognized Guid format.</summary>
		internal static string @Format_GuidUnrecognized = "Unrecognized Guid format.";
		/// <summary>Index (zero based) must be greater than or equal to zero and less than the size of the argument list.</summary>
		internal static string @Format_IndexOutOfRange = "Index (zero based) must be greater than or equal to zero and less than the size of the argument list.";
		/// <summary>Format string can be only "G", "g", "X", "x", "F", "f", "D" or "d".</summary>
		internal static string @Format_InvalidEnumFormatSpecification = "Format string can be only 'G', 'g', 'X', 'x', 'F', 'f', 'D' or 'd'.";
		/// <summary>Format string can be only "D", "d", "N", "n", "P", "p", "B", "b", "X" or "x".</summary>
		internal static string @Format_InvalidGuidFormatSpecification = "Format string can be only 'D', 'd', 'N', 'n', 'P', 'p', 'B', 'b', 'X' or 'x'.";
		/// <summary>Input string was not in a correct format.</summary>
		internal static string @Format_InvalidString = "Input string was not in a correct format.";
		/// <summary>There must be at least a partial date with a year present in the input string '{0}'.</summary>
		internal static string @Format_MissingIncompleteDate = "There must be at least a partial date with a year present in the input string '{0}'.";
		/// <summary>String must be exactly one character long.</summary>
		internal static string @Format_NeedSingleChar = "String must be exactly one character long.";
		/// <summary>Could not find any recognizable digits.</summary>
		internal static string @Format_NoParsibleDigits = "Could not find any recognizable digits.";
		/// <summary>The time zone offset of string '{0}' must be within plus or minus 14 hours.</summary>
		internal static string @Format_OffsetOutOfRange = "The time zone offset of string '{0}' must be within plus or minus 14 hours.";
		/// <summary>DateTime pattern '{0}' appears more than once with different values.</summary>
		internal static string @Format_RepeatDateTimePattern = "DateTime pattern '{0}' appears more than once with different values.";
		/// <summary>String cannot have zero length.</summary>
		internal static string @Format_StringZeroLength = "String cannot have zero length.";
		/// <summary>The string '{0}' was not recognized as a valid DateTime. There is an unknown word starting at index '{1}'.</summary>
		internal static string @Format_UnknownDateTimeWord = "The string '{0}' was not recognized as a valid DateTime. There is an unknown word starting at index '{1}'.";
		/// <summary>The UTC representation of the date '{0}' falls outside the year range 1-9999.</summary>
		internal static string @Format_UTCOutOfRange = "The UTC representation of the date '{0}' falls outside the year range 1-9999.";
		/// <summary>Unicode</summary>
		internal static string @Globalization_cp_1200 = "Unicode";
		/// <summary>Unicode (UTF-32)</summary>
		internal static string @Globalization_cp_12000 = "Unicode (UTF-32)";
		/// <summary>Unicode (UTF-32 Big-Endian)</summary>
		internal static string @Globalization_cp_12001 = "Unicode (UTF-32 Big-Endian)";
		/// <summary>Unicode (Big-Endian)</summary>
		internal static string @Globalization_cp_1201 = "Unicode (Big-Endian)";
		/// <summary>US-ASCII</summary>
		internal static string @Globalization_cp_20127 = "US-ASCII";
		/// <summary>Western European (ISO)</summary>
		internal static string @Globalization_cp_28591 = "Western European (ISO)";
		/// <summary>Unicode (UTF-7)</summary>
		internal static string @Globalization_cp_65000 = "Unicode (UTF-7)";
		/// <summary>Unicode (UTF-8)</summary>
		internal static string @Globalization_cp_65001 = "Unicode (UTF-8)";
		/// <summary>Array does not have that many dimensions.</summary>
		internal static string @IndexOutOfRange_ArrayRankIndex = "Array does not have that many dimensions.";
		/// <summary>Unmanaged memory stream position was beyond the capacity of the stream.</summary>
		internal static string @IndexOutOfRange_UMSPosition = "Unmanaged memory stream position was beyond the capacity of the stream.";
		/// <summary>Insufficient available memory to meet the expected demands of an operation at this time. Please try again later.</summary>
		internal static string @InsufficientMemory_MemFailPoint = "Insufficient available memory to meet the expected demands of an operation at this time. Please try again later.";
		/// <summary>Insufficient memory to meet the expected demands of an operation, and this system is likely to never satisfy this request. If this is a 32 bit system, consider booting in 3 GB mode.</summary>
		internal static string @InsufficientMemory_MemFailPoint_TooBig = "Insufficient memory to meet the expected demands of an operation, and this system is likely to never satisfy this request. If this is a 32 bit system, consider booting in 3 GB mode.";
		/// <summary>Insufficient available memory to meet the expected demands of an operation at this time, possibly due to virtual address space fragmentation. Please try again later.</summary>
		internal static string @InsufficientMemory_MemFailPoint_VAFrag = "Insufficient available memory to meet the expected demands of an operation at this time, possibly due to virtual address space fragmentation. Please try again later.";
		/// <summary>Type mismatch between source and destination types.</summary>
		internal static string @Interop_COM_TypeMismatch = "Type mismatch between source and destination types.";
		/// <summary>Cannot marshal: Encountered unmappable character.</summary>
		internal static string @Interop_Marshal_Unmappable_Char = "Cannot marshal: Encountered unmappable character.";
		/// <summary>Structures containing SafeHandle fields are not allowed in this operation.</summary>
		internal static string @Interop_Marshal_SafeHandle_InvalidOperation = "Structures containing SafeHandle fields are not allowed in this operation.";
		/// <summary>SafeHandle fields cannot be created from an unmanaged handle.</summary>
		internal static string @Interop_Marshal_CannotCreateSafeHandleField = "SafeHandle fields cannot be created from an unmanaged handle.";
		/// <summary>CriticalHandle fields cannot be created from an unmanaged handle.</summary>
		internal static string @Interop_Marshal_CannotCreateCriticalHandleField = "CriticalHandle fields cannot be created from an unmanaged handle.";
		/// <summary>Null object cannot be converted to a value type.</summary>
		internal static string @InvalidCast_CannotCastNullToValueType = "Null object cannot be converted to a value type.";
		/// <summary>Object cannot be coerced to the original type of the ByRef VARIANT it was obtained from.</summary>
		internal static string @InvalidCast_CannotCoerceByRefVariant = "Object cannot be coerced to the original type of the ByRef VARIANT it was obtained from.";
		/// <summary>Object cannot be cast to DBNull.</summary>
		internal static string @InvalidCast_DBNull = "Object cannot be cast to DBNull.";
		/// <summary>At least one element in the source array could not be cast down to the destination array type.</summary>
		internal static string @InvalidCast_DownCastArrayElement = "At least one element in the source array could not be cast down to the destination array type.";
		/// <summary>Object cannot be cast to Empty.</summary>
		internal static string @InvalidCast_Empty = "Object cannot be cast to Empty.";
		/// <summary>Object cannot be cast from DBNull to other types.</summary>
		internal static string @InvalidCast_FromDBNull = "Object cannot be cast from DBNull to other types.";
		/// <summary>Invalid cast from '{0}' to '{1}'.</summary>
		internal static string @InvalidCast_FromTo = "Invalid cast from '{0}' to '{1}'.";
		/// <summary>Object must implement IConvertible.</summary>
		internal static string @InvalidCast_IConvertible = "Object must implement IConvertible.";
		/// <summary>OleAut reported a type mismatch.</summary>
		internal static string @InvalidCast_OATypeMismatch = "OleAut reported a type mismatch.";
		/// <summary>Object cannot be stored in an array of this type.</summary>
		internal static string @InvalidCast_StoreArrayElement = "Object cannot be stored in an array of this type.";
		/// <summary>AsyncFlowControl objects can be used to restore flow only on a Context that had its flow suppressed.</summary>
		internal static string @InvalidOperation_AsyncFlowCtrlCtxMismatch = "AsyncFlowControl objects can be used to restore flow only on a Context that had its flow suppressed.";
		/// <summary>The stream is currently in use by a previous operation on the stream.</summary>
		internal static string @InvalidOperation_AsyncIOInProgress = "The stream is currently in use by a previous operation on the stream.";
		/// <summary>Method '{0}' does not have a method body.</summary>
		internal static string @InvalidOperation_BadEmptyMethodBody = "Method '{0}' does not have a method body.";
		/// <summary>ILGenerator usage is invalid.</summary>
		internal static string @InvalidOperation_BadILGeneratorUsage = "ILGenerator usage is invalid.";
		/// <summary>Opcodes using a short-form index cannot address a local position over 255.</summary>
		internal static string @InvalidOperation_BadInstructionOrIndexOutOfBound = "Opcodes using a short-form index cannot address a local position over 255.";
		/// <summary>Interface must be declared abstract.</summary>
		internal static string @InvalidOperation_BadInterfaceNotAbstract = "Interface must be declared abstract.";
		/// <summary>Method '{0}' cannot have a method body.</summary>
		internal static string @InvalidOperation_BadMethodBody = "Method '{0}' cannot have a method body.";
		/// <summary>Type must be declared abstract if any of its methods are abstract.</summary>
		internal static string @InvalidOperation_BadTypeAttributesNotAbstract = "Type must be declared abstract if any of its methods are abstract.";
		/// <summary>The method cannot be called twice on the same instance.</summary>
		internal static string @InvalidOperation_CalledTwice = "The method cannot be called twice on the same instance.";
		/// <summary>Unable to import a global method or field from a different module.</summary>
		internal static string @InvalidOperation_CannotImportGlobalFromDifferentModule = "Unable to import a global method or field from a different module.";
		/// <summary>A resolver is already set for the assembly.</summary>
		internal static string @InvalidOperation_CannotRegisterSecondResolver = "A resolver is already set for the assembly.";
		/// <summary>Cannot restore context flow when it is not suppressed.</summary>
		internal static string @InvalidOperation_CannotRestoreUnsupressedFlow = "Cannot restore context flow when it is not suppressed.";
		/// <summary>Context flow is already suppressed.</summary>
		internal static string @InvalidOperation_CannotSupressFlowMultipleTimes = "Context flow is already suppressed.";
		/// <summary>AsyncFlowControl object can be used only once to call Undo().</summary>
		internal static string @InvalidOperation_CannotUseAFCMultiple = "AsyncFlowControl object can be used only once to call Undo().";
		/// <summary>AsyncFlowControl object must be used on the thread where it was created.</summary>
		internal static string @InvalidOperation_CannotUseAFCOtherThread = "AsyncFlowControl object must be used on the thread where it was created.";
		/// <summary>Instances of abstract classes cannot be created.</summary>
		internal static string @InvalidOperation_CantInstantiateAbstractClass = "Instances of abstract classes cannot be created.";
		/// <summary>Instances of function pointers cannot be created.</summary>
		internal static string @InvalidOperation_CantInstantiateFunctionPointer = "Instances of function pointers cannot be created.";
		/// <summary>A prior operation on this collection was interrupted by an exception. Collection's state is no longer trusted.</summary>
		internal static string @InvalidOperation_CollectionCorrupted = "A prior operation on this collection was interrupted by an exception. Collection's state is no longer trusted.";
		/// <summary>Computer name could not be obtained.</summary>
		internal static string @InvalidOperation_ComputerName = "Computer name could not be obtained.";
		/// <summary>Operations that change non-concurrent collections must have exclusive access. A concurrent update was performed on this collection and corrupted its state. The collection's state is no longer correct.</summary>
		internal static string @InvalidOperation_ConcurrentOperationsNotSupported = "Operations that change non-concurrent collections must have exclusive access. A concurrent update was performed on this collection and corrupted its state. The collection's state is no longer correct.";
		/// <summary>Interface cannot have constructors.</summary>
		internal static string @InvalidOperation_ConstructorNotAllowedOnInterface = "Interface cannot have constructors.";
		/// <summary>Internal Error in DateTime and Calendar operations.</summary>
		internal static string @InvalidOperation_DateTimeParsing = "Internal Error in DateTime and Calendar operations.";
		/// <summary>Unable to access ILGenerator on a constructor created with DefineDefaultConstructor.</summary>
		internal static string @InvalidOperation_DefaultConstructorILGen = "Unable to access ILGenerator on a constructor created with DefineDefaultConstructor.";
		/// <summary>Enumeration already finished.</summary>
		internal static string @InvalidOperation_EnumEnded = "Enumeration already finished.";
		/// <summary>Collection was modified; enumeration operation may not execute.</summary>
		internal static string @InvalidOperation_EnumFailedVersion = "Collection was modified; enumeration operation may not execute.";
		/// <summary>Enumeration has not started. Call MoveNext.</summary>
		internal static string @InvalidOperation_EnumNotStarted = "Enumeration has not started. Call MoveNext.";
		/// <summary>Enumeration has either not started or has already finished.</summary>
		internal static string @InvalidOperation_EnumOpCantHappen = "Enumeration has either not started or has already finished.";
		/// <summary>This API does not support EventInfo tokens.</summary>
		internal static string @InvalidOperation_EventInfoNotAvailable = "This API does not support EventInfo tokens.";
		/// <summary>The generic parameters are already defined on this MethodBuilder.</summary>
		internal static string @InvalidOperation_GenericParametersAlreadySet = "The generic parameters are already defined on this MethodBuilder.";
		/// <summary>OSVersion's call to GetVersionEx failed.</summary>
		internal static string @InvalidOperation_GetVersion = "OSVersion's call to GetVersionEx failed.";
		/// <summary>Type definition of the global function has been completed.</summary>
		internal static string @InvalidOperation_GlobalsHaveBeenCreated = "Type definition of the global function has been completed.";
		/// <summary>Handle is not initialized.</summary>
		internal static string @InvalidOperation_HandleIsNotInitialized = "Handle is not initialized.";
		/// <summary>Handle is not pinned.</summary>
		internal static string @InvalidOperation_HandleIsNotPinned = "Handle is not pinned.";
		/// <summary>Hashtable insert failed. Load factor too high. The most common cause is multiple threads writing to the Hashtable simultaneously.</summary>
		internal static string @InvalidOperation_HashInsertFailed = "Hashtable insert failed. Load factor too high. The most common cause is multiple threads writing to the Hashtable simultaneously.";
		/// <summary>Failed to compare two elements in the array.</summary>
		internal static string @InvalidOperation_IComparerFailed = "Failed to compare two elements in the array.";
		/// <summary>Type definition of the method is complete.</summary>
		internal static string @InvalidOperation_MethodBaked = "Type definition of the method is complete.";
		/// <summary>The signature of the MethodBuilder can no longer be modified because an operation on the MethodBuilder caused the methodDef token to be created. For example, a call to SetCustomAttribute requires the methodDef token to emit the CustomAttribute token.</summary>
		internal static string @InvalidOperation_MethodBuilderBaked = "The signature of the MethodBuilder can no longer be modified because an operation on the MethodBuilder caused the methodDef token to be created. For example, a call to SetCustomAttribute requires the methodDef token to emit the CustomAttribute token.";
		/// <summary>Method already has a body.</summary>
		internal static string @InvalidOperation_MethodHasBody = "Method already has a body.";
		/// <summary>You must call Initialize on this object instance before using it.</summary>
		internal static string @InvalidOperation_MustCallInitialize = "You must call Initialize on this object instance before using it.";
		/// <summary>NativeOverlapped cannot be reused for multiple operations.</summary>
		internal static string @InvalidOperation_NativeOverlappedReused = "NativeOverlapped cannot be reused for multiple operations.";
		/// <summary>You cannot have more than one dynamic module in each dynamic assembly in this version of the runtime.</summary>
		internal static string @InvalidOperation_NoMultiModuleAssembly = "You cannot have more than one dynamic module in each dynamic assembly in this version of the runtime.";
		/// <summary>Cannot add the event handler since no public add method exists for the event.</summary>
		internal static string @InvalidOperation_NoPublicAddMethod = "Cannot add the event handler since no public add method exists for the event.";
		/// <summary>Cannot remove the event handler since no public remove method exists for the event.</summary>
		internal static string @InvalidOperation_NoPublicRemoveMethod = "Cannot remove the event handler since no public remove method exists for the event.";
		/// <summary>Not a debug ModuleBuilder.</summary>
		internal static string @InvalidOperation_NotADebugModule = "Not a debug ModuleBuilder.";
		/// <summary>The requested operation is invalid for DynamicMethod.</summary>
		internal static string @InvalidOperation_NotAllowedInDynamicMethod = "The requested operation is invalid for DynamicMethod.";
		/// <summary>Calling convention must be VarArgs.</summary>
		internal static string @InvalidOperation_NotAVarArgCallingConvention = "Calling convention must be VarArgs.";
		/// <summary>This operation is only valid on generic types.</summary>
		internal static string @InvalidOperation_NotGenericType = "This operation is only valid on generic types.";
		/// <summary>This API is not available when the concurrent GC is enabled.</summary>
		internal static string @InvalidOperation_NotWithConcurrentGC = "This API is not available when the concurrent GC is enabled.";
		/// <summary>Underlying type information on enumeration is not specified.</summary>
		internal static string @InvalidOperation_NoUnderlyingTypeOnEnum = "Underlying type information on enumeration is not specified.";
		/// <summary>Nullable object must have a value.</summary>
		internal static string @InvalidOperation_NoValue = "Nullable object must have a value.";
		/// <summary>The underlying array is null.</summary>
		internal static string @InvalidOperation_NullArray = "The underlying array is null.";
		/// <summary>Cannot call Set on a null context</summary>
		internal static string @InvalidOperation_NullContext = "Cannot call Set on a null context";
		/// <summary>The requested operation is invalid when called on a null ModuleHandle.</summary>
		internal static string @InvalidOperation_NullModuleHandle = "The requested operation is invalid when called on a null ModuleHandle.";
		/// <summary>Local variable scope was not properly closed.</summary>
		internal static string @InvalidOperation_OpenLocalVariableScope = "Local variable scope was not properly closed.";
		/// <summary>Cannot pack a packed Overlapped again.</summary>
		internal static string @InvalidOperation_Overlapped_Pack = "Cannot pack a packed Overlapped again.";
		/// <summary>This API does not support PropertyInfo tokens.</summary>
		internal static string @InvalidOperation_PropertyInfoNotAvailable = "This API does not support PropertyInfo tokens.";
		/// <summary>Instance is read-only.</summary>
		internal static string @InvalidOperation_ReadOnly = "Instance is read-only.";
		/// <summary>'{0}': ResourceSet derived classes must provide a constructor that takes a String file name and a constructor that takes a Stream.</summary>
		internal static string @InvalidOperation_ResMgrBadResSet_Type = "'{0}': ResourceSet derived classes must provide a constructor that takes a String file name and a constructor that takes a Stream.";
		/// <summary>Resource '{0}' was not a Stream - call GetObject instead.</summary>
		internal static string @InvalidOperation_ResourceNotStream_Name = "Resource '{0}' was not a Stream - call GetObject instead.";
		/// <summary>Resource '{0}' was not a String - call GetObject instead.</summary>
		internal static string @InvalidOperation_ResourceNotString_Name = "Resource '{0}' was not a String - call GetObject instead.";
		/// <summary>Resource was of type '{0}' instead of String - call GetObject instead.</summary>
		internal static string @InvalidOperation_ResourceNotString_Type = "Resource was of type '{0}' instead of String - call GetObject instead.";
		/// <summary>The NoGCRegion mode is in progress.End it and then set a different mode.</summary>
		internal static string @InvalidOperation_SetLatencyModeNoGC = "The NoGCRegion mode is in progress.End it and then set a different mode.";
		/// <summary>Method body should not exist.</summary>
		internal static string @InvalidOperation_ShouldNotHaveMethodBody = "Method body should not exist.";
		/// <summary>The thread was created with a ThreadStart delegate that does not accept a parameter.</summary>
		internal static string @InvalidOperation_ThreadWrongThreadStart = "The thread was created with a ThreadStart delegate that does not accept a parameter.";
		/// <summary>Timeouts are not supported on this stream.</summary>
		internal static string @InvalidOperation_TimeoutsNotSupported = "Timeouts are not supported on this stream.";
		/// <summary>The Timer was already closed using an incompatible Dispose method.</summary>
		internal static string @InvalidOperation_TimerAlreadyClosed = "The Timer was already closed using an incompatible Dispose method.";
		/// <summary>The given type cannot be boxed.</summary>
		internal static string @InvalidOperation_TypeCannotBeBoxed = "The given type cannot be boxed.";
		/// <summary>Unable to change after type has been created.</summary>
		internal static string @InvalidOperation_TypeHasBeenCreated = "Unable to change after type has been created.";
		/// <summary>Type has not been created.</summary>
		internal static string @InvalidOperation_TypeNotCreated = "Type has not been created.";
		/// <summary>This range in the underlying list is invalid. A possible cause is that elements were removed.</summary>
		internal static string @InvalidOperation_UnderlyingArrayListChanged = "This range in the underlying list is invalid. A possible cause is that elements were removed.";
		/// <summary>Unknown enum type.</summary>
		internal static string @InvalidOperation_UnknownEnumType = "Unknown enum type.";
		/// <summary>Either the IAsyncResult object did not come from the corresponding async method on this type, or the End method was called multiple times with the same IAsyncResult.</summary>
		internal static string @InvalidOperation_WrongAsyncResultOrEndCalledMultiple = "Either the IAsyncResult object did not come from the corresponding async method on this type, or the End method was called multiple times with the same IAsyncResult.";
		/// <summary>Common Language Runtime detected an invalid program.</summary>
		internal static string @InvalidProgram_Default = "Common Language Runtime detected an invalid program.";
		/// <summary>The time zone ID '{0}' was found on the local computer, but the file at '{1}' was corrupt.</summary>
		internal static string @InvalidTimeZone_InvalidFileData = "The time zone ID '{0}' was found on the local computer, but the file at '{1}' was corrupt.";
		/// <summary>The time zone ID '{0}' was found on the local computer, but the registry information was corrupt.</summary>
		internal static string @InvalidTimeZone_InvalidRegistryData = "The time zone ID '{0}' was found on the local computer, but the registry information was corrupt.";
		/// <summary>Invalid Julian day in POSIX strings.</summary>
		internal static string @InvalidTimeZone_InvalidJulianDay = "Invalid Julian day in POSIX strings.";
		/// <summary>There are no ttinfo structures in the tzfile. At least one ttinfo structure is required in order to construct a TimeZoneInfo object.</summary>
		internal static string @InvalidTimeZone_NoTTInfoStructures = "There are no ttinfo structures in the tzfile. At least one ttinfo structure is required in order to construct a TimeZoneInfo object.";
		/// <summary>'{0}' is not a valid POSIX-TZ-environment-variable MDate rule. A valid rule has the format 'Mm.w.d'.</summary>
		internal static string @InvalidTimeZone_UnparseablePosixMDateString = "'{0}' is not a valid POSIX-TZ-environment-variable MDate rule. A valid rule has the format 'Mm.w.d'.";
		/// <summary>Invariant failed.</summary>
		internal static string @InvariantFailed = "Invariant failed.";
		/// <summary>Invariant failed: {0}</summary>
		internal static string @InvariantFailed_Cnd = "Invariant failed: {0}";
		/// <summary>This assembly does not have a file table because it was loaded from memory.</summary>
		internal static string @IO_NoFileTableInInMemoryAssemblies = "This assembly does not have a file table because it was loaded from memory.";
		/// <summary>Unable to read beyond the end of the stream.</summary>
		internal static string @IO_EOF_ReadBeyondEOF = "Unable to read beyond the end of the stream.";
		/// <summary>Could not load the specified file.</summary>
		internal static string @IO_FileLoad = "Could not load the specified file.";
		/// <summary>Could not load the file '{0}'.</summary>
		internal static string @IO_FileLoad_FileName = "Could not load the file '{0}'.";
		/// <summary>File name: '{0}'</summary>
		internal static string @IO_FileName_Name = "File name: '{0}'";
		/// <summary>Unable to find the specified file.</summary>
		internal static string @IO_FileNotFound = "Unable to find the specified file.";
		/// <summary>Could not find file '{0}'.</summary>
		internal static string @IO_FileNotFound_FileName = "Could not find file '{0}'.";
		/// <summary>Cannot create '{0}' because a file or directory with the same name already exists.</summary>
		internal static string @IO_AlreadyExists_Name = "Cannot create '{0}' because a file or directory with the same name already exists.";
		/// <summary>Failed to create '{0}' with allocation size '{1}' because the disk was full.</summary>
		internal static string @IO_DiskFull_Path_AllocationSize = "Failed to create '{0}' with allocation size '{1}' because the disk was full.";
		/// <summary>Failed to create '{0}' with allocation size '{1}' because the file was too large.</summary>
		internal static string @IO_FileTooLarge_Path_AllocationSize = "Failed to create '{0}' with allocation size '{1}' because the file was too large.";
		/// <summary>BindHandle for ThreadPool failed on this handle.</summary>
		internal static string @IO_BindHandleFailed = "BindHandle for ThreadPool failed on this handle.";
		/// <summary>The file '{0}' already exists.</summary>
		internal static string @IO_FileExists_Name = "The file '{0}' already exists.";
		/// <summary>The OS handle's position is not what FileStream expected. Do not use a handle simultaneously in one FileStream and in Win32 code or another FileStream. This may cause data loss.</summary>
		internal static string @IO_FileStreamHandlePosition = "The OS handle's position is not what FileStream expected. Do not use a handle simultaneously in one FileStream and in Win32 code or another FileStream. This may cause data loss.";
		/// <summary>The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size.</summary>
		internal static string @IO_FileTooLong2GB = "The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size.";
		/// <summary>IO operation will not work. Most likely the file will become too long.</summary>
		internal static string @IO_FileTooLong = "IO operation will not work. Most likely the file will become too long.";
		/// <summary>IO operation will not work. Most likely the file will become too long or the handle was not opened to support synchronous IO operations.</summary>
		internal static string @IO_FileTooLongOrHandleNotSync = "IO operation will not work. Most likely the file will become too long or the handle was not opened to support synchronous IO operations.";
		/// <summary>Unable to expand length of this stream beyond its capacity.</summary>
		internal static string @IO_FixedCapacity = "Unable to expand length of this stream beyond its capacity.";
		/// <summary>BinaryReader encountered an invalid string length of {0} characters.</summary>
		internal static string @IO_InvalidStringLen_Len = "BinaryReader encountered an invalid string length of {0} characters.";
		/// <summary>Unable seek backward to overwrite data that previously existed in a file opened in Append mode.</summary>
		internal static string @IO_SeekAppendOverwrite = "Unable seek backward to overwrite data that previously existed in a file opened in Append mode.";
		/// <summary>An attempt was made to move the position before the beginning of the stream.</summary>
		internal static string @IO_SeekBeforeBegin = "An attempt was made to move the position before the beginning of the stream.";
		/// <summary>Unable to truncate data that previously existed in a file opened in Append mode.</summary>
		internal static string @IO_SetLengthAppendTruncate = "Unable to truncate data that previously existed in a file opened in Append mode.";
		/// <summary>The process cannot access the file '{0}' because it is being used by another process.</summary>
		internal static string @IO_SharingViolation_File = "The process cannot access the file '{0}' because it is being used by another process.";
		/// <summary>The process cannot access the file because it is being used by another process.</summary>
		internal static string @IO_SharingViolation_NoFileName = "The process cannot access the file because it is being used by another process.";
		/// <summary>Stream was too long.</summary>
		internal static string @IO_StreamTooLong = "Stream was too long.";
		/// <summary>Could not find a part of the path.</summary>
		internal static string @IO_PathNotFound_NoPathName = "Could not find a part of the path.";
		/// <summary>Could not find a part of the path '{0}'.</summary>
		internal static string @IO_PathNotFound_Path = "Could not find a part of the path '{0}'.";
		/// <summary>The specified file name or path is too long, or a component of the specified path is too long.</summary>
		internal static string @IO_PathTooLong = "The specified file name or path is too long, or a component of the specified path is too long.";
		/// <summary>The path '{0}' is too long, or a component of the specified path is too long.</summary>
		internal static string @IO_PathTooLong_Path = "The path '{0}' is too long, or a component of the specified path is too long.";
		/// <summary>Too many levels of symbolic links in '{0}'.</summary>
		internal static string @IO_TooManySymbolicLinkLevels = "Too many levels of symbolic links in '{0}'.";
		/// <summary>[Unknown]</summary>
		internal static string @IO_UnknownFileName = "[Unknown]";
		/// <summary>The lazily-initialized type does not have a public, parameterless constructor.</summary>
		internal static string @Lazy_CreateValue_NoParameterlessCtorForT = "The lazily-initialized type does not have a public, parameterless constructor.";
		/// <summary>The mode argument specifies an invalid value.</summary>
		internal static string @Lazy_ctor_ModeInvalid = "The mode argument specifies an invalid value.";
		/// <summary>ValueFactory returned null.</summary>
		internal static string @Lazy_StaticInit_InvalidOperation = "ValueFactory returned null.";
		/// <summary>Value is not created.</summary>
		internal static string @Lazy_ToString_ValueNotCreated = "Value is not created.";
		/// <summary>ValueFactory attempted to access the Value property of this instance.</summary>
		internal static string @Lazy_Value_RecursiveCallsToValue = "ValueFactory attempted to access the Value property of this instance.";
		/// <summary>The spinCount argument must be in the range 0 to {0}, inclusive.</summary>
		internal static string @ManualResetEventSlim_ctor_SpinCountOutOfRange = "The spinCount argument must be in the range 0 to {0}, inclusive.";
		/// <summary>There are too many threads currently waiting on the event. A maximum of {0} waiting threads are supported.</summary>
		internal static string @ManualResetEventSlim_ctor_TooManyWaiters = "There are too many threads currently waiting on the event. A maximum of {0} waiting threads are supported.";
		/// <summary>The event has been disposed.</summary>
		internal static string @ManualResetEventSlim_Disposed = "The event has been disposed.";
		/// <summary>Marshaler restriction: Excessively long string.</summary>
		internal static string @Marshaler_StringTooLong = "Marshaler restriction: Excessively long string.";
		/// <summary>Constructor on type '{0}' not found.</summary>
		internal static string @MissingConstructor_Name = "Constructor on type '{0}' not found.";
		/// <summary>Field not found.</summary>
		internal static string @MissingField = "Field not found.";
		/// <summary>Field '{0}' not found.</summary>
		internal static string @MissingField_Name = "Field '{0}' not found.";
		/// <summary>A case-insensitive lookup for resource file "{0}" in assembly "{1}" found multiple entries. Remove the duplicates or specify the exact case.</summary>
		internal static string @MissingManifestResource_MultipleBlobs = "A case-insensitive lookup for resource file '{0}' in assembly '{1}' found multiple entries. Remove the duplicates or specify the exact case.";
		/// <summary>Could not find the resource "{0}" among the resources {2} embedded in the assembly "{1}", nor among the resources in any satellite assemblies for the specified culture. Perhaps the resources were embedded with an incorrect name.</summary>
		internal static string @MissingManifestResource_NoNeutralAsm = "Could not find the resource '{0}' among the resources {2} embedded in the assembly '{1}', nor among the resources in any satellite assemblies for the specified culture. Perhaps the resources were embedded with an incorrect name.";
		/// <summary>Could not find any resources appropriate for the specified culture (or the neutral culture) on disk.</summary>
		internal static string @MissingManifestResource_NoNeutralDisk = "Could not find any resources appropriate for the specified culture (or the neutral culture) on disk.";
		/// <summary>Member not found.</summary>
		internal static string @MissingMember = "Member not found.";
		/// <summary>Member '{0}' not found.</summary>
		internal static string @MissingMember_Name = "Member '{0}' not found.";
		/// <summary>TypedReference can only be made on nested value Types.</summary>
		internal static string @MissingMemberNestErr = "TypedReference can only be made on nested value Types.";
		/// <summary>FieldInfo does not match the target Type.</summary>
		internal static string @MissingMemberTypeRef = "FieldInfo does not match the target Type.";
		/// <summary>Method '{0}' not found.</summary>
		internal static string @MissingMethod_Name = "Method '{0}' not found.";
		/// <summary>The satellite assembly named "{1}" for fallback culture "{0}" either could not be found or could not be loaded. This is generally a setup problem. Please consider reinstalling or repairing the application.</summary>
		internal static string @MissingSatelliteAssembly_Culture_Name = "The satellite assembly named '{1}' for fallback culture '{0}' either could not be found or could not be loaded. This is generally a setup problem. Please consider reinstalling or repairing the application.";
		/// <summary>Resource lookup fell back to the ultimate fallback resources in a satellite assembly, but that satellite either was not found or could not be loaded. Please consider reinstalling or repairing the application.</summary>
		internal static string @MissingSatelliteAssembly_Default = "Resource lookup fell back to the ultimate fallback resources in a satellite assembly, but that satellite either was not found or could not be loaded. Please consider reinstalling or repairing the application.";
		/// <summary>Delegates that are not of type MulticastDelegate may not be combined.</summary>
		internal static string @Multicast_Combine = "Delegates that are not of type MulticastDelegate may not be combined.";
		/// <summary>An assembly (probably "{1}") must be rewritten using the code contracts binary rewriter (CCRewrite) because it is calling Contract.{0} and the CONTRACTS_FULL symbol is defined. Remove any explicit definitions of the CONTRACTS_FULL symbol from your project and rebuild. CCRewrite can be downloaded from https://go.microsoft.com/fwlink/?LinkID=169180. \r\nAfter the rewriter is installed, it can be enabled in Visual Studio from the project's Properties page on the Code Contracts pane. Ensure that "Perform Runtime Contract Checking" is enabled, which will define CONTRACTS_FULL.</summary>
		internal static string @MustUseCCRewrite = "An assembly (probably '{1}') must be rewritten using the code contracts binary rewriter (CCRewrite) because it is calling Contract.{0} and the CONTRACTS_FULL symbol is defined. Remove any explicit definitions of the CONTRACTS_FULL symbol from your project and rebuild. CCRewrite can be downloaded from https://go.microsoft.com/fwlink/?LinkID=169180. \r\nAfter the rewriter is installed, it can be enabled in Visual Studio from the project's Properties page on the Code Contracts pane. Ensure that 'Perform Runtime Contract Checking' is enabled, which will define CONTRACTS_FULL.";
		/// <summary>This non-CLS method is not implemented.</summary>
		internal static string @NotSupported_AbstractNonCLS = "This non-CLS method is not implemented.";
		/// <summary>Activation Attributes are not supported.</summary>
		internal static string @NotSupported_ActivAttr = "Activation Attributes are not supported.";
		/// <summary>Assembly.LoadFrom with hashValue is not supported.</summary>
		internal static string @NotSupported_AssemblyLoadFromHash = "Assembly.LoadFrom with hashValue is not supported.";
		/// <summary>Cannot create boxed ByRef-like values.</summary>
		internal static string @NotSupported_ByRefLike = "Cannot create boxed ByRef-like values.";
		/// <summary>Cannot create arrays of ByRef-like values.</summary>
		internal static string @NotSupported_ByRefLikeArray = "Cannot create arrays of ByRef-like values.";
		/// <summary>ByRef to ByRef-like return values are not supported in reflection invocation.</summary>
		internal static string @NotSupported_ByRefToByRefLikeReturn = "ByRef to ByRef-like return values are not supported in reflection invocation.";
		/// <summary>ByRef to void return values are not supported in reflection invocation.</summary>
		internal static string @NotSupported_ByRefToVoidReturn = "ByRef to void return values are not supported in reflection invocation.";
		/// <summary>Vararg calling convention not supported.</summary>
		internal static string @NotSupported_CallToVarArg = "Vararg calling convention not supported.";
		/// <summary>Equals() on Span and ReadOnlySpan is not supported. Use operator== instead.</summary>
		internal static string @NotSupported_CannotCallEqualsOnSpan = "Equals() on Span and ReadOnlySpan is not supported. Use operator== instead.";
		/// <summary>GetHashCode() on Span and ReadOnlySpan is not supported.</summary>
		internal static string @NotSupported_CannotCallGetHashCodeOnSpan = "GetHashCode() on Span and ReadOnlySpan is not supported.";
		/// <summary>ChangeType operation is not supported.</summary>
		internal static string @NotSupported_ChangeType = "ChangeType operation is not supported.";
		/// <summary>Resolving to a collectible assembly is not supported.</summary>
		internal static string @NotSupported_CollectibleAssemblyResolve = "Resolving to a collectible assembly is not supported.";
		/// <summary>A non-collectible assembly may not reference a collectible assembly.</summary>
		internal static string @NotSupported_CollectibleBoundNonCollectible = "A non-collectible assembly may not reference a collectible assembly.";
		/// <summary>CreateInstance cannot be used with an object of type TypeBuilder.</summary>
		internal static string @NotSupported_CreateInstanceWithTypeBuilder = "CreateInstance cannot be used with an object of type TypeBuilder.";
		/// <summary>Only one DBNull instance may exist, and calls to DBNull deserialization methods are not allowed.</summary>
		internal static string @NotSupported_DBNullSerial = "Only one DBNull instance may exist, and calls to DBNull deserialization methods are not allowed.";
		/// <summary>The invoked member is not supported in a dynamic assembly.</summary>
		internal static string @NotSupported_DynamicAssembly = "The invoked member is not supported in a dynamic assembly.";
		/// <summary>Wrong MethodAttributes or CallingConventions for DynamicMethod. Only public, static, standard supported</summary>
		internal static string @NotSupported_DynamicMethodFlags = "Wrong MethodAttributes or CallingConventions for DynamicMethod. Only public, static, standard supported";
		/// <summary>The invoked member is not supported in a dynamic module.</summary>
		internal static string @NotSupported_DynamicModule = "The invoked member is not supported in a dynamic module.";
		/// <summary>Collection was of a fixed size.</summary>
		internal static string @NotSupported_FixedSizeCollection = "Collection was of a fixed size.";
		/// <summary>Generic methods with UnmanagedCallersOnlyAttribute are invalid.</summary>
		internal static string @InvalidProgram_GenericMethod = "Generic methods with UnmanagedCallersOnlyAttribute are invalid.";
		/// <summary>This operation is invalid on overlapping buffers.</summary>
		internal static string @InvalidOperation_SpanOverlappedOperation = "This operation is invalid on overlapping buffers.";
		/// <summary>Invoking default method with named arguments is not supported.</summary>
		internal static string @NotSupported_IDispInvokeDefaultMemberWithNamedArgs = "Invoking default method with named arguments is not supported.";
		/// <summary>Illegal one-byte branch at position: {0}. Requested branch was: {1}.</summary>
		internal static string @NotSupported_IllegalOneByteBranch = "Illegal one-byte branch at position: {0}. Requested branch was: {1}.";
		/// <summary>Mutating a key collection derived from a dictionary is not allowed.</summary>
		internal static string @NotSupported_KeyCollectionSet = "Mutating a key collection derived from a dictionary is not allowed.";
		/// <summary>Cannot create uninitialized instances of types requiring managed activation.</summary>
		internal static string @NotSupported_ManagedActivation = "Cannot create uninitialized instances of types requiring managed activation.";
		/// <summary>The number of WaitHandles must be less than or equal to 64.</summary>
		internal static string @NotSupported_MaxWaitHandles = "The number of WaitHandles must be less than or equal to 64.";
		/// <summary>The number of WaitHandles on a STA thread must be less than or equal to 63.</summary>
		internal static string @NotSupported_MaxWaitHandles_STA = "The number of WaitHandles on a STA thread must be less than or equal to 63.";
		/// <summary>Memory stream is not expandable.</summary>
		internal static string @NotSupported_MemStreamNotExpandable = "Memory stream is not expandable.";
		/// <summary>Module argument must be a ModuleBuilder.</summary>
		internal static string @NotSupported_MustBeModuleBuilder = "Module argument must be a ModuleBuilder.";
		/// <summary>Methods with UnmanagedCallersOnlyAttribute cannot be used as delegate target.</summary>
		internal static string @NotSupported_UnmanagedCallersOnlyTarget = "Methods with UnmanagedCallersOnlyAttribute cannot be used as delegate target.";
		/// <summary>No data is available for encoding {0}. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.</summary>
		internal static string @NotSupported_NoCodepageData = "No data is available for encoding {0}. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.";
		/// <summary>Function not marked with UnmanagedCallersOnlyAttribute.</summary>
		internal static string @InvalidOperation_FunctionMissingUnmanagedCallersOnly = "Function not marked with UnmanagedCallersOnlyAttribute.";
		/// <summary>Non-blittable parameter types are invalid for UnmanagedCallersOnly methods.</summary>
		internal static string @InvalidProgram_NonBlittableTypes = "Non-blittable parameter types are invalid for UnmanagedCallersOnly methods.";
		/// <summary>Not supported in a non-reflected type.</summary>
		internal static string @NotSupported_NonReflectedType = "Not supported in a non-reflected type.";
		/// <summary>Non-static methods with UnmanagedCallersOnlyAttribute are invalid.</summary>
		internal static string @InvalidProgram_NonStaticMethod = "Non-static methods with UnmanagedCallersOnlyAttribute are invalid.";
		/// <summary>Parent does not have a default constructor. The default constructor must be explicitly defined.</summary>
		internal static string @NotSupported_NoParentDefaultConstructor = "Parent does not have a default constructor. The default constructor must be explicitly defined.";
		/// <summary>Cannot resolve {0} to a TypeInfo object.</summary>
		internal static string @NotSupported_NoTypeInfo = "Cannot resolve {0} to a TypeInfo object.";
		/// <summary>This feature is not implemented.</summary>
		internal static string @NotSupported_NYI = "This feature is not implemented.";
		/// <summary>Found an obsolete .resources file in assembly '{0}'. Rebuild that .resources file then rebuild that assembly.</summary>
		internal static string @NotSupported_ObsoleteResourcesFile = "Found an obsolete .resources file in assembly '{0}'. Rebuild that .resources file then rebuild that assembly.";
		/// <summary>The given Variant type is not supported by this OleAut function.</summary>
		internal static string @NotSupported_OleAutBadVarType = "The given Variant type is not supported by this OleAut function.";
		/// <summary>Cannot create arrays of open type.</summary>
		internal static string @NotSupported_OpenType = "Cannot create arrays of open type.";
		/// <summary>Output streams do not support TypeBuilders.</summary>
		internal static string @NotSupported_OutputStreamUsingTypeBuilder = "Output streams do not support TypeBuilders.";
		/// <summary>The specified operation is not supported on Ranges.</summary>
		internal static string @NotSupported_RangeCollection = "The specified operation is not supported on Ranges.";
		/// <summary>Accessor does not support reading.</summary>
		internal static string @NotSupported_Reading = "Accessor does not support reading.";
		/// <summary>Collection is read-only.</summary>
		internal static string @NotSupported_ReadOnlyCollection = "Collection is read-only.";
		/// <summary>Cannot read resources that depend on serialization.</summary>
		internal static string @NotSupported_ResourceObjectSerialization = "Cannot read resources that depend on serialization.";
		/// <summary>SignalAndWait on a STA thread is not supported.</summary>
		internal static string @NotSupported_SignalAndWaitSTAThread = "SignalAndWait on a STA thread is not supported.";
		/// <summary>The string comparison type passed in is currently not supported.</summary>
		internal static string @NotSupported_StringComparison = "The string comparison type passed in is currently not supported.";
		/// <summary>Derived classes must provide an implementation.</summary>
		internal static string @NotSupported_SubclassOverride = "Derived classes must provide an implementation.";
		/// <summary>Not supported in an array method of a type definition that is not complete.</summary>
		internal static string @NotSupported_SymbolMethod = "Not supported in an array method of a type definition that is not complete.";
		/// <summary>Stack size too deep. Possibly too many arguments.</summary>
		internal static string @NotSupported_TooManyArgs = "Stack size too deep. Possibly too many arguments.";
		/// <summary>Type is not supported.</summary>
		internal static string @NotSupported_Type = "Type is not supported.";
		/// <summary>The invoked member is not supported before the type is created.</summary>
		internal static string @NotSupported_TypeNotYetCreated = "The invoked member is not supported before the type is created.";
		/// <summary>This operation is not supported for an UnmanagedMemoryStream created from a SafeBuffer.</summary>
		internal static string @NotSupported_UmsSafeBuffer = "This operation is not supported for an UnmanagedMemoryStream created from a SafeBuffer.";
		/// <summary>The UnitySerializationHolder object is designed to transmit information about other types and is not serializable itself.</summary>
		internal static string @NotSupported_UnitySerHolder = "The UnitySerializationHolder object is designed to transmit information about other types and is not serializable itself.";
		/// <summary>TypeCode '{0}' was not valid.</summary>
		internal static string @NotSupported_UnknownTypeCode = "TypeCode '{0}' was not valid.";
		/// <summary>WaitAll for multiple handles on a STA thread is not supported.</summary>
		internal static string @NotSupported_WaitAllSTAThread = "WaitAll for multiple handles on a STA thread is not supported.";
		/// <summary>Stream does not support reading.</summary>
		internal static string @NotSupported_UnreadableStream = "Stream does not support reading.";
		/// <summary>Stream does not support seeking.</summary>
		internal static string @NotSupported_UnseekableStream = "Stream does not support seeking.";
		/// <summary>Stream does not support writing.</summary>
		internal static string @NotSupported_UnwritableStream = "Stream does not support writing.";
		/// <summary>Custom marshalers for value types are not currently supported.</summary>
		internal static string @NotSupported_ValueClassCM = "Custom marshalers for value types are not currently supported.";
		/// <summary>Mutating a value collection derived from a dictionary is not allowed.</summary>
		internal static string @NotSupported_ValueCollectionSet = "Mutating a value collection derived from a dictionary is not allowed.";
		/// <summary>Arrays of System.Void are not supported.</summary>
		internal static string @NotSupported_VoidArray = "Arrays of System.Void are not supported.";
		/// <summary>Accessor does not support writing.</summary>
		internal static string @NotSupported_Writing = "Accessor does not support writing.";
		/// <summary>This .resources file should not be read with this reader. The resource reader type is "{0}".</summary>
		internal static string @NotSupported_WrongResourceReader_Type = "This .resources file should not be read with this reader. The resource reader type is '{0}'.";
		/// <summary>The pointer for this method was null.</summary>
		internal static string @NullReference_This = "The pointer for this method was null.";
		/// <summary>Cannot access a closed file.</summary>
		internal static string @ObjectDisposed_FileClosed = "Cannot access a closed file.";
		/// <summary>Cannot access a disposed object.</summary>
		internal static string @ObjectDisposed_Generic = "Cannot access a disposed object.";
		/// <summary>Object name: '{0}'.</summary>
		internal static string @ObjectDisposed_ObjectName_Name = "Object name: '{0}'.";
		/// <summary>Cannot write to a closed TextWriter.</summary>
		internal static string @ObjectDisposed_WriterClosed = "Cannot write to a closed TextWriter.";
		/// <summary>Cannot read from a closed TextReader.</summary>
		internal static string @ObjectDisposed_ReaderClosed = "Cannot read from a closed TextReader.";
		/// <summary>Cannot access a closed resource set.</summary>
		internal static string @ObjectDisposed_ResourceSet = "Cannot access a closed resource set.";
		/// <summary>Cannot access a closed Stream.</summary>
		internal static string @ObjectDisposed_StreamClosed = "Cannot access a closed Stream.";
		/// <summary>Cannot access a closed accessor.</summary>
		internal static string @ObjectDisposed_ViewAccessorClosed = "Cannot access a closed accessor.";
		/// <summary>Safe handle has been closed.</summary>
		internal static string @ObjectDisposed_SafeHandleClosed = "Safe handle has been closed.";
		/// <summary>The operation was canceled.</summary>
		internal static string @OperationCanceled = "The operation was canceled.";
		/// <summary>Value was either too large or too small for an unsigned byte.</summary>
		internal static string @Overflow_Byte = "Value was either too large or too small for an unsigned byte.";
		/// <summary>Value was either too large or too small for a character.</summary>
		internal static string @Overflow_Char = "Value was either too large or too small for a character.";
		/// <summary>Value was either too large or too small for a Currency.</summary>
		internal static string @Overflow_Currency = "Value was either too large or too small for a Currency.";
		/// <summary>Value was either too large or too small for a Decimal.</summary>
		internal static string @Overflow_Decimal = "Value was either too large or too small for a Decimal.";
		/// <summary>The duration cannot be returned for TimeSpan.MinValue because the absolute value of TimeSpan.MinValue exceeds the value of TimeSpan.MaxValue.</summary>
		internal static string @Overflow_Duration = "The duration cannot be returned for TimeSpan.MinValue because the absolute value of TimeSpan.MinValue exceeds the value of TimeSpan.MaxValue.";
		/// <summary>Value was either too large or too small for an Int16.</summary>
		internal static string @Overflow_Int16 = "Value was either too large or too small for an Int16.";
		/// <summary>Value was either too large or too small for an Int32.</summary>
		internal static string @Overflow_Int32 = "Value was either too large or too small for an Int32.";
		/// <summary>Value was either too large or too small for an Int64.</summary>
		internal static string @Overflow_Int64 = "Value was either too large or too small for an Int64.";
		/// <summary>The current thread attempted to reacquire a mutex that has reached its maximum acquire count.</summary>
		internal static string @Overflow_MutexReacquireCount = "The current thread attempted to reacquire a mutex that has reached its maximum acquire count.";
		/// <summary>Negating the minimum value of a twos complement number is invalid.</summary>
		internal static string @Overflow_NegateTwosCompNum = "Negating the minimum value of a twos complement number is invalid.";
		/// <summary>The string was being parsed as an unsigned number and could not have a negative sign.</summary>
		internal static string @Overflow_NegativeUnsigned = "The string was being parsed as an unsigned number and could not have a negative sign.";
		/// <summary>Value was either too large or too small for a signed byte.</summary>
		internal static string @Overflow_SByte = "Value was either too large or too small for a signed byte.";
		/// <summary>The TimeSpan string '{0}' could not be parsed because at least one of the numeric components is out of range or contains too many digits.</summary>
		internal static string @Overflow_TimeSpanElementTooLarge = "The TimeSpan string '{0}' could not be parsed because at least one of the numeric components is out of range or contains too many digits.";
		/// <summary>TimeSpan overflowed because the duration is too long.</summary>
		internal static string @Overflow_TimeSpanTooLong = "TimeSpan overflowed because the duration is too long.";
		/// <summary>Value was either too large or too small for a UInt16.</summary>
		internal static string @Overflow_UInt16 = "Value was either too large or too small for a UInt16.";
		/// <summary>Value was either too large or too small for a UInt32.</summary>
		internal static string @Overflow_UInt32 = "Value was either too large or too small for a UInt32.";
		/// <summary>Value was either too large or too small for a UInt64.</summary>
		internal static string @Overflow_UInt64 = "Value was either too large or too small for a UInt64.";
		/// <summary>ArgIterator is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_ArgIterator = "ArgIterator is not supported on this platform.";
		/// <summary>COM Interop is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_ComInterop = "COM Interop is not supported on this platform.";
		/// <summary>The named version of this synchronization primitive is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_NamedSynchronizationPrimitives = "The named version of this synchronization primitive is not supported on this platform.";
		/// <summary>Wait operations on multiple wait handles including a named synchronization primitive are not supported on this platform.</summary>
		internal static string @PlatformNotSupported_NamedSyncObjectWaitAnyWaitAll = "Wait operations on multiple wait handles including a named synchronization primitive are not supported on this platform.";
		/// <summary>Locking/unlocking file regions is not supported on this platform. Use FileShare on the entire file instead.</summary>
		internal static string @PlatformNotSupported_OSXFileLocking = "Locking/unlocking file regions is not supported on this platform. Use FileShare on the entire file instead.";
		/// <summary>ReflectionOnly loading is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_ReflectionOnly = "ReflectionOnly loading is not supported on this platform.";
		/// <summary>Remoting is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_Remoting = "Remoting is not supported on this platform.";
		/// <summary>Secure binary serialization is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_SecureBinarySerialization = "Secure binary serialization is not supported on this platform.";
		/// <summary>Strong-name signing is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_StrongNameSigning = "Strong-name signing is not supported on this platform.";
		/// <summary>This API is specific to the way in which Windows handles asynchronous I/O, and is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_OverlappedIO = "This API is specific to the way in which Windows handles asynchronous I/O, and is not supported on this platform.";
		/// <summary>Marshalling a System.Type to an unmanaged ITypeInfo or marshalling an ITypeInfo to a System.Type is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_ITypeInfo = "Marshalling a System.Type to an unmanaged ITypeInfo or marshalling an ITypeInfo to a System.Type is not supported on this platform.";
		/// <summary>Marshalling an IDispatchEx to an IReflect or IExpando is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_IExpando = "Marshalling an IDispatchEx to an IReflect or IExpando is not supported on this platform.";
		/// <summary>Secondary AppDomains are not supported on this platform.</summary>
		internal static string @PlatformNotSupported_AppDomains = "Secondary AppDomains are not supported on this platform.";
		/// <summary>Code Access Security is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_CAS = "Code Access Security is not supported on this platform.";
		/// <summary>Windows Principal functionality is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_Principal = "Windows Principal functionality is not supported on this platform.";
		/// <summary>Thread abort is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_ThreadAbort = "Thread abort is not supported on this platform.";
		/// <summary>Thread suspend is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_ThreadSuspend = "Thread suspend is not supported on this platform.";
		/// <summary>Postcondition failed.</summary>
		internal static string @PostconditionFailed = "Postcondition failed.";
		/// <summary>Postcondition failed: {0}</summary>
		internal static string @PostconditionFailed_Cnd = "Postcondition failed: {0}";
		/// <summary>Postcondition failed after throwing an exception.</summary>
		internal static string @PostconditionOnExceptionFailed = "Postcondition failed after throwing an exception.";
		/// <summary>Postcondition failed after throwing an exception: {0}</summary>
		internal static string @PostconditionOnExceptionFailed_Cnd = "Postcondition failed after throwing an exception: {0}";
		/// <summary>Precondition failed.</summary>
		internal static string @PreconditionFailed = "Precondition failed.";
		/// <summary>Precondition failed: {0}</summary>
		internal static string @PreconditionFailed_Cnd = "Precondition failed: {0}";
		/// <summary>The home directory of the current user could not be determined.</summary>
		internal static string @PersistedFiles_NoHomeDirectory = "The home directory of the current user could not be determined.";
		/// <summary>Only single dimension arrays are supported here.</summary>
		internal static string @Rank_MultiDimNotSupported = "Only single dimension arrays are supported here.";
		/// <summary>The specified arrays must have the same number of dimensions.</summary>
		internal static string @Rank_MustMatch = "The specified arrays must have the same number of dimensions.";
		/// <summary>Unable to load one or more of the requested types.</summary>
		internal static string @ReflectionTypeLoad_LoadFailed = "Unable to load one or more of the requested types.";
		/// <summary>ResourceReader is closed.</summary>
		internal static string @ResourceReaderIsClosed = "ResourceReader is closed.";
		/// <summary>Stream is not a valid resource file.</summary>
		internal static string @Resources_StreamNotValid = "Stream is not a valid resource file.";
		/// <summary>Multiple custom attributes of the same type found.</summary>
		internal static string @RFLCT_AmbigCust = "Multiple custom attributes of the same type found.";
		/// <summary>An Int32 must be provided for the filter criteria.</summary>
		internal static string @InvalidFilterCriteriaException_CritInt = "An Int32 must be provided for the filter criteria.";
		/// <summary>A String must be provided for the filter criteria.</summary>
		internal static string @InvalidFilterCriteriaException_CritString = "A String must be provided for the filter criteria.";
		/// <summary>'{0}' field specified was not found.</summary>
		internal static string @RFLCT_InvalidFieldFail = "'{0}' field specified was not found.";
		/// <summary>'{0}' property specified was not found.</summary>
		internal static string @RFLCT_InvalidPropFail = "'{0}' property specified was not found.";
		/// <summary>Object does not match target type.</summary>
		internal static string @RFLCT_Targ_ITargMismatch = "Object does not match target type.";
		/// <summary>Non-static field requires a target.</summary>
		internal static string @RFLCT_Targ_StatFldReqTarg = "Non-static field requires a target.";
		/// <summary>Non-static method requires a target.</summary>
		internal static string @RFLCT_Targ_StatMethReqTarg = "Non-static method requires a target.";
		/// <summary>An object that does not derive from System.Exception has been wrapped in a RuntimeWrappedException.</summary>
		internal static string @RuntimeWrappedException = "An object that does not derive from System.Exception has been wrapped in a RuntimeWrappedException.";
		/// <summary>Failed to get marshaler for IID {0}.</summary>
		internal static string @StandardOleMarshalObjectGetMarshalerFailed = "Failed to get marshaler for IID {0}.";
		/// <summary>The time zone ID '{0}' was found on the local computer, but the application does not have permission to read the file.</summary>
		internal static string @Security_CannotReadFileData = "The time zone ID '{0}' was found on the local computer, but the application does not have permission to read the file.";
		/// <summary>The time zone ID '{0}' was found on the local computer, but the application does not have permission to read the registry information.</summary>
		internal static string @Security_CannotReadRegistryData = "The time zone ID '{0}' was found on the local computer, but the application does not have permission to read the registry information.";
		/// <summary>Requested registry access is not allowed.</summary>
		internal static string @Security_RegistryPermission = "Requested registry access is not allowed.";
		/// <summary>The initialCount argument must be non-negative and less than or equal to the maximumCount.</summary>
		internal static string @SemaphoreSlim_ctor_InitialCountWrong = "The initialCount argument must be non-negative and less than or equal to the maximumCount.";
		/// <summary>The maximumCount argument must be a positive number. If a maximum is not required, use the constructor without a maxCount parameter.</summary>
		internal static string @SemaphoreSlim_ctor_MaxCountWrong = "The maximumCount argument must be a positive number. If a maximum is not required, use the constructor without a maxCount parameter.";
		/// <summary>The semaphore has been disposed.</summary>
		internal static string @SemaphoreSlim_Disposed = "The semaphore has been disposed.";
		/// <summary>The releaseCount argument must be greater than zero.</summary>
		internal static string @SemaphoreSlim_Release_CountWrong = "The releaseCount argument must be greater than zero.";
		/// <summary>The timeout must represent a value between -1 and Int32.MaxValue, inclusive.</summary>
		internal static string @SemaphoreSlim_Wait_TimeoutWrong = "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.";
		/// <summary>Non existent ParameterInfo. Position bigger than member's parameters length.</summary>
		internal static string @Serialization_BadParameterInfo = "Non existent ParameterInfo. Position bigger than member's parameters length.";
		/// <summary>The value of the field '{0}' is invalid. The serialized data is corrupt.</summary>
		internal static string @Serialization_CorruptField = "The value of the field '{0}' is invalid. The serialized data is corrupt.";
		/// <summary>Invalid serialized DateTime data. Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.</summary>
		internal static string @Serialization_DateTimeTicksOutOfRange = "Invalid serialized DateTime data. Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.";
		/// <summary>Serializing delegates is not supported on this platform.</summary>
		internal static string @Serialization_DelegatesNotSupported = "Serializing delegates is not supported on this platform.";
		/// <summary>Insufficient state to return the real object.</summary>
		internal static string @Serialization_InsufficientState = "Insufficient state to return the real object.";
		/// <summary>An error occurred while deserializing the object. The serialized data is corrupt.</summary>
		internal static string @Serialization_InvalidData = "An error occurred while deserializing the object. The serialized data is corrupt.";
		/// <summary>The serialized data contained an invalid escape sequence '\\{0}'.</summary>
		internal static string @Serialization_InvalidEscapeSequence = "The serialized data contained an invalid escape sequence '\\{0}'.";
		/// <summary>OnDeserialization method was called while the object was not being deserialized.</summary>
		internal static string @Serialization_InvalidOnDeser = "OnDeserialization method was called while the object was not being deserialized.";
		/// <summary>An IntPtr or UIntPtr with an eight byte value cannot be deserialized on a machine with a four byte word size.</summary>
		internal static string @Serialization_InvalidPtrValue = "An IntPtr or UIntPtr with an eight byte value cannot be deserialized on a machine with a four byte word size.";
		/// <summary>Only system-provided types can be passed to the GetUninitializedObject method. '{0}' is not a valid instance of a type.</summary>
		internal static string @Serialization_InvalidType = "Only system-provided types can be passed to the GetUninitializedObject method. '{0}' is not a valid instance of a type.";
		/// <summary>The keys and values arrays have different sizes.</summary>
		internal static string @Serialization_KeyValueDifferentSizes = "The keys and values arrays have different sizes.";
		/// <summary>Invalid serialized DateTime data. Unable to find 'ticks' or 'dateData'.</summary>
		internal static string @Serialization_MissingDateTimeData = "Invalid serialized DateTime data. Unable to find 'ticks' or 'dateData'.";
		/// <summary>The Keys for this Hashtable are missing.</summary>
		internal static string @Serialization_MissingKeys = "The Keys for this Hashtable are missing.";
		/// <summary>The values for this dictionary are missing.</summary>
		internal static string @Serialization_MissingValues = "The values for this dictionary are missing.";
		/// <summary>Serialized member does not have a ParameterInfo.</summary>
		internal static string @Serialization_NoParameterInfo = "Serialized member does not have a ParameterInfo.";
		/// <summary>Member '{0}' was not found.</summary>
		internal static string @Serialization_NotFound = "Member '{0}' was not found.";
		/// <summary>One of the serialized keys is null.</summary>
		internal static string @Serialization_NullKey = "One of the serialized keys is null.";
		/// <summary>Version value must be positive.</summary>
		internal static string @Serialization_OptionalFieldVersionValue = "Version value must be positive.";
		/// <summary>Cannot add the same member twice to a SerializationInfo object.</summary>
		internal static string @Serialization_SameNameTwice = "Cannot add the same member twice to a SerializationInfo object.";
		/// <summary>The serialized Capacity property of StringBuilder must be positive, less than or equal to MaxCapacity and greater than or equal to the String length.</summary>
		internal static string @Serialization_StringBuilderCapacity = "The serialized Capacity property of StringBuilder must be positive, less than or equal to MaxCapacity and greater than or equal to the String length.";
		/// <summary>The serialized MaxCapacity property of StringBuilder must be positive and greater than or equal to the String length.</summary>
		internal static string @Serialization_StringBuilderMaxCapacity = "The serialized MaxCapacity property of StringBuilder must be positive and greater than or equal to the String length.";
		/// <summary>Setter must have parameters.</summary>
		internal static string @SetterHasNoParams = "Setter must have parameters.";
		/// <summary>The calling thread does not hold the lock.</summary>
		internal static string @SpinLock_Exit_SynchronizationLockException = "The calling thread does not hold the lock.";
		/// <summary>Thread tracking is disabled.</summary>
		internal static string @SpinLock_IsHeldByCurrentThread = "Thread tracking is disabled.";
		/// <summary>The timeout must be a value between -1 and Int32.MaxValue, inclusive.</summary>
		internal static string @SpinLock_TryEnter_ArgumentOutOfRange = "The timeout must be a value between -1 and Int32.MaxValue, inclusive.";
		/// <summary>The calling thread already holds the lock.</summary>
		internal static string @SpinLock_TryEnter_LockRecursionException = "The calling thread already holds the lock.";
		/// <summary>The tookLock argument must be set to false before calling this method.</summary>
		internal static string @SpinLock_TryReliableEnter_ArgumentException = "The tookLock argument must be set to false before calling this method.";
		/// <summary>The condition argument is null.</summary>
		internal static string @SpinWait_SpinUntil_ArgumentNull = "The condition argument is null.";
		/// <summary>The timeout must represent a value between -1 and Int32.MaxValue, inclusive.</summary>
		internal static string @SpinWait_SpinUntil_TimeoutWrong = "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.";
		/// <summary>in {0}:token 0x{1:x}+0x{2:x}</summary>
		internal static string @StackTrace_InFileILOffset = "in {0}:token 0x{1:x}+0x{2:x}";
		/// <summary>in {0}:line {1}</summary>
		internal static string @StackTrace_InFileLineNumber = "in {0}:line {1}";
		/// <summary>The specified TaskContinuationOptions combined LongRunning and ExecuteSynchronously. Synchronous continuations should not be long running.</summary>
		internal static string @Task_ContinueWith_ESandLR = "The specified TaskContinuationOptions combined LongRunning and ExecuteSynchronously. Synchronous continuations should not be long running.";
		/// <summary>The specified TaskContinuationOptions excluded all continuation kinds.</summary>
		internal static string @Task_ContinueWith_NotOnAnything = "The specified TaskContinuationOptions excluded all continuation kinds.";
		/// <summary>The value needs to translate in milliseconds to -1 (signifying an infinite timeout), 0, or a positive integer less than or equal to the maximum allowed timer duration.</summary>
		internal static string @Task_InvalidTimerTimeSpan = "The value needs to translate in milliseconds to -1 (signifying an infinite timeout), 0, or a positive integer less than or equal to the maximum allowed timer duration.";
		/// <summary>The value needs to be either -1 (signifying an infinite timeout), 0 or a positive integer.</summary>
		internal static string @Task_Delay_InvalidMillisecondsDelay = "The value needs to be either -1 (signifying an infinite timeout), 0 or a positive integer.";
		/// <summary>A task may only be disposed if it is in a completion state (RanToCompletion, Faulted or Canceled).</summary>
		internal static string @Task_Dispose_NotCompleted = "A task may only be disposed if it is in a completion state (RanToCompletion, Faulted or Canceled).";
		/// <summary>It is invalid to specify TaskCreationOptions.LongRunning in calls to FromAsync.</summary>
		internal static string @Task_FromAsync_LongRunning = "It is invalid to specify TaskCreationOptions.LongRunning in calls to FromAsync.";
		/// <summary>It is invalid to specify TaskCreationOptions.PreferFairness in calls to FromAsync.</summary>
		internal static string @Task_FromAsync_PreferFairness = "It is invalid to specify TaskCreationOptions.PreferFairness in calls to FromAsync.";
		/// <summary>The tasks argument contains no tasks.</summary>
		internal static string @Task_MultiTaskContinuation_EmptyTaskList = "The tasks argument contains no tasks.";
		/// <summary>It is invalid to exclude specific continuation kinds for continuations off of multiple tasks.</summary>
		internal static string @Task_MultiTaskContinuation_FireOptions = "It is invalid to exclude specific continuation kinds for continuations off of multiple tasks.";
		/// <summary>The tasks argument included a null value.</summary>
		internal static string @Task_MultiTaskContinuation_NullTask = "The tasks argument included a null value.";
		/// <summary>RunSynchronously may not be called on a task that was already started.</summary>
		internal static string @Task_RunSynchronously_AlreadyStarted = "RunSynchronously may not be called on a task that was already started.";
		/// <summary>RunSynchronously may not be called on a continuation task.</summary>
		internal static string @Task_RunSynchronously_Continuation = "RunSynchronously may not be called on a continuation task.";
		/// <summary>RunSynchronously may not be called on a task not bound to a delegate, such as the task returned from an asynchronous method.</summary>
		internal static string @Task_RunSynchronously_Promise = "RunSynchronously may not be called on a task not bound to a delegate, such as the task returned from an asynchronous method.";
		/// <summary>RunSynchronously may not be called on a task that has already completed.</summary>
		internal static string @Task_RunSynchronously_TaskCompleted = "RunSynchronously may not be called on a task that has already completed.";
		/// <summary>Start may not be called on a task that was already started.</summary>
		internal static string @Task_Start_AlreadyStarted = "Start may not be called on a task that was already started.";
		/// <summary>Start may not be called on a continuation task.</summary>
		internal static string @Task_Start_ContinuationTask = "Start may not be called on a continuation task.";
		/// <summary>Start may not be called on a promise-style task.</summary>
		internal static string @Task_Start_Promise = "Start may not be called on a promise-style task.";
		/// <summary>Start may not be called on a task that has completed.</summary>
		internal static string @Task_Start_TaskCompleted = "Start may not be called on a task that has completed.";
		/// <summary>The task has been disposed.</summary>
		internal static string @Task_ThrowIfDisposed = "The task has been disposed.";
		/// <summary>The tasks array included at least one null element.</summary>
		internal static string @Task_WaitMulti_NullTask = "The tasks array included at least one null element.";
		/// <summary>A task was canceled.</summary>
		internal static string @TaskCanceledException_ctor_DefaultMessage = "A task was canceled.";
		/// <summary>The exceptions collection was empty.</summary>
		internal static string @TaskCompletionSourceT_TrySetException_NoExceptions = "The exceptions collection was empty.";
		/// <summary>The exceptions collection included at least one null element.</summary>
		internal static string @TaskCompletionSourceT_TrySetException_NullException = "The exceptions collection included at least one null element.";
		/// <summary>A Task's exception(s) were not observed either by Waiting on the Task or accessing its Exception property. As a result, the unobserved exception was rethrown by the finalizer thread.</summary>
		internal static string @TaskExceptionHolder_UnhandledException = "A Task's exception(s) were not observed either by Waiting on the Task or accessing its Exception property. As a result, the unobserved exception was rethrown by the finalizer thread.";
		/// <summary>(Internal)Expected an Exception or an IEnumerable<Exception></summary>
		internal static string @TaskExceptionHolder_UnknownExceptionType = "(Internal)Expected an Exception or an IEnumerable<Exception>";
		/// <summary>ExecuteTask may not be called for a task which was previously queued to a different TaskScheduler.</summary>
		internal static string @TaskScheduler_ExecuteTask_WrongTaskScheduler = "ExecuteTask may not be called for a task which was previously queued to a different TaskScheduler.";
		/// <summary>The current SynchronizationContext may not be used as a TaskScheduler.</summary>
		internal static string @TaskScheduler_FromCurrentSynchronizationContext_NoCurrent = "The current SynchronizationContext may not be used as a TaskScheduler.";
		/// <summary>The TryExecuteTaskInline call to the underlying scheduler succeeded, but the task body was not invoked.</summary>
		internal static string @TaskScheduler_InconsistentStateAfterTryExecuteTaskInline = "The TryExecuteTaskInline call to the underlying scheduler succeeded, but the task body was not invoked.";
		/// <summary>An exception was thrown by a TaskScheduler.</summary>
		internal static string @TaskSchedulerException_ctor_DefaultMessage = "An exception was thrown by a TaskScheduler.";
		/// <summary>{Not yet computed}</summary>
		internal static string @TaskT_DebuggerNoResult = "{Not yet computed}";
		/// <summary>An attempt was made to transition a task to a final state when it had already completed.</summary>
		internal static string @TaskT_TransitionToFinal_AlreadyCompleted = "An attempt was made to transition a task to a final state when it had already completed.";
		/// <summary>Failed to set the specified COM apartment state. Current apartment state '{0}'.</summary>
		internal static string @Thread_ApartmentState_ChangeFailed = "Failed to set the specified COM apartment state. Current apartment state '{0}'.";
		/// <summary>Use CompressedStack.(Capture/Run) instead.</summary>
		internal static string @Thread_GetSetCompressedStack_NotSupported = "Use CompressedStack.(Capture/Run) instead.";
		/// <summary>This operation must be performed on the same thread as that represented by the Thread instance.</summary>
		internal static string @Thread_Operation_RequiresCurrentThread = "This operation must be performed on the same thread as that represented by the Thread instance.";
		/// <summary>The wait completed due to an abandoned mutex.</summary>
		internal static string @Threading_AbandonedMutexException = "The wait completed due to an abandoned mutex.";
		/// <summary>No handle of the given name exists.</summary>
		internal static string @Threading_WaitHandleCannotBeOpenedException = "No handle of the given name exists.";
		/// <summary>A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.</summary>
		internal static string @Threading_WaitHandleCannotBeOpenedException_InvalidHandle = "A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.";
		/// <summary>The WaitHandle cannot be signaled because it would exceed its maximum count.</summary>
		internal static string @Threading_WaitHandleTooManyPosts = "The WaitHandle cannot be signaled because it would exceed its maximum count.";
		/// <summary>Adding the specified count to the semaphore would cause it to exceed its maximum count.</summary>
		internal static string @Threading_SemaphoreFullException = "Adding the specified count to the semaphore would cause it to exceed its maximum count.";
		/// <summary>Thread was interrupted from a waiting state.</summary>
		internal static string @Threading_ThreadInterrupted = "Thread was interrupted from a waiting state.";
		/// <summary>The ThreadLocal object has been disposed.</summary>
		internal static string @ThreadLocal_Disposed = "The ThreadLocal object has been disposed.";
		/// <summary>ValueFactory attempted to access the Value property of this instance.</summary>
		internal static string @ThreadLocal_Value_RecursiveCallsToValue = "ValueFactory attempted to access the Value property of this instance.";
		/// <summary>The ThreadLocal object is not tracking values. To use the Values property, use a ThreadLocal constructor that accepts the trackAllValues parameter and set the parameter to true.</summary>
		internal static string @ThreadLocal_ValuesNotAvailable = "The ThreadLocal object is not tracking values. To use the Values property, use a ThreadLocal constructor that accepts the trackAllValues parameter and set the parameter to true.";
		/// <summary>The time zone ID '{0}' was not found on the local computer.</summary>
		internal static string @TimeZoneNotFound_MissingData = "The time zone ID '{0}' was not found on the local computer.";
		/// <summary>Type constructor threw an exception.</summary>
		internal static string @TypeInitialization_Default = "Type constructor threw an exception.";
		/// <summary>The type initializer for '{0}' threw an exception.</summary>
		internal static string @TypeInitialization_Type = "The type initializer for '{0}' threw an exception.";
		/// <summary>Could not resolve nested type '{0}' in type "{1}'.</summary>
		internal static string @TypeLoad_ResolveNestedType = "Could not resolve nested type '{0}' in type '{1}'.";
		/// <summary>Could not resolve type '{0}'.</summary>
		internal static string @TypeLoad_ResolveType = "Could not resolve type '{0}'.";
		/// <summary>Could not resolve type '{0}' in assembly '{1}'.</summary>
		internal static string @TypeLoad_ResolveTypeFromAssembly = "Could not resolve type '{0}' in assembly '{1}'.";
		/// <summary>Access to the path is denied.</summary>
		internal static string @UnauthorizedAccess_IODenied_NoPathName = "Access to the path is denied.";
		/// <summary>Access to the path '{0}' is denied.</summary>
		internal static string @UnauthorizedAccess_IODenied_Path = "Access to the path '{0}' is denied.";
		/// <summary>MemoryStream's internal buffer cannot be accessed.</summary>
		internal static string @UnauthorizedAccess_MemStreamBuffer = "MemoryStream's internal buffer cannot be accessed.";
		/// <summary>Access to the registry key '{0}' is denied.</summary>
		internal static string @UnauthorizedAccess_RegistryKeyGeneric_Key = "Access to the registry key '{0}' is denied.";
		/// <summary>Unknown error "{0}".</summary>
		internal static string @UnknownError_Num = "Unknown error '{0}'.";
		/// <summary>Operation could destabilize the runtime.</summary>
		internal static string @Verification_Exception = "Operation could destabilize the runtime.";
		/// <summary>at</summary>
		internal static string @Word_At = "at";
		/// <summary>---- DEBUG ASSERTION FAILED ----</summary>
		internal static string @DebugAssertBanner = "---- DEBUG ASSERTION FAILED ----";
		/// <summary>---- Assert Long Message ----</summary>
		internal static string @DebugAssertLongMessage = "---- Assert Long Message ----";
		/// <summary>---- Assert Short Message ----</summary>
		internal static string @DebugAssertShortMessage = "---- Assert Short Message ----";
		/// <summary>A read lock may not be acquired with the write lock held in this mode.</summary>
		internal static string @LockRecursionException_ReadAfterWriteNotAllowed = "A read lock may not be acquired with the write lock held in this mode.";
		/// <summary>Recursive read lock acquisitions not allowed in this mode.</summary>
		internal static string @LockRecursionException_RecursiveReadNotAllowed = "Recursive read lock acquisitions not allowed in this mode.";
		/// <summary>Recursive write lock acquisitions not allowed in this mode.</summary>
		internal static string @LockRecursionException_RecursiveWriteNotAllowed = "Recursive write lock acquisitions not allowed in this mode.";
		/// <summary>Recursive upgradeable lock acquisitions not allowed in this mode.</summary>
		internal static string @LockRecursionException_RecursiveUpgradeNotAllowed = "Recursive upgradeable lock acquisitions not allowed in this mode.";
		/// <summary>Write lock may not be acquired with read lock held. This pattern is prone to deadlocks. Please ensure that read locks are released before taking a write lock. If an upgrade is necessary, use an upgrade lock in place of the read lock.</summary>
		internal static string @LockRecursionException_WriteAfterReadNotAllowed = "Write lock may not be acquired with read lock held. This pattern is prone to deadlocks. Please ensure that read locks are released before taking a write lock. If an upgrade is necessary, use an upgrade lock in place of the read lock.";
		/// <summary>The upgradeable lock is being released without being held.</summary>
		internal static string @SynchronizationLockException_MisMatchedUpgrade = "The upgradeable lock is being released without being held.";
		/// <summary>The read lock is being released without being held.</summary>
		internal static string @SynchronizationLockException_MisMatchedRead = "The read lock is being released without being held.";
		/// <summary>The lock is being disposed while still being used. It either is being held by a thread and/or has active waiters waiting to acquire the lock.</summary>
		internal static string @SynchronizationLockException_IncorrectDispose = "The lock is being disposed while still being used. It either is being held by a thread and/or has active waiters waiting to acquire the lock.";
		/// <summary>Upgradeable lock may not be acquired with read lock held.</summary>
		internal static string @LockRecursionException_UpgradeAfterReadNotAllowed = "Upgradeable lock may not be acquired with read lock held.";
		/// <summary>Upgradeable lock may not be acquired with write lock held in this mode. Acquiring Upgradeable lock gives the ability to read along with an option to upgrade to a writer.</summary>
		internal static string @LockRecursionException_UpgradeAfterWriteNotAllowed = "Upgradeable lock may not be acquired with write lock held in this mode. Acquiring Upgradeable lock gives the ability to read along with an option to upgrade to a writer.";
		/// <summary>The write lock is being released without being held.</summary>
		internal static string @SynchronizationLockException_MisMatchedWrite = "The write lock is being released without being held.";
		/// <summary>This method is not supported on signature types.</summary>
		internal static string @NotSupported_SignatureType = "This method is not supported on signature types.";
		/// <summary>Release all references before disposing this instance.</summary>
		internal static string @Memory_OutstandingReferences = "Release all references before disposing this instance.";
		/// <summary>HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.</summary>
		internal static string @HashCode_HashCodeNotSupported = "HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.";
		/// <summary>HashCode is a mutable struct and should not be compared with other HashCodes.</summary>
		internal static string @HashCode_EqualityNotSupported = "HashCode is a mutable struct and should not be compared with other HashCodes.";
		/// <summary>Specified type is not supported</summary>
		internal static string @Arg_TypeNotSupported = "Specified type is not supported";
		/// <summary>The read operation returned an invalid length.</summary>
		internal static string @IO_InvalidReadLength = "The read operation returned an invalid length.";
		/// <summary>Basepath argument is not fully qualified.</summary>
		internal static string @Arg_BasePathNotFullyQualified = "Basepath argument is not fully qualified.";
		/// <summary>Number of elements in source vector is greater than the destination array</summary>
		internal static string @Arg_ElementsInSourceIsGreaterThanDestination = "Number of elements in source vector is greater than the destination array";
		/// <summary>The method was called with a null array argument.</summary>
		internal static string @Arg_NullArgumentNullRef = "The method was called with a null array argument.";
		/// <summary>Abstract methods cannot be prepared.</summary>
		internal static string @Argument_CannotPrepareAbstract = "Abstract methods cannot be prepared.";
		/// <summary>The given generic instantiation was invalid.</summary>
		internal static string @Argument_InvalidGenericInstantiation = "The given generic instantiation was invalid.";
		/// <summary>Overlapping spans have mismatching alignment.</summary>
		internal static string @Argument_OverlapAlignmentMismatch = "Overlapping spans have mismatching alignment.";
		/// <summary>At least {0} element(s) are expected in the parameter "{1}".</summary>
		internal static string @Arg_InsufficientNumberOfElements = "At least {0} element(s) are expected in the parameter '{1}'.";
		/// <summary>The string must be null-terminated.</summary>
		internal static string @Arg_MustBeNullTerminatedString = "The string must be null-terminated.";
		/// <summary>The week parameter must be in the range 1 through 53.</summary>
		internal static string @ArgumentOutOfRange_Week_ISO = "The week parameter must be in the range 1 through 53.";
		/// <summary>PInvoke methods must be static and native and cannot be abstract.</summary>
		internal static string @Argument_BadPInvokeMethod = "PInvoke methods must be static and native and cannot be abstract.";
		/// <summary>PInvoke methods cannot exist on interfaces.</summary>
		internal static string @Argument_BadPInvokeOnInterface = "PInvoke methods cannot exist on interfaces.";
		/// <summary>Method has been already defined.</summary>
		internal static string @Argument_MethodRedefined = "Method has been already defined.";
		/// <summary>Cannot extract a Unicode scalar value from the specified index in the input.</summary>
		internal static string @Argument_CannotExtractScalar = "Cannot extract a Unicode scalar value from the specified index in the input.";
		/// <summary>Characters following the format symbol must be a number of {0} or less.</summary>
		internal static string @Argument_CannotParsePrecision = "Characters following the format symbol must be a number of {0} or less.";
		/// <summary>The 'G' format combined with a precision is not supported.</summary>
		internal static string @Argument_GWithPrecisionNotSupported = "The 'G' format combined with a precision is not supported.";
		/// <summary>Precision cannot be larger than {0}.</summary>
		internal static string @Argument_PrecisionTooLarge = "Precision cannot be larger than {0}.";
		/// <summary>Cannot load hostpolicy library. AssemblyDependencyResolver is currently only supported if the runtime is hosted through hostpolicy library.</summary>
		internal static string @AssemblyDependencyResolver_FailedToLoadHostpolicy = "Cannot load hostpolicy library. AssemblyDependencyResolver is currently only supported if the runtime is hosted through hostpolicy library.";
		/// <summary>Dependency resolution failed for component {0} with error code {1}. Detailed error: {2}</summary>
		internal static string @AssemblyDependencyResolver_FailedToResolveDependencies = "Dependency resolution failed for component {0} with error code {1}. Detailed error: {2}";
		/// <summary>The supplied object does not implement ICloneable.</summary>
		internal static string @Arg_EnumNotCloneable = "The supplied object does not implement ICloneable.";
		/// <summary>The returned enumerator does not implement IEnumVARIANT.</summary>
		internal static string @InvalidOp_InvalidNewEnumVariant = "The returned enumerator does not implement IEnumVARIANT.";
		/// <summary>sysctl {0} failed with {1} error.</summary>
		internal static string @InvalidSysctl = "sysctl {0} failed with {1} error.";
		/// <summary>Array size exceeds addressing limitations.</summary>
		internal static string @Argument_StructArrayTooLarge = "Array size exceeds addressing limitations.";
		/// <summary>ArrayWithOffset: offset exceeds array size.</summary>
		internal static string @IndexOutOfRange_ArrayWithOffset = "ArrayWithOffset: offset exceeds array size.";
		/// <summary>An action was attempted during deserialization that could lead to a security vulnerability. The action has been aborted.</summary>
		internal static string @Serialization_DangerousDeserialization = "An action was attempted during deserialization that could lead to a security vulnerability. The action has been aborted.";
		/// <summary>An action was attempted during deserialization that could lead to a security vulnerability. The action has been aborted. To allow the action, set the '{0}' AppContext switch to true.</summary>
		internal static string @Serialization_DangerousDeserialization_Switch = "An action was attempted during deserialization that could lead to a security vulnerability. The action has been aborted. To allow the action, set the '{0}' AppContext switch to true.";
		/// <summary>The startup hook simple assembly name '{0}' is invalid. It must be a valid assembly name and it may not contain directory separator, space or comma characters and must not end with '.dll'.</summary>
		internal static string @Argument_InvalidStartupHookSimpleAssemblyName = "The startup hook simple assembly name '{0}' is invalid. It must be a valid assembly name and it may not contain directory separator, space or comma characters and must not end with '.dll'.";
		/// <summary>Startup hook assembly '{0}' failed to load. See inner exception for details.</summary>
		internal static string @Argument_StartupHookAssemblyLoadFailed = "Startup hook assembly '{0}' failed to load. See inner exception for details.";
		/// <summary>COM register function must be static.</summary>
		internal static string @InvalidOperation_NonStaticComRegFunction = "COM register function must be static.";
		/// <summary>COM unregister function must be static.</summary>
		internal static string @InvalidOperation_NonStaticComUnRegFunction = "COM unregister function must be static.";
		/// <summary>COM register function must have a System.Type parameter and a void return type.</summary>
		internal static string @InvalidOperation_InvalidComRegFunctionSig = "COM register function must have a System.Type parameter and a void return type.";
		/// <summary>COM unregister function must have a System.Type parameter and a void return type.</summary>
		internal static string @InvalidOperation_InvalidComUnRegFunctionSig = "COM unregister function must have a System.Type parameter and a void return type.";
		/// <summary>The handle is invalid.</summary>
		internal static string @InvalidOperation_InvalidHandle = "The handle is invalid.";
		/// <summary>Type '{0}' has more than one COM registration function.</summary>
		internal static string @InvalidOperation_MultipleComRegFunctions = "Type '{0}' has more than one COM registration function.";
		/// <summary>Type '{0}' has more than one COM unregistration function.</summary>
		internal static string @InvalidOperation_MultipleComUnRegFunctions = "Type '{0}' has more than one COM unregistration function.";
		/// <summary>Attempt to update previously set global instance.</summary>
		internal static string @InvalidOperation_ResetGlobalComWrappersInstance = "Attempt to update previously set global instance.";
		/// <summary>Attempt to update previously set Objective-C msgSend API overrides.</summary>
		internal static string @InvalidOperation_ResetGlobalObjectiveCMsgSend = "Attempt to update previously set Objective-C msgSend API overrides.";
		/// <summary>Attempt to track an Objective-C Type without initializing.</summary>
		internal static string @InvalidOperation_ObjectiveCMarshalNotInitialized = "Attempt to track an Objective-C Type without initializing.";
		/// <summary>Attempt to reinitialize ObjectiveCMarshal.</summary>
		internal static string @InvalidOperation_ReinitializeObjectiveCMarshal = "Attempt to reinitialize ObjectiveCMarshal.";
		/// <summary>Attempt to track an Objective-C Type without a finalizer.</summary>
		internal static string @InvalidOperation_ObjectiveCTypeNoFinalizer = "Attempt to track an Objective-C Type without a finalizer.";
		/// <summary>Supplying a non-null inner should also be marked as Aggregated.</summary>
		internal static string @InvalidOperation_SuppliedInnerMustBeMarkedAggregation = "Supplying a non-null inner should also be marked as Aggregated.";
		/// <summary>Length of items must be same as length of keys.</summary>
		internal static string @Argument_SpansMustHaveSameLength = "Length of items must be same as length of keys.";
		/// <summary>Cannot write to a BufferedStream while the read buffer is not empty if the underlying stream is not seekable. Ensure that the stream underlying this BufferedStream can seek or avoid interleaving read and write operations on this BufferedStream.</summary>
		internal static string @NotSupported_CannotWriteToBufferedStreamIfReadBufferCannotBeFlushed = "Cannot write to a BufferedStream while the read buffer is not empty if the underlying stream is not seekable. Ensure that the stream underlying this BufferedStream can seek or avoid interleaving read and write operations on this BufferedStream.";
		/// <summary>Found invalid data while decoding.</summary>
		internal static string @GenericInvalidData = "Found invalid data while decoding.";
		/// <summary>Resource type in the ResourceScope enum is going from a more restrictive resource type to a more general one. From: "{0}" To: "{1}"</summary>
		internal static string @Argument_ResourceScopeWrongDirection = "Resource type in the ResourceScope enum is going from a more restrictive resource type to a more general one. From: '{0}' To: '{1}'";
		/// <summary>The type parameter cannot be null when scoping the resource's visibility to Private or Assembly.</summary>
		internal static string @ArgumentNull_TypeRequiredByResourceScope = "The type parameter cannot be null when scoping the resource's visibility to Private or Assembly.";
		/// <summary>Unknown value for the ResourceScope: {0} Too many resource type bits may be set.</summary>
		internal static string @Argument_BadResourceScopeTypeBits = "Unknown value for the ResourceScope: {0} Too many resource type bits may be set.";
		/// <summary>Unknown value for the ResourceScope: {0} Too many resource visibility bits may be set.</summary>
		internal static string @Argument_BadResourceScopeVisibilityBits = "Unknown value for the ResourceScope: {0} Too many resource visibility bits may be set.";
		/// <summary>The value cannot be an empty string.</summary>
		internal static string @Argument_EmptyString = "The value cannot be an empty string.";
		/// <summary>FrameworkName is invalid.</summary>
		internal static string @Argument_FrameworkNameInvalid = "FrameworkName is invalid.";
		/// <summary>FrameworkName version component is invalid.</summary>
		internal static string @Argument_FrameworkNameInvalidVersion = "FrameworkName version component is invalid.";
		/// <summary>FrameworkName version component is missing.</summary>
		internal static string @Argument_FrameworkNameMissingVersion = "FrameworkName version component is missing.";
		/// <summary>FrameworkName cannot have less than two components or more than three components.</summary>
		internal static string @Argument_FrameworkNameTooShort = "FrameworkName cannot have less than two components or more than three components.";
		/// <summary>Non-exhaustive switch expression failed to match its input.</summary>
		internal static string @Arg_SwitchExpressionException = "Non-exhaustive switch expression failed to match its input.";
		/// <summary>Attempted to marshal an object across a context boundary.</summary>
		internal static string @Arg_ContextMarshalException = "Attempted to marshal an object across a context boundary.";
		/// <summary>Attempted to access an unloaded AppDomain.</summary>
		internal static string @Arg_AppDomainUnloadedException = "Attempted to access an unloaded AppDomain.";
		/// <summary>Unmatched value was {0}.</summary>
		internal static string @SwitchExpressionException_UnmatchedValue = "Unmatched value was {0}.";
		/// <summary>Support for UTF-7 is disabled. See {0} for more information.</summary>
		internal static string @Encoding_UTF7_Disabled = "Support for UTF-7 is disabled. See {0} for more information.";
		/// <summary>Type '{0}' returned by IDynamicInterfaceCastable does not implement the requested interface '{1}'.</summary>
		internal static string @IDynamicInterfaceCastable_DoesNotImplementRequested = "Type '{0}' returned by IDynamicInterfaceCastable does not implement the requested interface '{1}'.";
		/// <summary>Type '{0}' returned by IDynamicInterfaceCastable does not have the attribute '{1}'.</summary>
		internal static string @IDynamicInterfaceCastable_MissingImplementationAttribute = "Type '{0}' returned by IDynamicInterfaceCastable does not have the attribute '{1}'.";
		/// <summary>Type '{0}' returned by IDynamicInterfaceCastable is not an interface.</summary>
		internal static string @IDynamicInterfaceCastable_NotInterface = "Type '{0}' returned by IDynamicInterfaceCastable is not an interface.";
		/// <summary>Object must be of type Half.</summary>
		internal static string @Arg_MustBeHalf = "Object must be of type Half.";
		/// <summary>Object must be of type Rune.</summary>
		internal static string @Arg_MustBeRune = "Object must be of type Rune.";
		/// <summary>BinaryFormatter serialization and deserialization are disabled within this application. See https://aka.ms/binaryformatter for more information.</summary>
		internal static string @BinaryFormatter_SerializationDisallowed = "BinaryFormatter serialization and deserialization are disabled within this application. See https://aka.ms/binaryformatter for more information.";
		/// <summary>CodeBase is not supported on assemblies loaded from a single-file bundle.</summary>
		internal static string @NotSupported_CodeBase = "CodeBase is not supported on assemblies loaded from a single-file bundle.";
		/// <summary>Cannot dynamically create an instance of type '{0}'. Reason: {1}</summary>
		internal static string @Activator_CannotCreateInstance = "Cannot dynamically create an instance of type '{0}'. Reason: {1}";
		/// <summary>The argv[0] argument cannot include a double quote.</summary>
		internal static string @Argv_IncludeDoubleQuote = "The argv[0] argument cannot include a double quote.";
		/// <summary>Use of ResourceManager for custom types is disabled. Set the MSBuild Property CustomResourceTypesSupport to true in order to enable it.</summary>
		internal static string @ResourceManager_ReflectionNotAllowed = "Use of ResourceManager for custom types is disabled. Set the MSBuild Property CustomResourceTypesSupport to true in order to enable it.";
		/// <summary>The assembly can not be edited or changed.</summary>
		internal static string @InvalidOperation_AssemblyNotEditable = "The assembly can not be edited or changed.";
		/// <summary>The assembly update failed.</summary>
		internal static string @InvalidOperation_EditFailed = "The assembly update failed.";
		/// <summary>Assembly updates cannot be applied while a debugger is attached.</summary>
		internal static string @NotSupported_DebuggerAttached = "Assembly updates cannot be applied while a debugger is attached.";
		/// <summary>Method body replacement not supported in this runtime.</summary>
		internal static string @NotSupported_MethodBodyReplacement = "Method body replacement not supported in this runtime.";
		/// <summary>Built-in COM has been disabled via a feature switch. See https://aka.ms/dotnet-illink/com for more information.</summary>
		internal static string @NotSupported_COM = "Built-in COM has been disabled via a feature switch. See https://aka.ms/dotnet-illink/com for more information.";
		/// <summary>Queue empty.</summary>
		internal static string @InvalidOperation_EmptyQueue = "Queue empty.";
		/// <summary>The target file '{0}' is a directory, not a file.</summary>
		internal static string @Arg_FileIsDirectory_Name = "The target file '{0}' is a directory, not a file.";
		/// <summary>Invalid File or Directory attributes value.</summary>
		internal static string @Arg_InvalidFileAttrs = "Invalid File or Directory attributes value.";
		/// <summary>Second path fragment must not be a drive or UNC name.</summary>
		internal static string @Arg_Path2IsRooted = "Second path fragment must not be a drive or UNC name.";
		/// <summary>Path must not be a drive.</summary>
		internal static string @Arg_PathIsVolume = "Path must not be a drive.";
		/// <summary>The stream's length cannot be changed.</summary>
		internal static string @Argument_FileNotResized = "The stream's length cannot be changed.";
		/// <summary>The directory specified, '{0}', is not a subdirectory of '{1}'.</summary>
		internal static string @Argument_InvalidSubPath = "The directory specified, '{0}', is not a subdirectory of '{1}'.";
		/// <summary>The specified directory '{0}' cannot be created.</summary>
		internal static string @IO_CannotCreateDirectory = "The specified directory '{0}' cannot be created.";
		/// <summary>The source '{0}' and destination '{1}' are the same file.</summary>
		internal static string @IO_CannotReplaceSameFile = "The source '{0}' and destination '{1}' are the same file.";
		/// <summary>The specified path '{0}' is not a file.</summary>
		internal static string @IO_NotAFile = "The specified path '{0}' is not a file.";
		/// <summary>Source and destination path must be different.</summary>
		internal static string @IO_SourceDestMustBeDifferent = "Source and destination path must be different.";
		/// <summary>Source and destination path must have identical roots. Move will not work across volumes.</summary>
		internal static string @IO_SourceDestMustHaveSameRoot = "Source and destination path must have identical roots. Move will not work across volumes.";
		/// <summary>Synchronous operations should not be performed on the UI thread. Consider wrapping this method in Task.Run.</summary>
		internal static string @IO_SyncOpOnUIThread = "Synchronous operations should not be performed on the UI thread. Consider wrapping this method in Task.Run.";
		/// <summary>Probable I/O race condition detected while copying memory. The I/O package is not thread safe by default. In multithreaded applications, a stream must be accessed in a thread-safe way, such as a thread-safe wrapper returned by TextReader's or TextWriter's Synchronized methods. This also applies to classes like StreamWriter and StreamReader.</summary>
		internal static string @IndexOutOfRange_IORaceCondition = "Probable I/O race condition detected while copying memory. The I/O package is not thread safe by default. In multithreaded applications, a stream must be accessed in a thread-safe way, such as a thread-safe wrapper returned by TextReader's or TextWriter's Synchronized methods. This also applies to classes like StreamWriter and StreamReader.";
		/// <summary>File encryption is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_FileEncryption = "File encryption is not supported on this platform.";
		/// <summary>A MemberInfo that matches '{0}' could not be found.</summary>
		internal static string @Arg_MemberInfoNotFound = "A MemberInfo that matches '{0}' could not be found.";
		/// <summary>Bad magic in '{0}': Header starts with '{1}' instead of 'tzdata'</summary>
		internal static string @InvalidOperation_BadTZHeader = "Bad magic in '{0}': Header starts with '{1}' instead of 'tzdata'";
		/// <summary>Unable to fully read from file '{0}' at offset {1} length {2}; read {3} bytes expected {4}.</summary>
		internal static string @InvalidOperation_ReadTZError = "Unable to fully read from file '{0}' at offset {1} length {2}; read {3} bytes expected {4}.";
		/// <summary>Length in index file less than AndroidTzDataHeader</summary>
		internal static string @InvalidOperation_BadIndexLength = "Length in index file less than AndroidTzDataHeader";
		/// <summary>Unable to properly load any time zone data files.</summary>
		internal static string @TimeZoneNotFound_ValidTimeZoneFileMissing = "Unable to properly load any time zone data files.";
		/// <summary>NullabilityInfoContext is not supported in the current application because 'System.Reflection.NullabilityInfoContext.IsSupported' is set to false. Set the MSBuild Property 'NullabilityInfoContextSupport' to true in order to enable it.</summary>
		internal static string @NullabilityInfoContext_NotSupported = "NullabilityInfoContext is not supported in the current application because 'System.Reflection.NullabilityInfoContext.IsSupported' is set to false. Set the MSBuild Property 'NullabilityInfoContextSupport' to true in order to enable it.";
		/// <summary>Thread is running or terminated; it cannot restart.</summary>
		internal static string @ThreadState_AlreadyStarted = "Thread is running or terminated; it cannot restart.";
		/// <summary>Thread has not been started.</summary>
		internal static string @ThreadState_NotStarted = "Thread has not been started.";
		/// <summary>Thread is dead; priority cannot be accessed.</summary>
		internal static string @ThreadState_Dead_Priority = "Thread is dead; priority cannot be accessed.";
		/// <summary>Thread is dead; state cannot be accessed.</summary>
		internal static string @ThreadState_Dead_State = "Thread is dead; state cannot be accessed.";
		/// <summary>Unable to set thread priority.</summary>
		internal static string @ThreadState_SetPriorityFailed = "Unable to set thread priority.";
		/// <summary>The target method returned a null reference.</summary>
		internal static string @NullReference_InvokeNullRefReturned = "The target method returned a null reference.";
		/// <summary>Cannot create an instance of {0} as it is an open type.</summary>
		internal static string @Arg_OpenType = "Cannot create an instance of {0} as it is an open type.";
		/// <summary>lohSize can't be greater than totalSize</summary>
		internal static string @ArgumentOutOfRange_NoGCLohSizeGreaterTotalSize = "lohSize can't be greater than totalSize";
		/// <summary>totalSize is too large. For more information about setting the maximum size, see "Latency Modes" in http://go.microsoft.com/fwlink/?LinkId=522706.</summary>
		internal static string @ArgumentOutOfRangeException_NoGCRegionSizeTooLarge = "totalSize is too large. For more information about setting the maximum size, see 'Latency Modes' in http://go.microsoft.com/fwlink/?LinkId=522706.";
		/// <summary>The NoGCRegion mode was already in progress.</summary>
		internal static string @InvalidOperationException_AlreadyInNoGCRegion = "The NoGCRegion mode was already in progress.";
		/// <summary>Allocated memory exceeds specified memory for NoGCRegion mode.</summary>
		internal static string @InvalidOperationException_NoGCRegionAllocationExceeded = "Allocated memory exceeds specified memory for NoGCRegion mode.";
		/// <summary>Garbage collection was induced in NoGCRegion mode.</summary>
		internal static string @InvalidOperationException_NoGCRegionInduced = "Garbage collection was induced in NoGCRegion mode.";
		/// <summary>NoGCRegion mode must be set.</summary>
		internal static string @InvalidOperationException_NoGCRegionNotInProgress = "NoGCRegion mode must be set.";
		/// <summary>Dynamic code generation is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_ReflectionEmit = "Dynamic code generation is not supported on this platform.";
		/// <summary>AssemblyName.GetAssemblyName() is not supported on this platform.</summary>
		internal static string @PlatformNotSupported_AssemblyName_GetAssemblyName = "AssemblyName.GetAssemblyName() is not supported on this platform.";
		/// <summary>Arrays with non-zero lower bounds are not supported.</summary>
		internal static string @PlatformNotSupported_NonZeroLowerBound = "Arrays with non-zero lower bounds are not supported.";
		/// <summary>The body of this method was removed by the AOT compiler because it's not callable.</summary>
		internal static string @NotSupported_BodyRemoved = "The body of this method was removed by the AOT compiler because it's not callable.";
		/// <summary>The feature associated with this method was removed.</summary>
		internal static string @NotSupported_FeatureBodyRemoved = "The feature associated with this method was removed.";
		/// <summary>The body of this instance method was removed by the AOT compiler. This can happen if the owning type was not seen as allocated by the AOT compiler.</summary>
		internal static string @NotSupported_InstanceBodyRemoved = "The body of this instance method was removed by the AOT compiler. This can happen if the owning type was not seen as allocated by the AOT compiler.";
		/// <summary>Attempted to load a type that was not created during ahead of time compilation.</summary>
		internal static string @Arg_UnavailableTypeLoadException = "Attempted to load a type that was not created during ahead of time compilation.";
		/// <summary>A type initializer threw an exception. To determine which type, inspect the InnerException's StackTrace property.</summary>
		internal static string @TypeInitialization_Type_NoTypeAvailable = "A type initializer threw an exception. To determine which type, inspect the InnerException's StackTrace property.";
		/// <summary>The given assembly name or codebase was invalid</summary>
		internal static string @InvalidAssemblyName = "The given assembly name or codebase was invalid";
		/// <summary>Invalid assembly public key.</summary>
		internal static string @Security_InvalidAssemblyPublicKey = "Invalid assembly public key.";
		/// <summary>Could not load type '{0}' from assembly '{1}'.</summary>
		internal static string @ClassLoad_General = "Could not load type '{0}' from assembly '{1}'.";
		/// <summary>'{0}' from assembly '{1}' has too many dimensions.</summary>
		internal static string @ClassLoad_RankTooLarge = "'{0}' from assembly '{1}' has too many dimensions.";
		/// <summary>Could not load type '{0}' from assembly '{1}' because generic types cannot have explicit layout.</summary>
		internal static string @ClassLoad_ExplicitGeneric = "Could not load type '{0}' from assembly '{1}' because generic types cannot have explicit layout.";
		/// <summary>Could not load type '{0}' from assembly '{1}' because the format is invalid.</summary>
		internal static string @ClassLoad_BadFormat = "Could not load type '{0}' from assembly '{1}' because the format is invalid.";
		/// <summary>Array of type '{0}' from assembly '{1}' cannot be created because base value type is too large.</summary>
		internal static string @ClassLoad_ValueClassTooLarge = "Array of type '{0}' from assembly '{1}' cannot be created because base value type is too large.";
		/// <summary>Could not load type '{0}' from assembly '{1}' because it contains an object field at offset '{2}' that is incorrectly aligned or overlapped by a non-object field.</summary>
		internal static string @ClassLoad_ExplicitLayout = "Could not load type '{0}' from assembly '{1}' because it contains an object field at offset '{2}' that is incorrectly aligned or overlapped by a non-object field.";
		/// <summary>Method not found: '{0}'.</summary>
		internal static string @EE_MissingMethod = "Method not found: '{0}'.";
		/// <summary>Field not found: '{0}'.</summary>
		internal static string @EE_MissingField = "Field not found: '{0}'.";
		/// <summary>Common Language Runtime detected an invalid program. The body of method '{0}' is invalid.</summary>
		internal static string @InvalidProgram_Specific = "Common Language Runtime detected an invalid program. The body of method '{0}' is invalid.";
		/// <summary>Method '{0}' has a variable argument list. Variable argument lists are not supported in .NET Core.</summary>
		internal static string @InvalidProgram_Vararg = "Method '{0}' has a variable argument list. Variable argument lists are not supported in .NET Core.";
		/// <summary>Object.Finalize() can not be called directly. It is only callable by the runtime.</summary>
		internal static string @InvalidProgram_CallVirtFinalize = "Object.Finalize() can not be called directly. It is only callable by the runtime.";
		/// <summary>The corresponding delegate has been garbage collected. Please make sure the delegate is still referenced by managed code when you are using the marshalled native function pointer.</summary>
		internal static string @Delegate_GarbageCollected = "The corresponding delegate has been garbage collected. Please make sure the delegate is still referenced by managed code when you are using the marshalled native function pointer.";
		/// <summary>Unable to find an entry point named '{0}' in native library '{1}'.</summary>
		internal static string @Arg_EntryPointNotFoundExceptionParameterized = "Unable to find an entry point named '{0}' in native library '{1}'.";
		/// <summary>Unable to find an entry point named '{0}' in native library.</summary>
		internal static string @Arg_EntryPointNotFoundExceptionParameterizedNoLibrary = "Unable to find an entry point named '{0}' in native library.";
		/// <summary>Unable to native library '{0}' or one of its dependencies.</summary>
		internal static string @Arg_DllNotFoundExceptionParameterized = "Unable to native library '{0}' or one of its dependencies.";
		/// <summary>COM Interop requires ComWrapper instance registered for marshalling.</summary>
		internal static string @InvalidOperation_ComInteropRequireComWrapperInstance = "COM Interop requires ComWrapper instance registered for marshalling.";
	}
}