using System;
using DowJones.Mapping;
using Ninject;

namespace DowJones.Web.Mvc.UI.Canvas.Mapping
{
    public class FactivaModuleModelMapper : TypeMapper
    {
        public FactivaModuleModelMapper(IKernel kernel, Type targetType)
            : base(source => kernel.Get(targetType))
        {
        }
    }
}