// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsstandPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.Common;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "newsstandPackage", Namespace = "")]
    [KnownType(typeof(NewsstandHeadlinesPackage))]
    [KnownType(typeof(NewsstandDiscoveredEntitiesPackage))] 
    [KnownType(typeof(NewsstandHeadlineHitCountsPackage))]
    public abstract class AbstractNewsstandPackage : IPackage
    {
    }

    /// <summary>
    /// </summary>
    /// <remarks></remarks>
    [DataContract(Name = "newsstandHeadlinesPackage", Namespace = "")]
    public class NewsstandHeadlinesPackage : AbstractNewsstandPackage
    {
        /// <summary>
        /// Gets or sets the newsstand section headline list.
        /// </summary>
        /// <value>The newsstand section headline list.</value>
        /// <remarks></remarks>
        [DataMember(Name = "newsstandSections")]
        public List<NewsstandSection> NewsstandSections { get; set; }
    }

    [DataContract(Name = "newsstandDiscoveredEntitiesPackage", Namespace = "")]
    public class NewsstandDiscoveredEntitiesPackage : AbstractNewsstandPackage
    {
        /// <summary>
        /// Gets or sets the region news volume.
        /// </summary>
        /// <value>The region news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "topNewsVolumeEntities")]
        public List<NewsEntity> TopNewsVolumeEntities { get; set; }
       

        /// <summary>
        /// Gets or sets a value indicating whether [portal headlines are available].
        /// </summary>
        /// <value>
        /// <c>true</c> if [portal headlines are available]; otherwise, <c>false</c>.
        /// </value>
        [XmlElement(Type = typeof(bool), ElementName = "portalHeadlinesAreAvailable", IsNullable = false,
           Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("portalHeadlinesAreAvailable")]
        public bool PortalHeadlinesAreAvailable { get; set; }
    }

    [DataContract(Name = "newsstandHeadlineHitCountsPackage", Namespace = "")]
    public class NewsstandHeadlineHitCountsPackage : AbstractNewsstandPackage
    {
         [DataMember(Name = "newsstandHeadlineHitCounts")]
         public List<NewsstandHeadlineHitCount> NewsstandHeadlineHitCounts { get; set; }
    }

     [DataContract(Name = "newsstandHeadlineHitCount", Namespace = "")]
     public class NewsstandHeadlineHitCount
     {
         /// <summary>
         /// Gets or sets the status.
         /// </summary>
         /// <value>
         /// The status.
         /// </value>
         [DataMember(Name = "status")]
         public int Status { get; set; }

         /// <summary>
         /// Gets or sets the feed URI.
         /// </summary>
         /// <value>The feed URI.</value>
         /// <remarks></remarks>
         [DataMember(Name = "sectionTitle")]
         public string SectionTitle { get; set; }

         /// <summary>
         /// Gets or sets the type of the feed.
         /// </summary>
         /// <value>The type of the feed.</value>
         /// <remarks></remarks>
         [DataMember(Name = "sourceTitle")]
         public string SourceTitle { get; set; }

         /// <summary>
         /// Gets or sets the feed URI.
         /// </summary>
         /// <value>The feed URI.</value>
         /// <remarks></remarks>
         [DataMember(Name = "sectionId")]
         public string SectionId { get; set; }

         /// <summary>
         /// Gets or sets the type of the feed.
         /// </summary>
         /// <value>The type of the feed.</value>
         /// <remarks></remarks>
         [DataMember(Name = "sourceCode")]
         public string SourceCode { get; set; }


         /// <summary>
         /// Gets or sets the result.
         /// </summary>
         /// <value>The result.</value>
         /// <remarks></remarks>
         [DataMember(Name = "hitCount")]
         public int HitCount { get; set; }

         [DataMember(Name = "searchContextRef")]
         public string SearchContextRef { get; set; }
     }


    [DataContract(Name = "newsstandSection", Namespace = "")]
    public class NewsstandSection : IPortalHeadlines
    {
        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        /// <remarks></remarks>
        [DataMember(Name = "status")]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        /// <remarks></remarks>
        [DataMember(Name = "statusMessage")]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        /// <remarks></remarks>
        [DataMember(Name = "sectionTitle")]
        public string SectionTitle { get; set; }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        /// <remarks></remarks>
        [DataMember(Name = "sourceTitle")]
        public string SourceTitle { get; set; }

        /// <summary>
        /// Gets or sets the source logo URI.
        /// </summary>
        /// <value>
        /// The source logo URI.
        /// </value>
        [DataMember(Name = "sourceLogoUrl")]
        public string SourceLogoUrl { get; set; }

        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        /// <remarks></remarks>
        [DataMember(Name = "sectionId")]
        public string SectionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        /// <remarks></remarks>
        [DataMember(Name = "sourceCode")]
        public string SourceCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        /// <remarks></remarks>
        [DataMember(Name = "SectionDate")]
        public DateTime? SectionDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        /// <remarks></remarks>
        [DataMember(Name = "SectionDateDescriptor")]
        public string SectionDateDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        /// <remarks></remarks>
        [DataMember(Name = "result")]
        public PortalHeadlineListDataResult Result { get; set; }

        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }
    }
}
