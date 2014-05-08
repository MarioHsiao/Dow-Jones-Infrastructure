using System;
using System.Web;
using DowJones.Extensions.Web;

namespace DowJones.Web.ClientResources
{
    public class ClientResourceModuleName
    {
        public string ClientResourceName { get; private set; }

        public string Value { get; private set; }


        public ClientResourceModuleName(string resourceName)
        {
            ClientResourceName = resourceName;
            Value = Parse(resourceName);
        }

        private static string Parse(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                return resourceName;

            var module = resourceName;

            if (module.EndsWith(".js"))
            {
                module = module.Substring(0, resourceName.Length - 3);
            }

            return module;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(ClientResourceModuleName name)
        {
            return name.Value;
        }
    }
}
