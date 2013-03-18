using Ninject;

namespace DowJones.Mapping
{
    /// <summary>
    /// Locates and registers all of the TypeMappers that exist in the AssemblyRegistry
    /// </summary>
    public class TypeMappingRegistrationModule : DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            var locator = Kernel.Get<TypeMappingLocator>();
            
            var mapperDefinitions = locator.Locate(mapperType => (ITypeMapper)Kernel.Get(mapperType));

            Mapper.Instance.Register(mapperDefinitions);
        }
    }
}
