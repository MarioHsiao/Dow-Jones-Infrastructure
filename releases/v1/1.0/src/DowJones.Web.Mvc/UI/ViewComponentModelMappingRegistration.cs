using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentModelMappingRegistration : DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            IEnumerable<Type> viewComponentModelMappingTypes = 
                AssemblyRegistry.GetConcreteTypesDerivingFrom(typeof(ViewComponentBase));

            IEnumerable<ViewComponentModelMapping> mappings =
                viewComponentModelMappingTypes
                    .ToGenericBaseTypeMappings()
                    .Select(x => new ViewComponentModelMapping(x));

            foreach (var mapping in mappings)
            {
                Bind<ViewComponentModelMapping>()
                    .ToConstant(mapping)
                    .InSingletonScope();
            }
        }
    }
}
