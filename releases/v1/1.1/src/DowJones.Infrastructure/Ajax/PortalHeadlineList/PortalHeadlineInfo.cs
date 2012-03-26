// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalHeadlineInfo.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Formatters;
using Newtonsoft.Json;

namespace DowJones.Tools.Ajax.PortalHeadlineList
{
    [CollectionDataContract(ItemName = "author", Namespace = "")]
    public class AuthorCollection : List<string>
    {
        public AuthorCollection() {}

        public AuthorCollection(IEnumerable<string> items) : base(items) {}
    }

    [CollectionDataContract(ItemName = "snippet", Namespace = "")]
    public class SnippetCollection : List<string>
    { 
        public SnippetCollection() {}

        public SnippetCollection(IEnumerable<string> items) : base(items) {}
    }

    [DataContract(Name = "portalHeadline", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "portalHeadline")]
    public class PortalHeadlineInfo
    {
        private Reference reference;

        [DataMember(Name = "sourceCode")]
        [JsonProperty("sourceCode")]
        public string SourceCode { get; set; }

        [DataMember(Name = "sourceDescriptor")]
        [JsonProperty("sourceDescriptor")]
        public string SourceDescriptor { get; set; }

        [DataMember(Name = "title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [DataMember(Name = "truncatedTitle")]
        [JsonProperty("truncatedTitle")]
        public string TruncatedTitle { get; set; }

        [DataMember(Name = "toolTip")]
        [JsonProperty("toolTip")]
        public string ToolTip { get; set; }

        [DataMember(Name = "headlineUrl")]
        [JsonProperty("headlineUrl")]
        public string HeadlineUrl { get; set; }

        [DataMember(Name = "publicationDateTime")]
        [JsonProperty("publicationDateTime")]
        public DateTime PublicationDateTime { get; set; }

        [DataMember(Name = "modificationDateTime")]
        [JsonProperty("modificationDateTime")]
        public DateTime ModificationDateTime { get; set; }

        [DataMember(Name = "modificationDateTimeDescriptor")]
        [JsonProperty("modificationDateTimeDescriptor")]
        public string ModificationDateTimeDescriptor { get; set; }

        [DataMember(Name = "modificationTimeDescriptor")]
        [JsonProperty("modificationTimeDescriptor")]
        public string ModificationTimeDescriptor { get; set; }

        [DataMember(Name = "modificationDateDescriptor")]
        [JsonProperty("modificationDateDescriptor")]
        public string ModificationDateDescriptor { get; set; }

        [DataMember(Name = "publicationDateTimeDescriptor")]
        [JsonProperty("publicationDateTimeDescriptor")]
        public string PublicationDateTimeDescriptor { get; set; }

        [DataMember(Name = "publicationDateDescriptor")]
        [JsonProperty("publicationDateDescriptor")]
        public string PublicationDateDescriptor { get; set; }

        [DataMember(Name = "publicationTimeDescriptor")]
        [JsonProperty("publicationTimeDescriptor")]
        public string PublicationTimeDescriptor { get; set; }

        [DataMember(Name = "hasPublicationTime")]
        [JsonProperty("hasPublicationTime")]
        public bool HasPublicationTime { get; set; }

        [DataMember(Name = "authors")]
        [JsonProperty("author")]
        public AuthorCollection Authors { get; set; }

        [DataMember(Name = "reference")]
        [JsonProperty("reference")]
        public Reference Reference
        {
            get { return reference ?? (reference = new Reference()); }
            set { reference = value; }
        }

        [DataMember(Name = "snippets")]
        [JsonProperty("snippet")]
        public SnippetCollection Snippets { get; set; }

        [DataMember(Name = "contentCategoryDescriptor")]
        [JsonProperty("contentCategoryDescriptor")]
        public string ContentCategoryDescriptor { get; set; }

        [DataMember(Name = "contentSubCategoryDescriptor")]
        [JsonProperty("contentSubCategoryDescriptor")]
        public string ContentSubCategoryDescriptor { get; set; }

        [DataMember(Name = "wordCount")]
        [JsonProperty("wordCount")]
        public WholeNumber WordCount { get; set; }

        [DataMember(Name = "wordCountDescriptor")]
        [JsonProperty("wordCountDescriptor")]
        public string WordCountDescriptor { get; set; }

        /// <summary>
        /// Gets or sets base language {en,fr,es,de ...}
        /// </summary>
        [DataMember(Name = "baseLanguage")]
        [JsonProperty("baseLanguage")]
        public string BaseLanguage { get; set; }

        /// <summary>
        /// Gets or sets translated language descriptor
        /// </summary>
        [DataMember(Name = "baseLanguageDescriptor")]
        [JsonProperty("baseLanguageDescriptor")]
        public string BaseLanguageDescriptor { get; set; }

        [DataMember(Name = "thumbnailImage")]
        [JsonProperty("thumbnailImage")]
        public ThumbnailImage ThumbnailImage { get; set; }

        /// <summary>
        /// Gets or sets time for a movie only applied to Multimedia content.
        /// </summary>
        [DataMember(Name = "mediaLength")]
        [JsonProperty("mediaLength")]
        public string MediaLength { get; set; }
    }
}
