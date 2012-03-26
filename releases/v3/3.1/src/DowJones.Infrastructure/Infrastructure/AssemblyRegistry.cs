using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;

namespace DowJones.Infrastructure
{
    public class AssemblyRegistry : IAssemblyRegistry
    {
        public static readonly string[] DefaultFilePatterns = new [] { "DowJones" };

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


        public AssemblyRegistry(params Assembly[] assemblies)
        {
            _assemblies = assemblies ?? Enumerable.Empty<Assembly>();
        }


        public IEnumerable<Type> GetConcreteTypesDerivingFrom(Type baseType)
        {
            IEnumerable<Type> concreteDerivedTypes =
                ConcreteTypes.Where(baseType.IsAssignableFrom);

            return concreteDerivedTypes;
        }


        public static AssemblyRegistry Create(IEnumerable<string> filePatterns = null)
        {
            filePatterns = filePatterns ?? DefaultFilePatterns;

            if (HttpContext.Current == null)
                return CreateFromAssemblyReferences(Assembly.GetEntryAssembly(), filePatterns);
            else
                return CreateFromBuildManager(filePatterns);
        }

        public static AssemblyRegistry CreateFromBuildManager(IEnumerable<string> filePatterns = null)
        {
            filePatterns = filePatterns ?? DefaultFilePatterns;

            var assemblies =
                    from pattern in filePatterns
                    from assembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>()
                    let assemblyName = assembly.GetName().Name
                    where assemblyName.StartsWith(pattern)
                    select assembly;

            return new AssemblyRegistry(assemblies.ToArray());
        }

        public static AssemblyRegistry CreateFromAssemblyReferences(Assembly sourceAssembly, IEnumerable<string> filePatterns)
        {
            filePatterns = filePatterns ?? DefaultFilePatterns;

            var referencedAssemblies =
                from assembly in sourceAssembly.GetReferencedAssemblies()
                from pattern in filePatterns
                where assembly.Name.StartsWith(pattern)
                select assembly;

            var assemblies =
                new[] { sourceAssembly }
                .Union(referencedAssemblies.Select(Assembly.Load));

            return new AssemblyRegistry(assemblies.ToArray());
        }

        public static implicit operator AssemblyRegistry(Assembly[] assemblies)
        {
            return new AssemblyRegistry(assemblies);
        }
    }
}
