using System;
using Ninject;

namespace DowJones.DependencyInjection
{
    public class NinjectServiceLocator : IServiceLocator
    {
        private bool disposed;
        private readonly IKernel _kernel;

        public IKernel Kernel
        {
            get { return _kernel; }
        }

        public NinjectServiceLocator(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Dispose()
        {
            if (disposed)
                return;

            _kernel.Dispose();
            disposed = true;
        }

        public IServiceLocator Register<TService>(Func<IServiceLocator, TService> factory)
        {
            throw new NotImplementedException();
        }

        public IServiceLocator Register<TService>(TService service)
        {
            _kernel.Bind<TService>().ToConstant(service).InRequestScope();
            return this;
        }

        public void Inject(object target)
        {
            _kernel.Inject(target);
        }

        public object Resolve(Type targetType)
        {
            return _kernel.Get(targetType);
        }

        public T Resolve<T>()
        {
            return _kernel.Get<T>();
        }

        public T Resolve<T>(string name)
        {
            return _kernel.Get<T>(name);
        }

    }
}