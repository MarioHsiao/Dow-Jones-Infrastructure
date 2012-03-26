// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AssemblyExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Handlers;
using DowJones.Infrastructure;

namespace DowJones.Extensions
{
    public static class AssemblyExtensions
    {
        private static readonly Lazy<MethodInfo> GetWebResourceUrlMethod = new Lazy<MethodInfo>(() => {
            var method = 
                typeof(AssemblyResourceLoader).FindMembers(
                    MemberTypes.Method, 
                    BindingFlags.Static | BindingFlags.NonPublic,
                    (member, obj) => member.Name.ToString() == obj.ToString(), 
                    "GetWebResourceUrlInternal"
                )
                .OfType<MethodInfo>()
                .Single(x => x.GetParameters().Length == 5);

            return method;
        });

        public static string GetWebResourceUrl(this Assembly targetAssembly, string resourceName)
        {
            Guard.IsNotNullOrEmpty(resourceName, "resourceName");

            var resourceUrl = (string)GetWebResourceUrlMethod.Value.Invoke(null, new object[] { targetAssembly, resourceName, false, false, null });

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
