// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPageModuleAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using AlertsNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.AlertsNewspageModule;
using CompanyOverviewNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.CompanyOverviewNewspageModule;
using CustomTopicsNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.CustomTopicsNewspageModule;
using DataAccessModel = DowJones.Web.Mvc.UI.Models.NewsPages.Modules;
using FactivaDataModel = Factiva.Gateway.Messages.Assets.Pages.V1_0;
using NewsstandNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.NewsstandNewspageModule;
using RadarNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.RadarNewspageModule;
using RegionalMapNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.RegionalMapNewspageModule;
using SourcesNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.SourcesNewspageModule;
using SummaryNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.SummaryNewspageModule;
using SyndicationNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.SyndicationNewspageModule;
using TopNewsNewspageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.TopNewsNewspageModule;
using TrendingNewsPageModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules.TrendingNewsPageModule;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages
{
    /// <summary>
    /// The news page module assembler.
    /// </summary>
    public class NewsPageModuleAssembler : AbstractPageModuleAssembler, 
                                           IAssembler<AlertsNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.AlertsNewspageModule>, 
                                           IAssembler<CompanyOverviewNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.CompanyOverviewNewspageModule>, 
                                           IAssembler<CustomTopicsNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.CustomTopicsNewspageModule>, 
                                           IAssembler<NewsstandNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsstandNewspageModule>, 
                                           IAssembler<RadarNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.RadarNewspageModule>, 
                                           IAssembler<SourcesNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.SourcesNewspageModule>, 
                                           IAssembler<SyndicationNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.SyndicationNewspageModule>, 
                                           IAssembler<TopNewsNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.TopNewsNewspageModule>, 
                                           IAssembler<TrendingNewsPageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.TrendingNewsPageModule>, 
                                           IAssembler<RegionalMapNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.RegionalMapNewspageModule>,
                                           IAssembler<SummaryNewspageModule, Factiva.Gateway.Messages.Assets.Pages.V1_0.SummaryNewspageModule>
    {
        public NewsPageModuleAssembler(Factiva.Gateway.Utils.V1_0.ControlData controlData, IPreferences preferences) : base(controlData, preferences)
        {
        }

        #region IAssembler<AlertsNewspageModule,AlertsNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.AlertsNewspageModule"/>.
        /// </returns>
        public AlertsNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.AlertsNewspageModule source)
        {
            var module = Initialize(new AlertsNewspageModule(), source) as AlertsNewspageModule;
            if (module != null && source != null)
            {
                // module.HeadlineCount = source.HeadlineCount;
                if (source.AlertCollection != null && source.AlertCollection.Count > 0)
                {
                    foreach (var alert in source.AlertCollection)
                    {
                        module.AlertIDCollection.Add(alert.AlertID);
                    }

                    module.MaxPartsAvailable = source.AlertCollection.Count;
                }
            }

            return module;
        }

        #endregion

        #region IAssembler<CompanyOverviewNewspageModule,CompanyOverviewNewspageModule> Members

        /// <summary>
        /// Converts the specified
        /// <seealso cref="FactivaDataModel.CompanyOverviewNewspageModule"/>
        /// source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.CompanyOverviewNewspageModule"/>.
        /// </returns>
        public CompanyOverviewNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.CompanyOverviewNewspageModule source)
        {
            var module = Initialize(new CompanyOverviewNewspageModule(), source) as CompanyOverviewNewspageModule;
            if (module != null && source != null)
            {
                module.Fcode.AddRange(source.FCodeCollection != null ? source.FCodeCollection.ToArray() : new string[0]);
            }

            return module;
        }

        #endregion

        #region IAssembler<CustomTopicsNewspageModule,CustomTopicsNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.CustomTopicsNewspageModule"/>.
        /// </returns>
        public CustomTopicsNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.CustomTopicsNewspageModule source)
        {
            var module = Initialize(new CustomTopicsNewspageModule(), source) as CustomTopicsNewspageModule;

            if (module != null)
            {
                module.MaxPartsAvailable = source.CustomTopicCollection.Count;
            }

            return module;
        }

        #endregion

        #region IAssembler<NewsstandNewspageModule,NewsstandNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.NewsstandNewspageModule"/>.
        /// </returns>
        public NewsstandNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsstandNewspageModule source)
        {
            var module = Initialize(new NewsstandNewspageModule(), source) as NewsstandNewspageModule;
            return module;
        }

        #endregion

        #region IAssembler<RadarNewspageModule,RadarNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.RadarNewspageModule"/>.
        /// </returns>
        public RadarNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.RadarNewspageModule source)
        {
            var module = Initialize(new RadarNewspageModule(), source) as RadarNewspageModule;
            return module;
        }

        #endregion

        #region IAssembler<RegionalMapNewspageModule,RegionalMapNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.RegionalMapNewspageModule"/>.
        /// </returns>
        public RegionalMapNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.RegionalMapNewspageModule source)
        {
            var module = Initialize(new RegionalMapNewspageModule(), source) as RegionalMapNewspageModule;
            return module;
        }

        #endregion

        #region IAssembler<SourcesNewspageModule, SourcesNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.SourcesNewspageModule"/>.
        /// </returns>
        public SourcesNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.SourcesNewspageModule source)
        {
            var module = Initialize(new SourcesNewspageModule(), source) as SourcesNewspageModule;
            if (Preferences != null)
            {
                if (module != null)
                {
                    module.MaxPartsAvailable = GetSourceListByContentLanguages(source.SourcesListCollection, Preferences).Count;
                }
            }

            return module;
        }

        #endregion

        #region IAssembler<SyndicationNewspageModule,SyndicationNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.SyndicationNewspageModule"/>.
        /// </returns>
        public SyndicationNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.SyndicationNewspageModule source)
        {
            // DataAccessModel.SyndicationNewspageModule module 
            var module = Initialize(new SyndicationNewspageModule(), source) as SyndicationNewspageModule;
            if (module != null)
            {
                // module.HeadlineCount = source.HeadlineCount;
                foreach (var feedid in source.SyndicationFeedIDCollection)
                {
                    module.SyndicationFeedIdCollection.Add(feedid);
                }

                module.MaxPartsAvailable = source.SyndicationFeedIDCollection.Count();
            }

            return module;
        }

        #endregion

        #region IAssembler<TopNewsNewspageModule,TopNewsNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.TopNewsNewspageModule"/>.
        /// </returns>
        public TopNewsNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.TopNewsNewspageModule source)
        {
            var module = Initialize(new TopNewsNewspageModule(), source) as TopNewsNewspageModule;
            return module;
        }

        #endregion

        #region IAssembler<TrendingNewsPageModule,TrendingNewsPageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.TrendingNewsPageModule"/>.
        /// </returns>
        public TrendingNewsPageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.TrendingNewsPageModule source)
        {
            var module = Initialize(new TrendingNewsPageModule(), source) as TrendingNewsPageModule;
            if (module != null)
            {
                // module.TimePeriod = source.TimePeriod;
            }

            return module;
        }

        #endregion
        
        #region IAssembler<SummaryNewspageModule,SummaryNewspageModule> Members

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   A <seealso cref="DataAccessModel.SummaryNewspageModule"/>.
        /// </returns>
        public SummaryNewspageModule Convert(Factiva.Gateway.Messages.Assets.Pages.V1_0.SummaryNewspageModule source)
        {
            var module = Initialize(new SummaryNewspageModule(), source) as SummaryNewspageModule;

            if (module != null)
            {
                module.HasMarketDataSymbol = source.MarketIndex.IsNotEmpty();
            }

            return module;
        }

        #endregion

        /// <summary>
        /// The get source list by content languages.
        /// </summary>
        /// <param name="sourcesCollection">The sources collection.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>A list of source codes</returns>
        protected internal List<string> GetSourceListByContentLanguages(SourceListCollection sourcesCollection, IPreferences preferences)
        {
            var temp = new List<string>();

            // Handle the all condition
            if (preferences.ContentLanguages.Count == 0)
            {
                foreach (var sourceList in sourcesCollection)
                {
                    temp.AddRange(sourceList.SourceCodes.SourceCodeCollection);
                }

                return temp;
            }

            foreach (var sourcelist in sourcesCollection.Where(sourcelist => ValidateSource(sourcelist.SourceListId, preferences)))
            {
                temp.AddRange(sourcelist.SourceCodes.SourceCodeCollection);
            }

            return temp;
        }

        /// <summary>
        /// The validate source.
        /// </summary>
        /// <param name="sourceLanguage">The source language.</param>
        /// <param name="preferences">The preferences.</param>
        /// <returns>
        /// A Boolean indicating whether the source is valid.
        /// </returns>
        protected internal bool ValidateSource(string sourceLanguage, IPreferences preferences)
        {
            return preferences.ContentLanguages.Any(contentLanguage => contentLanguage.ToLowerInvariant() == sourceLanguage.ToLowerInvariant());
        }
    }
}