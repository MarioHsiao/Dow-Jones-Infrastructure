using System;

namespace DowJones.Web.Mvc.UI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ClientDataAttribute : ClientStateAttribute
    {
        public ClientDataAttribute()
            : this(default(string))
        {

        }

        public ClientDataAttribute(string name)
            : base(name)
        {
           
        }
    }
}