using System;
using System.Linq;
using DowJones.Infrastructure;
using Ninject;
using Ninject.Activation;

namespace DowJones.DependencyInjection.Ninject
{
    internal class FactoryProvider<TFactory> : IProvider
        where TFactory : IFactory
    {
        public virtual object Create(IContext context)
        {
            return context.Kernel.Get<TFactory>().Create();
        }

        public virtual Type Type
        {
            get
            {
                var factoryType = typeof (TFactory);

                if(factoryType.ContainsGenericParameters)
                {
                    var genericParameters = factoryType.GetGenericParameterConstraints();
                    
                    if(genericParameters.Count() == 1)
                        return genericParameters.First();
                }

                throw new NotSupportedException("FactoryProvider only support IFactory<T> factories");
            }
        }
    }
}