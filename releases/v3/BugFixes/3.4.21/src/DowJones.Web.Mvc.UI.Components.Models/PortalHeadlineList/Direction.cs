using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Components.PortalHeadlineList
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Direction
	{
		Vertical,
		Horizontal
	}
}
