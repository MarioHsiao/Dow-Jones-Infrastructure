using System;

namespace DowJones.DependencyInjection
{
    public interface IServiceLocator
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

        public static object Resolve(Type targetType)
        {
            IServiceLocator serviceLocator = Current;

            EnsureServiceLocatorInitialized(serviceLocator);

            return serviceLocator.Resolve(targetType);
        }

        public static T Resolve<T>()
        {
            IServiceLocator serviceLocator = Current;

            EnsureServiceLocatorInitialized(serviceLocator);

            return serviceLocator.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            IServiceLocator serviceLocator = Current;

            EnsureServiceLocatorInitialized(serviceLocator);

            return serviceLocator.Resolve<T>(name);
        }

        public static T Resolve<T>(Type targetType)
        {
            object target = Resolve(targetType);

            if (target is T)
                return (T)target;
            else
                return default(T);
        }

        private static void EnsureServiceLocatorInitialized(IServiceLocator serviceLocator)
        {
            if(serviceLocator == null)
                throw new ServiceLocatorNotInitializedException();
        }

        internal class ServiceLocatorNotInitializedException : Exception
        {
        }
    }
}
