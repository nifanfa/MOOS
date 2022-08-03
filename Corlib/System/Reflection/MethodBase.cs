using System;
using System.Collections.Generic;
using System.Debugger.DebugData;
using System.Text;

namespace System.Reflection
{
    [Serializable]
    public abstract class MethodBase : MemberInfo
    {
        /// <summary>
        /// Gets the attributes associated with this method.
        /// </summary>
        public abstract MethodAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating the calling conventions for this method.
        /// </summary>
        public virtual CallingConventions CallingConvention
        {
            get
            {
                return CallingConventions.Standard;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the generic method contains unassigned generic type parameters.
        /// </summary>
        public virtual bool ContainsGenericParameters
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the method is abstract.
        /// </summary>
        public bool IsAbstract
        {
            get
            {
                return (Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor is described by MethodAttributes.Assembly; that is, the method or constructor is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        public bool IsAssembly
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the method is a constructor.
        /// </summary>
        public bool IsConstructor
        {
            get
            {
                return (this is ConstructorInfo && !IsStatic && (Attributes & MethodAttributes.RTSpecialName) == MethodAttributes.RTSpecialName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor is described by MethodAttributes.Family; that is, the method or constructor is visible only within its class and derived classes.
        /// </summary>
        public bool IsFamily
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor is described by MethodAttributes.FamANDAssem; that is, the method or constructor can be called by derived classes, but only if they are in the same assembly.
        /// </summary>
        public bool IsFamilyAndAssembly
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor is described by MethodAttributes.FamORAssem; that is, the method or constructor can be called by derived classes wherever they are, and by classes in the same assembly.
        /// </summary>
        public bool IsFamilyOrAssembly
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this method is final.
        /// </summary>
        public bool IsFinal
        {
            get
            {
                return (Attributes & MethodAttributes.Final) == MethodAttributes.Final;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the method is generic.
        /// </summary>
        public virtual bool IsGenericMethod
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the method is a generic method definition.
        /// </summary>
        public virtual bool IsGenericMethodDefinition
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class.
        /// </summary>
        public bool IsHideBySig
        {
            get
            {
                return (Attributes & MethodAttributes.HideBySig) == MethodAttributes.HideBySig;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this method is private.
        /// </summary>
        public bool IsPrivate
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this method is public.
        /// </summary>
        public bool IsPublic
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this method has a special name.
        /// </summary>
        public bool IsSpecialName
        {
            get
            {
                return (Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this method is static.
        /// </summary>
        public bool IsStatic
        {
            get
            {
                return (Attributes & MethodAttributes.Static) == MethodAttributes.Static;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this method is virtual.
        /// </summary>
        public bool IsVirtual
        {
            get
            {
                return (Attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual;
            }
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance, or null.</param>
        /// <returns>True if obj equals the type and value of this instance; otherwise, False.</returns>
        public override bool Equals(object obj)
        {
            return this == obj;
        }

        /// <summary>
        /// Returns an array of Type objects that represent the type arguments of a generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <returns>An array of Type objects that represent the type arguments of a generic method or the type parameters of a generic method definition. Returns an empty array if the current method is not a generic method.</returns>
        public virtual Type[] GetGenericArguments()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets method information by using the method's internal metadata representation (handle).
        /// </summary>
        /// <param name="handle">The method's handle.</param>
        /// <returns>A MethodBase containing information about the method.</returns>
        public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle)
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a MethodBase object for the constructor or method represented by the specified handle, for the specified generic type.
        /// </summary>
        /// <param name="handle">A handle to the internal metadata representation of a constructor or method.</param>
        /// <param name="declaringType">A handle to the generic type that defines the constructor or method.</param>
        /// <returns>A MethodBase object representing the method or constructor specified by handle, in the generic type specified by declaringType.</returns>
        public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType)
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, gets the parameters of the specified method or constructor.
        /// </summary>
        /// <returns>An array of type ParameterInfo containing information that matches the signature of the method (or constructor) reflected by this MethodBase instance.</returns>
        public abstract ParameterInfo[] GetParameters();

        /// <summary>
        /// Invokes the method or constructor represented by the current instance, using the specified parameters.
        /// </summary>
        /// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.</param>
        /// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, parameters should be null.
        /// If the method or constructor represented by this instance takes a ref parameter (ByRef in Visual Basic), no special attribute is required for that parameter in order to invoke the method or constructor using this function. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type.</param>
        /// <returns>An object containing the return value of the invoked method, or null in the case of a constructor.</returns>
        public object Invoke(object obj, object[] parameters)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
