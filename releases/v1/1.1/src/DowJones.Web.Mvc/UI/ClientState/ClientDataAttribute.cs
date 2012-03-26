using System;

namespace DowJones.Web.Mvc.UI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ClientDataAttribute : ClientStateAttribute
    {
    }
}