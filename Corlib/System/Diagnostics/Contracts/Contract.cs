using System;
using System.Collections.Generic;
using System.Text;

namespace System.Diagnostics.Contracts
{
    #region Attributes

    /// <summary>
    /// Methods and classes marked with this attribute can be used within calls to Contract methods. Such methods not make any visible state changes.
    /// </summary>
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate | AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class PureAttribute : Attribute
    {
        private readonly bool _sophisticated;

        public PureAttribute()
        {
        }

        public PureAttribute(bool sophisticated)
        {
            _sophisticated = sophisticated;
        }

        public bool Sophisticated
        {
            get => _sophisticated;
        }
    }

    /// <summary>
    /// Types marked with this attribute specify that a separate type contains the contracts for this type.
    /// </summary>
    [Conditional("CONTRACTS_FULL")]
    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public sealed class ContractClassAttribute : Attribute
    {
        private readonly Type _typeWithContracts;

        public ContractClassAttribute(Type typeContainingContracts)
        {
            _typeWithContracts = typeContainingContracts;
        }

        public Type TypeContainingContracts
        {
            get { return _typeWithContracts; }
        }
    }

    /// <summary>
    /// Types marked with this attribute specify that they are a contract for the type that is the argument of the constructor.
    /// </summary>
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ContractClassForAttribute : Attribute
    {
        private readonly Type _typeIAmAContractFor;

        public ContractClassForAttribute(Type typeContractsAreFor)
        {
            _typeIAmAContractFor = typeContractsAreFor;
        }

        public Type TypeContractsAreFor
        {
            get { return _typeIAmAContractFor; }
        }
    }

    /// <summary>
    /// This attribute is used to mark a method as being the invariant
    /// method for a class. The method can have any name, but it must
    /// return "void" and take no parameters. The body of the method
    /// must consist solely of one or more calls to the method
    /// Contract.Invariant. A suggested name for the method is
    /// "ObjectInvariant".
    /// </summary>
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ContractInvariantMethodAttribute : Attribute { }

    /// <summary>
    /// Attribute that specifies that an assembly is a reference assembly with contracts.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class ContractReferenceAssemblyAttribute : Attribute { }

    /// <summary>
    /// Methods (and properties) marked with this attribute can be used within calls to Contract methods, but have no runtime behavior associated with them.
    /// </summary>
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ContractRuntimeIgnoredAttribute : Attribute { }

