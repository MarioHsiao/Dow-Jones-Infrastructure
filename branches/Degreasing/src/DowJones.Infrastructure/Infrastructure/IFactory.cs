using System;
using Ninject;

namespace DowJones.Infrastructure
{
    public interface IFactory
    {
        object Create();
        Type Type { get; }
    }

    public interface IFactory<out T> : IFactory
    {
        new T Create();
    }

    public abstract class Factory<T> : IFactory<T>
    {
        [DependencyInjection.Inject("Avoding derived constructors having to know about Ninject")]
        public IKernel Kernel { get; set; }

        object IFactory.Create()
        {
            return Create();
        }

        public abstract T Create();

        public Type Type
        {
            get { return typeof(T); }
        }
    }
}
