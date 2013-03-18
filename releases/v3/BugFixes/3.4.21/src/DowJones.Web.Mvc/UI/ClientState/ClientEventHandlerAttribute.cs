using System;

namespace DowJones.Web.Mvc.UI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ClientEventHandlerAttribute : ClientStateAttribute
    {
        public ClientEventHandlerAttribute()
            : this(default(string))
        {

        }

        public ClientEventHandlerAttribute(string name)
            : base(name)
        {

        }
    }
}