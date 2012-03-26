using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Utilities.Formatters;

namespace DowJones.Utilities.Ajax
{
    [DataContract(Name = "tagType", Namespace = "")]
    public enum TagType
    {
        [EnumMember]
        Keyword,

        [EnumMember]
        Company,

        [EnumMember]
        NewsSubject,

        [EnumMember]
        Industry,

        [EnumMember]
        Region,

        [EnumMember]
        Author,

        [EnumMember]
        Executive,

        [EnumMember]
        Source,
    }

    public interface ITag
    {
        /// <summary>
        /// Gets or sets the text of the tag.
        /// </summary>
        /// <value>
        /// The text of the tag.
        /// </value>
        [DataMember(Name = "text")]
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the code associated with this tag.
        /// </summary>
        /// <value>
        /// The code associated with the tag.
        /// </value>
        [DataMember(Name = "code")]
        string Code { get; set; }

        /// <summary>
        /// Gets or sets the type of the tag.
        /// </summary>
        /// <value>
        /// The type of tag.
        /// </value>
        [DataMember(Name = "type")]
        TagType Type { get; set; }

        /// <summary>
        /// Gets or sets the url that is used when a tag is clicked.
        /// Leave empty or null to produce a non clickable tag.
        /// </summary>
        /// <value>
        /// The navigate URL.
        /// </value>
        [DataMember(Name = "navigateUrl")]
        string NavigateUrl { get; set; }

        /// <summary>
        /// Gets or sets the weight of the tag.
        /// </summary>
        /// <value>
        /// The tag weight.
        /// </value>
        [DataMember(Name = "tagWeight")]
        DoubleNumber TagWeight { get; set; }

        /// /// <summary>
        /// Gets or sets the weight of the tag.
        /// </summary>
        /// <value>
        /// The tag weight.
        /// </value>
        [DataMember(Name = "distributionIndex")]
        int DistributionIndex { get; set; }
        
        /// <summary>
        /// Gets or sets the html attribute of the tag.
        /// </summary>
        /// <value>
        /// The tag weight.
        /// </value>
        [DataMember(Name = "htmlAttributes")]
        IDictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets the snippet/tooltip text of the tag.
        /// The text should displayed when the cursor hovers over the tag.
        /// </summary>
        /// <value>
        /// The snippet.
        /// </value>
        [DataMember(Name = "snippet")]
        string Snippet { get; set; }

        [DataMember(Name = "searchContextRef")]
        string SearchContextRef { get; set; }
    }
}