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
            WsjLive = 3,
        }

        protected override void OnLoad()
        {
            var dataSources = WsjUs()
                .Union(WsjJapan())
                .Union(WsjChina())
                .Union(WsjGermany())
                .Union(WsjLive());
//                .Union(SmartMoney())
//                .Union(Barrons())
//                .Union(Marketwatch());

            foreach (var dataSource in dataSources)
            {
                Bind<IDataSource>().ToConstant(dataSource);
            }
        }

        public IEnumerable<IDataSource> WsjLive()
        {
            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "live.wsj.com-BasicHostConfiguration",
                "BasicHostConfiguration",
                new BasicHostConfiguration
                {
                    Domain = "live.wsj.com",
                    MapType = MapType.Country,
                    Region = "us",
                    PerformanceZones = new[]
                            {
                                new PerformanceZone{To = 0, From = 5, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone{To = 5, From = 7, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone{To = 7, From = 100, ZoneType = PerformanceZoneType.Hot},
                            },
                });
            yield return new ChartBeatApiDataSource("live.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "live.wsj.com");
            yield return new ChartBeatApiDataSource("live.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "live.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatApiDataSource("live.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "live.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatApiDataSource("live.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "live.wsj.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatApiDataSource("live.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "live.wsj.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatApiDataSource("live.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3", "live.wsj.com");
            yield return new ChartBeatApiDataSource("live.wsj.com-Referrers", "Referrers", "/live/referrers/v3", "live.wsj.com");
            yield return new ChartBeatApiDataSource("live.wsj.com-TopPages", "TopPages", "/toppages", "live.wsj.com", new Dictionary<string, object>
                {
                    {"limit", 10},
                });

            yield return new GomezDataSource("live.wsj.com-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]", new Dictionary<string, object>
                {
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjLive},
                });
            yield return new GomezDataSource("live.wsj.com-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjLive},
                });
            /*yield return new GomezDataSource("live.wsj.com-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]", new Dictionary<string, object>
                {
                    {"pageid", 421139},
                    {"seconds", 300},
                    //{"site", (int) Sites.WsjGermany},
                });*/
            yield return new GomezDataSource("live.wsj.com-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]", new Dictionary<string, object>
                {
                    {"days", 7},
                    {"site", (int) Sites.WsjLive},
                });
            yield return new GomezDataSource("live.wsj.com-PageTimings", "PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]", new Dictionary<string, object>
                {
                    {"seconds", 300},
                    {"site", (int) Sites.WsjLive},
                });
            yield return new GomezDataSource("live.wsj.com-PageLoadDetailsBySubCountryforCountryNew", "PageLoadDetailsBySubCountryforCountry", @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]", new Dictionary<string, object>
                {
                    {"country", 57},
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjLive},
                });

			yield return new GomezDataSource("live.wsj.com-PageLoadDetailsByCountryForRegion", "PageLoadDetailsByCountryForRegion", @"[SplunkExport].[dbo].[GetPageLoadDetailsByCountryForRegion]", new Dictionary<string, object>
                {
                    {"region", 0},
                    {"seconds", 300},
                    {"site", (int) Sites.WsjLive},
                });
        } 

        public IEnumerable<IDataSource> WsjGermany()
        {
            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "wallstreetjournal.de-BasicHostConfiguration", 
                "BasicHostConfiguration", 
                new BasicHostConfiguration
                    {
                        Domain = "wallstreetjournal.de",
                        MapType = MapType.Country,
                        Region = "de", 
                        PerformanceZones = new[]
                            {
                                new PerformanceZone{To = 0, From = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone{To = 8, From = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone{To = 10, From = 100, ZoneType = PerformanceZoneType.Hot},
                            },
                    }
                );
            yield return new ChartBeatApiDataSource("wallstreetjournal.de-DashboardStats", "DashboardStats", "/dashapi/stats/", "wallstreetjournal.de");
            //yield return new ChartBeatSiteDataSource("wallstreetjournal.de-DashboardStats", "DashboardStats", "/dashapi/stats/", "wallstreetjournal.de");
            yield return new ChartBeatApiDataSource("wallstreetjournal.de-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatApiDataSource("wallstreetjournal.de-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatApiDataSource("wallstreetjournal.de-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatApiDataSource("wallstreetjournal.de-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "wallstreetjournal.de", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatApiDataSource("wallstreetjournal.de-QuickStats", "QuickStats", "/live/quickstats/v3", "wallstreetjournal.de");
            /*yield return new ChartBeatSiteDataSource("wallstreetjournal.de-QuickStats", "QuickStats", "/api/quickstats/", "wallstreetjournal.de",
                new Dictionary<string, object>()
                    {
                        {"v", "2"},
                        {"path", ""},
                        {"author", ""},
                        {"section", ""},
                        {"now_on", "false"},                        
                    }
                );*/
            yield return new ChartBeatApiDataSource("wallstreetjournal.de-Referrers", "Referrers", "/live/referrers/v3", "wallstreetjournal.de");
            yield return new ChartBeatApiDataSource("wallstreetjournal.de-TopPages", "TopPages", "/toppages", "wallstreetjournal.de", new Dictionary<string, object>
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
                    {"country", 57},
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjGermany},
                });
        }

        public IEnumerable<IDataSource> Barrons()
        {
            yield return new ChartBeatApiDataSource("online.barrons.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "online.barrons.com");
            yield return new ChartBeatApiDataSource("online.barrons.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "online.barrons.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatApiDataSource("online.barrons.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "online.barrons.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatApiDataSource("online.barrons.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "online.barrons.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatApiDataSource("online.barrons.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "online.barrons.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatApiDataSource("online.barrons.com-QuickStats", "QuickStats", "/live/quickstats/v3", "online.barrons.com");
            yield return new ChartBeatApiDataSource("online.barrons.com-Referrers", "Referrers", "/live/referrers/v3", "online.barrons.com");
            yield return new ChartBeatApiDataSource("online.barrons.com-TopPages", "TopPages", "/toppages", "online.barrons.com", new Dictionary<string, object>
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
            yield return new ChartBeatApiDataSource("smartmoney.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "smartmoney.com");
            yield return new ChartBeatApiDataSource("smartmoney.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "smartmoney.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatApiDataSource("smartmoney.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "smartmoney.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatApiDataSource("smartmoney.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "smartmoney.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatApiDataSource("smartmoney.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "smartmoney.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatApiDataSource("smartmoney.com-QuickStats", "QuickStats", "/live/quickstats/v3", "smartmoney.com");
            yield return new ChartBeatApiDataSource("smartmoney.com-Referrers", "Referrers", "/live/referrers/v3", "smartmoney.com");
            yield return new ChartBeatApiDataSource("smartmoney.com-TopPages", "TopPages", "/toppages", "smartmoney.com", new Dictionary<string, object>
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
            yield return new ChartBeatApiDataSource("marketwatch.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "marketwatch.com");
            yield return new ChartBeatApiDataSource("marketwatch.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "marketwatch.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatApiDataSource("marketwatch.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "marketwatch.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatApiDataSource("marketwatch.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "marketwatch.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatApiDataSource("marketwatch.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "marketwatch.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatApiDataSource("marketwatch.com-QuickStats", "QuickStats", "/live/quickstats/v3", "marketwatch.com");
            yield return new ChartBeatApiDataSource("marketwatch.com-Referrers", "Referrers", "/live/referrers/v3", "marketwatch.com");
            yield return new ChartBeatApiDataSource("marketwatch.com-TopPages", "TopPages", "/toppages", "marketwatch.com", new Dictionary<string, object>
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
            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "cn.wsj.com-BasicHostConfiguration",
                "BasicHostConfiguration",
                new BasicHostConfiguration
                {
                    Domain = "cn.wsj.com",
                    MapType = MapType.Country,
                    Region = "cn",
                    PerformanceZones = new[]
                            {
                                new PerformanceZone{To = 0, From = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone{To = 8, From = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone{To = 10, From = 100, ZoneType = PerformanceZoneType.Hot},
                            },
                });
            yield return new ChartBeatApiDataSource("cn.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "cn.wsj.com");
            yield return new ChartBeatApiDataSource("cn.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatApiDataSource("cn.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatApiDataSource("cn.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatApiDataSource("cn.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "cn.wsj.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatApiDataSource("cn.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3", "cn.wsj.com");
            yield return new ChartBeatApiDataSource("cn.wsj.com-Referrers", "Referrers", "/live/referrers/v3", "cn.wsj.com");
            yield return new ChartBeatApiDataSource("cn.wsj.com-TopPages", "TopPages", "/toppages", "cn.wsj.com", new Dictionary<string, object>
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
                    {"country", 49},
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjChina},
                });
        }

        public IEnumerable<IDataSource> WsjJapan()
        {
            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "jp.wsj.com-BasicHostConfiguration",
                "BasicHostConfiguration",
                new BasicHostConfiguration
                {
                    Domain = "jp.wsj.com",
                    MapType = MapType.Country,
                    Region = "jp",
                    PerformanceZones = new[]
                            {
                                new PerformanceZone{To = 0, From = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone{To = 8, From = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone{To = 10, From = 100, ZoneType = PerformanceZoneType.Hot},
                            },
                });
            yield return new ChartBeatApiDataSource("jp.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "jp.wsj.com");
            yield return new ChartBeatApiDataSource("jp.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatApiDataSource("jp.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatApiDataSource("jp.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatApiDataSource("jp.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "jp.wsj.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatApiDataSource("jp.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3", "jp.wsj.com");
            yield return new ChartBeatApiDataSource("jp.wsj.com-Referrers", "Referrers", "/live/referrers/v3", "jp.wsj.com");
            yield return new ChartBeatApiDataSource("jp.wsj.com-TopPages", "TopPages", "/toppages", "jp.wsj.com", new Dictionary<string, object>
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
                    {"country", 111},
                    {"seconds", 3600},
                    {"site", (int) Sites.WsjJapan},
                });
        }

        public IEnumerable<IDataSource> WsjUs()
        {
            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "online.wsj.com-BasicHostConfiguration", 
                "BasicHostConfiguration", 
                new BasicHostConfiguration
                    {
                        Domain = "online.wsj.com",
                        MapType = MapType.Country,
                        Region = "us", 
                        PerformanceZones = new[]
                            {
                                new PerformanceZone{To = 0, From = 5, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone{To = 5, From = 7, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone{To = 7, From = 100, ZoneType = PerformanceZoneType.Hot},
                            },
                    }
                );
            yield return new ChartBeatApiDataSource("online.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/", "online.wsj.com");
            yield return new ChartBeatApiDataSource("online.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", "online.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"}
                });
            yield return new ChartBeatApiDataSource("online.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", "online.wsj.com", new Dictionary<string, object>
                {
                    {"frequency", "15"},
                    {"days_ago", "7"}
                });
            yield return new ChartBeatApiDataSource("online.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/", "online.wsj.com", new Dictionary<string, object>
                {
                    {"fields", "srvload,peoples"},
                    {"properties_ago", "min,max,avg"},
                });
            yield return new ChartBeatApiDataSource("online.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/", "online.wsj.com", new Dictionary<string, object>
                {
                    {"days_ago", "0"},
                    {"limit", "1"},
                    {"fields", "internal,search,links,direct,social"},
                });
            yield return new ChartBeatApiDataSource("online.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3", "online.wsj.com");
            yield return new ChartBeatApiDataSource("online.wsj.com-Referrers", "Referrers", "/live/referrers/v3", "online.wsj.com");
            yield return new ChartBeatApiDataSource("online.wsj.com-TopPages", "TopPages", "/toppages", "online.wsj.com", new Dictionary<string, object>
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