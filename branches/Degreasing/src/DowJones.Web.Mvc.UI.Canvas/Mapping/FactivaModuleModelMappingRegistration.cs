using System;
using DowJones.Mapping;

namespace DowJones.Web.Mvc.UI.Canvas.Mapping
{
    public class FactivaModuleModelMappingRegistration : MappingRegistrationModule<Module>
    {
        protected override Func<ITypeMapper> CreateMapperFactory(Type targetType)
        {
            return () => new FactivaModuleModelMapper(Kernel, targetType);
        }
    }
}
