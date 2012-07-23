using System;

namespace DowJones.Web.Mvc.UI.Exceptions
{
    public class SectionNotImplementedException : Exception
    {
        public Type ComponentType { get; private set; }
        public string SectionName { get; private set; }

        public SectionNotImplementedException(Type componentType, string sectionName)
            : base(GetMessage(componentType, sectionName))
        {
            ComponentType = componentType;
            SectionName = sectionName;
        }

        private static string GetMessage(Type componentType, string sectionName)
        {
            var componentTypeName = (componentType == null)
                                        ? "[Unspecified]"
                                        : componentType.FullName ?? componentType.Name;

            return string.Format("section '{0}' is required but was not implemented in component {1}",
                                 sectionName, componentTypeName);
        }
    }
}