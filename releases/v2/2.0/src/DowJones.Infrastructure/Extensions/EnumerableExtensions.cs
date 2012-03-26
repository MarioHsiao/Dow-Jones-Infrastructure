// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Extension methods for all kinds of (typed) enumerable data (Array, List, ...)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DowJones.Infrastructure;

namespace DowJones.Extensions
{
    /// <summary>
    /// Extension methods for all kinds of (typed) enumerable data (Array, List, ...)
    /// </summary>
    public static class EnumerableExtensions
    {

        public static bool Contains(this IEnumerable<string> source, string value, bool caseInsensitive)
        {
            return caseInsensitive
                ? source.Contains(value, new CaseInsensitiveComparer())
                : source.Contains(value);
        }

        public static IEnumerable<T> Distinct<T, TRet>(this IEnumerable<T> source, Func<T, TRet> projector) 
            where TRet : IComparable<TRet>
        {
            return Distinct(source, projector, null);
        }

        public static IEnumerable<T> Distinct<T, TRet>(this IEnumerable<T> source, Func<T, TRet> projector, IEqualityComparer<TRet> comparer) 
            where TRet : IComparable<TRet>
        {
            return source.Distinct(new GenericEqualityComparer<T, TRet>(projector, comparer));
        }

        /// <summary>
        /// Filters out all null items from a list
        /// </summary>
        /// <returns>The non-null items in a list</returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
        {
            return source.Where(x => !ReferenceEquals(x, null));
        }

        /// <summary>
        /// Converts all items of a list and returns them as enumerable.
        /// </summary>
        /// <typeparam name="TSource">The source data type</typeparam>
        /// <typeparam name="TTarget">The target data type</typeparam>
        /// <param name="source">The source data.</param>
        /// <returns>The converted data</returns>
        /// <example>
        /// var values = new[] { "1", "2", "3" };
        /// values.ConvertList&lt;string, int&gt;().ForEach(Console.WriteLine);
        /// </example>
        public static IEnumerable<TTarget> ConvertList<TSource, TTarget>(this IEnumerable<TSource> source)
        {
            return source.Select(value => value.ConvertTo<TTarget>());
        }

        /// <summary>
        /// Performs an action for each item in the enumerable
        /// </summary>
        /// <typeparam name="T">The enumerable data type</typeparam>
        /// <param name="values">The data values.</param>
        /// <param name="action">The action to be performed.</param>
        /// <example>
        /// var values = new[] { "1", "2", "3" };
        /// values.ConvertList&lt;string, int&gt;().ForEach(Console.WriteLine);
        /// </example>
        /// <remarks>This method was intended to return the passed values to provide method chaining. However due to defered execution the compiler would actually never run the entire code at all.</remarks>
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (var value in values)
            {
                action(value);
            }
        }

        public static string Join(this IEnumerable<string> stringsToJoin, string separator)
        {
            Guard.IsNotNull(stringsToJoin, "stringsToJoin");
            Guard.IsNotNull(separator, "separator");

            return string.Join(separator, stringsToJoin.ToArray());
        }

        /// <summary>
        /// Eaches the specified instance.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The action.</param>
        public static void Each<T>(this IEnumerable<T> instance, Action<T, int> action)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNull(action, "action");

