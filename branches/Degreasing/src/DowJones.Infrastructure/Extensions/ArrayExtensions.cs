// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Diagnostics;
using DowJones.Infrastructure;

namespace DowJones.Extensions
{
    /// <summary>
    /// Contains extension methods of T[].
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Determines whether the specified array is empty or null.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified instance]; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this T[] instance)
        {
            return (instance == null) || (instance.Length == 0);
        }

        /// <summary>
        /// Determines whether the specified array is empty.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified instance is empty; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsEmpty<T>(this T[] instance)
        {
            Guard.IsNotNull(instance, "instance");

            return instance.Length == 0;
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="start">The start index.</param>
        /// <param name="end">The end index.</param>
        /// <returns>An sliced array.</returns>
        [DebuggerStepThrough]
        public static T[] Slice<T>(this T[] instance, int start, int end)
        {
            Guard.IsNotNull(instance, "instance");

            // Handles negative ends.
            if (end < 0)
            {
                end = instance.Length + end;
            }

            var len = end - start;

            // Return new array.
            var res = new T[len];
            for (var i = 0; i < len; i++)
            {
                res[i] = instance[i + start];
            }

            return res;
        }
    }
}