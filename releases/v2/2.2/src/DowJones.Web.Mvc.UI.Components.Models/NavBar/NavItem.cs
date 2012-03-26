using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using DowJones.Web.Mvc.UI.Components.Menu;

namespace DowJones.Web.Mvc.UI.Components.NavBar
{
    [JsonObject(MemberSerialization.OptIn)]
    public class NavItem
    {

        #region ..:: Constants ::..

        const string DefaultIconClass = "fi fi_gear";

        #endregion


        #region ..:: Client Properties ::..

        /// <summary>
        /// Gets or sets the custom DOM id of the tab.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        /// <remarks>
        /// Must maintain the uniqueness of Ids yourself if you're providing custom id. 
        /// For better maintainability, use different selectors other than ID based selectors.
        /// </remarks>
        [JsonProperty("id")]
        public string Id { get; set; }


        /// <summary>
        /// Gets or sets the label on tab.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [JsonProperty("label")]
        public string Label { get; set; }


        /// <summary>
        /// Gets or sets the tab's logical position.
        /// </summary>
        /// <value>
        /// The logical position. E.g, zeroth Nav Item can be shown at 3rd position
        /// </value>
        [JsonProperty("position")]
        public int Position { get; set; }


        /// <summary>
        /// Gets or sets any addtional meta data that needs to be passed on tab click event on client side.
        /// </summary>
        /// <value>
        /// The meta data object. Must be serailizable as JSON.
        /// </value>
        [JsonProperty("metaData")]
        public object MetaData { get; set; }


        /// <summary>
        /// Gets or sets the tab URL.
        /// </summary>
        /// <value>
        /// The URL string.
        /// </value>
        [JsonProperty("url")]
        public string Url
        {
            get { return string.IsNullOrWhiteSpace(_url) ? "javascript:void(0)" : _url; }
            set { _url = value; }
        }


        /// <summary>
        /// True if tab is shown as selected
        /// </summary>
        [JsonProperty("isSelected")]
        public bool IsSelected { get; set; }


        /// <summary>
        /// Gets or sets the context menu items that are shown on click of the gear icon.
        /// </summary>
        /// <value>
        /// List of menu items.
        /// </value>
        [JsonProperty("menuItems")]
        public IEnumerable<MenuItem> MenuItems { get; set; }


        #endregion


        /// <summary>
        /// Gets or sets the Tab is Selectable.
        /// </summary>
        /// <value>
        /// True if selectable, false if not.
        /// </value>
        public bool IsSelectable { get; set; }

        /// <summary>
        /// Gets or sets the CSS class for Action Item icon on the tab.
        /// </summary>
        /// <value>
        /// Icon's CSS class. Default is the gear icon.
        /// </value>
        public string IconClass { get; set; }


        /// <summary>
        /// Gets or sets the tooltip for the tabs.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public string Tooltip { get; set; }
        

        /// <summary>
        /// Gets or sets the additional CSS classes to render custom styles.
        /// </summary>
        /// <value>
        /// The CSS class(es).
        /// </value>
        /// <remarks>
        /// Appends to existing style. 
        /// To change default style, override the CssClass in your stylesheets.
        /// </remarks>
        public string CssClass { get; set; }
        

        private string _url;

        public bool HasMenuItems { get; set; }


        public NavItem()
        {
            IconClass = DefaultIconClass;
        }
    }


}
