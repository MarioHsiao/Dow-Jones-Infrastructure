// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeadlineInfo.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Formatters;

namespace DowJones.Ajax.HeadlineList
{
    /// <summary>
    /// The thumbnail image.
    /// </summary>
    [DataContract(Name = "thumbnailImage", Namespace = "")]
    public class ThumbnailImage
    {
        /// <summary>
        /// The guid.
        /// </summary>
        [DataMember(Name="guid")]
        public string GUID;

        /// <summary>
        /// The src.
        /// </summary>
        [DataMember(Name = "src")]
        public string SRC;

        /// <summary>
        /// The uri.
        /// </summary>
        [DataMember(Name = "uri")]
        public string URI;
    }

    /// <summary>
    /// The truncation.
    /// </summary>
    public class Truncation
    {
        /// <summary>
        /// The extrasmall.
        /// </summary>
        public int Extrasmall;

        /// <summary>
        /// The large.
        /// </summary>
        public int Large;

        /// <summary>
        /// The medium.
        /// </summary>
        public int Medium;

        /// <summary>
        /// The small.
        /// </summary>
        public int Small;
    }

    /// <summary>
    /// The reference.
    /// </summary>
    [DataContract(Name = "reference", Namespace = "")]
    public class Reference
    {
        /// <summary>
        /// The external uri.
        /// </summary>
        [DataMember] public string externalUri;

        /// <summary>
        /// The guid.
        /// </summary>
        [DataMember] public string guid;

        /// <summary>
        /// The mimetype.
        /// </summary>
        [DataMember] public string mimetype;

        /// <summary>
        /// The ref.
        /// </summary>
        [DataMember] public string @ref;

        /// <summary>
        /// The type.
        /// </summary>
        [DataMember] public string type;

        /// <summary>
        /// The subtype.
        /// </summary>
        [DataMember] public string subType;

        /// <summary>
        /// The image type.
        /// </summary>
        [DataMember] public string imageType;

                                              
        /// <summary>
        /// The original content category.
        /// </summary>
        public string originalContentCategory;

        /// <summary>
        /// The content category.
        /// </summary>
        [DataMember]
        public ContentCategory contentCategory;

        /// <summary>
        /// The content category descriptor.
        /// </summary>
        [DataMember]
        public string contentCategoryDescriptor;

        /// <summary>
        /// The content sub category.
        /// </summary>
        [DataMember]
        public ContentSubCategory contentSubCategory;

        /// <summary>
        /// The content sub category descriptor.
        /// </summary>
        [DataMember]
        public string contentSubCategoryDescriptor;
    }

    /// <summary>
    /// The headline info.
    /// </summary>
    public class HeadlineInfo
    {
        /// <summary>
        /// The _reference.
        /// </summary>
        private Reference _reference;

        /// <summary>
        /// The _thumbnail image.
        /// </summary>
        private ThumbnailImage _thumbnailImage;

        /// <summary>
        /// The _truncation rules.
        /// </summary>
        private Truncation _truncationRules;

        /// <summary>
        /// Translated Descriptor for <see cref="baseLanguage"/>
        /// </summary>
        public string baseLanguageDescriptor;

        /// <summary>
        /// Base Language {en,fr,es,de ...}
        /// </summary>
        public string baseLanguage;

        /// <summary>
        /// The byline.
        /// </summary>
        public List<Para> byline;

        /// <summary>
        /// The byline.
        /// </summary>
        public List<Para> codedAuthors;

        /// <summary>
        /// The column name.
        /// </summary>
        public List<Para> columnName;

        /// <summary>
        /// The comment.
        /// </summary>
        public string comment;

        /// <summary>
        /// The content category.
        /// </summary>
        public ContentCategory contentCategory;

        /// <summary>
        /// The content category descriptor.
        /// </summary>
        public string contentCategoryDescriptor;

        /// <summary>
        /// The content sub category.
        /// </summary>
        public ContentSubCategory contentSubCategory;

