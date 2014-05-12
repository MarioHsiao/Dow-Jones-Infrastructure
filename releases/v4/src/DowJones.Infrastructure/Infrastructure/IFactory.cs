using System;
using DowJones.DependencyInjection;

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
        [Inject("Avoding derived constructors having to know about IoC container")]
		public IContainer Kernel { get; set; }

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
