// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Utilities.Ajax;
using DowJones.Utilities.Ajax.TagCloud;
using DowJones.Utilities.Formatters;
using DowJones.Web.Mvc.UI.Models.Company;
using DowJones.Web.Mvc.UI.Models.Core;

namespace DowJones.Web.Mvc.Models.News
{
    [DataContract(Name = "newsDataPoint", Namespace = "")]
    public class NewsDataPoint : IDataPoint
    {
        [DataMember(Name = "date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "dateDisplay")]
        public string DateDisplay { get; set; }

        [DataMember(Name = "dataPoint")]
        public Number DataPoint { get; set; }
    }

    [CollectionDataContract(Name = "newsDataPoints", Namespace = "")]
    public class NewsDataPointCollection : List<NewsDataPoint>
    {

    }


    [CollectionDataContract(Name = "tags", ItemName = "tag", Namespace = "")]
    public class TagCollection : List<ITag>
    {
        public TagCollection()
            : base()
        {
        }

        public TagCollection(IEnumerable<ITag> tags)
            : base(tags)
        {
        }
    }

    [DataContract(Name = "historicalNewsDataResult", Namespace = "")]
    public class HistoricalNewsDataResult
    {
        public HistoricalNewsDataResult()
        {
            DataPoints = new NewsDataPointCollection();
        }
        [DataMember(Name = "dataPoints")]
        public NewsDataPointCollection DataPoints { get; set; }

        [DataMember(Name = "frequency")]
        public DataPointFrequency? Frequency { get; set; }
    }

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

    /// <summary>
    /// Represents a single tag in the cloud
    /// </summary>
    [DataContract(Name = "tag", Namespace = "")]
    [KnownType(typeof(Tag))]
    public class Tag : AbstractTag
    {
    }

    public class DiscoveryDataResult
    {

    }
}
