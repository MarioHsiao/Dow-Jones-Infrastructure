using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Infrastructure;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace DowJones.DependencyInjection
{
    public abstract class DependencyInjectionModule : IPackage
    {

		public IContainer Container { get; private set; }

        protected IAssemblyRegistry AssemblyRegistry
        {
            get;
            private set;
        }

        
        /// <summary>
        /// Automagically discovers all of the public concrete
        /// types that implement the type <typeparamref name="T"/>
        /// and binds them in the kernel.
        /// </summary>
        /// <example>
        /// <code>
        /// AutoBind&lt;IBootstrapperTask&gt;();
        /// </code>
        /// will bind all of the implementations of IBootstrapperTask
        /// located in the currently-loaded assemblies.
        /// </example>
        public void AutoBind<T>(Func<Type, bool> predicate = null)
        {
            var targetTypes =
                AssemblyRegistry.GetConcreteTypesDerivingFrom<T>();

            if (predicate != null)
                targetTypes = targetTypes.Where(predicate);

            BindToTypes<T>(targetTypes);
        }

        

        public void BindToTypes<T>(IEnumerable<Type> targets)
        {
			Container.RegisterAll<T>(targets);
        }

        public void BindToConstants<T>(IEnumerable<T> targets)
        {
            foreach (var target in targets)
            {
                Container.RegisterSingle(typeof(T), target);
            }
        }
		
		protected abstract void OnLoad(IContainer container);

	    /// <summary>
	    /// Registers the set of services in the specified <paramref name="container"/>.
	    /// </summary>
	    /// <param name="container">The container the set of services is registered into.</param>
		public void RegisterServices(Container container)
	    {
		    Container = new SimpleInjectContainer(container);
		    AssemblyRegistry = Infrastructure.AssemblyRegistry.Create();
			OnLoad(Container);
	    }

	    
    }
}
