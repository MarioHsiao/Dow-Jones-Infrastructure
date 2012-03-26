// (c) Some content Copyright 2002-2010 Telerik 
// This source is subject to the GNU General Public License, version 2
// See http://www.gnu.org/licenses/gpl-2.0.html. 
// All other rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DowJones.Extensions
{
    /// <summary>
    /// Contains extension methods of ICollection&lt;T&gt;.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a range of value uniquely to a collection and returns the amount of values added.
        /// </summary>
        /// <typeparam name="T">The generic collection value type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="values">The values to be added.</param>
        /// <returns>The amount if values that were added.</returns>
        public static int AddRangeUnique<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            Guard.IsNotNull(collection, "collection");
            Guard.IsNotNull(values, "values");

            return values.Count(value => collection.AddUnique(value));
        }


        /// <summary>
        /// Adds the specified elements to the end of the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance to add.</param>
        /// <param name="collection"> The collection whose elements should be added to the end of the ICollection&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        [DebuggerStepThrough]
        public static void AddRange<T>(this ICollection<T> instance, IEnumerable<T> collection)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNull(collection, "collection");

            foreach (T item in collection)
            {
                instance.Add(item);
            }
        }

        /// <summary>
        /// Adds a value uniquely to to a collection and returns a value whether the value was added or not.
        /// </summary>
        /// <typeparam name="T">The generic collection value type</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns>Indicates whether the value was added or not</returns>
        /// <example><code>
        /// list.AddUnique(1); // returns true;
        /// list.AddUnique(1); // returns false the second time;
        /// </code></example>
        public static bool AddUnique<T>(this ICollection<T> collection, T value)
        {
            Guard.IsNotNull(collection, "collection");
            Guard.IsNotNull(value, "values");

            if (collection.Contains(value) == false)
            {
                collection.Add(value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified collection is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// <c>true</c> if the specified instance is empty; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsEmpty<T>(this IEnumerable<T> instance)
        {
            Guard.IsNotNull(instance, "instance");

            return !instance.Any();
        }

        /// <summary>
        /// Determines whether the specified collection instance is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance to check.</param>
        /// <returns>
        /// <c>true</c> if the specified instance is null or empty; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this ICollection<T> instance)
        {
            return (instance == null) || (instance.Count == 0);
        }
    }
}