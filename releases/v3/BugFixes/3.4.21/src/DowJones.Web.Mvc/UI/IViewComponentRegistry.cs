using System;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI
{
    public interface IViewComponentRegistry
    {
        IEnumerable<IViewComponent> GetRegisteredComponents();

        void Register(IViewComponent component);
        bool ContainsComponentOfType<TViewComponent>();
        bool ContainsComponentOfType(Type componentType);
    }
}
