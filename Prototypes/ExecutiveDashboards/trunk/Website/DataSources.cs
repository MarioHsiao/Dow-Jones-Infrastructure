using System.Collections.Generic;
using System.Linq;
using DowJones.Dash.DataSources;

namespace DowJones.Dash.Website
{
    public class DataSources : DependencyInjection.DependencyInjectionModule
    {
        public enum Sites
        {
            WsjUs = 1,
            WsjGermany = 2,
            WsjJapan = 3,
            WsjChina = 4,
            SmartMoney = 5,
            Barrons = 6,
            MarketWatch = 7,
            Factiva = 8,
        }

        protected override void OnLoad()
        {
            var dataSources = WsjUs()
                .Union(WsjJapan())
                .Union(WsjChina())
                .Union(WsjGermany())
                .Union(SmartMoney())
                .Union(Barrons())
                .Union(Marketwatch());

            foreach (var dataSource in dataSources)
            {
                Bind<IDataSource>().ToConstant(dataSource);
            }
        }

        public IEnumerable<IDataSource> WsjGermany()
        {
            yield return new ChartBeatDataSource("wallstreetjournal.de-DashboardStats", "DashboardStats", "/dashapi/stats/", "wallstreetjournal.de");
            yield return new ChartBeatDataSource("wallstreetjournal.de-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatDataSource("wallstreetjournal.de-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatDataSource("wallstreetjournal.de-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatDataSource("wallstreetjournal.de-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatDataSource("wallstreetjournal.de-QuickStats", "QuickStats", "/live/quickstats/v3", "wallstreetjournal.de");
            yield return new ChartBeatDataSource("wallstreetjournal.de-Referrers", "Referrers", "/live/referrers/v3", "wallstreetjournal.de");
            yield return new ChartBeatDataSource("wallstreetjournal.de-TopPages", "TopPages", "/toppages", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"limit", 10},
                });

            yield return new GomezDataSource("wallstreetjournal.de-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]", new Dictionary<string, object>
                {
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjGermany},
                });
            yield return new GomezDataSource("wallstreetjournal.de-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjGermany},
                });
            /*yield return new GomezDataSource("wallstreetjournal.de-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]", new Dictionary<string, object>
                {
                    {"pageid", 421139},
                    {"seconds", 300},
                    //{"site", (int) Sites.WsjGermany},
                });*/
            yield return new GomezDataSource("wallstreetjournal.de-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]", new Dictionary<string, object>
                {
                    {"days", 7},
                    {"site", (int) Sites.WsjGermany},
                });
            yield return new GomezDataSource("wallstreetjournal.de-PageTimings", "PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjGermany},
                });
            yield return new GomezDataSource("wallstreetjournal.de-PageLoadDetailsBySubCountryforCountry", "PageLoadDetailsBySubCountryforCountry", @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]", new Dictionary<string, object>
                {
                    {"country", 223},
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjGermany},
                });
        }

        public IEnumerable<IDataSource> Barrons()
        {
            yield return new ChartBeatDataSource("online.barrons.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "online.barrons.com");
            yield return new ChartBeatDataSource("online.barrons.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "online.barrons.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatDataSource("online.barrons.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "online.barrons.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatDataSource("online.barrons.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "online.barrons.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatDataSource("online.barrons.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "online.barrons.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatDataSource("online.barrons.com-QuickStats", "QuickStats", "/live/quickstats/v3", "online.barrons.com");
            yield return new ChartBeatDataSource("online.barrons.com-Referrers", "Referrers", "/live/referrers/v3", "online.barrons.com");
            yield return new ChartBeatDataSource("online.barrons.com-TopPages", "TopPages", "/toppages", "online.barrons.com", new Dictionary<string, object>
                {
                    {"limit", 10},
                });

            yield return new GomezDataSource("online.barrons.com-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]", new Dictionary<string, object>
                {
                    {"seconds", 3600},
                    {"site", (int) Sites.Barrons},
                });
            yield return new GomezDataSource("online.barrons.com-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.Barrons},
                });
            /*yield return new GomezDataSource("online.barrons.com-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]", new Dictionary<string, object>
                {
                    {"pageid", 421139},
                    {"seconds", 300},
                    //{"site", (int) Sites.Barrons},
                });*/
            yield return new GomezDataSource("online.barrons.com-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]", new Dictionary<string, object>
                {
                    {"days", 7},
                    {"site", (int) Sites.Barrons},
                });
            yield return new GomezDataSource("online.barrons.com-PageTimings", "PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.Barrons},
                });
            yield return new GomezDataSource("online.barrons.com-PageLoadDetailsBySubCountryforCountry", "PageLoadDetailsBySubCountryforCountry", @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]", new Dictionary<string, object>
                {
                    {"country", 223},
                    {"seconds", 3600},
                    {"site", (int) Sites.Barrons},
                });
        }

        public IEnumerable<IDataSource> SmartMoney()
        {
            yield return new ChartBeatDataSource("smartmoney.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "smartmoney.com");
            yield return new ChartBeatDataSource("smartmoney.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "smartmoney.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatDataSource("smartmoney.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "smartmoney.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatDataSource("smartmoney.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "smartmoney.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatDataSource("smartmoney.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "smartmoney.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatDataSource("smartmoney.com-QuickStats", "QuickStats", "/live/quickstats/v3", "smartmoney.com");
            yield return new ChartBeatDataSource("smartmoney.com-Referrers", "Referrers", "/live/referrers/v3", "smartmoney.com");
            yield return new ChartBeatDataSource("smartmoney.com-TopPages", "TopPages", "/toppages", "smartmoney.com", new Dictionary<string, object>
                {
                    {"limit", 10},
                });

            yield return new GomezDataSource("smartmoney.com-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]", new Dictionary<string, object>
                {
                    {"seconds", 3600},
                    {"site", (int) Sites.SmartMoney},
                });
            yield return new GomezDataSource("smartmoney.com-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.SmartMoney},
                });
            /*yield return new GomezDataSource("smartmoney.com-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]", new Dictionary<string, object>
                {
                    {"pageid", 421139},
                    {"seconds", 300},
                    //{"site", (int) Sites.SmartMoney},
                });*/
            yield return new GomezDataSource("smartmoney.com-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]", new Dictionary<string, object>
                {
                    {"days", 7},
                    {"site", (int) Sites.SmartMoney},
                });
            yield return new GomezDataSource("smartmoney.com-PageTimings", "PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.SmartMoney},
                });
            yield return new GomezDataSource("smartmoney.com-PageLoadDetailsBySubCountryforCountry", "PageLoadDetailsBySubCountryforCountry", @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]", new Dictionary<string, object>
                {
                    {"country", 223},
                    {"seconds", 3600},
                    {"site", (int) Sites.SmartMoney},
                });
        }

        public IEnumerable<IDataSource> Marketwatch()
        {
            yield return new ChartBeatDataSource("marketwatch.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "marketwatch.com");
            yield return new ChartBeatDataSource("marketwatch.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "marketwatch.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatDataSource("marketwatch.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "marketwatch.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatDataSource("marketwatch.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "marketwatch.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatDataSource("marketwatch.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "marketwatch.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatDataSource("marketwatch.com-QuickStats", "QuickStats", "/live/quickstats/v3", "marketwatch.com");
            yield return new ChartBeatDataSource("marketwatch.com-Referrers", "Referrers", "/live/referrers/v3", "marketwatch.com");
            yield return new ChartBeatDataSource("marketwatch.com-TopPages", "TopPages", "/toppages", "marketwatch.com", new Dictionary<string, object>
                {
                    {"limit", 10},
                });

            yield return new GomezDataSource("marketwatch.com-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]", new Dictionary<string, object>
                {
                    {"seconds", 3600},
                    {"site", (int) Sites.MarketWatch},
                });
            yield return new GomezDataSource("marketwatch.com-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.MarketWatch},
                });
            /*yield return new GomezDataSource("marketwatch.com-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]", new Dictionary<string, object>
                {
                    {"pageid", 421139},
                    {"seconds", 300},
                    //{"site", (int) Sites.MarketWatch},
                });*/
            yield return new GomezDataSource("marketwatch.com-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]", new Dictionary<string, object>
                {
                    {"days", 7},
                    {"site", (int) Sites.MarketWatch},
                });
            yield return new GomezDataSource("marketwatch.com-PageTimings", "PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.MarketWatch},
                });
            yield return new GomezDataSource("marketwatch.com-PageLoadDetailsBySubCountryforCountry", "PageLoadDetailsBySubCountryforCountry", @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]", new Dictionary<string, object>
                {
                    {"country", 223},
                    {"seconds", 3600},
                    {"site", (int) Sites.MarketWatch},
                });
        }

        public IEnumerable<IDataSource> WsjChina()
        {
            yield return new ChartBeatDataSource("cn.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "cn.wsj.com");
            yield return new ChartBeatDataSource("cn.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatDataSource("cn.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatDataSource("cn.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatDataSource("cn.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatDataSource("cn.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3", "cn.wsj.com");
            yield return new ChartBeatDataSource("cn.wsj.com-Referrers", "Referrers", "/live/referrers/v3", "cn.wsj.com");
            yield return new ChartBeatDataSource("cn.wsj.com-TopPages", "TopPages", "/toppages", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"limit", 10},
                });

            yield return new GomezDataSource("cn.wsj.com-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]", new Dictionary<string, object>
                {
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjChina},
                });
            yield return new GomezDataSource("cn.wsj.com-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjChina},
                });
            /*yield return new GomezDataSource("cn.wsj.com-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]", new Dictionary<string, object>
                {
                    {"pageid", 421139},
                    {"seconds", 300},
                    //{"site", (int) Sites.WsjChina},
                });*/
            yield return new GomezDataSource("cn.wsj.com-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]", new Dictionary<string, object>
                {
                    {"days", 7},
                    {"site", (int) Sites.WsjChina},
                });
            yield return new GomezDataSource("cn.wsj.com-PageTimings", "PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjChina},
                });
            yield return new GomezDataSource("cn.wsj.com-PageLoadDetailsBySubCountryforCountry", "PageLoadDetailsBySubCountryforCountry", @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]", new Dictionary<string, object>
                {
                    {"country", 223},
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjChina},
                });
        }

        public IEnumerable<IDataSource> WsjJapan()
        {
            yield return new ChartBeatDataSource("jp.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "jp.wsj.com");
            yield return new ChartBeatDataSource("jp.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatDataSource("jp.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatDataSource("jp.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatDataSource("jp.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatDataSource("jp.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3", "jp.wsj.com");
            yield return new ChartBeatDataSource("jp.wsj.com-Referrers", "Referrers", "/live/referrers/v3", "jp.wsj.com");
            yield return new ChartBeatDataSource("jp.wsj.com-TopPages", "TopPages", "/toppages", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"limit", 10},
                });

            yield return new GomezDataSource("jp.wsj.com-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]", new Dictionary<string, object>
                {
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjJapan},
                });
            yield return new GomezDataSource("jp.wsj.com-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjJapan},
                });
            /*yield return new GomezDataSource("jp.wsj.com-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]", new Dictionary<string, object>
                {
                    {"pageid", 421139},
                    {"seconds", 300},
                    //{"site", (int) Sites.WsjJapan},
                });*/
            yield return new GomezDataSource("jp.wsj.com-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]", new Dictionary<string, object>
                {
                    {"days", 7},
                    {"site", (int) Sites.WsjJapan},
                });
            yield return new GomezDataSource("jp.wsj.com-PageTimings", "PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjJapan},
                });
            yield return new GomezDataSource("jp.wsj.com-PageLoadDetailsBySubCountryforCountry", "PageLoadDetailsBySubCountryforCountry", @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]", new Dictionary<string, object>
                {
                    {"country", 223},
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjJapan},
                });
        }

        public IEnumerable<IDataSource> WsjUs()
        {
            yield return new ChartBeatDataSource("online.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "online.wsj.com");
            yield return new ChartBeatDataSource("online.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "online.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatDataSource("online.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "online.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatDataSource("online.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "online.wsj.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatDataSource("online.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "online.wsj.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatDataSource("online.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3", "online.wsj.com");
            yield return new ChartBeatDataSource("online.wsj.com-Referrers", "Referrers", "/live/referrers/v3", "online.wsj.com");
            yield return new ChartBeatDataSource("online.wsj.com-TopPages", "TopPages", "/toppages", "online.wsj.com", new Dictionary<string, object>
                {
                    {"limit", 10},
                });

            yield return new GomezDataSource("online.wsj.com-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]", new Dictionary<string, object>
                {
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjUs},
                });
            yield return new GomezDataSource("online.wsj.com-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjUs},
                });
            /*yield return new GomezDataSource("online.wsj.com-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]", new Dictionary<string, object>
                {
                    {"pageid", 421139},
                    {"seconds", 300},
                    //{"site", (int) Sites.WsjUs},
                });*/
            yield return new GomezDataSource("online.wsj.com-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]", new Dictionary<string, object>
                {
                    {"days", 7},
                    {"site", (int) Sites.WsjUs},
                });
            yield return new GomezDataSource(
                "online.wsj.com-PageTimings",
                "PageTimings",
                @"[SplunkExport].[dbo].[GetPageLoadDetails]",
                new Dictionary<string, object>
                    {
                        {"seconds", 300},
                        {"site", (int) Sites.WsjUs},
                    });

            yield return new GomezDataSource(
                "online.wsj.com-PageLoadDetailsBySubCountryforCountry",
                "PageLoadDetailsBySubCountryforCountry",
                @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]",
                new Dictionary<string, object>
                    {
                        {"country", 223},
                        {"seconds", 3600},
                        {"site", (int) Sites.WsjUs},
                    });
        }
    }
}