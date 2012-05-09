using System;
using System.Collections.Generic;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.Diagnostics
{
    public class ValidateClientUI
    {
        public IEnumerable<Type> ViewComponentTypes { get; set; }

        public ValidateClientUI()
            : this(ServiceLocator.Resolve<IAssemblyRegistry>().GetConcreteTypesDerivingFrom<IViewComponent>())
        {
        }

        public ValidateClientUI(IEnumerable<Type> viewComponentTypes)
        {
            ViewComponentTypes = viewComponentTypes;
        }
    }
}
