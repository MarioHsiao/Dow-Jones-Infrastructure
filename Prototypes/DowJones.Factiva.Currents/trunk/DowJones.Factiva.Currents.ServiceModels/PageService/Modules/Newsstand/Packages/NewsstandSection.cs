using System;
using System.Runtime.Serialization;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Packages
{
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