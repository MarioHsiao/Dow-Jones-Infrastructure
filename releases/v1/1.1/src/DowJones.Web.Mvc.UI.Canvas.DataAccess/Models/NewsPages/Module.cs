// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Module.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Web.Mvc.UI.Models.NewsPages.Modules
{
    #region alertsNewspageModule

    [DataContract(Name = "alertsNewspageModule", Namespace = "")]
    public class AlertsNewspageModule : NewsPageModule
    {
        private AlertIDCollection alertIDCollection;

        [DataMember(Name = "alertIds")]
        public AlertIDCollection AlertIDCollection
        {
            get
            {
                if (alertIDCollection == null)
                {
                    alertIDCollection = new AlertIDCollection();
                    MaxPartsAvailable = alertIDCollection.Count;
                }

                return alertIDCollection;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                alertIDCollection = value;
                MaxPartsAvailable = alertIDCollection.Count;
            }
        }

        [DataMember(Name = "maxPartsAvailable")]
        public int MaxPartsAvailable { get; set; }
    }

    [CollectionDataContract(Name = "alertIds", ItemName = "alertId", Namespace = "")]
    public class AlertIDCollection : List<string>
    {
    }

    #endregion //alertsNewspageModule

    #region syndicationNewspageModule
    [DataContract(Name = "syndicationNewspageModule", Namespace = "")]
    public class SyndicationNewspageModule : NewsPageModule
    {
        private SyndicationFeedUriCollection syndicationFeedUriCollection;
        private SyndicationFeedIDCollection syndicationFeedIdCollection;
        
        [DataMember(Name = "syndicationFeedUris")]
        public SyndicationFeedUriCollection SyndicationFeedUriCollection
        {
            get
            {
                return syndicationFeedUriCollection ?? (syndicationFeedUriCollection = new SyndicationFeedUriCollection());
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                syndicationFeedUriCollection = value;
            }
        }

        [DataMember(Name = "syndicationFeedIds")]
        public SyndicationFeedIDCollection SyndicationFeedIdCollection
        {
            get
            {
                return syndicationFeedIdCollection ?? (syndicationFeedIdCollection = new SyndicationFeedIDCollection());
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                syndicationFeedIdCollection = value;
            }
        }

        [DataMember(Name = "maxPartsAvailable")]
        public int MaxPartsAvailable { get; set; }
    }

    [CollectionDataContract(Name = "syndicationFeedUris", ItemName = "feed",   Namespace = "")]
    public class SyndicationFeedUriCollection : List<string>
    {
    }
    [CollectionDataContract(Name = "syndicationFeedIds", ItemName = "feedId", Namespace = "")]
    public class SyndicationFeedIDCollection : List<string>
    {
    }

    #endregion //syndicationNewspageModule

    #region newsstandNewspageModule

    [DataContract(Name = "newsstandNewspageModule", Namespace = "")]
    public class NewsstandNewspageModule : NewsPageModule
    {
    }

    #endregion //newsstandNewspageModule

    #region customTopicsNewspageModule

    [DataContract(Name = "customTopicsNewspageModule", Namespace = "")]
    public class CustomTopicsNewspageModule : NewsPageModule
    {
        [DataMember(Name = "maxPartsAvailable")]
        public int MaxPartsAvailable { get; set; }
    }

    #endregion //customTopicsNewspageModule

    #region TopNewsNewspageModule

    [DataContract(Name = "topNewsNewspageModule", Namespace = "")]
    public class TopNewsNewspageModule : NewsPageModule
    {
    }
    #endregion

    #region TrendingNewsPageModule

    [DataContract(Name = "trendingNewsPageModule", Namespace = "")]
    public class TrendingNewsPageModule : NewsPageModule
    {
    }

    #endregion

    #region CompanyOverviewNewspageModule

    [DataContract(Name = "companyOverviewNewspageModule", Namespace = "")]
    public class CompanyOverviewNewspageModule : NewsPageModule
    {
        public CompanyOverviewNewspageModule()
        {
            Fcode = new FcodeCollection();
        }

        [DataMember(Name = "fcodes")]
        public FcodeCollection Fcode { get; set; }
    }

    [CollectionDataContract(Name = "fcodes", ItemName = "fcode", Namespace = "")]
    public class FcodeCollection : List<string>
    {
    }
    #endregion

    #region SourcesNewspageModule

    [DataContract(Name = "sourcesNewspageModule", Namespace = "")]
    public class SourcesNewspageModule : NewsPageModule
    {
        [DataMember(Name = "maxPartsAvailable")]
        public int MaxPartsAvailable { get; set; }
    }

    #endregion

    #region RadarNewspageModule

    [DataContract(Name = "radarNewspageModule", Namespace = "")]
    public class RadarNewspageModule : NewsPageModule
    {
    }

    #endregion

    #region RegionalMapNewspageModule
    
    [DataContract(Name = "regionalMapNewspageModule", Namespace = "")]
    public class RegionalMapNewspageModule : NewsPageModule
    {
    }
    
    #endregion //RegionalMapNewspageModule

    #region SummaryMapNewspageModule

    [DataContract(Name = "summaryNewspageModule", Namespace = "")]
    public class SummaryNewspageModule : NewsPageModule
    {
        [DataMember(Name = "hasMarketDataSymbol")]
        public bool HasMarketDataSymbol { get; set; }
    }

    #endregion //SummaryMapNewspageModule

    #region Module
    // [KnownType(typeof(SummaryNewspageModule))]
    [KnownType(typeof(RadarNewspageModule))]
    [KnownType(typeof(SourcesNewspageModule))]
    [KnownType(typeof(TopNewsNewspageModule))]
    [KnownType(typeof(TrendingNewsPageModule))]
    [KnownType(typeof(CompanyOverviewNewspageModule))]
    [KnownType(typeof(AlertsNewspageModule))]
    [KnownType(typeof(SyndicationNewspageModule))]
    [KnownType(typeof(NewsstandNewspageModule))]
    [KnownType(typeof(CustomTopicsNewspageModule))]
    [KnownType(typeof(RegionalMapNewspageModule))]
    [KnownType(typeof(SummaryNewspageModule))]
    [DataContract(Name = "newsPageModule", Namespace = "")]
    [XmlInclude(typeof(SyndicationNewspageModule))]
    [XmlInclude(typeof(RadarNewspageModule))]
    [XmlInclude(typeof(SourcesNewspageModule))]
    [XmlInclude(typeof(TopNewsNewspageModule))]
    [XmlInclude(typeof(TrendingNewsPageModule))]
    [XmlInclude(typeof(CompanyOverviewNewspageModule))]
    [XmlInclude(typeof(AlertsNewspageModule))]
    [XmlInclude(typeof(NewsstandNewspageModule))]
    [XmlInclude(typeof(CustomTopicsNewspageModule))]
    [XmlInclude(typeof(RegionalMapNewspageModule))]
    [XmlInclude(typeof(SummaryNewspageModule))]
    public abstract class NewsPageModule
    {
        protected NewsPageModule()
        {
            ModuleProperties = new ModuleProperties();
        }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "moduleProperties")]
        public ModuleProperties ModuleProperties { get; set; }

        [DataMember(Name = "moduleQualifier")]
        public AccessQualifier ModuleQualifier { get; set; }
    }

    [DataContract(Name = "moduleProperties", Namespace = "")]
    public class ModuleProperties
    {
        [DataMember(Name = "categoryInfo")]
        public PublishStatusScope CategoryInfo { get; set; }

        [DataMember(Name = "publishStatusScope")]
        public PublishStatusScope PublishStatusScope { get; set; }

        [DataMember(Name = "moduleMetaData")]
        public MetaData ModuleMetaData { get; set; }
    }

    [DataContract(Name = "moduleState", Namespace = "")]
    public enum ModuleState
    {
        /// <summary>
        /// Minimized Module State.
        /// </summary>
        [EnumMember]
        Minimized = 0,

        /// <summary>
        /// Maximized Module State.
        /// </summary>
        [EnumMember]
        Maximized = 1,
    }
    #endregion
}
