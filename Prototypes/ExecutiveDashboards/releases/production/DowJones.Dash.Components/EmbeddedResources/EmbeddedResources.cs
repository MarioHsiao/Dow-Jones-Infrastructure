using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using DowJones.Dash.Components.EmbeddedResources;
using DowJones.Web;

#region Scripts

[assembly: WebResource(EmbeddedResources.Js.Counter, KnownMimeTypes.JavaScript)]

#endregion

namespace DowJones.Dash.Components.EmbeddedResources
{
	public static class EmbeddedResources
	{
		[ScriptResource(ResourceName = Counter, DependencyLevel = ClientResourceDependencyLevel.Independent, Name = "jquery-counter", DependsOn = new[]{"jquery"})]

		public static class Js
		{
			public const string BasePath = "DowJones.Dash.Components.EmbeddedResources.js.";
			public const string Counter = BasePath + "jquery.counter.js";
		}
	}
}
