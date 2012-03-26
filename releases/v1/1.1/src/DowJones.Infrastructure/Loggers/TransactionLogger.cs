// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransactionLogger.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Reflection;
using log4net;

namespace DowJones.Utilities.Loggers
{
    /// <summary>
    /// Class used for loging transactions.
    /// </summary>
    public class TransactionLogger : IDisposable
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILog log;

        /// <summary>
        /// The method base.
        /// </summary>
        private readonly MethodBase methodBase;

        /// <summary>
        /// The startup watch.
        /// </summary>
        private readonly Stopwatch startupWatch;

        /// <summary>
        /// The is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// The running watch.
        /// </summary>
        private Stopwatch runningWatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionLogger"/> class.
        /// </summary>
        /// <param name="log">
        /// The logger.
        /// </param>
        public TransactionLogger(ILog log)
        {
            isDisposed = false;
            startupWatch = new Stopwatch();
            runningWatch = new Stopwatch();
            startupWatch.Start();
            runningWatch.Start();
            this.log = log;
            if (this.log.IsInfoEnabled)
            {
                this.log.Info("\n**** <- Start Of TransactionLogger -> ****");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionLogger"/> class.
        /// </summary>
        /// <param name="log">The logger.</param>
        /// <param name="methodBase">The method base.</param>
        public TransactionLogger(ILog log, MethodBase methodBase)
        {
            this.methodBase = methodBase;
            startupWatch = new Stopwatch();
            runningWatch = new Stopwatch();
            startupWatch.Start();
            runningWatch.Start();

            this.log = log;
            if (this.log.IsInfoEnabled)
            {
                this.log.InfoFormat("\n**** <- Start Of TransactionLogger for method {0} of {1}  -> ****", methodBase.Name, methodBase.ReflectedType.Name);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TransactionLogger"/> class. 
        /// </summary>
        ~TransactionLogger()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the elapsed time since invocation.
        /// </summary>
        /// <value>The elapsed time since invocation.</value>
        public int ElapsedTimeSinceInvocation
        {
            get { return startupWatch.Elapsed.Milliseconds; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get the time stamp.
        /// </summary>
        /// <remarks>
        /// Used to get the time stamp before the back end transaction is about to be performed.
        /// </remarks>
        public void Reset()
        {
            if (runningWatch.IsRunning)
            {
                runningWatch.Stop();
            }

            runningWatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Logs the time spent since reset.
        /// </summary>
        public void LogTimeSpentSinceReset()
        {
            LogTimeSpentSinceReset(null);
        }

        /// <summary>
        /// Log the time spent to perform the transaction to the logging target.
        /// </summary>
        /// <param name="message">
        /// Tranasction Name
        /// </param>
        /// <remarks>
        /// Used to get the time spent to perform the back end transaction.
        /// </remarks>
        public void LogTimeSpentSinceReset(string message)
        {
            if (!log.IsInfoEnabled || !runningWatch.IsRunning)
            {
                return;
            }

            if (methodBase != null && !string.IsNullOrEmpty(message))
            {
                log.InfoFormat("\nLogTimeSpentSinceReset: {0} of {1} took {2}\nMessage: {3}", methodBase.Name, methodBase.ReflectedType.Name, runningWatch.Elapsed, message);
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                log.InfoFormat("\nLogTimeSpentSinceReset: {0}\nMessage: {1}", runningWatch.Elapsed, message);
                return;
            }

            if (methodBase != null)
            {
                log.InfoFormat("\nLogTimeSpentSinceReset: {0} of {1} took {2}", methodBase.Name, methodBase.ReflectedType.Name, runningWatch.Elapsed);
                return;
            }

            log.InfoFormat("\nLogTimeSpentSinceReset took {0}", runningWatch.Elapsed);
        }

        /// <summary>
        /// Logs the time since invocation.
        /// </summary>
        public void LogTimeSinceInvocation()
        {
            LogTimeSinceInvocation(null);
        }

        /// <summary>
        /// Logs the time since invocation.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void LogTimeSinceInvocation(string message)
        {
            if (!log.IsInfoEnabled || !startupWatch.IsRunning)
            {
                return;
            }

            if (methodBase != null && !string.IsNullOrEmpty(message))
            {
                log.InfoFormat("\nLogTimeSinceInvocation: {0} of {1} took {2}\nMessage: {3}", methodBase.Name, methodBase.ReflectedType.Name, startupWatch.Elapsed, message);
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                log.InfoFormat("\nLogTimeSinceInvocation: {0}\nMessage: {1}", startupWatch.Elapsed, message);
                return;
            }

            if (methodBase != null)
            {
                log.InfoFormat("\nLogTimeSinceInvocation: {0} of {1} took {2}", methodBase.Name, methodBase.ReflectedType.Name, startupWatch.Elapsed);
                return;
            }

            log.InfoFormat("\nLogTimeSinceInvocation took {0}", startupWatch.Elapsed);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // Stop the StopWatches
                    if (runningWatch.IsRunning)
                    {
                        runningWatch.Stop();
                    }

                    if (startupWatch.IsRunning)
                    {
                        startupWatch.Stop();
                    }

                    // Write to the log if active
                    LogTimeSinceInvocation();

                    // Code to dispose managed resources held by the class
                }
            }

            // Code to dispose unmanaged resources held by the class
            isDisposed = true;
        }
    }
}