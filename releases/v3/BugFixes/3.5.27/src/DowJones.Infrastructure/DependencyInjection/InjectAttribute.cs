using System;

namespace DowJones.DependencyInjection
{
    public class InjectAttribute : Attribute
    {
        public string Reason { get; set; }

        public InjectAttribute(string reason)
        {
            Reason = reason;
        }
    }
}
