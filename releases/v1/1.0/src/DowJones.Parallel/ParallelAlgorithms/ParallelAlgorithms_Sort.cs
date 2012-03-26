﻿//--------------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved. 
// 
//  File: ParallelAlgorithms_Sort.cs
//
//--------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Threading.Algorithms
{
    public static partial class ParallelAlgorithms
    {
        /// <summary>Sorts an array in parallel.</summary>
        /// <typeparam name="T">Specifies the type of data in the array.</typeparam>
        /// <param name="array">The array to be sorted.</param>
        public static void Sort<T>(T [] array)
        {
            Sort(array, null);
        }

        /// <summary>Sorts an array in parallel.</summary>
        /// <typeparam name="T">Specifies the type of data in the array.</typeparam>
        /// <param name="array">The array to be sorted.</param>
        /// <param name="comparer">The comparer used to compare two elements during the sort operation.</param>
        public static void Sort<T>(T[] array, IComparer<T> comparer)
        {
            if (array == null) throw new ArgumentNullException("array");
            Sort<T, object>(array, null, 0, array.Length, comparer);
        }

        /// <summary>Sorts an array in parallel.</summary>
        /// <typeparam name="T">Specifies the type of data in the array.</typeparam>
        /// <param name="array">The array to be sorted.</param>
        /// <param name="index">The index at which to start the sort, inclusive.</param>
        /// <param name="length">The number of elements to be sorted, starting at the start index.</param>
        public static void Sort<T>(T [] array, Int32 index, Int32 length)
        {
            Sort<T, object>(array, null, index, length, null);
        }

        /// <summary>Sorts an array in parallel.</summary>
        /// <typeparam name="T">Specifies the type of data in the array.</typeparam>
        /// <param name="array">The array to be sorted.</param>
        /// <param name="index">The index at which to start the sort, inclusive.</param>
        /// <param name="length">The number of elements to be sorted, starting at the start index.</param>
        /// <param name="comparer">The comparer used to compare two elements during the sort operation.</param>
        public static void Sort<T>(T[] array, Int32 index, Int32 length, IComparer<T> comparer)
        {
            Sort<T,object>(array, null, index, length, comparer);
        }

        /// <summary>Sorts key/value arrays in parallel.</summary>
        /// <typeparam name="TKey">Specifies the type of the data in the keys array.</typeparam>
        /// <typeparam name="TValue">Specifies the type of the data in the items array.</typeparam>
        /// <param name="keys">The keys to be sorted.</param>
        /// <param name="items">The items to be sorted based on the corresponding keys.</param>
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items)
        {
            Sort(keys, items, 0, keys.Length, null);
        }

        /// <summary>Sorts key/value arrays in parallel.</summary>
        /// <typeparam name="TKey">Specifies the type of the data in the keys array.</typeparam>
        /// <typeparam name="TValue">Specifies the type of the data in the items array.</typeparam>
        /// <param name="keys">The keys to be sorted.</param>
        /// <param name="items">The items to be sorted based on the corresponding keys.</param>
        /// <param name="comparer">The comparer used to compare two elements during the sort operation.</param>
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, IComparer<TKey> comparer)
        {
            if (keys == null) throw new ArgumentNullException("keys");
            Sort(keys, items, 0, keys.Length, comparer);
        }

        /// <summary>Sorts key/value arrays in parallel.</summary>
        /// <typeparam name="TKey">Specifies the type of the data in the keys array.</typeparam>
        /// <typeparam name="TValue">Specifies the type of the data in the items array.</typeparam>
        /// <param name="keys">The keys to be sorted.</param>
        /// <param name="items">The items to be sorted based on the corresponding keys.</param>
        /// <param name="index">The index at which to start the sort, inclusive.</param>
        /// <param name="length">The number of elements to be sorted, starting at the start index.</param>
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, Int32 index, Int32 length)
        {
            Sort(keys, items, index, length, null);
        }

        /// <summary>Sorts key/value arrays in parallel.</summary>
        /// <typeparam name="TKey">Specifies the type of the data in the keys array.</typeparam>
        /// <typeparam name="TValue">Specifies the type of the data in the items array.</typeparam>
        /// <param name="keys">The keys to be sorted.</param>
        /// <param name="items">The items to be sorted based on the corresponding keys.</param>
        /// <param name="index">The index at which to start the sort, inclusive.</param>
        /// <param name="length">The number of elements to be sorted, starting at the start index.</param>
        /// <param name="comparer">The comparer used to compare two elements during the sort operation.</param>
        public static void Sort<TKey, TValue>(TKey [] keys, TValue [] items, Int32 index, Int32 length, IComparer<TKey> comparer)
        {
            if (keys == null) throw new ArgumentNullException("keys");
            if ((index < 0) || (length < 0)) throw new ArgumentOutOfRangeException(length < 0 ? "length" : "index");
            if (((keys.Length - index) < length) || ((items != null) && (index > (items.Length - length)))) throw new ArgumentException("index");

            // Run the core sort operation
            new Sorter<TKey, TValue>(keys, items, comparer).QuickSort(index, index + length - 1);
        }

        // Stores the data necessary for the sort, and provides the core sorting method
        private sealed class Sorter<TKey, TItem>
        {
            private readonly TKey[] keys;
            private readonly TItem[] items;
            private readonly IComparer<TKey> comparer;

            public Sorter(TKey[] keys, TItem[] items, IComparer<TKey> comparer)
            {
                if (comparer == null) comparer = Comparer<TKey>.Default;
                this.keys = keys;
                this.items = items;
                this.comparer = comparer;
            }

            // Gets a recommended depth for recursion.  This assumes that every level will
            // spawn two child tasks, which isn't actually the case with the algorithm, but
            // it's a "good enough" approximation.
            private static int GetMaxDepth()
            {
                return (int)Math.Log(Environment.ProcessorCount, 2);
            }

            // Swaps the items at the two specified indexes if they need to be swapped
            private void SwapIfGreaterWithItems(int a, int b)
            {
                if (a == b ||comparer.Compare(keys[a], keys[b]) <= 0 )
                {
                    return;
                }

                var temp = keys[a];
                keys[a] = keys[b];
                keys[b] = temp;
                if (items == null)
                {
                    return;
                }

                var item = items[a];
                items[a] = items[b];
                items[b] = item;
            }

            // Gets the middle value between the provided low and high
            private static int GetMiddle(int low, int high) { return low + ((high - low) >> 1); }

            // Does a quicksort of the stored data, between the positions (inclusive specified by left and right)
            internal void QuickSort(int left, int right)
            {
                QuickSort(left, right, 0, GetMaxDepth());
            }

            // Does a quicksort of the stored data, between the positions (inclusive specified by left and right).
            // Depth specifies the current recursion depth, while maxDepth specifies the maximum depth
            // we should recur to until we switch over to sequential.
            private void QuickSort(int left, int right, int depth, int maxDepth)
            {
                const int sequentialThreshold = 0x1000;

                // If the max depth has been reached or if we've hit the sequential
                // threshold for the input array size, run sequential.
                if (depth >= maxDepth || (right - left + 1) <= sequentialThreshold)
                {
                    Array.Sort(keys, items, left, right - left + 1, comparer);
                    return;
                }

                // Store all tasks generated to process sub arrays
                var tasks = new List<Task>();

                // Run the same basic algorithm used by Array.Sort, but spawning Tasks for all recursive calls
                do
                {
                    var i = left;
                    var j = right;

                    // Pre-sort the low, middle (pivot), and high values in place.
                    var middle = GetMiddle(i, j);
                    SwapIfGreaterWithItems(i, middle); // swap the low with the mid point
                    SwapIfGreaterWithItems(i, j);      // swap the low with the high
                    SwapIfGreaterWithItems(middle, j); // swap the middle with the high

                    // Get the pivot
                    var x = keys[middle];

                    // Move all data around the pivot value
                    do
                    {
                        while (comparer.Compare(keys[i], x) < 0) i++;
                        while (comparer.Compare(x, keys[j]) < 0) j--;

                        if (i > j) break;
                        if (i < j)
                        {
                            var key = keys[i];
                            keys[i] = keys[j];
                            keys[j] = key;
                            if (items != null)
                            {
                                var item = items[i];
                                items[i] = items[j];
                                items[j] = item;
                            }
                        }
                        i++;
                        j--;
                    } while (i <= j);

                    if (j - left <= right - i)
                    {
                        if (left < j)
                        {
                            int leftcopy = left, jcopy = j;
                            tasks.Add(Task.Factory.StartNew(() => QuickSort(leftcopy, jcopy, depth + 1, maxDepth)));
                        }
                        left = i;
                    }
                    else
                    {
                        if (i < right)
                        {
                            int icopy = i, rightcopy = right;
                            tasks.Add(Task.Factory.StartNew(() => QuickSort(icopy, rightcopy, depth + 1, maxDepth)));
                        }
                        right = j;
                    }
                } while (left < right);

                // Wait for all of this level's tasks to complete
                Task.WaitAll(tasks.ToArray());
            }
        }
    }
}