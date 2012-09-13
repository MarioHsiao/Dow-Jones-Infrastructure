﻿//--------------------------------------------------------------------------
// 
//  Copyright (c) Microsoft Corporation.  All rights reserved. 
// 
//  File: WorkStealingTaskScheduler.cs
//
//--------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace System.Threading.Tasks.Schedulers
{
    /// <summary>Provides a work-stealing scheduler.</summary>
    public class WorkStealingTaskScheduler : TaskScheduler, IDisposable
    {
        private readonly int concurrencyLevel;
        private readonly Queue<Task> queue = new Queue<Task>();
        private WorkStealingQueue<Task>[] workStealingQueues = new WorkStealingQueue<Task>[Environment.ProcessorCount];
        private readonly Lazy<Thread[]> threads;
        private int threadsWaiting;
        private bool shutdown;
        private bool disposed;

        [ThreadStatic]
        private static WorkStealingQueue<Task> _wsq;

        /// <summary>Initializes a new instance of the WorkStealingTaskScheduler class.</summary>
        /// <remarks>This constructors defaults to using twice as many threads as there are processors.</remarks>
        public WorkStealingTaskScheduler() : this(Environment.ProcessorCount * 2) { }

        /// <summary>Initializes a new instance of the WorkStealingTaskScheduler class.</summary>
        /// <param name="concurrencyLevel">The number of threads to use in the scheduler.</param>
        public WorkStealingTaskScheduler(int concurrencyLevel)
        {
            // Store the concurrency level
            if (concurrencyLevel <= 0) throw new ArgumentOutOfRangeException("concurrencyLevel");
            this.concurrencyLevel = concurrencyLevel;

            // Set up threads
            threads = new Lazy<Thread[]>(() =>
            {
                var _threads = new Thread[this.concurrencyLevel];
                for (var i = 0; i < _threads.Length; i++)
                {
                    _threads[i] = new Thread(DispatchLoop) { IsBackground = true };
                    _threads[i].Start();
                }
                return _threads;
            });
        }

        // Finalizer simply calls Dispose(false)
        ~WorkStealingTaskScheduler()
        {
            Dispose(false);
        }

        /// <summary>Queues a task to the scheduler.</summary>
        /// <param name="task">The task to be scheduled.</param>
        protected override void QueueTask(Task task)
        {
            // Make sure the pool is started, e.g. that all threads have been created.
            threads.Force();

            // If the task is marked as long-running, give it its own dedicated thread
            // rather than queuing it.
            if ((task.CreationOptions & TaskCreationOptions.LongRunning) != 0)
            {
                new Thread(state => TryExecuteTask((Task)state)) { IsBackground = true }.Start(task);
            }
            else
            {
                // Otherwise, insert the work item into a queue, possibly waking a thread.
                // If there's a local queue and the task does not prefer to be in the global queue,
                // add it to the local queue.
                WorkStealingQueue<Task> wsq = _wsq;
                if (wsq != null && ((task.CreationOptions & TaskCreationOptions.PreferFairness) == 0))
                {
                    // Add to the local queue and notify any waiting threads that work is available.
                    // Races may occur which result in missed event notifications, but they're benign in that
                    // this thread will eventually pick up the work item anyway, as will other threads when another
                    // work item notification is received.
                    wsq.LocalPush(task);
                    if (threadsWaiting > 0) // OK to read lock-free.
                    {
                        lock (queue) { Monitor.Pulse(queue); }
                    }
                }
                // Otherwise, add the work item to the global queue
                else
                {
                    lock (queue)
                    {
                        queue.Enqueue(task);
                        if (threadsWaiting > 0) Monitor.Pulse(queue);
                    }
                }
            }
        }

        /// <summary>Executes a task on the current thread.</summary>
        /// <param name="task">The task to be executed.</param>
        /// <param name="taskWasPreviouslyQueued">Ignored.</param>
        /// <returns>Whether the task could be executed.</returns>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);

            // // Optional replacement: Instead of always trying to execute the task (which could
            // // benignly leave a task in the queue that's already been executed), we
            // // can search the current work-stealing queue and remove the task,
            // // executing it inline only if it's found.
            // WorkStealingQueue<Task> wsq = m_wsq;
            // return wsq != null && wsq.TryFindAndPop(task) && TryExecuteTask(task);
        }

        /// <summary>Gets the maximum concurrency level supported by this scheduler.</summary>
        public override int MaximumConcurrencyLevel
        {
            get { return concurrencyLevel; }
        }

        /// <summary>Gets all of the tasks currently scheduled to this scheduler.</summary>
        /// <returns>An enumerable containing all of the scheduled tasks.</returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            // Keep track of all of the tasks we find
            var tasks = new List<Task>();

            // Get all of the global tasks.  We use TryEnter so as not to hang
            // a debugger if the lock is held by a frozen thread.
            var lockTaken = false;
            try
            {
                Monitor.TryEnter(queue, ref lockTaken);
                if (lockTaken) tasks.AddRange(queue.ToArray());
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(queue);
            }

            // Now get all of the tasks from the work-stealing queues
            var queues = workStealingQueues;
            foreach (var wsq in queues.Where(wsq => wsq != null))
            {
                tasks.AddRange(wsq.ToArray());
            }

            // Return to the debugger all of the collected task instances
            return tasks;
        }

        /// <summary>Adds a work-stealing queue to the set of queues.</summary>
        /// <param name="wsq">The queue to be added.</param>
        private void AddWsq(WorkStealingQueue<Task> wsq)
        {
            lock (workStealingQueues)
            {
                // Find the next open slot in the array. If we find one,
                // store the queue and we're done.
                int i;
                for (i = 0; i < workStealingQueues.Length; i++)
                {
                    if (workStealingQueues[i] != null)
                    {
                        continue;
                    }

                    workStealingQueues[i] = wsq;
                    return;
                }

                // We couldn't find an open slot, so double the length 
                // of the array by creating a new one, copying over,
                // and storing the new one. Here, i == m_wsQueues.Length.
                var queues = new WorkStealingQueue<Task>[i * 2];
                Array.Copy(workStealingQueues, queues, i);
                queues[i] = wsq;
                workStealingQueues = queues;
            }
        }

        /// <summary>Remove a work-stealing queue from the set of queues.</summary>
        /// <param name="wsq">The work-stealing queue to remove.</param>
        private void RemoveWsq(WorkStealingQueue<Task> wsq)
        {
            lock (workStealingQueues)
            {
                // Find the queue, and if/when we find it, null out its array slot
                for (int i = 0; i < workStealingQueues.Length; i++)
                {
                    if (workStealingQueues[i] == wsq)
                    {
                        workStealingQueues[i] = null;
                    }
                }
            }
        }

        /// <summary>
        /// The dispatch loop run by each thread in the scheduler.
        /// </summary>
        private void DispatchLoop()
        {
            // Create a new queue for this thread, store it in TLS for later retrieval,
            // and add it to the set of queues for this scheduler.
            var wsq = new WorkStealingQueue<Task>();
            _wsq = wsq;
            AddWsq(wsq);

            try
            {
                // Until there's no more work to do...
                while (true)
                {
                    Task wi = null;

                    // Search order: (1) local WSQ, (2) global Q, (3) steals from other queues.
                    if (!wsq.LocalPop(ref wi))
                    {
                        // We weren't able to get a task from the local WSQ
                        var searchedForSteals = false;
                        while (true)
                        {
                            lock (queue)
                            {
                                // If shutdown was requested, exit the thread.
                                if (shutdown)
                                    return;

                                // (2) try the global queue.
                                if (queue.Count != 0)
                                {
                                    // We found a work item! Grab it ...
                                    wi = queue.Dequeue();
                                    break;
                                }
                                if (searchedForSteals)
                                {
                                    // Note that we're not waiting for work, and then wait
                                    threadsWaiting++;
                                    try { Monitor.Wait(queue); }
                                    finally { threadsWaiting--; }

                                    // If we were signaled due to shutdown, exit the thread.
                                    if (shutdown)
                                        return;

                                    searchedForSteals = false;
                                    continue;
                                }
                            }

                            // (3) try to steal.
                            var wsQueues = workStealingQueues;
                            int i;
                            for (i = 0; i < wsQueues.Length; i++)
                            {
                                var q = wsQueues[i];
                                if (q != null && q != wsq && q.TrySteal(ref wi)) break;
                            }

                            if (i != wsQueues.Length) break;

                            searchedForSteals = true;
                        }
                    }

                    // ...and Invoke it.
                    TryExecuteTask(wi);
                }
            }
            finally
            {
                RemoveWsq(wsq);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>Signal the scheduler to shutdown and wait for all threads to finish.</summary>
        protected void Dispose(bool disposing)
        {
            if (disposed) return;
            if (!disposing) return;
            shutdown = true;
            if (queue != null && threads.IsValueCreated)
            {
                var thrds = threads.Value;
                lock (queue)
                {
                    Monitor.PulseAll(queue);
                }

                foreach (var t in thrds)
                {
                    t.Join();
                }
            }
            disposed = true;
        }
    }

    /// <summary>A work-stealing queue.</summary>
    /// <typeparam name="T">Specifies the type of data stored in the queue.</typeparam>
    internal class WorkStealingQueue<T> where T : class
    {
        private const int INITIAL_SIZE = 32;
        private T[] m_array = new T[INITIAL_SIZE];
        private int m_mask = INITIAL_SIZE - 1;
        private volatile int m_headIndex;
        private volatile int m_tailIndex;

        private readonly object m_foreignLock = new object();

        internal void LocalPush(T obj)
        {
            var tail = m_tailIndex;

            // When there are at least 2 elements' worth of space, we can take the fast path.
            if (tail < m_headIndex + m_mask)
            {
                m_array[tail & m_mask] = obj;
                m_tailIndex = tail + 1;
            }
            else
            {
                // We need to contend with foreign pops, so we lock.
                lock (m_foreignLock)
                {
                    var head = m_headIndex;
                    var count = m_tailIndex - m_headIndex;

                    // If there is still space (one left), just add the element.
                    if (count >= m_mask)
                    {
                        // We're full; expand the queue by doubling its size.
                        var newArray = new T[m_array.Length << 1];
                        for (var i = 0; i < m_array.Length; i++)
                            newArray[i] = m_array[(i + head) & m_mask];

                        // Reset the field values, incl. the mask.
                        m_array = newArray;
                        m_headIndex = 0;
                        m_tailIndex = tail = count;
                        m_mask = (m_mask << 1) | 1;
                    }

                    m_array[tail & m_mask] = obj;
                    m_tailIndex = tail + 1;
                }
            }
        }

        internal bool LocalPop(ref T obj)
        {
            while (true)
            {
                // Decrement the tail using a fence to ensure subsequent read doesn't come before.
                int tail = m_tailIndex;
                if (m_headIndex >= tail)
                {
                    obj = null;
                    return false;
                }

                tail -= 1;
#pragma warning disable 0420
                Interlocked.Exchange(ref m_tailIndex, tail);
#pragma warning restore 0420

                // If there is no interaction with a take, we can head down the fast path.
                if (m_headIndex <= tail)
                {
                    int idx = tail & m_mask;
                    obj = m_array[idx];

                    // Check for nulls in the array.
                    if (obj == null) continue;

                    m_array[idx] = null;
                    return true;
                }
                // Interaction with takes: 0 or 1 elements left.
                lock (m_foreignLock)
                {
                    if (m_headIndex <= tail)
                    {
                        // Element still available. Take it.
                        int idx = tail & m_mask;
                        obj = m_array[idx];

                        // Check for nulls in the array.
                        if (obj == null) continue;

                        m_array[idx] = null;
                        return true;
                    }
                    // We lost the race, element was stolen, restore the tail.
                    m_tailIndex = tail + 1;
                    obj = null;
                    return false;
                }
            }
        }

        internal bool TrySteal(ref T obj)
        {
            obj = null;

            while (true)
            {
                if (m_headIndex >= m_tailIndex)
                    return false;

                lock (m_foreignLock)
                {
                    // Increment head, and ensure read of tail doesn't move before it (fence).
                    var head = m_headIndex;
#pragma warning disable 0420
                    Interlocked.Exchange(ref m_headIndex, head + 1);
#pragma warning restore 0420

                    if (head < m_tailIndex)
                    {
                        var idx = head & m_mask;
                        obj = m_array[idx];

                        // Check for nulls in the array.
                        if (obj == null) continue;

                        m_array[idx] = null;
                        return true;
                    }
                    // Failed, restore head.
                    m_headIndex = head;
                }

                return false;
            }
        }

        internal bool TryFindAndPop(T obj)
        {
            // We do an O(N) search for the work item. The theory of work stealing and our
            // in-lining logic is that most waits will happen on recently queued work.  And
            // since recently queued work will be close to the tail end (which is where we
            // begin our search), we will likely find it quickly.  In the worst case, we
            // will traverse the whole local queue; this is typically not going to be a
            // problem (although degenerate cases are clearly an issue) because local work
            // queues tend to be somewhat shallow in length, and because if we fail to find
            // the work item, we are about to block anyway (which is very expensive).

            for (int i = m_tailIndex - 1; i >= m_headIndex; i--)
            {
                if (m_array[i & m_mask] == obj)
                {
                    // If we found the element, block out steals to avoid interference.
                    lock (m_foreignLock)
                    {
                        // If we lost the race, bail.
                        if (m_array[i & m_mask] == null)
                        {
                            return false;
                        }

                        // Otherwise, null out the element.
                        m_array[i & m_mask] = null;

                        // And then check to see if we can fix up the indexes (if we're at
                        // the edge).  If we can't, we just leave nulls in the array and they'll
                        // get filtered out eventually (but may lead to superflous resizing).
                        if (i == m_tailIndex)
                            m_tailIndex -= 1;
                        else if (i == m_headIndex)
                            m_headIndex += 1;

                        return true;
                    }
                }
            }

            return false;
        }

        internal T[] ToArray()
        {
            var list = new List<T>();
            for (var i = m_tailIndex - 1; i >= m_headIndex; i--)
            {
                var obj = m_array[i & m_mask];
                if (obj != null) list.Add(obj);
            }
            return list.ToArray();
        }
    }
}