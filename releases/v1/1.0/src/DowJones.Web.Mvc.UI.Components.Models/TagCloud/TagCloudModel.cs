using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DowJones.Utilities.Ajax;
using System.Globalization;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class TagCloudModel :ViewComponentModel
    {
        public TagCloudModel()
        {
            TagCssClassPrefix = "dj_tag_cloud-weight";
        }

        /// <summary>
        /// Gets or sets the Tag objects of the tag cloud.
        /// </summary>
        public IEnumerable<ITag> Tags { get; set; }

        [ClientProperty("tagCloudCssPrefix")]
        public string TagCssClassPrefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable event firing].
        /// </summary>
        /// <value><c>true</c> if [enable event firing]; otherwise, <c>false</c>.</value>
        [ClientProperty("EnableEventFiring")]
        [DefaultValue(false)]
        public bool EnableEventFiring { get; set; }

        /// <summary>
        /// Gets or sets the client side handler function 
        /// for the tag's client click event.
        /// </summary>
        [ClientEventHandler("dj.TagCloud.OnTagItemClientClick")]
        public string OnTagItemClientClick { get; set; }

        public string GetLiAttributes(IDictionary<string,object> htmlArrtributes)
        {
            var sb = new StringBuilder();
            if (htmlArrtributes != null)
            {
                foreach (var attr in htmlArrtributes)
                {
                    sb.AppendFormat("{0}='{1}' ", attr.Key, Convert.ToString(attr.Value, CultureInfo.InvariantCulture));
                }
            }
            return sb.ToString();
        }

        public string GetToolTip(string toolTip)
        {
            return !string.IsNullOrEmpty(toolTip) ? string.Format("title='{0}'", toolTip) : string.Empty;
        }

        public string GetNavigateUrl(string url)
        {
            return !string.IsNullOrEmpty(url) ? url : "javascript:void(0);";
        }
    }
}
