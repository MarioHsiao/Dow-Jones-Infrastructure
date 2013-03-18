using System;

namespace DowJones.Web.Mvc.UI.Exceptions
{
    public class UnknownComponentTypeException : Exception
    {
        public UnknownComponentTypeException(string componentName)
            : base(string.Format("Could not locate component type '{0}'", componentName))
        {
        }
    }
}