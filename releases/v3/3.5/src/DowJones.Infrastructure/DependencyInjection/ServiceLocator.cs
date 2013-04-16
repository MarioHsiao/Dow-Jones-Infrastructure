using System;

namespace DowJones.DependencyInjection
{
    public interface IServiceLocator : IDisposable
    {
        void Inject(object target);
        object Resolve(Type type);
        TService Resolve<TService>();
        TService Resolve<TService>(string name);
        IServiceLocator Register<TService>(TService service);
        IServiceLocator Register<TService>(Func<IServiceLocator, TService> factory);
    }

    public static class ServiceLocator
    {
        public static IServiceLocator Current { get; set; }

        public static void Initialize()
        {
            Initialize(new Ninject.NinjectServiceLocatorFactory());
        }

        public static void Initialize(IServiceLocatorFactory factory)
        {
            Current = factory.Create();
        }

        public static object Resolve(Type targetType)
        {
            var serviceLocator = Current;

            EnsureServiceLocatorInitialized(serviceLocator);

            return serviceLocator.Resolve(targetType);
        }

        public static T Resolve<T>()
        {
            var serviceLocator = Current;

            EnsureServiceLocatorInitialized(serviceLocator);

            return serviceLocator.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            var serviceLocator = Current;

            EnsureServiceLocatorInitialized(serviceLocator);

            return serviceLocator.Resolve<T>(name);
        }

        public static T Resolve<T>(Type targetType)
        {
            var target = Resolve(targetType);
            if (target is T)
            {
                return (T)target;
            }
            return default(T);
        }

        private static void EnsureServiceLocatorInitialized(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null)
            {
                throw new ServiceLocatorNotInitializedException();
            }
        }

        internal class ServiceLocatorNotInitializedException : Exception
        {
        }
    }
}
