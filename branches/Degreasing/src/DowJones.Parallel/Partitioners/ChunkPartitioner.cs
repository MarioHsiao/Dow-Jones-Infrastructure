//--------------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved. 
// 
//  File: ChunkPartitioner.cs
//
//--------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;

namespace System.Collections.Concurrent.Partitioners
{
    /// <summary>
    /// Partitions an enumerable into chunks based on user-supplied criteria.
    /// </summary>
    public static class ChunkPartitioner
    {
        /// <summary>Creates a practitioner that chooses the next chunk size based on a user-supplied function.</summary>
        /// <typeparam name="TSource">The type of the data being partitioned.</typeparam>
        /// <param name="source">The data being partitioned.</param>
        /// <param name="nextChunkSizeFunc">A function that determines the next chunk size based on the
        /// previous chunk size.</param>
        /// <returns>A practitioner.</returns>
        public static OrderablePartitioner<TSource> Create<TSource>(
            IEnumerable<TSource> source, Func<int, int> nextChunkSizeFunc)
        {
            return new ChunkPartitioner<TSource>(source, nextChunkSizeFunc);
        }

        /// <summary>Creates a practitioner that always uses a user-specified chunk size.</summary>
        /// <typeparam name="TSource">The type of the data being partitioned.</typeparam>
        /// <param name="source">The data being partitioned.</param>
        /// <param name="chunkSize">The chunk size to be used.</param>
        /// <returns>A practitioner.</returns>
        public static OrderablePartitioner<TSource> Create<TSource>(
            IEnumerable<TSource> source, int chunkSize)
        {
            return new ChunkPartitioner<TSource>(source, chunkSize);
        }

        /// <summary>Creates a practitioner that chooses chunk sizes between the user-specified min and max.</summary>
        /// <typeparam name="TSource">The type of the data being partitioned.</typeparam>
        /// <param name="source">The data being partitioned.</param>
        /// <param name="minChunkSize">The minimum chunk size to use.</param>
        /// <param name="maxChunkSize">The maximum chunk size to use.</param>
        /// <returns>A practitioner.</returns>
        public static OrderablePartitioner<TSource> Create<TSource>(
            IEnumerable<TSource> source, int minChunkSize, int maxChunkSize)
        {
            return new ChunkPartitioner<TSource>(source, minChunkSize, maxChunkSize);
        }
    }

    /// <summary>
    /// Partitions an enumerable into chunks based on user-supplied criteria.
    /// </summary>
    internal sealed class ChunkPartitioner<T> : OrderablePartitioner<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly Func<int, int> _nextChunkSizeFunc;

        public ChunkPartitioner(IEnumerable<T> source, Func<int, int> nextChunkSizeFunc)
            // The keys will be ordered across both individual partitions and across partitions,
            // and they will be normalized.
            : base(true, true, true)
        {
            // Validate and store the enumerable and function (used to determine how big
            // to make the next chunk given the current chunk size)
            if (source == null) throw new ArgumentNullException("source");
            if (nextChunkSizeFunc == null) throw new ArgumentNullException("nextChunkSizeFunc");
            _source = source;
            _nextChunkSizeFunc = nextChunkSizeFunc;
        }

        public ChunkPartitioner(IEnumerable<T> source, int chunkSize)
            : this(source, prev => chunkSize) // uses a function that always returns the specified chunk size
        {
            if (chunkSize <= 0) throw new ArgumentOutOfRangeException("chunkSize");
        }

        public ChunkPartitioner(IEnumerable<T> source, int minChunkSize, int maxChunkSize) :
            this(source, CreateFuncFromMinAndMax(minChunkSize, maxChunkSize)) // uses a function that grows from min to max
        {
            if (minChunkSize <= 0 ||
                minChunkSize > maxChunkSize) throw new ArgumentOutOfRangeException("minChunkSize");
        }

        private static Func<int, int> CreateFuncFromMinAndMax(int minChunkSize, int maxChunkSize)
        {
            // Create a function that returns exponentially growing chunk sizes between minChunkSize and maxChunkSize
            return delegate(int prev)
            {
                if (prev < minChunkSize) return minChunkSize;
                if (prev >= maxChunkSize) return maxChunkSize;
                int next = prev * 2;
                if (next >= maxChunkSize || next < 0) return maxChunkSize;
                return next;
            };
        }

        /// <summary>
        /// Partitions the underlying collection into the specified number of orderable partitions.
        /// </summary>
        /// <param name="partitionCount">The number of partitions to create.</param>
        /// <returns>An object that can create partitions over the underlying data source.</returns>
        public override IList<IEnumerator<KeyValuePair<long, T>>> GetOrderablePartitions(int partitionCount)
        {
            // Validate parameters
            if (partitionCount <= 0) throw new ArgumentOutOfRangeException("partitionCount");

            // Create an array of dynamic partitions and return them
            var partitions = new IEnumerator<KeyValuePair<long, T>>[partitionCount];
            var dynamicPartitions = GetOrderableDynamicPartitions(true); 
            for (int i = 0; i < partitionCount; i++)
            {
                partitions[i] = dynamicPartitions.GetEnumerator(); // Create and store the next partition
            }
            return partitions;
        }

        /// <summary>Gets whether additional partitions can be created dynamically.</summary>
        public override bool SupportsDynamicPartitions { get { return true; } }

