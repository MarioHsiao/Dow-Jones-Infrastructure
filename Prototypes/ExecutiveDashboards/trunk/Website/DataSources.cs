using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Dash.DataSources;

namespace DowJones.Dash.Website
{
    public class DataSources : DependencyInjection.DependencyInjectionModule
    {
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
            yield return new ChartBeatDataSource("wallstreetjournal.de-DashboardStats", "DashboardStats", "/dashapi/stats/",
                host: "wallstreetjournal.de");
            yield return new ChartBeatDataSource("wallstreetjournal.de-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/",
                host: "wallstreetjournal.de",
                parameters: new Dictionary<string, object> {
                        { "frequency", "15" }
                    });
            yield return new ChartBeatDataSource("wallstreetjournal.de-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/",
                host: "wallstreetjournal.de",
                parameters: new Dictionary<string, object> {
                        {"frequency", "15"},
                        {"days_ago", "7"}
                    });
            yield return new ChartBeatDataSource("wallstreetjournal.de-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/",
                host: "wallstreetjournal.de",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,peoples"},
                        {"properties_ago", "min,max,avg"},
                    });
            yield return new ChartBeatDataSource("wallstreetjournal.de-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/",
                host: "wallstreetjournal.de",
                parameters: new Dictionary<string, object> {
                        {"days_ago", "0"},
                        {"limit", "1"},
                        {"fields", "internal,search,links,direct,social"},
                    });
            yield return new ChartBeatDataSource("wallstreetjournal.de-QuickStats", "QuickStats", "/live/quickstats/v3",
                host: "wallstreetjournal.de");
            yield return new ChartBeatDataSource("wallstreetjournal.de-Referrers", "Referrers", "/live/referrers/v3",
                host: "wallstreetjournal.de");
            yield return new ChartBeatDataSource("wallstreetjournal.de-TopPages", "TopPages", "/toppages",
                host: "wallstreetjournal.de",
                parameters: new Dictionary<string, object> {
                        {"limit", 10},
                    });
        }

        public IEnumerable<IDataSource> Barrons()
        {
            yield return new ChartBeatDataSource("online.barrons.com-DashboardStats", "DashboardStats", "/dashapi/stats/",
                host: "online.barrons.com");
            yield return new ChartBeatDataSource("online.barrons.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/",
                host: "online.barrons.com",
                parameters: new Dictionary<string, object> {
                        { "frequency", "15" }
                    });
            yield return new ChartBeatDataSource("online.barrons.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/",
                host: "online.barrons.com",
                parameters: new Dictionary<string, object> {
                        {"frequency", "15"},
                        {"days_ago", "7"}
                    });
            yield return new ChartBeatDataSource("online.barrons.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/",
                host: "online.barrons.com",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,peoples"},
                        {"properties_ago", "min,max,avg"},
                    });
            yield return new ChartBeatDataSource("online.barrons.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/",
                host: "online.barrons.com",
                parameters: new Dictionary<string, object> {
                        {"days_ago", "0"},
                        {"limit", "1"},
                        {"fields", "internal,search,links,direct,social"},
                    });
            yield return new ChartBeatDataSource("online.barrons.com-QuickStats", "QuickStats", "/live/quickstats/v3",
                host: "online.barrons.com");
            yield return new ChartBeatDataSource("online.barrons.com-Referrers", "Referrers", "/live/referrers/v3",
                host: "online.barrons.com");
            yield return new ChartBeatDataSource("online.barrons.com-TopPages", "TopPages", "/toppages",
                host: "online.barrons.com",
                parameters: new Dictionary<string, object> {
                        {"limit", 10},
                    });
        }

        public IEnumerable<IDataSource> SmartMoney()
        {
            yield return new ChartBeatDataSource("smartmoney.com-DashboardStats", "DashboardStats", "/dashapi/stats/",
                host: "smartmoney.com");
            yield return new ChartBeatDataSource("smartmoney.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/",
                host: "smartmoney.com",
                parameters: new Dictionary<string, object> {
                        { "frequency", "15" }
                    });
            yield return new ChartBeatDataSource("smartmoney.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/",
                host: "smartmoney.com",
                parameters: new Dictionary<string, object> {
                        {"frequency", "15"},
                        {"days_ago", "7"}
                    });
            yield return new ChartBeatDataSource("smartmoney.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/",
                host: "smartmoney.com",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,peoples"},
                        {"properties_ago", "min,max,avg"},
                    });
            yield return new ChartBeatDataSource("smartmoney.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/",
                host: "smartmoney.com",
                parameters: new Dictionary<string, object> {
                        {"days_ago", "0"},
                        {"limit", "1"},
                        {"fields", "internal,search,links,direct,social"},
                    });
            yield return new ChartBeatDataSource("smartmoney.com-QuickStats", "QuickStats", "/live/quickstats/v3",
                host: "smartmoney.com");
            yield return new ChartBeatDataSource("smartmoney.com-Referrers", "Referrers", "/live/referrers/v3",
                host: "smartmoney.com");
            yield return new ChartBeatDataSource("smartmoney.com-TopPages", "TopPages", "/toppages",
                host: "smartmoney.com",
                parameters: new Dictionary<string, object> {
                        {"limit", 10},
                    });
        }

        public IEnumerable<IDataSource> Marketwatch()
        {
            yield return new ChartBeatDataSource("marketwatch.com-DashboardStats", "DashboardStats", "/dashapi/stats/",
                host: "marketwatch.com");
            yield return new ChartBeatDataSource("marketwatch.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/",
                host: "marketwatch.com",
                parameters: new Dictionary<string, object> {
                        { "frequency", "15" }
                    });
            yield return new ChartBeatDataSource("marketwatch.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/",
                host: "marketwatch.com",
                parameters: new Dictionary<string, object> {
                        {"frequency", "15"},
                        {"days_ago", "7"}
                    });
            yield return new ChartBeatDataSource("marketwatch.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/",
                host: "marketwatch.com",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,peoples"},
                        {"properties_ago", "min,max,avg"},
                    });
            yield return new ChartBeatDataSource("marketwatch.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/",
                host: "marketwatch.com",
                parameters: new Dictionary<string, object> {
                        {"days_ago", "0"},
                        {"limit", "1"},
                        {"fields", "internal,search,links,direct,social"},
                    });
            yield return new ChartBeatDataSource("marketwatch.com-QuickStats", "QuickStats", "/live/quickstats/v3",
                host: "marketwatch.com");
            yield return new ChartBeatDataSource("marketwatch.com-Referrers", "Referrers", "/live/referrers/v3",
                host: "marketwatch.com");
            yield return new ChartBeatDataSource("marketwatch.com-TopPages", "TopPages", "/toppages",
                host: "marketwatch.com",
                parameters: new Dictionary<string, object> {
                        {"limit", 10},
                    });
        }

        public IEnumerable<IDataSource> WsjChina()
        {
            yield return new ChartBeatDataSource("cn.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/",
                host: "cn.wsj.com");
            yield return new ChartBeatDataSource("cn.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/",
                host: "cn.wsj.com",
                parameters: new Dictionary<string, object> {
                        { "frequency", "15" }
                    });
            yield return new ChartBeatDataSource("cn.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/",
                host: "cn.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"frequency", "15"},
                        {"days_ago", "7"}
                    });
            yield return new ChartBeatDataSource("cn.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/",
                host: "cn.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,peoples"},
                        {"properties_ago", "min,max,avg"},
                    });
            yield return new ChartBeatDataSource("cn.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/",
                host: "cn.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"days_ago", "0"},
                        {"limit", "1"},
                        {"fields", "internal,search,links,direct,social"},
                    });
            yield return new ChartBeatDataSource("cn.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3",
                host: "cn.wsj.com");
            yield return new ChartBeatDataSource("cn.wsj.com-Referrers", "Referrers", "/live/referrers/v3",
                host: "cn.wsj.com");
            yield return new ChartBeatDataSource("cn.wsj.com-TopPages", "TopPages", "/toppages",
                host: "cn.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"limit", 10},
                    });
        }

        public IEnumerable<IDataSource> WsjJapan()
        {
            yield return new ChartBeatDataSource("jp.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/",
                host: "jp.wsj.com");
            yield return new ChartBeatDataSource("jp.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/",
                host: "jp.wsj.com",
                parameters: new Dictionary<string, object> {
                        { "frequency", "15" }
                    });
            yield return new ChartBeatDataSource("jp.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/",
                host: "jp.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"frequency", "15"},
                        {"days_ago", "7"}
                    });
            yield return new ChartBeatDataSource("jp.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/",
                host: "jp.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,peoples"},
                        {"properties_ago", "min,max,avg"},
                    });
            yield return new ChartBeatDataSource("jp.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/",
                host: "jp.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"days_ago", "0"},
                        {"limit", "1"},
                        {"fields", "internal,search,links,direct,social"},
                    });
            yield return new ChartBeatDataSource("jp.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3",
                host: "jp.wsj.com");
            yield return new ChartBeatDataSource("jp.wsj.com-Referrers", "Referrers", "/live/referrers/v3",
                host: "jp.wsj.com");
            yield return new ChartBeatDataSource("jp.wsj.com-TopPages", "TopPages", "/toppages",
                host: "jp.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"limit", 10},
                    });
        }

        public IEnumerable<IDataSource> WsjUs()
        {
            yield return new ChartBeatDataSource("online.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/",
                host: "online.wsj.com");
            yield return new ChartBeatDataSource("online.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", 
                host: "online.wsj.com",
                parameters: new Dictionary<string, object> {
                        { "frequency", "15" }
                    });
            yield return new ChartBeatDataSource("online.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/",
                host: "online.wsj.com", 
                parameters: new Dictionary<string, object> {
                        {"frequency", "15"},
                        {"days_ago", "7"}
                    });
            yield return new ChartBeatDataSource("online.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/",
                host: "online.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,peoples"},
                        {"properties_ago", "min,max,avg"},
                    });
            yield return new ChartBeatDataSource("online.wsj.com-HistoricalTrafficValues", "HistoricalTrafficValues", "/historical/traffic/values/",
                host: "online.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"days_ago", "0"},
                        {"limit", "1"},
                        {"fields", "internal,search,links,direct,social"},
                    });
            yield return new ChartBeatDataSource("online.wsj.com-QuickStats", "QuickStats", "/live/quickstats/v3",
                host: "online.wsj.com");
            yield return new ChartBeatDataSource("online.wsj.com-Referrers", "Referrers", "/live/referrers/v3",
                host: "online.wsj.com");
            yield return new ChartBeatDataSource("online.wsj.com-TopPages", "TopPages", "/toppages",
                host: "online.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"limit", 10},
                    });

            yield return new GomezDataSource("online.wsj.com-BrowserStats", "BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]",
                parameters: new Dictionary<string, object> {
                        {"seconds", 300},
                    });
            yield return new GomezDataSource("online.wsj.com-DeviceTraffic", "DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]",
                parameters: new Dictionary<string, object> {
                        {"seconds", 300},
                    });
            yield return new GomezDataSource("online.wsj.com-DeviceTrafficByPage", "DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]",
                parameters: new Dictionary<string, object> {
                        {"pageid", 421139},
                        {"seconds", 300},
                    });
            yield return new GomezDataSource("online.wsj.com-PageLoadHistoricalDetails", "PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]",
                parameters: new Dictionary<string, object> {
                        {"days", 7},
                    });
            yield return new GomezDataSource("online.wsj.com-PageTimings", "PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]",
                parameters: new Dictionary<string, object> {
                        {"seconds", 300},
                    });
            yield return new GomezDataSource("online.wsj.com-PageLoadDetailsBySubCountryforCountry", "PageLoadDetailsBySubCountryforCountry", @"[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountry]",
				parameters: new Dictionary<string, object> {
                        {"country", 223},{"seconds", 3600}
                    });
        }
    }
}
