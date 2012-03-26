using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.DependencyInjection.Ninject;
using DowJones.Infrastructure;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Syntax;

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
        public void AutoBind<T>(Func<Type, bool> predicate = null, Action<IBindingWhenInNamedWithOrOnSyntax<T>> bindingAction = null)
        {
            IEnumerable<Type> targetTypes =
                AssemblyRegistry.GetConcreteTypesDerivingFrom<T>();

            if (predicate != null)
                targetTypes = targetTypes.Where(predicate);

            BindToTypes(targetTypes, bindingAction);
        }

        public void BindToTypes<T>(IEnumerable<Type> targets, Action<IBindingWhenInNamedWithOrOnSyntax<T>> postBindingAction = null)
        {
            foreach (var target in targets)
            {
                var binding = Bind<T>().To(target);

                if (postBindingAction != null)
                    postBindingAction(binding);
            }
        }

        public void BindToConstants<T>(IEnumerable<T> targets, Action<IBindingWhenInNamedWithOrOnSyntax<T>> postBindingAction = null)
        {
            foreach (var target in targets)
            {
                var binding = Bind<T>().ToConstant(target);

                if (postBindingAction != null)
                    postBindingAction(binding);
            }
        }

        public IBindingInSyntax<T> BindToFactory<T, TFactory>() 
            where TFactory : IFactory
        {
            var provider = new FactoryProvider<TFactory>();
            return Bind<T>().ToProvider(provider);
        }

        public IBindingInSyntax<T> RebindToFactory<T, TFactory>() 
            where TFactory : IFactory
        {
            Unbind<T>();
            return BindToFactory<T, TFactory>();
        }

        protected abstract void OnLoad();
    }
}
