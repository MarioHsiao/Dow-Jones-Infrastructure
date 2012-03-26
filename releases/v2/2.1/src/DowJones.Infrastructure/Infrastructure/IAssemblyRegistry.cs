using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DowJones.Infrastructure
{
    public interface IAssemblyRegistry
    {
        IEnumerable<Assembly> Assemblies { get; }
        IEnumerable<ManifestResourceInfo> EmbeddedResources { get; }
        IEnumerable<Type> ExportedTypes { get; }
        IEnumerable<Type> ConcreteTypes { get; }
        IEnumerable<Type> GetConcreteTypesDerivingFrom(Type baseType);
    }

// ReSharper disable InconsistentNaming
    public static class IAssemblyRegistryExtensions
// ReSharper restore InconsistentNaming
    {

        public static IEnumerable<Type> GetConcreteTypesDerivingFrom<T>(this IAssemblyRegistry registry)
        {
            return registry.GetConcreteTypesDerivingFrom(typeof (T));
        }

        public static IEnumerable<Type> GetKnownTypesOf<T>(this IAssemblyRegistry registry)
        {
            return registry.GetKnownTypesOf(typeof(T));
        }
        
        public static IEnumerable<Type> GetKnownTypesOf(this IAssemblyRegistry registry, Type baseType)
        {
            return registry.GetConcreteTypesDerivingFrom(baseType).Where(x => !x.IsGenericType);
        }
        
    }
}