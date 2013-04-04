using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Ajax.TagCloud;
using DowJones.Formatters;

namespace DowJones.Pages
{
    [DataContract(Namespace = "")]
    public class AbstractTag : ITag
    {
        /// <summary>
        /// Gets or sets the text of the tag.
        /// </summary>
        /// <value>
        /// The text of the tag.
        /// </value>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the code associated with this tag.
        /// </summary>
        /// <value>
        /// The code associated with the tag.
        /// </value>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the type of the tag.
        /// </summary>
        /// <value>
        /// The type of tag.
        /// </value>
        [DataMember(Name = "type")]
        public TagType Type { get; set; }

        /// <summary>
        /// Gets or sets the url that is used when a tag is clicked.
        /// Leave empty or null to produce a non clickable tag.
        /// </summary>
        /// <value>
        /// The navigate URL.
        /// </value>
        [DataMember(Name = "navigateUrl")]
        public string NavigateUrl { get; set; }

        /// <summary>
        /// Gets or sets the weight of the tag.
        /// </summary>
        /// <value>
        /// The tag weight.
        /// </value>
        [DataMember(Name = "tagWeight")]
        public DoubleNumber TagWeight { get; set; }

        [DataMember(Name = "distributionIndex")]
        public int DistributionIndex { get; set; }

        /// <summary>
        /// Gets or sets the snippet/tooltip text of the tag.
        /// The text should displayed when the cursor hovers over the tag.
        /// </summary>
        /// <value>
        /// The snippet.
        /// </value>
        [DataMember(Name = "snippet")]
        public string Snippet { get; set; }

        /// <summary>
        /// Gets or sets the css class of the tag.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        ///		<para>Gets or sets the javascript function that will be called on element click.</para>
        ///		<para>Leave empty or null if no client click handler is needed.</para>
        /// </summary>
        public string OnClientClick { get; set; }

        /// <summary>
        /// Gets or sets the additional html attributes for this tag.
        /// </summary>
        public IDictionary<string, object> HtmlAttributes { get; set; }

        [DataMember(Name = "searchContextRef")]
        public string SearchContextRef { get; set; }
    }
}