using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DowJones.Infrastructure
{
    public class CustomAssemblyRegistry : IAssemblyRegistry
    {
        public IEnumerable<Assembly> Assemblies
        {
            get { return _assemblies; }
        }
        private readonly IEnumerable<Assembly> _assemblies;

        public IEnumerable<Type> ConcreteTypes
        {
            get
            {
                IEnumerable<Type> concreteTypes =
                    ExportedTypes
                        .Where(type => !type.IsAbstract)
                        .Where(type => !type.IsInterface);

                return concreteTypes;
            }
        }

        public IEnumerable<ManifestResourceInfo> EmbeddedResources
        {
            get
            {
                IEnumerable<ManifestResourceInfo> resources =
                    from assembly in Assemblies
                    from resource in assembly.GetManifestResourceNames()
                    select assembly.GetManifestResourceInfo(resource);
                
                return resources;
            }
        }

        public IEnumerable<Type> ExportedTypes
        {
            get
            {
                _exportedTypes = _exportedTypes 
                    ?? _assemblies.SelectMany(x => x.GetExportedTypes());
                
                return _exportedTypes;
            }
        }
        private IEnumerable<Type> _exportedTypes;


        public CustomAssemblyRegistry(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies ?? Enumerable.Empty<Assembly>();
        }

        public IEnumerable<Type> GetConcreteTypesDerivingFrom(Type baseType)
        {
            IEnumerable<Type> concreteDerivedTypes =
                ConcreteTypes.Where(baseType.IsAssignableFrom);

            return concreteDerivedTypes;
        }

    }
}
