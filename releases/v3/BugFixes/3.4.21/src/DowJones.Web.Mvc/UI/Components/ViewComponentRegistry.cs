using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentRegistry : IViewComponentRegistry
    {
        private readonly IList<IViewComponent> _registeredComponents;

        public ViewComponentRegistry()
        {
            _registeredComponents = new List<IViewComponent>();
        }

        public IEnumerable<IViewComponent> GetRegisteredComponents()
        {
            return _registeredComponents.Distinct();
        }

        public void Register(IViewComponent component)
        {
            if (_registeredComponents.Contains(component))
                return;

            _registeredComponents.Add(component);

        }

        public bool ContainsComponentOfType<TViewComponent>()
        {
            var componentsOfType = _registeredComponents.OfType<TViewComponent>();
            return componentsOfType.Any();
        }

        public bool ContainsComponentOfType(Type componentType)
        {
            var componentsOfType = _registeredComponents.Where(x => x.GetType() == componentType);
            return componentsOfType.Any();
        }

    }
}