using System;

namespace DowJones.Web.Mvc.UI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ClientTokenAttribute : ClientStateAttribute
    {
        public ClientTokenAttribute()
        {
            // Always merge IDictionary values into 
            // the client state tokens dictionary
            Merge = true;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ClientTokensAttribute : ClientStateAttribute
    {
        public ClientTokensAttribute()
        {
            // Always merge IDictionary values into 
            // the client state tokens dictionary
            Merge = true;
        }
    }
}