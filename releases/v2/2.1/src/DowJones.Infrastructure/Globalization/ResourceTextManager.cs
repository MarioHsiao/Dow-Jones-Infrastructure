using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Security;
using System.Threading;
using System.Web;
using DowJones.Attributes;
using DowJones.DependencyInjection;
using DowJones.Properties;
using log4net;

namespace DowJones.Globalization
{
    public class ResourceTextManager : IResourceTextManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResourceTextManager));

        private readonly ResourceManager _resourceManager;

        public IEnumerable<KeyValuePair<string, string>> Aliases
        {
            get { return _aliases; }
        }
        private readonly IDictionary<string, string> _aliases;

        public static ResourceTextManager Instance
        {
            get { return instance ?? (instance = CreateFromResourceAssembly()); }
            internal set { instance = value; }
        }
        private static volatile ResourceTextManager instance;

        public bool IsResourceAssemblyLoaded
        {
            get { return instance != null; }
        }



        [Inject("Disambiguation between constructor with optional parameter")]
        protected internal ResourceTextManager(ResourceManager resourceManager)
            : this(resourceManager, null)
        {
        }

        protected internal ResourceTextManager(ResourceManager resourceManager, IDictionary<string, string> aliases)
        {
            _resourceManager = resourceManager;
            _aliases = aliases ?? new Dictionary<string, string>();
        }


        /// <summary>
        /// Creates an alias for an existing token
        /// </summary>
        /// <param name="existingToken">The existing token name to alias</param>
        /// <param name="alias">The alias (new token name) for the existing token</param>
        public void Alias(string existingToken, string alias)
        {
            _aliases.Add(alias, existingToken);
        }

        public string GetAssignedToken(object value)
        {
            if (value == null)
                return null;

            var type = value.GetType();
            var s = value.ToString();
            var assignedToken = (AssignedToken)Attribute.GetCustomAttribute(type.GetField(s), typeof(AssignedToken));
            return assignedToken != null ? GetString(assignedToken.Token) : string.Empty;
        }

        public string GetString(string name)
        {
            if (IsAlias(name))
                return GetString(_aliases[name]);

            string value = null;

            try
            {
                if (_resourceManager != null && Thread.CurrentThread.CurrentUICulture.LCID != 127)
                {
                    value = _resourceManager.GetString(name, Thread.CurrentThread.CurrentUICulture);
                }
            }
            catch (Exception ex)
            {
                Log.Debug(string.Format("Error resolving token {0}", name), ex);
            }

            return value ?? "${" + name + "}";
        }

        /// <summary>
        /// Resolve the user-rea framework/gateway error message
        /// </summary>
        /// <param name="errorNumber"></param>
        /// <returns></returns>
        public string GetErrorMessage(string errorNumber)
        {
            bool isLocalUser;
            try
            {
                isLocalUser = IsLocalUser();
            }
            catch (SecurityException)
            {
                isLocalUser = false;
                // do not log exception.  This will be thrown from xslt parsing.
            }
            catch (Exception)
            {
                isLocalUser = false;
                // do not log this error
            }

            var tokenPrefix = isLocalUser ? "errorForDev" : "errorForUser";
            var token = tokenPrefix + errorNumber.Replace("-", "Minus");
            var errorMessage = GetString(token);

            if (!isLocalUser
                && (string.Format("${{{0}}}", token).Equals(errorMessage, StringComparison.OrdinalIgnoreCase)))
            {
                token = tokenPrefix + "Minus1";
                errorMessage = GetString(token);
            }

            return errorMessage;
        }

        /// <summary>
        /// Is the given token name an alias for another token?
        /// </summary>
        public bool IsAlias(string tokenName)
        {
            return _aliases.ContainsKey(tokenName);
        }


        public static ResourceTextManager CreateFromResourceAssembly(string assemblyName = null, string resourceName = null)
        {
            resourceName = resourceName ?? Settings.Default.ResourceManagerResourceName;
            assemblyName = assemblyName ?? Settings.Default.ResourceManagerAssemblyName;

            ResourceManager resourceManager = null;

            try
            {
                var logger = LogManager.GetLogger(typeof(ResourceTextManager));
                logger.DebugFormat("Attempting to load assembly: {0}", assemblyName);
                var asm = Assembly.Load(assemblyName);

                if (Log.IsDebugEnabled)
                {
                    Debug.WriteLine(Thread.CurrentThread.CurrentUICulture);
                }

                resourceManager = new ResourceManager(resourceName, asm, null);
            }
            catch (Exception ex)
            {
                Log.Error("Error Getting ResourceAssembly", ex);
            }

            // TODO: Load Aliases

            return new ResourceTextManager(resourceManager);
        }

        public static bool IsLocalUser()
        {
            if (HttpContext.Current == null)
            {
                return false;
            }
            var hostName = Dns.GetHostName();
            var hostEntry = Dns.GetHostEntry(hostName);
            var ipAddress = hostEntry.AddressList[0].ToString();
            return ipAddress.Equals(HttpContext.Current.Request.ServerVariables["remote_addr"]);
        }

    }
}