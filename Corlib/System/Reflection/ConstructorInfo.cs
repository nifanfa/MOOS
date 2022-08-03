using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    [Serializable]
    public abstract class ConstructorInfo : MethodBase
    {
        /// <summary>
        /// Represents the name of the class constructor method as it is stored in metadata. This name is always ".ctor". This field is read-only.
        /// </summary>
        public static readonly string ConstructorName = ".ctor";

        /// <summary>
        /// Represents the name of the type constructor method as it is stored in metadata. This name is always ".cctor". This field is read-only.
        /// </summary>
        public static readonly string TypeConstructorName = ".cctor";

        /// <summary>
        /// Invokes the constructor reflected by the instance that has the specified parameters, providing default values for the parameters not commonly used.
        /// </summary>
        /// <param name="parameters">An array of values that matches the number, order and type (under the constraints of the default binder) of the parameters for this constructor. If this constructor takes no parameters, then use either an array with zero elements or null, as in Object[] parameters = new Object[0]. Any object in this array that is not explicitly initialized with a value will contain the default value for that object type. For reference-type elements, this value is null. For value-type elements, this value is 0, 0.0, or false, depending on the specific element type.</param>
        /// <returns>An instance of the class associated with the constructor.</returns>
        public object Invoke(object[] parameters)
        {
            // TODO
            throw new NotImplementedException();
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
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