            var index = 0;
            foreach (var item in instance)
            {
                action(item, index++);
            }
        }

        /// <summary>
        /// Executes the provided delegate for each item.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The action to be applied.</param>
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNull(action, "action");

            foreach (var item in instance)
            {
                action(item);
            }
        }

        // Source: http://work.j832.com/2008/01/selectrecursive-if-3rd-times-charm-4th.html
        public static IEnumerable<TSource> SelectRecursive<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> recursiveSelector)
        {
            Guard.IsNotNull(source, "source");
            Guard.IsNotNull(recursiveSelector, "recursiveSelector");

            var stack = new Stack<IEnumerator<TSource>>();
            stack.Push(source.GetEnumerator());

            try
            {
                while (stack.Count > 0)
                {
                    if (stack.Peek().MoveNext())
                    {
                        var current = stack.Peek().Current;

                        yield return current;

                        var children = recursiveSelector(current);
                        if (children != null)
                        {
                            stack.Push(children.GetEnumerator());
                        }
                    }
                    else
                    {
                        stack.Pop().Dispose();
                    }
                }
            }
            finally
            {
                while (stack.Count > 0)
                {
                    stack.Pop().Dispose();
                }
            }
        }

        /// <summary>
        /// Consolidates the specified first.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TSecond">The type of the second.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns>An enumberable object</returns>
        /// <exception cref="ArgumentNullException"><c>first</c> is null.</exception>
        /// <exception cref="ArgumentNullException"><c>second</c> is null.</exception>
        /// <exception cref="ArgumentNullException"><c>resultSelector</c> is null.</exception>
        public static IEnumerable<TResult> Consolidate<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }

            return ZipIterator(first, second, resultSelector);
        }

        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                return DefaultReadOnlyCollection<T>.Empty;
            }

            var onlys = sequence as ReadOnlyCollection<T>;
            return onlys ?? new ReadOnlyCollection<T>(sequence.ToArray());
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Do Not Want Catch General Exception Types")]
        internal static IEnumerable AsGenericEnumerable(this IEnumerable source)
        {
            Guard.IsNotNull(source, "source");

            Type elementType = typeof(object);

            var type = source.GetType().FindGenericType(typeof(IEnumerable<>));
            if (type != null)
            {
                return source;
            }

            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == null)
                {
                    continue;
                }

                elementType = enumerator.Current.GetType();
                try
                {
                    enumerator.Reset();
                }
                catch
                {
                }

                break;
            }

            var genericType = typeof(GenericEnumerable<>).MakeGenericType(elementType);
            var constructorParameters = new object[] { source };

            return (IEnumerable)Activator.CreateInstance(genericType, constructorParameters);
        }

        internal static int IndexOf(this IEnumerable source, object item)
        {
            Guard.IsNotNull(source, "source");
            Guard.IsNotNull(item, "item");

            var index = 0;
            foreach (var i in source)
            {
                if (Equals(i, item))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        /// <summary>
        /// Elements at.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <returns>A static object.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>index</c> is out of range.</exception>
        internal static object ElementAt(this IEnumerable source, int index)
        {
            Guard.IsNotNegative(index, "index");

            var list = source as IList;
            if (list != null && list.Count > 0)
            {
                return list[index];
            }

            foreach (var item in source)
            {
                if (index == 0)
                {
                    return item;
                }

                index--;
            }

            return null;
        }

        private static IEnumerable<TResult> ZipIterator<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            using (var e1 = first.GetEnumerator())
            {
                using (var e2 = second.GetEnumerator())
                {
                    while (e1.MoveNext() && e2.MoveNext())
                    {
                        yield return resultSelector(e1.Current, e2.Current);
                    }
                }
            }
        }

        public class GenericEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable source;

            /// <summary>
            /// Initializes a new instance of the <see cref="GenericEnumerable{T}"/> class.
            /// </summary>
            /// <param name="source">The source.</param>
            public GenericEnumerable(IEnumerable source)
            {
                Guard.IsNotNull(source, "source");

                this.source = source;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return source.GetEnumerator();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return source.Cast<T>().GetEnumerator();
            }
        }

        private static class DefaultReadOnlyCollection<T>
        {
            /// <summary>
            /// </summary>
            private static ReadOnlyCollection<T> _defaultCollection;

            /// <summary>
            /// Gets the empty.
            /// </summary>
            /// <value>The empty.</value>
            internal static ReadOnlyCollection<T> Empty
            {
                get { return _defaultCollection ?? (_defaultCollection = new ReadOnlyCollection<T>(new T[0])); }
            }
        }

        [Obsolete]
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return first.Except(second, new LambdaComparer<TSource>(comparer));
        }

        [Obsolete]
        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return first.Intersect(second, new LambdaComparer<TSource>(comparer));
        }

    }

    [Obsolete("Use GenericEqualityComparer<T, TRet>")]
    internal class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _lambdaComparer;
        private readonly Func<T, int> _lambdaHash;

        public LambdaComparer(Func<T, T, bool> lambdaComparer) :
            this(lambdaComparer, o => 0)
        {
        }

        public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
        {
            if (lambdaComparer == null)
                throw new ArgumentNullException("lambdaComparer");
            if (lambdaHash == null)
                throw new ArgumentNullException("lambdaHash");

            _lambdaComparer = lambdaComparer;
            _lambdaHash = lambdaHash;
        }

        public bool Equals(T x, T y)
        {
            return _lambdaComparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _lambdaHash(obj);
        }
    }
}