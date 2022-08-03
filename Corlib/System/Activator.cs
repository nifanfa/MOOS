using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    /// <summary>
	/// Implementation of the "Activator" class.
	/// </summary>
	public class Activator
    {
        /// <summary>
        /// Creates an instance of the type designated by the specified generic type parameter, using the parameterless constructor.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <returns>A reference to the newly created object.</returns>
        public static T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        /// <summary>
        /// Creates an instance of the specified type using that type's default constructor.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static object CreateInstance(Type type)
        {
            return CreateInstance(type, null);
        }

        /// <summary>
        /// Creates an instance of the specified type using the constructor that best matches the specified parameters.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <param name="args">
        /// An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        /// If args is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static object CreateInstance(Type type, params object[] args)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return RuntimeHelpers.CreateInstance(type, args);
        }
    }
}