        /// <summary>
        /// Creates an object that can partition the underlying collection into a variable number of
        /// partitions.
        /// </summary>
        /// <returns>
        /// An object that can create partitions over the underlying data source.
        /// </returns>
        public override IEnumerable<KeyValuePair<long, T>> GetOrderableDynamicPartitions()
        {
            return new EnumerableOfEnumerators(this, false);
        }

        private IEnumerable<KeyValuePair<long, T>> GetOrderableDynamicPartitions(bool referenceCountForDisposal)
        {
            return new EnumerableOfEnumerators(this, referenceCountForDisposal);
        }

        // The object used to dynamically create partitions
        private class EnumerableOfEnumerators : IEnumerable<KeyValuePair<long, T>>, IDisposable
        {
            private readonly ChunkPartitioner<T> parentPartitioner;
            private readonly IEnumerator<T> sharedEnumerator;
            private long nextSharedIndex;
            private int activeEnumerators;
            private bool noMoreElements;
            private bool disposed;
            private readonly bool referenceCountForDisposal;

            public EnumerableOfEnumerators(ChunkPartitioner<T> parentPartitioner, bool referenceCountForDisposal)
            {
                // Validate parameters
                if (parentPartitioner == null) throw new ArgumentNullException("parentPartitioner");

                // Store the data, including creating an enumerator from the underlying data source
                this.parentPartitioner = parentPartitioner;
                sharedEnumerator = parentPartitioner._source.GetEnumerator();
                nextSharedIndex = -1;
                this.referenceCountForDisposal = referenceCountForDisposal;
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
            public IEnumerator<KeyValuePair<long, T>> GetEnumerator()
            {
                if (referenceCountForDisposal)
                {
                    Interlocked.Increment(ref activeEnumerators);
                }
                return new Enumerator(this);
            }

            private void DisposeEnumerator()
            {
                if (!referenceCountForDisposal)
                {
                    return;
                }

                if (Interlocked.Decrement(ref activeEnumerators) == 0)
                {
                    sharedEnumerator.Dispose();
                }
            }

            private class Enumerator : IEnumerator<KeyValuePair<long, T>>
            {
                private readonly EnumerableOfEnumerators parentEnumerable;
                private readonly List<KeyValuePair<long, T>> currentChunk = new List<KeyValuePair<long, T>>();
                private int currentChunkCurrentIndex;
                private int lastRequestedChunkSize;
                private bool disposed;

                public Enumerator(EnumerableOfEnumerators parentEnumerable)
                {
                    if (parentEnumerable == null) throw new ArgumentNullException("parentEnumerable");
                    this.parentEnumerable = parentEnumerable;
                }

                public bool MoveNext()
                {
                    if (disposed) throw new ObjectDisposedException(GetType().Name);

                    // Move to the next cached element. If we already retrieved a chunk and if there's still
                    // data left in it, just use the next item from it.
                    ++currentChunkCurrentIndex;
                    if (currentChunkCurrentIndex >= 0 &&
                        currentChunkCurrentIndex < currentChunk.Count) return true;

                    // First, figure out how much new data we want. The previous requested chunk size is used
                    // as input to figure out how much data the user now wants.  The initial chunk size
                    // supplied is 0 so that the user delegate is made aware that this is the initial request
                    // such that it can select the initial chunk size on first request.
                    int nextChunkSize = parentEnumerable.parentPartitioner._nextChunkSizeFunc(lastRequestedChunkSize);
                    if (nextChunkSize <= 0) throw new InvalidOperationException(
                        "Invalid chunk size requested: chunk sizes must be positive.");
                    lastRequestedChunkSize = nextChunkSize;

                    // Reset the list
                    currentChunk.Clear();
                    currentChunkCurrentIndex = 0;
                    if (nextChunkSize > currentChunk.Capacity) currentChunk.Capacity = nextChunkSize;

                    // Try to grab the next chunk of data
                    lock (parentEnumerable.sharedEnumerator)
                    {
                        // If we've already discovered that no more elements exist (and we've gotten this
                        // far, which means we don't have any elements cached), we're done.
                        if (parentEnumerable.noMoreElements) return false;

                        // Get another chunk
                        for (var i = 0; i < nextChunkSize; i++)
                        {
                            // If there are no more elements to be retrieved from the shared enumerator, mark
                            // that so that other partitions don't have to check again. Return whether we
                            // were able to retrieve any data at all.
                            if (!parentEnumerable.sharedEnumerator.MoveNext())
                            {
                                parentEnumerable.noMoreElements = true;
                                return currentChunk.Count > 0;
                            }

                            ++parentEnumerable.nextSharedIndex;
                            currentChunk.Add(new KeyValuePair<long, T>(
                                parentEnumerable.nextSharedIndex,
                                parentEnumerable.sharedEnumerator.Current));
                        }
                    }

                    // We got at least some data
                    return true;
                }

                public KeyValuePair<long, T> Current
                {
                    get
                    {
                        if (currentChunkCurrentIndex >= currentChunk.Count)
                        {
                            throw new InvalidOperationException("There is no current item.");
                        }
                        return currentChunk[currentChunkCurrentIndex];
                    }
                }

                public void Dispose()
                {
                    if (disposed) return;
                    parentEnumerable.DisposeEnumerator();
                    disposed = true;
                }

                object IEnumerator.Current { get { return Current; } }
                public void Reset() { throw new NotSupportedException(); }
            }

            public void Dispose()
            {
                if (disposed) return;
                if (!referenceCountForDisposal) sharedEnumerator.Dispose();
                disposed = true;
            }
        }
    }
}