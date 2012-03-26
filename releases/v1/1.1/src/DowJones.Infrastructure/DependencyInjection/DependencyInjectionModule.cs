using System;
using System.Collections.Generic;
using System.Web;
using DowJones.Infrastructure;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;

namespace DowJones.DependencyInjection
{
    public abstract class DependencyInjectionModule : NinjectModule
    {
        protected static readonly Func<IContext, object> IsDebuggingEnabled = 
            x => x.Kernel.Get<HttpContextBase>().IsDebuggingEnabled;


        protected IAssemblyRegistry AssemblyRegistry
        {
            get;
            private set;
        }

        public override void Load()
        {
            AssemblyRegistry = Kernel.TryGet<IAssemblyRegistry>();

            OnLoad();
        }


        /// <summary>
        /// Automagically discovers all of the public concrete
        /// types that implement the type <typeparamref name="T"/>
        /// and binds them in the kernel.
        /// </summary>
        /// <example>
        /// <code>
        /// AutoBind<IBootstrapperTask>();
        /// </code>
        /// will bind all of the implementations of IBootstrapperTask
        /// located in the currently-loaded assemblies.
        /// </example>
        public void AutoBind<T>()
        {
            IEnumerable<Type> targetTypes =
                AssemblyRegistry.GetConcreteTypesDerivingFrom<T>();

            foreach (var targetType in targetTypes)
            {
                Bind<T>().To(targetType);
            }
        }

        public void BindToTypes<T>(IEnumerable<Type> targets)
        {
            foreach (var target in targets)
            {
                Bind<T>().To(target);
            }
        }

        public void BindToConstants<T>(IEnumerable<T> targets)
        {
            foreach (var target in targets)
            {
                Bind<T>().ToConstant(target);
            }
        }


        protected abstract void OnLoad();
    }
}
