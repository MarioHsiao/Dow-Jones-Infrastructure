// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePerformanceCounterAttribute.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Aspect that, when applied on a method, increments a performance counter
//   by the time elapsed during the execution of this method.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using PostSharp.Aspects;

namespace EMG.widgets.ui.Attributes.AOP
{
    /// <summary>
    /// Aspect that, when applied on a method, increments a performance counter
    /// by the time elapsed during the execution of this method.
    /// </summary>
    public sealed class TimePerformanceCounterAttribute : PerformanceCounterAttribute
    {
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        static TimePerformanceCounterAttribute()
        {
            Stopwatch.Start();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePerformanceCounterAttribute"/> class.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        public TimePerformanceCounterAttribute(string categoryName, string counterName)
            : base(categoryName, counterName, false)
        {
        }

        /// <summary>
        /// Method invoked before the execution of the method to which the current
        /// aspect is applied.
        /// </summary>
        /// <param name="args">Method Execution Arguments.</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = Stopwatch.ElapsedTicks;
            base.OnEntry(args);
        }

        /// <summary>
        /// Method invoked after the execution of the method to which the current
        /// aspect is applied.
        /// </summary>
        /// <param name="args">Method Execution Arguments.</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            var time = Stopwatch.ElapsedTicks - (long)args.MethodExecutionTag;
            PerformanceCounter.IncrementBy(time);
            base.OnExit(args);
        }
    }
}