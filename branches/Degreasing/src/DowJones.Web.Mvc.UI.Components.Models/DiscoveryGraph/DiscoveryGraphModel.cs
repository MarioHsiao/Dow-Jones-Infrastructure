using DowJones.Models.Common;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.DiscoveryGraph
{
    /// <summary>
    /// Model for DiscoveryGraph Component
    /// </summary>
    public class DiscoveryGraphModel : ViewComponentModel
    {
		/// <summary>
		/// Whether the items can be dragged and re-arranged. 
		/// </summary>
		/// <remarks>
		/// Applicable only for vertical orientation.
		/// </remarks>
		[ClientProperty]
	    public bool Sortable { get; set; }

		/// <summary>
		/// Specifies whether the component can be scrolled or not
		/// </summary>
		[ClientProperty]
		public bool Scrollable { get; set; }

		/// <summary>
		/// Orientation of the graph: horizontal (default) or vertical
		/// </summary>
		[ClientProperty]
		public Orientation Orientation { get; set; }

		[ClientData]
		public Entities Data { get; set; }
    }
}