        /// <summary>
        /// The content sub category descriptor.
        /// </summary>
        public string contentSubCategoryDescriptor;

        /// <summary>
        /// The copyright.
        /// </summary>
        public List<Para> copyright;

        /// <summary>
        /// The credit.
        /// </summary>
        public List<Para> credit;

        /// <summary>
        /// The document vector.
        /// </summary>
        public string documentVector;

        /// <summary>
        /// The duplicate headlines.
        /// </summary>
        public List<DedupHeadlineInfo> duplicateHeadlines = new List<DedupHeadlineInfo>();

        /// <summary>
        /// The has duplicates.
        /// </summary>
        public bool hasDuplicates;

        /// <summary>
        /// The has publication time.
        /// </summary>
        public bool hasPublicationTime;

        /// <summary>
        /// The importance.
        /// </summary>
        public string importance;

        /// <summary>
        /// The importance descriptor.
        /// </summary>
        public string importanceDescriptor;

        /// <summary>
        /// The index.
        /// </summary>
        public WholeNumber index;

        /// <summary>
        /// The mime-type.
        /// </summary>
        public string mimetype;

        /// <summary>
        /// The publication date time.
        /// </summary>
        public DateTime publicationDateTime;

        /// <summary>
        /// The publication date time descriptor.
        /// </summary>
        public string publicationDateTimeDescriptor;

        /// <summary>
        /// The publication time descriptor.
        /// </summary>
        public string publicationTimeDescriptor;

        /// <summary>
        /// The publication date descriptor.
        /// </summary>
        public string publicationDateDescriptor;

        /// <summary>
        /// The publication date time.
        /// </summary>
        public DateTime modificationDateTime;

        /// <summary>
        /// The publication date time descriptor.
        /// </summary>
        public string modificationDateTimeDescriptor;

        /// <summary>
        /// The publication time descriptor.
        /// </summary>
        public string modificationTimeDescriptor;

        /// <summary>
        /// The publication date descriptor.
        /// </summary>
        public string modificationDateDescriptor;

        /// <summary>
        /// The section name.
        /// </summary>
        public List<Para> sectionName;

        /// <summary>
        /// The selected.
        /// </summary>
        public bool selected;

        /// <summary>
        /// The snippet.
        /// </summary>
        public List<Para> snippet;

        /// <summary>
        /// The source descriptor.
        /// </summary>
        public string sourceDescriptor;

        /// <summary>
        /// The source reference.
        /// </summary>
        public string sourceReference;

        /// <summary>
        /// The summary.
        /// </summary>
        public string summary;

        /// <summary>
        /// The time.
        /// </summary>
        public string time;

        /// <summary>
        /// The title.
        /// </summary>
        public List<Para> title;

        /// <summary>
        /// The truncated title.
        /// </summary>
        public string truncatedTitle;

        /// <summary>
        /// The word count.
        /// </summary>
        public int wordCount;

        /// <summary>
        /// The word count descriptor.
        /// </summary>
        public string wordCountDescriptor;

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>The reference.</value>
        public Reference reference
        {
            get { return _reference ?? (_reference = new Reference()); }
            set { _reference = value; }
        }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>The reference.</value>
        public ThumbnailImage thumbnailImage
        {
            get { return _thumbnailImage ?? (_thumbnailImage = new ThumbnailImage()); }
            set { _thumbnailImage = value; }
        }

        /// <summary>
        /// Gets or sets the type of the truncation.
        /// </summary>
        /// <value>The type of the truncation.</value>
        public Truncation truncationRules
        {
            get { return _truncationRules ?? (_truncationRules = new Truncation()); }
            set { _truncationRules = value; }
        }

        public List<Code> author { get; set; }
    }

    [DataContract(Name = "code", Namespace = "")]
    public class Code
    {
        public string id { get; set; }

        public string value { get; set; }
    }
}