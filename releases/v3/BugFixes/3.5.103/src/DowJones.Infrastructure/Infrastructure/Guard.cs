// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Guard.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Helper class for argument validation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DowJones.Extensions;
using DowJones.Properties;

// ReSharper disable CheckNamespace
namespace DowJones.Infrastructure
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Helper class for argument validation.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures the specified argument is not null.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName, Resources.CannotBeNull.FormatWith(parameterName));
            }
        }

        /// <summary>
        /// Ensures the specified string is not blank.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty(string parameter, string parameterName)
        {
            if (string.IsNullOrEmpty((parameter ?? string.Empty)))
            {
                throw new ArgumentException(Resources.CannotBeNullOrEmpty.FormatWith(parameterName));
            }
        }

        /// <summary>
        /// Ensures the specified array is not null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty<T>(IEnumerable<T> parameter, string parameterName)
        {
            IsNotNull(parameter, parameterName);

            if (!parameter.Any())
            {
                throw new ArgumentException(Resources.ArrayCannotBeEmpty.FormatWith(parameterName));
            }
        }

        /// <summary>
        /// Ensures the specified collection is not null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty<T>(ICollection<T> parameter, string parameterName)
        {
            IsNotNull(parameter, parameterName);

            if (parameter.Count == 0)
            {
                throw new ArgumentException(Resources.CollectionCannotBeEmpty.FormatWith(parameterName), parameterName);
            }
        }

        /// <summary>
        /// Ensures the specified value is a positive integer.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotZeroOrNegative(int parameter, string parameterName) 
        {
            if (parameter <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, Resources.CannotBeNegativeOrZero.FormatWith(parameterName));
            }
        }

        [DebuggerStepThrough]
        public static void IsNotZeroOrNegative(double parameter, string parameterName)
        {
            if (parameter <= 0.0)
            {
                throw new ArgumentOutOfRangeException(parameterName, Resources.CannotBeNegativeOrZero.FormatWith(parameterName));
            }
        }


        /// <summary>
        /// Ensures the specified value is not a negative integer.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotNegative(int parameter, string parameterName)
        {
            if (parameter < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, Resources.CannotBeNegative.FormatWith(parameterName));
            }
        }

        /// <summary>
        /// Ensures the specified value is not a negative float.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotNegative(float parameter, string parameterName)
        {
            if (parameter < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, Resources.CannotBeNegative.FormatWith(parameterName));
            }
        }

        /// <summary>
        /// Ensures the specified path is a virtual path which starts with ~/.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void IsNotVirtualPath(string parameter, string parameterName)
        {
            IsNotNullOrEmpty(parameter, parameterName);

            if (!parameter.StartsWith("~/", StringComparison.Ordinal))
            {
                throw new ArgumentException(Resources.SourceMustBeAVirtualPathWhichShouldStartsWithTileAndSlash, parameterName);
            }
        }

        /// <summary>
        /// A generic, "freehand" guard that evaluates a predicate
        /// </summary>
        /// <param name="predicate">The predicate that should be evaluated</param>
        /// <param name="message">Error message to display when predicate succeeds</param>
        [DebuggerStepThrough]
        public static void Against(bool predicate, string message)
        {
            if (predicate)
            {
                throw new ArgumentException(message);
            }
        }
    }
}