using System.Collections.Generic;
using DowJones.Globalization;

namespace DowJones.Token
{
    public abstract class AbstractTokenBase : Dictionary<string, string>
    {
        /// <summary>
        /// Gets the value of the token property based on the property <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Token property name.</param>
        /// <returns>The value of the token property based on the property <paramref name="name"/>.  
        /// If the property is not present, then returns <code>null</code>.</returns>
        public string GetTokenByName(string name)
        {
            return ResourceTextManager.Instance.GetString(name);
        }
    }
}
