using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Reflection
{
    [Serializable]
    public abstract class Assembly
    {
        /// <summary>
        /// Gets a collection that contains this assembly's custom attributes.
        /// </summary>
        public virtual IEnumerable<CustomAttributeData> CustomAttributes
        {
            get { return new CustomAttributeData[0]; }
        }

        /// <summary>
        /// Gets a collection of the types defined in this assembly.
        /// </summary>
        public abstract IEnumerable<TypeInfo> DefinedTypes { get; }

        public virtual IEnumerable<Type> ExportedTypes
        {
            get { return new Type[0]; }
        }

        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        public virtual string FullName
        {
            get { return ""; }
        }

        /// <summary>
        /// Determines whether this assembly and the specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>True if o is equal to this instance; otherwise, False.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Assembly))
                return false;

            return ((Assembly)obj).FullName == FullName;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        /// <summary>
        /// Gets all loaded assemblies.
        /// </summary>
        /// <returns>Assemblies</returns>
        public static IEnumerable<Assembly> GetAssemblies()
        {
            return RuntimeHelpers.GetAssemblies();
        }
    }
}
