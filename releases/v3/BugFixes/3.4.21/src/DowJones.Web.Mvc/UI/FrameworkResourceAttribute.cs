using System;
using DowJones.Web.Mvc.Resources;

namespace DowJones.Web.Mvc.UI
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class FrameworkResourceAttribute : ClientResourceAttribute
    {
        public FrameworkResourceAttribute(string resourceName)
            : base(resourceName)
        {
            DeclaringType = typeof(EmbeddedResources);
            DependencyLevel = ClientResourceDependencyLevel.MidLevel;
            ResourceName = resourceName;
            ResourceKind = ClientResourceKind.Script;
        }
    }

}
