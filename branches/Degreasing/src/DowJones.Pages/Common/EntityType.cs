using System;

namespace DowJones.Pages.Common
{
    public enum EntityType
    {
        [NavigationControlId("co")]
        Companies = 0,

        [NavigationControlId("pe")]
        People = 1,

        [NavigationControlId("ns")]
        Subjects = 2,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NavigationControlIdAttribute : Attribute
    {
        public NavigationControlIdAttribute(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
