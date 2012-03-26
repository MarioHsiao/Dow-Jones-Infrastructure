// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewScreenCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.CompanyOverview
{
    public abstract class AbstractCompanyOverviewCacheKeyGenerator<T> : AbstractModuleCacheKeyGenerator<T>
    {
        protected AbstractCompanyOverviewCacheKeyGenerator() 
            : base(string.Empty)
        {
        }

        protected AbstractCompanyOverviewCacheKeyGenerator(string moduleId, string companyCode, Product product = Product.Np)
            : base(moduleId, product)
        {
            CompanyCode = companyCode;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string CompanyCode { get; set; }
    }

    public sealed class CompanyOverviewScreeningCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewScreeningCacheKeyGenerator>
    {
        private const int DefaultCacheExpirationTime = 4 * 60;
        private const int DefaultCacheRefreshInterval = 0;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private const CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheExiprationPolicy.Absolute;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_Screening";

        public CompanyOverviewScreeningCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewScreeningCacheKeyGenerator(string moduleId, string companyCode, string interfaceLanguage, Product product = Product.Np) : 
            base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            InterfaceLanguage = interfaceLanguage;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.InterfaceLanguage)]
        public string InterfaceLanguage { get; set; }
    }

    public sealed class CompanyOverviewHistoricalMarketDataCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewHistoricalMarketDataCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_HistoricalMarketData";

        public CompanyOverviewHistoricalMarketDataCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewHistoricalMarketDataCacheKeyGenerator(string moduleId, string companyCode, Product product = Product.Np) :
            base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }

    public sealed class CompanyOverviewQuoteCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewQuoteCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_Quote";

        public CompanyOverviewQuoteCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewQuoteCacheKeyGenerator(string moduleId, string companyCode, string interfaceLanguage, Product product = Product.Np) 
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }

    public sealed class CompanyOverviewReportListCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewReportListCacheKeyGenerator>
    {
        private const int DefaultCacheExpirationTime = 4 * 60;
        private const int DefaultCacheRefreshInterval = 0;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private const CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheExiprationPolicy.Absolute;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_ReportList";
        
        public CompanyOverviewReportListCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewReportListCacheKeyGenerator(string moduleId, string companyCode, ReportType reportType,  Product product = Product.Np)
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
            Type = reportType;
        }

        public enum ReportType
        {
            /// <summary>
            /// Ivestext Financial Reports
            /// </summary>
            Investext,

            /// <summary>
            /// Zacks Financial Reports
            /// </summary>
            Zacks,
        }

        [JsonProperty(PropertyName = CacheKeyConstants.ReportType)]
        public ReportType Type { get; set; }
    }

    public sealed class CompanyOverviewNewsChartCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewNewsChartCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_NewsChart";

        public CompanyOverviewNewsChartCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewNewsChartCacheKeyGenerator(string moduleId, string companyCode, Product product = Product.Np)
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }

    public sealed class CompanyOverviewCogenCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewReportListCacheKeyGenerator>
    {
        private const int DefaultCacheExpirationTime = 4 * 60;
        private const int DefaultCacheRefreshInterval = 0;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private const CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheExiprationPolicy.Absolute;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_CogenArchive";

        public CompanyOverviewCogenCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewCogenCacheKeyGenerator(string moduleId, string companyCode, Product product = Product.Np)
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }

    public sealed class CompanyOverviewCogenReportList : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewCogenReportList>
    {
        private const int DefaultCacheExpirationTime = 4 * 60;
        private const int DefaultCacheRefreshInterval = 0;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private const CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheExiprationPolicy.Absolute;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_CogenReportList";

        public CompanyOverviewCogenReportList()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewCogenReportList(string moduleId, string companyCode, Product product = Product.Np)
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }

    public sealed class CompanyOverviewDataMonitorCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewDataMonitorCacheKeyGenerator>
    {
        private const int DefaultCacheExpirationTime = 4 * 60;
        private const int DefaultCacheRefreshInterval = 0;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private const CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheExiprationPolicy.Absolute;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_ReportList_Search";

        public CompanyOverviewDataMonitorCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewDataMonitorCacheKeyGenerator(string moduleId, string companyCode, Product product = Product.Np)
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public enum ReportType
        {
            DataMonitor
        }

        [JsonProperty(PropertyName = CacheKeyConstants.ReportType)]
        public ReportType Type { get; set; }
    }

    public sealed class CompanyOverviewSymbologyCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewSymbologyCacheKeyGenerator>
    {
        private const int DefaultCacheExpirationTime = 4 * 60;
        private const int DefaultCacheRefreshInterval = 0;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private const CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheExiprationPolicy.Absolute;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_Symbology";

        public CompanyOverviewSymbologyCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewSymbologyCacheKeyGenerator(string moduleId, string companyCode, string interfaceLanguage, Product product = Product.Np)
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
            
            InterfaceLanguage = interfaceLanguage;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.InterfaceLanguage)]
        public string InterfaceLanguage { get; set; }
    }

    public sealed class CompanyOverviewTrendingCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewTrendingCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_Trending";

        public CompanyOverviewTrendingCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewTrendingCacheKeyGenerator(string moduleId, string companyCode, Product product = Product.Np)
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }
}
