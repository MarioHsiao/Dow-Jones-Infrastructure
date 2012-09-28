using System;
using System.Collections.Generic;
using DowJones.Dash.DataSources;

namespace DowJones.Dash.Website
{
    public class DataSources : DependencyInjection.DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            var dataSources = GetDataSources();

            foreach (var dataSource in dataSources)
            {
                Bind<IDataSource>().ToConstant(dataSource);
            }
        }

        public IEnumerable<IDataSource> GetDataSources()
        {
            yield return new ChartBeatDataSource("online.wsj.com-DashboardStats", "DashboardStats", "/dashapi/stats/",
                host: "online.wsj.com");
            yield return new ChartBeatDataSource("online.wsj.com-HistorialTrafficSeries", "HistorialTrafficSeries", "/historical/traffic/series/", 
                host: "online.wsj.com",
                parameters: new Dictionary<string, object> {
                        { "frequency", "15" }
                    }, 
                pollDelay: (int)TimeSpan.FromMinutes(3).TotalSeconds);
            yield return new ChartBeatDataSource("online.wsj.com-HistorialTrafficSeriesWeekAgo", "HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/",
                host: "online.wsj.com", 
                parameters: new Dictionary<string, object> {
                        {"frequency", "15"},
                        {"days_ago", "7"},
                        {"limit", "288"},
                    }, 
                pollDelay: (int)TimeSpan.FromMinutes(3).TotalSeconds);
            yield return new ChartBeatDataSource("online.wsj.com-HistoricalTrafficStats", "HistoricalTrafficStats", "/historical/traffic/stats/",
                host: "online.wsj.com",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,people,srvload"},
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
                        {"country", 223},{"seconds", 600},
                    });
        }
    }
}
