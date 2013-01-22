// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncrementPerformanceCounterAttribute.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Aspect that, when applied on a method, increments a performance counter
//   before each execution of this method.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using PostSharp.Aspects;

namespace EMG.widgets.ui.Attributes.AOP
{
    /// <summary>
    /// Aspect that, when applied on a method, increments a performance counter
    /// before each execution of this method.
    /// </summary>
    [Serializable]
    public sealed class IncrementPerformanceCounterAttribute : PerformanceCounterAttribute
    {
        private long increment = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementPerformanceCounterAttribute"/> class.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        public IncrementPerformanceCounterAttribute(string categoryName, string counterName)
            : base(categoryName, counterName, false)
        {
        }

        /// <summary>
        /// Gets or sets the value of which the performance counter is incremented
        /// before each execution of target method. The default value is 1.
        /// </summary>
        public long Increment
        {
            get { return this.increment; }
            set { this.increment = value; }
        }

        /// <summary>
        /// Method invoked before the execution of the method to which the current
        /// aspect is applied.
        /// </summary>
        /// <param name="args">Method Execution Arguments.</param>
        /// <inheritdoc/>
        public override void OnEntry(MethodExecutionArgs args)
        {
            this.PerformanceCounter.IncrementBy(this.increment);
            base.OnEntry(args);
        }
    }
}