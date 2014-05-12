using System;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Mapping;
using DowJones.Mapping.Types;

namespace DowJones.Web.Mvc.UI.Canvas.Mapping
{
    public abstract class MappingRegistrationModule<TMappingTarget> : DependencyInjectionModule
    {
        protected abstract Func<ITypeMapper> CreateMapperFactory(Type targetType);

        protected override void OnLoad(IContainer container)
        {
            var targetTypes = AssemblyRegistry.GetConcreteTypesDerivingFrom(typeof(TMappingTarget));

            var genericMappings = targetTypes.ToGenericBaseTypeMappings();

	        var mappingDefinitions =
		        from mapping in genericMappings
		        let targetType = mapping.DeclaringType
		        select new TypeMapperDefinition(mapping)
		        {
			        Factory = CreateMapperFactory(targetType)
		        };

            Mapper.Instance.Register(mappingDefinitions);
        }
    }
}