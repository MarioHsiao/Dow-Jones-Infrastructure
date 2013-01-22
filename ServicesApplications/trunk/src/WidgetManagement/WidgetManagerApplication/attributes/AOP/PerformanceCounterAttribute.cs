// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceCounterAttribute.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Base class for all performance counter aspects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using PostSharp.Aspects;

namespace EMG.widgets.ui.Attributes.AOP
{
    /// <summary>
    /// Base class for all performance counter aspects.
    /// </summary>
    /// <remarks>This class takes care of the identification and initialization of the
    /// performance counter. Derived class may access the performance counter at runtime 
    /// on the <see cref="PerformanceCounter"/> property.</remarks>
    [Serializable]
    public abstract class PerformanceCounterAttribute : OnMethodBoundaryAspect
    {
        // Serialized fields: set at build time, used at run time.
        [NonSerialized]
        private readonly bool includeInstanceName;

        private readonly string categoryName;
        private readonly string counterName;
        private string instanceName;
        
        // Not serialized because used at rutime only.
        [NonSerialized]
        private PerformanceCounter performanceCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounterAttribute"/> class.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="counterName">Name of the counter.</param>
        /// <param name="includeInstanceName">if set to <c>true</c> [include instance name].</param>
        protected PerformanceCounterAttribute(string categoryName, string counterName, bool includeInstanceName)
        {
            //// Not serialized because used at build time only.
            this.categoryName = categoryName;
            this.includeInstanceName = includeInstanceName;
            this.counterName = counterName;
        }

        /// <summary>
        /// Gets the performance counter (can be invoked at runtime).
        /// </summary>
        protected PerformanceCounter PerformanceCounter
        {
            get { return performanceCounter; }
        }

        /// <summary>
        /// Method executed at build time. Initializes the aspect instance. After the execution
        /// of <see cref="CompileTimeInitialize"/>, the aspect is serialized as a managed 
        /// resource inside the transformed assembly, and deserialized at runtime.
        /// </summary>
        /// <param name="method">Method to which the current aspect instance 
        /// has been applied.</param>
        /// <param name="aspectInfo">Unused. Aspect Info</param>
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            if (includeInstanceName)
            {
                instanceName = method.DeclaringType.FullName + "." + method.Name;
            }
        }

        /// <summary>
        /// Method executed at run time just after the aspect is deserialized.
        /// </summary>
        /// <param name="method">Method to which the current aspect instance has been applied.</param>
        public override void RuntimeInitialize(MethodBase method)
        {
            performanceCounter = new PerformanceCounter(categoryName, counterName, instanceName, false);
        }
    }
}