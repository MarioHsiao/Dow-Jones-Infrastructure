using System;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Canvas.Mapping;

namespace DowJones.Web.Mvc.Mappings
{
    public class ViewComponentModelMappingRegistration : MappingRegistrationModule<ViewComponentBase>
    {
        protected override Func<ITypeMapper> CreateMapperFactory(Type targetType)
        {
            return () => new ViewComponentModelMapper(Kernel, targetType);
        }
    }
}
