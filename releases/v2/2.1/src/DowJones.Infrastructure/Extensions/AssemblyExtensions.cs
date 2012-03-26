// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AssemblyExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Web;
using System.Web.Handlers;
using DowJones.Infrastructure;

namespace DowJones.Extensions
{
    public static class AssemblyExtensions
    {
        private static readonly object GetWebResourceUrlLock = new object();
        private static MethodInfo _getWebResourceUrlMethod;

        public static string GetWebResourceUrl(this Assembly targetAssembly, string resourceName)
        {
            Guard.IsNotNullOrEmpty(resourceName, "resourceName");

            if (_getWebResourceUrlMethod == null)
            {
                lock (GetWebResourceUrlLock)
                {
                    if (_getWebResourceUrlMethod == null)
                    {
                        _getWebResourceUrlMethod =
                            typeof(AssemblyResourceLoader).GetMethod(
                                "GetWebResourceUrlInternal",
                                BindingFlags.NonPublic | BindingFlags.Static);
                    }
                }
            }

            var resourceUrl = (string) _getWebResourceUrlMethod.Invoke(null, new object[] { targetAssembly, resourceName, false, false, null });

            // If we're in debug mode, add the Resource Name to the URL
            if (Properties.Settings.Default.IsDebugMode)
            {
                var encodedName = HttpUtility.UrlEncode(resourceName);
                var queryParameter = string.Format("ResourceName={0}&", encodedName);
                var queryStringStart = resourceUrl.IndexOf('?');
                resourceUrl = resourceUrl.Insert(queryStringStart + 1, queryParameter);
            }

            return resourceUrl;
        }
    }
}
