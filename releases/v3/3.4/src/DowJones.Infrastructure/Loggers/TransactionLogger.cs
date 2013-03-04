// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransactionLogger.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Reflection;
using log4net;

namespace DowJones.Loggers
{
    public interface ITransactionTimer
    {
        ITransactionLogger Start();
        ITransactionLogger Start(ILog log, MethodBase methodBase = null);
    }

    public class BasicTransactionTimer : ITransactionTimer
    {
        public ITransactionLogger Start()
        {
            return new TransactionLogger();
        }

        public ITransactionLogger Start(ILog log, MethodBase methodBase = null)
        {
            return new TransactionLogger(log, methodBase);
        }
    }

    public interface ITransactionLogger : IDisposable
    {
    }

    public abstract class AbstractTransactionLogger : ITransactionLogger
    {
        private static readonly ILog BaseLog = LogManager.GetLogger(typeof (AbstractTransactionLogger));
        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILog Log;

        /// <summary>
        /// The method base.
        /// </summary>
        protected readonly MethodBase MethodBase;


        protected AbstractTransactionLogger(ILog log, MethodBase methodBase = null)
        {
            Log = log;
            MethodBase = methodBase;
        }

        protected AbstractTransactionLogger()
        {
            Log = BaseLog;
        }

        public abstract void Dispose();
    }



    /// <summary>
    /// Class used for logging transactions.
    /// </summary>
    public class TransactionLogger : AbstractTransactionLogger
    {
        /// <summary>
        /// The startup watch.
        /// </summary>
        private readonly Stopwatch _startupWatch;

        /// <summary>
        /// The is disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// The running watch.
        /// </summary>
        private Stopwatch _runningWatch;

       
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionLogger"/> class.
        /// </summary>
        /// <param name="log">The logger.</param>
        /// <param name="methodBase">The method base.</param>
        public TransactionLogger(ILog log, MethodBase methodBase = null) : base(log, methodBase)  
        {
            _isDisposed = false;
            _startupWatch = Stopwatch.StartNew();
            _runningWatch = Stopwatch.StartNew();

            if (!Log.IsInfoEnabled) return;
           
            if (methodBase == null)
            {
                Log.Info("\nStart Of TransactionLogger");
            }
            else
            {
                Log.InfoFormat("\nStart Of TransactionLogger for method {0} of {1}", methodBase.Name, methodBase.ReflectedType.Name);
            }
        }

        public TransactionLogger()
        {
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
            get { return _startupWatch.Elapsed.Milliseconds; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Dispose()
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
            if (_runningWatch.IsRunning)
            {
                _runningWatch.Stop();
            }

            _runningWatch = Stopwatch.StartNew();
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
        /// Transaction Name
        /// </param>
        /// <remarks>
        /// Used to get the time spent to perform the back end transaction.
        /// </remarks>
        public void LogTimeSpentSinceReset(string message)
        {
            if (!Log.IsInfoEnabled || !_runningWatch.IsRunning)
            {
                return;
            }

            if (MethodBase != null && !string.IsNullOrEmpty(message))
            {
                Log.InfoFormat("\nLogTimeSpentSinceReset: {0} of {1} took {2}\nMessage: {3}", MethodBase.Name, MethodBase.ReflectedType.Name, _runningWatch.Elapsed, message);
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                Log.InfoFormat("\nLogTimeSpentSinceReset: {0}\nMessage: {1}", _runningWatch.Elapsed, message);
                return;
            }

            if (MethodBase != null)
            {
                Log.InfoFormat("\nLogTimeSpentSinceReset: {0} of {1} took {2}", MethodBase.Name, MethodBase.ReflectedType.Name, _runningWatch.Elapsed);
                return;
            }

            Log.InfoFormat("\nLogTimeSpentSinceReset took {0}", _runningWatch.Elapsed);
        }

        /// <summary>
        /// Logs the time since invocation.
        /// </summary>
        public void LogTimeSinceInvocation()
        {
            LogTimeSinceInvocation(null);
        }


        private string GetTypeName()
        {
            return MethodBase.DeclaringType != null ? MethodBase.DeclaringType.Name : MethodBase.ReflectedType.Name;
        }

        /// <summary>
        /// Logs the time since invocation.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void LogTimeSinceInvocation(string message)
        {
            if (!Log.IsInfoEnabled || !_startupWatch.IsRunning)
            {
                return;
            }
            
            if (MethodBase != null && !string.IsNullOrEmpty(message))
            {
                Log.InfoFormat("\nLogTimeSinceInvocation: {0} of {1} took {2}\nMessage: {3}", MethodBase.Name, GetTypeName(), _startupWatch.Elapsed, message);
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                Log.InfoFormat("\nLogTimeSinceInvocation: {0}\nMessage: {1}", _startupWatch.Elapsed, message);
                return;
            }

            if (MethodBase != null)
            {
                Log.InfoFormat("\nLogTimeSinceInvocation: {0} of {1} took {2}", MethodBase.Name, GetTypeName(), _startupWatch.Elapsed);
                return;
            }

            Log.InfoFormat("\nLogTimeSinceInvocation took {0}", _startupWatch.Elapsed);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Write to the log if active
                    LogTimeSinceInvocation();

                    // Stop the StopWatches
                    if (_runningWatch.IsRunning)
                    {
                        _runningWatch.Stop();
                    }

                    if (_startupWatch.IsRunning)
                    {
                        _startupWatch.Stop();
                    }
                }
            }

            // Code to dispose unmanaged resources held by the class
            _isDisposed = true;
        }
    }
}