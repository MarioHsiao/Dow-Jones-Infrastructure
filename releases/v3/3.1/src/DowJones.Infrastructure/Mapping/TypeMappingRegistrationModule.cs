using System;
using System.Linq;
using Ninject;

namespace DowJones.Mapping
{
    /// <summary>
    /// Locates and registers all of the TypeMappers that exist in the AssemblyRegistry
    /// </summary>
    public class TypeMappingRegistrationModule : DependencyInjection.DependencyInjectionModule
    {
        protected static readonly Type TypeMappingStrategyInterfaceType = typeof(ITypeMapper);

        protected override void OnLoad()
        {
            var mappingStrategyTypes =
                AssemblyRegistry.GetConcreteTypesDerivingFrom(TypeMappingStrategyInterfaceType);

            var genericMappers =
                from mappingStrategy in mappingStrategyTypes
                where mappingStrategy != typeof (TypeMapper)
                from @interface in mappingStrategy.GetInterfaces()
                where TypeMappingStrategyInterfaceType.IsAssignableFrom(@interface)
                      && @interface.GetGenericArguments().Any()
                let args = @interface.GetGenericArguments()
                where args.Count() == 2
                group @interface by mappingStrategy into groupedInterfaces
                select groupedInterfaces;

            var mapperDefinitions =
                from interfaceGroup in genericMappers
                let mapperType = interfaceGroup.Key
                from @interface in interfaceGroup
                let genericArgs = @interface.GetGenericArguments().ToArray()
                let sourceType = genericArgs[0]
                let targetType = genericArgs.Count() == 2 ? genericArgs[1] : null
                where sourceType != typeof(Object)
                select new TypeMapperDefinition
                {
                    SourceType = sourceType,
                    TargetType = targetType == typeof(Object) ? null : targetType,
                    Factory = () => (ITypeMapper)Kernel.Get(mapperType)
                };

            Mapper.Instance.Register(mapperDefinitions);
        }
    }
}
