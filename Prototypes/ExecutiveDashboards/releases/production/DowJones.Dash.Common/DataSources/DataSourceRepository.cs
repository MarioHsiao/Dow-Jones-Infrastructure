using System.Collections.Generic;

namespace DowJones.Dash.DataSources
{
    public interface IDataSourceRepository
    {
        IEnumerable<IDataSource> GetDataSources();
    }

    public class DataSourceRepository : IDataSourceRepository
    {
        public IEnumerable<IDataSource> GetDataSources()
        {
            yield return new ChartBeatDataSource("DashboardStats", "/dashapi/stats/");
            yield return new ChartBeatDataSource("HistorialTrafficSeries", "/historical/traffic/series/", 
                parameters: new Dictionary<string, object> {
                        { "frequency", "5" }
                    });
            yield return new ChartBeatDataSource("HistorialTrafficSeriesWeekAgo", "/historical/traffic/series/", 
                parameters: new Dictionary<string, object> {
                        {"frequency", "5"},
                        {"days_ago", "7"},
                        {"limit", "288"},
                    });
            yield return new ChartBeatDataSource("HistoricalTrafficStats", "/historical/traffic/stats/",
                parameters: new Dictionary<string, object> {
                        {"fields", "srvload,people,srvload"},
                        {"properties_ago", "min,max,avg"},
                    });
            yield return new ChartBeatDataSource("HistoricalTrafficValues", "/historical/traffic/values/",
                parameters: new Dictionary<string, object> {
                        {"days_ago", "0"},
                        {"limit", "1"},
                        {"fields", "internal,search,links,direct,social"},
                    });
            yield return new ChartBeatDataSource("QuickStats", "/live/quickstats/v3");
            yield return new ChartBeatDataSource("Referrers", "/live/referrers/v3");
            yield return new ChartBeatDataSource("TopPages", "/toppages",
                parameters: new Dictionary<string, object> {
                        {"limit", 10},
                    });



            yield return new GomezDataSource("BrowserStats", @"[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]",
                parameters: new Dictionary<string, object> {
                        {"seconds", 300},
                    });
            yield return new GomezDataSource("DeviceTraffic", @"[SplunkExport].[dbo].[GetDeviceTraffic]",
                parameters: new Dictionary<string, object> {
                        {"seconds", 300},
                    });
            yield return new GomezDataSource("DeviceTrafficByPage", @"[SplunkExport].[dbo].[GetDeviceTrafficByPage]",
                parameters: new Dictionary<string, object> {
                        {"pageid", 421139},
                        {"seconds", 300},
                    });
            yield return new GomezDataSource("PageLoadHistoricalDetails", @"[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]",
                parameters: new Dictionary<string, object> {
                        {"days", 7},
                    });
            yield return new GomezDataSource("PageTimings", @"[SplunkExport].[dbo].[GetPageLoadDetails]",
                parameters: new Dictionary<string, object> {
                        {"seconds", 300},
                    });
			yield return new GomezDataSource("PageLoadDetailsByCityForUS", @"[SplunkExport].[dbo].[GetPageLoadDetailsByCityForUS]",
				parameters: new Dictionary<string, object> {
                        {"seconds", 600},
                    });
        }
    }
}
