using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using DowJones.DependencyInjection;
using DowJones.Extensions.Web;
using DowJones.Infrastructure;
using log4net;
using System.Linq;

namespace DowJones.Web
{
	public class ClientResourcePopulator : IClientResourceProcessor
	{
        
		[Inject("Cross-cutting concern")]
		protected internal ILog Log { get; set; }
        
	    public HttpContextBase HttpContext { get; set; }

	    public ClientResourceProcessorOrder? Order
		{
			get { return ClientResourceProcessorOrder.Last; }
		}

		public ClientResourceProcessorKind ProcessorKind
		{
			get { return ClientResourceProcessorKind.Populator; }
		}

        public void Process(ProcessedClientResource resource)
		{
			var clientResource = resource.ClientResource;

			if (resource.ContentLoaded)
				return;

			resource.Content = GetResourceContent(clientResource);
			LoadDependentResourcesContent(resource);
			resource.ContentLoaded = true;

		}

		private string GetResourceContent(ClientResource clientResource)
		{
			Stream contentStream;

			if (clientResource is EmbeddedClientResource)
				contentStream = RetrieveInternalWebResourceStream(clientResource as EmbeddedClientResource);

			else if (clientResource.IsAbsoluteUrl())
				contentStream = RetrieveExternalWebResourceStream(clientResource);

			else
				contentStream = RetrieveLocalResourceStream(clientResource);


			if (contentStream == null || !contentStream.CanRead)
				throw new ClientResourcePopulationException(clientResource);

			string resourceContent;

			using (var reader = new StreamReader(contentStream))
			{
				resourceContent = reader.ReadToEnd();
			}
			return resourceContent;
		}

		private void LoadDependentResourcesContent(ProcessedClientResource resource)
		{
			foreach (var dependentResource in resource.ClientTemplates)
			{
				dependentResource.Content = GetResourceContent(dependentResource.ClientResource);
				dependentResource.ContentLoaded = true;
			}
		}

		protected internal virtual Stream RetrieveInternalWebResourceStream(EmbeddedClientResource embeddedClientResource)
		{
			Guard.IsNotNull(embeddedClientResource, "embeddedClientResource");

			Log.DebugFormat("Retrieving internal web resource {0}...", embeddedClientResource.Url);

			var resourceName = GetPreferredResourceName(embeddedClientResource);
			var stream = embeddedClientResource.TargetAssembly.GetManifestResourceStream(resourceName);

			if (HttpContext.DebugEnabled())
			{
				var localFile = RetrieveLocallyModifiedEmbeddedResourceFile(embeddedClientResource);
				stream = localFile ?? stream;
			}

			return stream;
		}

		/// <summary>
		/// prefer minified js resources in release mode
		/// </summary>
		private string GetPreferredResourceName(EmbeddedClientResource embeddedClientResource)
	    {
			var resourceName = embeddedClientResource.ResourceName;
			var assembly = embeddedClientResource.TargetAssembly;

			// ignore non js files
			if (!resourceName.EndsWith(".js"))
				return resourceName;

			// ignore minified resources
			if (resourceName.EndsWith(".min.js"))
				return resourceName;

			// the developer knows what he's doing!
			if (HttpContext.DebugEnabled())
				return resourceName;

			var minifiedResourceName = Regex.Replace(resourceName, "\\.js$", ".min.js");

			// we're in release mode! see if there's a min version.
			if (!assembly.GetManifestResourceNames().Contains(minifiedResourceName))
				return resourceName;

			embeddedClientResource.IsMinified = true;
			return minifiedResourceName;
	    }

		private Stream RetrieveLocallyModifiedEmbeddedResourceFile(EmbeddedClientResource embeddedClientResource)
		{
			var websiteDirectory = HttpContext.Server.MapPath(".");

			var assemblyName = embeddedClientResource.TargetAssembly.GetName().Name;
			var resourceNameWithoutAssembly = embeddedClientResource.ResourceName.Remove(0, assemblyName.Length + 1);
			var fileExtensionIndex = resourceNameWithoutAssembly.LastIndexOf('.', resourceNameWithoutAssembly.LastIndexOf('.') - 1);
			var rootServerPath =
				Path.Combine(websiteDirectory, "..", assemblyName,
							 resourceNameWithoutAssembly.Substring(0, fileExtensionIndex).Replace('.', '\\'));

			var filename = resourceNameWithoutAssembly.Substring(fileExtensionIndex + 1);

			var fullServerPath = Path.Combine(rootServerPath, filename);

			return File.Exists(fullServerPath) ? File.OpenRead(fullServerPath) : null;
		}

		protected internal virtual Stream RetrieveExternalWebResourceStream(ClientResource clientResource)
		{
			Guard.IsNotNull(clientResource, "clientResource");

			Log.DebugFormat("Retrieving remote resource {0}...", clientResource.Url);

			var request = WebRequest.Create(clientResource.Url);

			var response = request.GetResponse();
			if (response.ContentLength > 0)
			{
				Log.Warn("Could not retrieve remote resource: " + clientResource.Url);
				return null;
			}

			var responseStream = response.GetResponseStream();

			if (responseStream == null)
				return null;

			return new BufferedStream(responseStream);
		}

		protected internal virtual Stream RetrieveLocalResourceStream(ClientResource clientResource)
		{
			Guard.IsNotNull(clientResource, "clientResource");

			var url = clientResource.Url;

			Log.DebugFormat("Retrieving local resource {0}...", url);

			if (string.IsNullOrWhiteSpace(url) || !(url.StartsWith("~/") || url.StartsWith("/")))
				throw new ClientResourcePopulationException("Invalid local resource path: " + url);

		    var relativePath = clientResource.IsAbsoluteUrl() ? url : VirtualPathUtility.ToAbsolute(url);
			var filename = HttpContext.Server.MapPath(relativePath);
			var fileInfo = new FileInfo(filename);

			if (!fileInfo.Exists)
			{
				Log.WarnFormat("Local resource {0} does not exist", filename);
				return null;
			}

			return fileInfo.OpenRead();
		}


		public class ClientResourcePopulationException : Exception
		{
			public ClientResource ClientResource { get; private set; }

			public ClientResourcePopulationException(string message)
				: base(message)
			{
			}

			public ClientResourcePopulationException(ClientResource resource)
				: base(GetMessage(resource))
			{
				ClientResource = resource;
			}

			private static string GetMessage(ClientResource resource)
			{
				if (resource == null)
					return "Unknown Client Resource";

				return string.Format("Unable to retrieve content for {0} {1} ({2})",
									 resource.GetType().Name, resource.Name, resource.Url);
			}
		}

	}
}