using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>Represents the method that defines a set of criteria and determines whether the specified object meets those criteria.</summary>
    /// <param name="obj">The object to compare against the criteria defined within the method represented by this delegate.</param>
    /// <typeparam name="T">The type of the object to compare.</typeparam>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> meets the criteria defined within the method represented by this delegate; otherwise, <see langword="false" />.</returns>
    public delegate bool Predicate<in T>(T obj);
}
