using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Infrastructure;

namespace DowJones.Mapping
{
    internal class TypeMappingLocator
    {
        protected static readonly Type TypeMappingStrategyInterfaceType = typeof(ITypeMapper);

        private readonly IAssemblyRegistry _assemblyRegistry;


        public TypeMappingLocator(IAssemblyRegistry assemblyRegistry)
        {
            _assemblyRegistry = assemblyRegistry;
        }


        public IEnumerable<TypeMapperDefinition> Locate(Func<Type, ITypeMapper> typeMapperFactory)
        {
            var mappingStrategyTypes =
                _assemblyRegistry.GetConcreteTypesDerivingFrom(TypeMappingStrategyInterfaceType);

            var genericMappers =
                from mappingStrategy in mappingStrategyTypes
                where mappingStrategy != typeof (TypeMapper)
                from @interface in mappingStrategy.GetInterfaces()
                where TypeMappingStrategyInterfaceType.IsAssignableFrom(@interface)
                      && @interface.GetGenericArguments().Any()
                let args = @interface.GetGenericArguments()
                where args.Count() == 2
                group @interface by mappingStrategy
                into groupedInterfaces
                select groupedInterfaces;

            var mapperDefinitions =
                from interfaceGroup in genericMappers
                let mapperType = interfaceGroup.Key
                from @interface in interfaceGroup
                let genericArgs = @interface.GetGenericArguments().ToArray()
                let sourceType = genericArgs[0]
                let targetType = genericArgs.Count() == 2 ? genericArgs[1] : null
                where sourceType != typeof (Object)
                select new TypeMapperDefinition
                           {
                               SourceType = sourceType,
                               TargetType = targetType == typeof (Object) ? null : targetType,
                               Factory = () => typeMapperFactory(mapperType)
                           };
            return mapperDefinitions;
        }
    }
}