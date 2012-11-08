using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Pages;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Syndication.Packages
{
     [DataContract(Name = "syndicationPackage", Namespace = "")]
     [KnownType(typeof(SyndicationPackage))]
    public class SyndicationPackage : AbstractHeadlinePackage
    {
        /// <summary>
        /// Gets or sets the feed title.
        /// </summary>
        /// <value>The feed title.</value>
        [DataMember(Name = "feedTitle")]
        [XmlElement(Type = typeof(string), ElementName = "feedTitle", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("feedTitle")]
        public string FeedTitle { get; set; }

        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        [DataMember(Name = "feedId")]
        [XmlElement(Type = typeof(string), ElementName = "feedId", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("feedId")]
        public string FeedId { get; set; }
         
        /// <summary>
        /// Gets or sets the HTML page for feed URI.
        /// </summary>
        /// <value>
        /// The HTML page feed URI.
        /// </value>
        [DataMember(Name = "htmlPageForFeedUri")]
        [XmlElement(Type = typeof(string), ElementName = "htmlPageForFeedUri", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("feedTitle")]
        public string HtmlPageForFeedUri { get; set; }

        /// <summary>
        /// Gets or sets the favicon URI.
        /// </summary>
        /// <value>
        /// The favicon URI.
        /// </value>
        [DataMember(Name = "faviconUri")]
        [XmlElement(Type = typeof(string), ElementName = "faviconUri", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("feedTitle")]
        public string FaviconUri { get; set; }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        [DataMember(Name = "feedType")]
        [XmlElement(Type = typeof(FeedType), ElementName = "feedType", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("feedType")]
        public FeedType FeedType { get; set; }
    }
}