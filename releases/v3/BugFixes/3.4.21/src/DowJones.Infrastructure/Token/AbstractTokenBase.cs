using System.Collections.Generic;
using DowJones.DependencyInjection;
using DowJones.Globalization;

namespace DowJones.Token
{
    public abstract class AbstractTokenBase : Dictionary<string, string>
    {
        private readonly IResourceTextManager _resources;

        protected AbstractTokenBase(IResourceTextManager resources = null)
        {
            _resources = resources ?? ServiceLocator.Resolve<IResourceTextManager>();
        }

        /// <summary>
        /// Gets the value of the token property based on the property <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Token property name.</param>
        /// <returns>The value of the token property based on the property <paramref name="name"/>.  
        /// If the property is not present, then returns <code>null</code>.</returns>
        public string GetTokenByName(string name)
        {
            return _resources.GetString(name);
        }
    }
}
