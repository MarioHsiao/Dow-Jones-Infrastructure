using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Mvc.UI.Exceptions
{
    public class AmbiguousComponentTypeException : Exception
    {
        public AmbiguousComponentTypeException(string componentName, IEnumerable<Type> componentTypes)
            : base(GetMessage(componentName, componentTypes))
        {
        }

        private static string GetMessage(string componentName, IEnumerable<Type> componentTypes)
        {
            var componentTypeNames = string.Join("; ", componentTypes.Select(x => x.FullName));

            return string.Format("Multiple component types found for '{0}': {1}", 
                                 componentName, componentTypeNames);
        }
    }
}