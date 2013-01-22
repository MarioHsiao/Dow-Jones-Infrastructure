// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TraceAttribute.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Trace implementation of MethodBoundary Aspect
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading;
using log4net;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace EMG.widgets.ui.Attributes.AOP
{
    /// <summary>
    /// Trace implementation of MethodBoundary Aspect
    /// </summary>
    [MulticastAttributeUsage(MulticastTargets.Method, TargetMemberAttributes = MulticastAttributes.Instance)]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [Serializable]
    public sealed class TraceAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Unique guid named slot identifier
        /// </summary>
        [NonSerialized]
        private const string GuidNamedDataSlot = "EMG.Utility.Logging.Unique.GUID";

        /// <summary>
        /// Stopwatch for monitoring
        /// </summary>
        [NonSerialized]
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        /// <summary>
        /// Initializes static members of the <see cref="TraceAttribute"/> class. 
        /// </summary>
        static TraceAttribute()
        {
            Stopwatch.Start();
        }

        /// <summary>
        /// Implementation of OnEntry classs.
        /// </summary>
        /// <param name="args">
        /// The argsuments.
        /// </param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = Stopwatch.ElapsedMilliseconds;
            var log = LogManager.GetLogger(args.Method.DeclaringType.FullName);
            if (log.IsInfoEnabled)
            {
                log.InfoFormat(
                    "{2}:--> Entering {0}.{1}: ",
                    args.Method.DeclaringType.FullName,
                    args.Method.Name,
                    OptimizedGetNamedSlot());
            }
        }

        /// <summary>
        /// Implementation of OnExit classs.
        /// </summary>
        /// <param name="args">
        /// The argsuments.
        /// </param>
        public override void OnExit(MethodExecutionArgs args)
        {
            var log = LogManager.GetLogger(args.Method.DeclaringType.FullName);
            if (!log.IsInfoEnabled)
            {
                return;
            }

            log.InfoFormat(
                "{3}:--> Leaviing {0}.{1}: --Duration(ms):: {2}ms",
                args.Method.DeclaringType.FullName,
                args.Method.Name,
                (Stopwatch.ElapsedMilliseconds - (long)args.MethodExecutionTag), // milliseconds
                OptimizedGetNamedSlot());
        }

        /// <summary>
        /// Creates the named slot.
        /// </summary>
        /// <param name="guid">The unique GUID.</param>
        internal static void CreateNamedSlot(string guid)
        {
            // update the guid
            Thread.SetData(Thread.GetNamedDataSlot(GuidNamedDataSlot), guid);
        }

        /// <summary>
        /// Updates the named slot.
        /// </summary>
        /// <param name="guid">he unique GUID</param>
        internal static void UpdateNamedSlot(string guid)
        {
            // update the guid
            Thread.SetData(Thread.GetNamedDataSlot(GuidNamedDataSlot), guid);
        }

        /// <summary>
        /// Deletes the named slot.
        /// </summary>
        internal static void DeleteNamedSlot()
        {
            // update the guid
            Thread.SetData(Thread.GetNamedDataSlot(GuidNamedDataSlot), null);
        }

        /// <summary>
        /// Gets the named slot.
        /// </summary>
        /// <returns>The get named slot.</returns>
        internal static object GetNamedSlot()
        {
            try
            {
                // update the guid
                return Thread.GetData(Thread.GetNamedDataSlot(GuidNamedDataSlot));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Optimizes the get named slot.
        /// </summary>
        /// <returns>A string object</returns>
        private static string OptimizedGetNamedSlot()
        {
            try
            {
                var guid = (string)Thread.GetData(Thread.GetNamedDataSlot(GuidNamedDataSlot));
                if (string.IsNullOrEmpty(guid))
                {
                    guid = Guid.NewGuid().ToString();

                    // update the guid
                    CreateNamedSlot(guid);
                }

                return guid;
            }
            catch 
            {
                return string.Empty;
            }
        }
    }
}