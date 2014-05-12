using DowJones.DependencyInjection;

namespace DowJones.Mapping
{
    /// <summary>
    /// Locates and registers all of the TypeMappers that exist in the AssemblyRegistry
    /// </summary>
    public class TypeMappingRegistrationModule : DependencyInjectionModule
    {
        protected override void OnLoad(IContainer container)
        {
			// with SimpleInjector, GetInstance will lock the container, preventing further registrations.
			//var locator = Container.GetInstance<TypeMappingLocator>();
			var locator = new TypeMappingLocator(AssemblyRegistry);

			var mapperDefinitions = locator.Locate(mapperType => (ITypeMapper)Container.GetInstance(mapperType));

			Mapper.Instance.Register(mapperDefinitions);
        }
    }
}
