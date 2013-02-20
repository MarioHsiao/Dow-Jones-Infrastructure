// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransactionLogger.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using DowJones.Extensions;

namespace DowJones.Loggers
{
    /// <summary>
    /// Class used for loging transactions.
    /// </summary>
    public class TransactionLogger : IDisposable
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILog _log;

        /// <summary>
        /// The method base.
        /// </summary>
        private readonly MethodBase _methodBase;

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
        /// <param name="log">
        /// The logger.
        /// </param>
        public TransactionLogger(ILog log)
        {
            _isDisposed = false;
            _startupWatch = new Stopwatch();
            _runningWatch = new Stopwatch();
            _startupWatch.Start();
            _runningWatch.Start();
            _log = log;
            if (_log.IsInfoEnabled)
            {
                _log.Info("\nStart Of TransactionLogger");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionLogger"/> class.
        /// </summary>
        /// <param name="log">The logger.</param>
        /// <param name="methodBase">The method base.</param>
        public TransactionLogger(ILog log, MethodBase methodBase)
        {
            _methodBase = methodBase;
            _startupWatch = new Stopwatch();
            _runningWatch = new Stopwatch();
            _startupWatch.Start();
            _runningWatch.Start();

            _log = log;
            if (_log.IsInfoEnabled)
            {
                _log.InfoFormat("\nStart Of TransactionLogger for method {0} of {1}", methodBase.Name, methodBase.ReflectedType.Name);
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
            get { return _startupWatch.Elapsed.Milliseconds; }
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
            if (!_log.IsInfoEnabled || !_runningWatch.IsRunning)
            {
                return;
            }

            if (_methodBase != null && !string.IsNullOrEmpty(message))
            {
                _log.InfoFormat("\nLogTimeSpentSinceReset: {0} of {1} took {2}\nMessage: {3}", _methodBase.Name, _methodBase.ReflectedType.Name, _runningWatch.Elapsed, message);
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                _log.InfoFormat("\nLogTimeSpentSinceReset: {0}\nMessage: {1}", _runningWatch.Elapsed, message);
                return;
            }

            if (_methodBase != null)
            {
                _log.InfoFormat("\nLogTimeSpentSinceReset: {0} of {1} took {2}", _methodBase.Name, _methodBase.ReflectedType.Name, _runningWatch.Elapsed);
                return;
            }

            _log.InfoFormat("\nLogTimeSpentSinceReset took {0}", _runningWatch.Elapsed);
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
            return _methodBase.DeclaringType != null ? _methodBase.DeclaringType.Name : _methodBase.ReflectedType.Name;
        }

        /// <summary>
        /// Logs the time since invocation.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void LogTimeSinceInvocation(string message)
        {
            if (!_log.IsInfoEnabled || !_startupWatch.IsRunning)
            {
                return;
            }
            
            if (_methodBase != null && !string.IsNullOrEmpty(message))
            {
                _log.InfoFormat("\nLogTimeSinceInvocation: {0} of {1} took {2}\nMessage: {3}", _methodBase.Name, GetTypeName(), _startupWatch.Elapsed, message);
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                _log.InfoFormat("\nLogTimeSinceInvocation: {0}\nMessage: {1}", _startupWatch.Elapsed, message);
                return;
            }

            if (_methodBase != null)
            {
                _log.InfoFormat("\nLogTimeSinceInvocation: {0} of {1} took {2}", _methodBase.Name, GetTypeName(), _startupWatch.Elapsed);
                return;
            }

            _log.InfoFormat("\nLogTimeSinceInvocation took {0}", _startupWatch.Elapsed);
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