using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Threading;
using DowJones.Infrastructure;
using DowJones.Properties;
using log4net;

namespace DowJones.Globalization
{
    public class ResourceTextManagerFactory : Factory<IResourceTextManager>
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof (ResourceTextManagerFactory));

        private static readonly Lazy<ResourceManager> Manager = 
            new Lazy<ResourceManager>(() => CreateFromResourceAssembly());

        public override IResourceTextManager Create()
        {
            return new ResourceTextManager(Manager.Value);
        }

        public static ResourceManager CreateFromResourceAssembly(string assemblyName = null, string resourceName = null)
        {
            assemblyName = assemblyName ?? Settings.Default.ResourceManagerAssemblyName;
            resourceName = resourceName ?? Settings.Default.ResourceManagerResourceName;

            ResourceManager resourceManager = null;

            try
            {
                Log.DebugFormat("Attempting to load assembly: {0}", assemblyName);
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

            return resourceManager;
        }
    }
}