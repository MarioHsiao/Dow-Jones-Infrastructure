// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TraceVerboseAttribute.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the TraceVerboseAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using log4net;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace EMG.widgets.ui.Attributes.AOP
{
    /// <summary>
    /// Trace Verbose Attribute is used for logging the entry, exit and values of public method calls
    /// </summary>
    [MulticastAttributeUsage(MulticastTargets.Method, TargetMemberAttributes = MulticastAttributes.Instance)]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [Serializable]
    public sealed class TraceVerboseAttribute : OnMethodBoundaryAspect
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
        /// Initializes static members of the <see cref="TraceVerboseAttribute"/> class. 
        /// </summary>
        static TraceVerboseAttribute()
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
        /// Override of the OnException method.
        /// </summary>
        /// <param name="args">Arguments object</param>
        /// <inheritdoc/>
        public override void OnException(MethodExecutionArgs args)
        {
            var log = LogManager.GetLogger(args.Method.DeclaringType.FullName);
            var stringBuilder = new StringBuilder(1024);

            // Write the exit message.
            stringBuilder.Append(args.Method.Name);
            stringBuilder.Append('(');

            // Write the current instance object, unless the method
            // is static.
            var instance = args.Instance;
            if (instance != null)
            {
                stringBuilder.Append("this=");
                stringBuilder.Append(instance);

                if (args.Arguments != null && args.Arguments.Count > 0)
                {
                    stringBuilder.Append("; ");
                }
            }

            if (args.Arguments != null && args.Arguments.Count > 0)
            {
                // Write the list of all arguments.
                for (var i = 0; i < args.Arguments.Count; i++)
                {
                    if (i > 0)
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(args.Arguments.GetArgument(i) ?? "null");
                }
            }

            // Write the exception message.
            stringBuilder.AppendFormat("): Exception ");
            stringBuilder.Append(args.Exception.GetType().Name);
            stringBuilder.Append(": ");
            stringBuilder.Append(args.Exception.Message);

            if (log.IsInfoEnabled)
            {
                // Finally emit the error.
                log.Info(stringBuilder);
            }
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
        internal static void GetNamedSlot()
        {
            // update the guid
            Thread.GetData(Thread.GetNamedDataSlot(GuidNamedDataSlot));
        }

        /// <summary>
        /// Optimizes the get named slot.
        /// </summary>
        /// <returns>A string object</returns>
        private static string OptimizedGetNamedSlot()
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
    }
}