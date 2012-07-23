using System;
using DowJones.Mapping;
using Ninject;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentModelMapper : TypeMapper
    {
        public ViewComponentModelMapper(IKernel kernel, Type targetType)
            : base(source => (ViewComponentBase)kernel.Get(targetType))
        {
        }
    }
}