    /// <summary>
    /// Instructs downstream tools whether to assume the correctness of this assembly, type or member without performing any verification or not.
    /// Can use [ContractVerification(false)] to explicitly mark assembly, type or member as one to *not* have verification performed on it.
    /// Most specific element found (member, type, then assembly) takes precedence.
    /// (That is useful if downstream tools allow a user to decide which polarity is the default, unmarked case.)
    /// </summary>
    /// <remarks>
    /// Apply this attribute to a type to apply to all members of the type, including nested types.
    /// Apply this attribute to an assembly to apply to all types and members of the assembly.
    /// Apply this attribute to a property to apply to both the getter and setter.
    /// </remarks>
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property)]
    public sealed class ContractVerificationAttribute : Attribute
    {
        private readonly bool _value;

        public ContractVerificationAttribute(bool value)
        {
            _value = value;
        }

        public bool Value
        {
            get { return _value; }
        }
    }

    /// <summary>
    /// Allows a field f to be used in the method contracts for a method m when f has less visibility than m.
    /// For instance, if the method is public, but the field is private.
    /// </summary>
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ContractPublicPropertyNameAttribute : Attribute
    {
        private readonly String _publicName;

        public ContractPublicPropertyNameAttribute(String name)
        {
            _publicName = name;
        }

        public String Name
        {
            get { return _publicName; }
        }
    }

    /// <summary>
    /// Enables factoring legacy if-then-throw into separate methods for reuse and full control over
    /// thrown exception and arguments
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Conditional("CONTRACTS_FULL")]
    public sealed class ContractArgumentValidatorAttribute : Attribute { }

    /// <summary>
    /// Enables writing abbreviations for contracts that get copied to other methods
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Conditional("CONTRACTS_FULL")]
    public sealed class ContractAbbreviatorAttribute : Attribute { }

    /// <summary>
    /// Allows setting contract and tool options at assembly, type, or method granularity.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    [Conditional("CONTRACTS_FULL")]
    public sealed class ContractOptionAttribute : Attribute
    {
        private readonly string _category;
        private readonly string _setting;
        private readonly bool _enabled;
        private readonly string _value;

        public ContractOptionAttribute(string category, string setting, bool enabled)
        {
            _category = category;
            _setting = setting;
            _enabled = enabled;
        }

        public ContractOptionAttribute(string category, string setting, string value)
        {
            _category = category;
            _setting = setting;
            _value = value;
        }

        public string Category
        {
            get { return _category; }
        }

        public string Setting
        {
            get { return _setting; }
        }

        public bool Enabled
        {
            get { return _enabled; }
        }

        public string Value
        {
            get { return _value; }
        }
    }

    #endregion Attributes

    public static partial class Contract
    {
        #region User Methods

        #region Assume

        [Pure]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Assume(bool condition)
        {
            if (!condition)
            {
                ReportFailure(ContractFailureKind.Assume, null, null, null);
            }
        }

        [Pure]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Assume(bool condition, String userMessage)
        {
            if (!condition)
            {
                ReportFailure(ContractFailureKind.Assume, userMessage, null, null);
            }
        }

        #endregion Assume

        #region Assert

        [Pure]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Assert(bool condition)
        {
            if (!condition)
                ReportFailure(ContractFailureKind.Assert, null, null, null);
        }

        [Pure]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Assert(bool condition, String userMessage)
        {
            if (!condition)
                ReportFailure(ContractFailureKind.Assert, userMessage, null, null);
        }

        #endregion Assert

        #region Requires

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        public static void Requires(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        public static void Requires(bool condition, String userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
        }

        [Pure]
        public static void Requires<TException>(bool condition) where TException : Exception
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
        }

        [Pure]
        public static void Requires<TException>(bool condition, String userMessage) where TException : Exception
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
        }

        #endregion Requires

        #region Ensures

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        public static void Ensures(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        public static void Ensures(bool condition, String userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        public static void EnsuresOnThrow<TException>(bool condition) where TException : Exception
        {
            AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        public static void EnsuresOnThrow<TException>(bool condition, String userMessage) where TException : Exception
        {
            AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
        }

        #region Old, Result, and Out Parameters

        [Pure]
        public static T Result<T>() { return default(T); }

        [Pure]
        public static T ValueAtReturn<T>(out T value) { value = default(T); return value; }

        [Pure]
        public static T OldValue<T>(T value) { return default(T); }

        #endregion Old, Result, and Out Parameters

        #endregion Ensures

        #region Invariant

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        public static void Invariant(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        public static void Invariant(bool condition, String userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
        }

        #endregion Invariant

        #region Quantifiers

        #region ForAll

        [Pure]
        public static bool ForAll(int fromInclusive, int toExclusive, Predicate<int> predicate)
        {
            if (fromInclusive > toExclusive)
                throw new ArgumentException("fromInclusive must be less than or equal to toExclusive.");

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Contract.EndContractBlock();

            for (int i = fromInclusive; i < toExclusive; i++)
                if (!predicate(i)) return false;
            return true;
        }

        [Pure]
        public static bool ForAll<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Contract.EndContractBlock();

            foreach (T t in collection)
                if (!predicate(t)) return false;
            return true;
        }

        #endregion ForAll

        #region Exists

        [Pure]
        public static bool Exists(int fromInclusive, int toExclusive, Predicate<int> predicate)
        {
            if (fromInclusive > toExclusive)
                throw new ArgumentException("fromInclusive must be less than or equal to toExclusive.");

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Contract.EndContractBlock();

            for (int i = fromInclusive; i < toExclusive; i++)
                if (predicate(i)) return true;
            return false;
        }

        [Pure]
        public static bool Exists<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Contract.EndContractBlock();

            foreach (T t in collection)
                if (predicate(t)) return true;
            return false;
        }

        #endregion Exists

        #endregion Quantifiers

        #region Pointers

        private static readonly ulong MaxWritableExtent = (UIntPtr.Size == 4) ? UInt32.MaxValue : UInt64.MaxValue;

        [CLSCompliant(false)]
        [Pure]
        [ContractRuntimeIgnored]
        public static ulong WritableBytes(UIntPtr startAddress) { return MaxWritableExtent - startAddress.ToUInt64(); }

        [CLSCompliant(false)]
        [Pure]
        [ContractRuntimeIgnored]
        public static ulong WritableBytes(IntPtr startAddress) { return MaxWritableExtent - (ulong)startAddress; }

        [CLSCompliant(false)]
        [Pure]
        [ContractRuntimeIgnored]
        unsafe public static ulong WritableBytes(void* startAddress) { return MaxWritableExtent - (ulong)startAddress; }

        [CLSCompliant(false)]
        [Pure]
        [ContractRuntimeIgnored]
        public static ulong ReadableBytes(UIntPtr startAddress) { return MaxWritableExtent - startAddress.ToUInt64(); }

        [CLSCompliant(false)]
        [Pure]
        [ContractRuntimeIgnored]
        public static ulong ReadableBytes(IntPtr startAddress) { return MaxWritableExtent - (ulong)startAddress; }

        //NOT IMPLEMENTED BECAUSE UNSAFE:
        /*
		 * [CLSCompliant(false)]
         * [Pure]
         * [ContractRuntimeIgnored]
         * [SecurityCritical]
		 * unsafe public static ulong ReadableBytes(void* startAddress) { return MaxWritableExtent - (ulong)startAddress; }
		 */

        #endregion Pointers

        #region Misc.

        /// <summary>
        /// Marker to indicate the end of the contract section of a method.
        /// </summary>
        [Conditional("CONTRACTS_FULL")]
        public static void EndContractBlock() { }

        #endregion Misc.

        #endregion User Methods

        #region Failure Behavior

        static partial void ReportFailure(ContractFailureKind failureKind, String userMessage, String conditionText, Exception innerException);

        static partial void AssertMustUseRewriter(ContractFailureKind kind, String contractKind);

        #endregion Failure Behavior
    }

    public enum ContractFailureKind
    {
        Precondition,
        Postcondition,
        PostconditionOnException,
        Invariant,
        Assert,
        Assume,
    }
}

