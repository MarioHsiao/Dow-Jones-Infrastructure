using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Handlers;
using DowJones.Infrastructure;
using System.Web.UI;

namespace DowJones.Extensions
{
	public static class AssemblyExtensions
	{
		public static DateTime GetAssemblyTimestamp(this Assembly assembly)
		{
			return File.GetLastWriteTimeUtc(assembly.Location);
		}

		private static readonly Lazy<MethodInfo> GetWebResourceUrlMethod = new Lazy<MethodInfo>(() =>
		{


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


			// always add the resourcename since without it, flow player doesn't seem to work.
			var encodedName = HttpUtility.UrlEncode(resourceName);
			var queryParameter = string.Format("ResourceName={0}&", encodedName);
			var queryStringStart = resourceUrl.IndexOf('?');
			resourceUrl = resourceUrl.Insert(queryStringStart + 1, queryParameter);


			return resourceUrl;
		}
	}
}
