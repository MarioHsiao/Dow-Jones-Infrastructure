// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopNewsPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.TopNews.Packages
{
    [DataContract(Name = "abstractTopNewsPackage", Namespace = "")]
    [KnownType(typeof(TopNewsEditorsChoicePackage))]
    [KnownType(typeof(TopNewsVideoAndAudioPackage))]
    [KnownType(typeof(TopNewsOpinionAndAnalysisPackage))]
    public abstract class AbstractTopNewsPackage : AbstractHeadlinePackage
    {

        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }
    }
}