//left out some stuff.
/*
 // Note: In .NET FX 4.5, we duplicated the ContractHelper class in the System.Runtime.CompilerServices
// namespace to remove an ugly wart of a namespace from the Windows 8 profile.  But we still need the
// old locations left around, so we can support rewritten .NET FX 4.0 libraries.  Consider removing
// these from our reference assembly in a future version.
namespace System.Diagnostics.Contracts.Internal
{
    [Obsolete("Use the ContractHelper class in the System.Runtime.CompilerServices namespace instead.")]
    public static class ContractHelper
    {
        #region Rewriter Failure Hooks
        /// <summary>
        /// Rewriter will call this method on a contract failure to allow listeners to be notified.
        /// The method should not perform any failure (assert/throw) itself.
        /// </summary>
        /// <returns>null if the event was handled and should not trigger a failure.
        ///          Otherwise, returns the localized failure message</returns>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        [System.Diagnostics.DebuggerNonUserCode]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static string RaiseContractFailedEvent(ContractFailureKind failureKind, String userMessage, String conditionText, Exception innerException)
        {
            return System.Runtime.CompilerServices.ContractHelper.RaiseContractFailedEvent(failureKind, userMessage, conditionText, innerException);
        }
        /// <summary>
        /// Rewriter calls this method to get the default failure behavior.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#endif
        public static void TriggerFailure(ContractFailureKind kind, String displayMessage, String userMessage, String conditionText, Exception innerException)
        {
            System.Runtime.CompilerServices.ContractHelper.TriggerFailure(kind, displayMessage, userMessage, conditionText, innerException);
        }
        #endregion Rewriter Failure Hooks
    }
}  // namespace System.Diagnostics.Contracts.Internal
namespace System.Runtime.CompilerServices
{
    public static partial class ContractHelper
    {
        #region Rewriter Failure Hooks
        /// <summary>
        /// Rewriter will call this method on a contract failure to allow listeners to be notified.
        /// The method should not perform any failure (assert/throw) itself.
        /// </summary>
        /// <returns>null if the event was handled and should not trigger a failure.
        ///          Otherwise, returns the localized failure message</returns>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        [System.Diagnostics.DebuggerNonUserCode]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static string RaiseContractFailedEvent(ContractFailureKind failureKind, String userMessage, String conditionText, Exception innerException)
        {
            var resultFailureMessage = "Contract failed"; // default in case implementation does not assign anything.
            RaiseContractFailedEventImplementation(failureKind, userMessage, conditionText, innerException, ref resultFailureMessage);
            return resultFailureMessage;
        }
        /// <summary>
        /// Rewriter calls this method to get the default failure behavior.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#endif
        public static void TriggerFailure(ContractFailureKind kind, String displayMessage, String userMessage, String conditionText, Exception innerException)
        {
            TriggerFailureImplementation(kind, displayMessage, userMessage, conditionText, innerException);
        }
        #endregion Rewriter Failure Hooks
        #region Implementation Stubs
        /// <summary>
        /// Rewriter will call this method on a contract failure to allow listeners to be notified.
        /// The method should not perform any failure (assert/throw) itself.
        /// This method has 3 functions:
        /// 1. Call any contract hooks (such as listeners to Contract failed events)
        /// 2. Determine if the listeneres deem the failure as handled (then resultFailureMessage should be set to null)
        /// 3. Produce a localized resultFailureMessage used in advertising the failure subsequently.
        /// </summary>
        /// <param name="resultFailureMessage">Should really be out (or the return value), but partial methods are not flexible enough.
        /// On exit: null if the event was handled and should not trigger a failure.
        ///          Otherwise, returns the localized failure message</param>
        static partial void RaiseContractFailedEventImplementation(ContractFailureKind failureKind, String userMessage, String conditionText, Exception innerException, ref string resultFailureMessage);
        /// <summary>
        /// Implements the default failure behavior of the platform. Under the BCL, it triggers an Assert box.
        /// </summary>
        static partial void TriggerFailureImplementation(ContractFailureKind kind, String displayMessage, String userMessage, String conditionText, Exception innerException);
        #endregion Implementation Stubs
    }
}  // namespace System.Runtime.CompilerServices
*/

