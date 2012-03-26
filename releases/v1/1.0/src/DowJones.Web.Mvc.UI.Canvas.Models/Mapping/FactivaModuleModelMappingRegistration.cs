using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.UI.Canvas.Mapping
{
    public class FactivaModuleModelMappingRegistration : DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            IEnumerable<Type> factivaModuleModelMappingTypes = 
                AssemblyRegistry.GetConcreteTypesDerivingFrom(typeof (Module));

            IEnumerable<FactivaModuleModelMapping> mappings =
                factivaModuleModelMappingTypes
                    .ToGenericBaseTypeMappings()
                    .Select(x => new FactivaModuleModelMapping(x));

            foreach (var mapping in mappings)
            {
                Bind<FactivaModuleModelMapping>()
                    .ToConstant(mapping)
                    .InSingletonScope();
            }
        }
    }
}
