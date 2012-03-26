// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SummaryPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.Models.News;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.Common;
using DowJones.Web.Mvc.UI.Models.Company;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(SummaryChartPackage))]
    [KnownType(typeof(SummaryRegionalMapPackage))]
    [KnownType(typeof(SummaryVideosPackage))]
    [KnownType(typeof(SummaryRecentArticlesPackage))]
    [KnownType(typeof(SummaryTrendingPackage))]
    public abstract class AbstractSummaryPackage : IPackage
    {
    }

    [DataContract(Namespace = "")]
    [KnownType(typeof(SummaryVideosPackage))]
    [KnownType(typeof(SummaryRecentArticlesPackage))]
    public abstract class AbstractSummaryHeadlinesPackage : AbstractSummaryPackage, IPortalHeadlines
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private PortalHeadlineListDataResult headlineListDataResult;

        #region IPortalHeadlines Members

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        /// <remarks></remarks>
        [DataMember(Name = "portalHeadlineListDataResult")]
        public PortalHeadlineListDataResult Result
        {
            get { return headlineListDataResult ?? (headlineListDataResult = new PortalHeadlineListDataResult()); }

            set { headlineListDataResult = value; }
        }

        #endregion
    }

    [DataContract(Name = "summaryChartPackage", Namespace = "")]
    public class SummaryChartPackage : AbstractSummaryPackage
    {
        [DataMember(Name = "marketIndexIntradayResult")]
        public MarketIndexIntradayResult MarketIndexIntradayResult { get; protected internal set; }
    }

    [DataContract(Name = "summaryRecentArticlesPackage", Namespace = "")]
    public class SummaryRecentArticlesPackage : AbstractSummaryHeadlinesPackage
    {
    }

    [DataContract(Name = "summaryVideosPackage", Namespace = "")]
    public class SummaryVideosPackage : AbstractSummaryHeadlinesPackage
    {
    }

    [DataContract(Name = "summaryRegionalMapPackage", Namespace = "")]
    public class SummaryRegionalMapPackage : AbstractSummaryPackage
    {
        /// <summary>
        /// Gets or sets the region news volume.
        /// </summary>
        /// <value>The region news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "regionNewsVolume")]
        public List<NewsEntity> RegionNewsVolume { get; set; }
    }

    [DataContract(Name = "summaryTrendingPackage", Namespace = "")]
    public class SummaryTrendingPackage : AbstractSummaryPackage
    {
        [DataMember(Name = "keywordsTagCollection")]
        public TagCollection KeywordsTagCollection { get; protected internal set; }

        [DataMember(Name = "companyNewsEntities")]
        public List<NewsEntity> CompanyNewsEntities { get; protected internal set; }

        [DataMember(Name = "newsSubjectsNewsEntities")]
        public List<NewsEntity> NewsSubjectsNewsEntities { get; protected internal set; }

        [DataMember(Name = "industriesNewsEntities")]
        public List<NewsEntity> IndustriesNewsEntities { get; protected internal set; }

        [DataMember(Name = "executivesNewsEntities")]
        public List<NewsEntity> ExecutivesNewsEntities { get; protected internal set; }
    }
}
