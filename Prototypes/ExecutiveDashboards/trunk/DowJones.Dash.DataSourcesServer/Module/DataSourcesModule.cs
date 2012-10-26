using System.Collections.Generic;
using DowJones.Dash.Common.DataSources;
using DowJones.Dash.Extentions;
using Ninject.Modules;

namespace DowJones.Dash.DataSourcesServer.Module
{
    public class DataSourcesModule : NinjectModule
    {
        #region GomezCountryCode enum

        public enum GomezCountryCode
        {
            Us = 223,
            Germany = 57,
            China = 49,
            Japan = 111,
            Indonesia = 100,
            AsiaPacific = 2,
            Korea = 119,
            World = 0,
        }

        #endregion

        #region Site enum

        public enum Site
        {
            WsjUs = 1,
            WsjGermany = 2,
            WsjJapan = 3,
            WsjChina = 4,
            SmartMoney = 5,
            Barrons = 6,
            MarketWatch = 7,
            WsjAsiaPacific = 8,
            WsjIndonesia = 9,
            WsjKorea = 10,
        }

        #endregion

        public override void Load()
        {
            var dataSourceConfigurations = new[]
                {
                    new DataSourceConfig
                        {
                            Site = Site.WsjUs,
                            Host = "online.wsj.com",
                            Domain = "online.wsj.com",
                            Path = string.Empty,
                            GomezCountryCode = GomezCountryCode.Us,
							MapType = MapType.Country
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjGermany,
                            Host = "wallstreetjournal.de",
                            Domain = "wallstreetjournal.de",
                            Path = string.Empty,
                            GomezCountryCode = GomezCountryCode.Germany,
							MapType = MapType.Country
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjJapan,
                            Host = "jp.wsj.com",
                            Domain = "jp.wsj.com",
                            Path = string.Empty,
                            GomezCountryCode = GomezCountryCode.Japan,
							MapType = MapType.Country
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjChina,
                            Host = "cn.wsj.com",
                            Domain = "cn.wsj.com",
                            Path = string.Empty,
                            GomezCountryCode = GomezCountryCode.China,
							MapType = MapType.Country
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjAsiaPacific,
                            Host = "online.wsj.com",
                            Domain = "asia.wsj.com",
                            Path = "asia.wsj.com/home-page",
                            GomezCountryCode = GomezCountryCode.AsiaPacific,
							MapType = MapType.Region
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjIndonesia,
                            Host = "online.wsj.com",
                            Domain = "indo.wsj.com",
                            Path = "indo.wsj.com/home-page",
                            GomezCountryCode = GomezCountryCode.Indonesia,
							MapType = MapType.Country
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjKorea,
                            Host = "online.wsj.com",
                            Domain = "kr.wsj.com",
                            Path = "kr.wsj.com/home-page",
                            GomezCountryCode = GomezCountryCode.Korea,
							MapType = MapType.Country
                        },
						new DataSourceConfig
                        {
                            Site = Site.WsjUs,
                            Host = "online.wsj.com",
                            Domain = "world.wsj.com",
                            Path = "world.wsj.com/home-page",
                            GomezCountryCode = GomezCountryCode.World,
							MapType = MapType.World
                        }
                };

            var dataSources = new List<IDataSource>();

            dataSources.AddRange(GetHostConfigurations());

            foreach (var dataSourceConfiguration in dataSourceConfigurations)
            {
                dataSources.AddRange(GetDataSourceBySite(dataSourceConfiguration));
            }


            foreach (var dataSource in dataSources)
            {
                Bind<IDataSource>().ToConstant(dataSource);
            }
        }


        public IEnumerable<IDataSource> GetDataSourceBySite(DataSourceConfig dataSourceConfig)
        {
            yield return new ChartBeatApiDataSource("{0}-DashboardStats".FormatWith(dataSourceConfig.Domain),
                                                    "DashboardStats",
                                                    "/dashapi/stats/",
                                                    dataSourceConfig.Host,
                                                    new Dictionary<string, object>
                                                        {
                                                            {"path", dataSourceConfig.Path}
                                                        });

            yield return new ChartBeatApiDataSource("{0}-HistorialTrafficSeries".FormatWith(dataSourceConfig.Domain),
                                                    "HistorialTrafficSeries",
                                                    "/historical/traffic/series/",
                                                    dataSourceConfig.Host,
                                                    new Dictionary<string, object>
                                                        {
                                                            {"path", dataSourceConfig.Path},
                                                            {"frequency", "15"}
                                                        });

            yield return new ChartBeatApiDataSource("{0}-HistorialTrafficSeriesWeekAgo".FormatWith(dataSourceConfig.Domain),
                                                    "HistorialTrafficSeriesWeekAgo",
                                                    "/historical/traffic/series/",
                                                    dataSourceConfig.Host,
                                                    new Dictionary<string, object>
                                                        {
                                                            {"path", dataSourceConfig.Path},
                                                            {"frequency", "15"},
                                                            {"days_ago", "7"}
                                                        });

            yield return new ChartBeatApiDataSource("{0}-HistoricalTrafficStats".FormatWith(dataSourceConfig.Domain),
                                                    "HistoricalTrafficStats",
                                                    "/historical/traffic/stats/",
                                                    dataSourceConfig.Host,
                                                    new Dictionary<string, object>
                                                        {
                                                            {"path", dataSourceConfig.Path},
                                                            {"fields", "srvload,people"},
                                                            {"properties", "min,max,avg"},
                                                        });

            yield return new ChartBeatApiDataSource("{0}-HistoricalTrafficValues".FormatWith(dataSourceConfig.Domain),
                                                    "HistoricalTrafficValues",
                                                    "/historical/traffic/values/",
                                                    dataSourceConfig.Host,
                                                    new Dictionary<string, object>
                                                        {
                                                            {"path", dataSourceConfig.Path},
                                                            {"days_ago", "0"},
                                                            {"limit", "1"},
                                                            {"fields", "internal,search,links,direct,social"},
                                                        });

            yield return new ChartBeatApiDataSource("{0}-QuickStats".FormatWith(dataSourceConfig.Domain),
                                                    "QuickStats",
                                                    "/live/quickstats/v3",
                                                    dataSourceConfig.Host,
                                                    new Dictionary<string, object>
                                                        {
                                                            {"path", dataSourceConfig.Path},
                                                        });

            yield return new ChartBeatApiDataSource("{0}-Referrers".FormatWith(dataSourceConfig.Domain),
                                                    "Referrers",
                                                    "/live/referrers/v3",
                                                    dataSourceConfig.Host,
                                                    new Dictionary<string, object>
                                                        {
                                                            {"path", dataSourceConfig.Path},
                                                        });

            yield return new ChartBeatApiDataSource("{0}-TopPages".FormatWith(dataSourceConfig.Domain),
                                                    "TopPages",
                                                    "/toppages",
                                                    dataSourceConfig.Host,
                                                    new Dictionary<string, object>
                                                        {
                                                            {"path", dataSourceConfig.Path},
                                                            {"limit", 10},
                                                        });

            yield return new GomezDataSource("{0}-BrowserStats".FormatWith(dataSourceConfig.Domain),
                                             "BrowserStats",
                                             "[SplunkExport].[dbo].[GetPageLoadDetailsByBrowser]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"seconds", 3600},
                                                     {"site", (int) dataSourceConfig.Site},
                                                 });

            yield return new GomezDataSource("{0}-DeviceTraffic".FormatWith(dataSourceConfig.Domain),
                                             "DeviceTraffic",
                                             "[SplunkExport].[dbo].[GetDeviceTraffic]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"seconds", 300},
                                                     {"site", (int) dataSourceConfig.Site},
                                                 });

            yield return new GomezDataSource("{0}-PageLoadHistoricalDetails".FormatWith(dataSourceConfig.Domain),
                                             "PageLoadHistoricalDetails",
                                             "[SplunkExport].[dbo].[GetPageLoadHistoricalDetails]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"days", 7},
                                                     {"site", (int) dataSourceConfig.Site},
                                                 });

            yield return new GomezDataSource("{0}-PageTimings".FormatWith(dataSourceConfig.Domain), "PageTimings",
                                             "[SplunkExport].[dbo].[GetPageLoadDetails]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"seconds", 3600},
                                                     {"site", (int) dataSourceConfig.Site},
                                                 });

            yield return new GomezDataSource("{0}-PageLoadDetailsByType".FormatWith(dataSourceConfig.Domain),
                                             "PageLoadDetailsByType",
                                             "[SplunkExport].[dbo].[GetPageLoadDetailsByType]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"type", dataSourceConfig.MapType.ToString().ToLower()},
													 {"id", dataSourceConfig.GomezCountryCode},
                                                     {"seconds", 3600},
                                                     {"site", (int) dataSourceConfig.Site},
													 
                                                 });

            yield return new GomezDataSource("{0}-PageLoadDetailsByTypeForWorld".FormatWith(dataSourceConfig.Domain),
                                             "PageLoadDetailsByTypeForWorld",
                                             "[SplunkExport].[dbo].[GetPageLoadDetailsByType]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"type", MapType.World.ToString().ToLower()},
													 {"id", 0},
                                                     {"seconds", 3600},
                                                     {"site", (int) dataSourceConfig.Site},
                                                 });


            yield return new GomezDataSource("{0}-DeviceTraffic".FormatWith(dataSourceConfig.Domain),
                                             "DeviceTraffic",
                                             "[SplunkExport].[dbo].[GetDeviceTraffic]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"seconds", 3600},
                                                     {"site", (int) dataSourceConfig.Site},
                                                 });
        }


        public IEnumerable<IDataSource> GetHostConfigurations()
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
                                new PerformanceZone {From = 0, To = 5, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 5, To = 7, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 7, To = 30, ZoneType = PerformanceZoneType.Hot}
                            },
                }
                );

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
                                new PerformanceZone {From = 0, To = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 8, To = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 10, To = 30, ZoneType = PerformanceZoneType.Hot}
                            },
                }
                );

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
                                new PerformanceZone {From = 0, To = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 8, To = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 10, To = 30, ZoneType = PerformanceZoneType.Hot}
                            },
                });

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
                                new PerformanceZone {From = 0, To = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 8, To = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 10, To = 30, ZoneType = PerformanceZoneType.Hot}
                            },
                });

            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "asia.wsj.com-BasicHostConfiguration",
                "BasicHostConfiguration",
                new BasicHostConfiguration
                {
                    Domain = "online.wsj.com",
                    MapType = MapType.Region,
                    Region = "apac",
                    PerformanceZones = new[]
                            {
                                new PerformanceZone {From = 0, To = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 8, To = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 10, To = 30, ZoneType = PerformanceZoneType.Hot}
                            },
                }
                );

            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "indo.wsj.com-BasicHostConfiguration",
                "BasicHostConfiguration",
                new BasicHostConfiguration
                {
                    Domain = "indo.wsj.com",
                    MapType = MapType.Country,
                    Region = "id", // indonesia TLD
                    PerformanceZones = new[]
                            {
                                new PerformanceZone {From = 0, To = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 8, To = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 10, To = 30, ZoneType = PerformanceZoneType.Hot}
                            },
                }
                );

            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "kr.wsj.com-BasicHostConfiguration",
                "BasicHostConfiguration",
                new BasicHostConfiguration
                {
                    Domain = "kr.wsj.com",
                    MapType = MapType.Country,
                    Region = "kr",
                    PerformanceZones = new[]
                            {
                                new PerformanceZone {From = 0, To = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 8, To = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 10, To = 30, ZoneType = PerformanceZoneType.Hot}
                            },
                }
                );

            /*yield return new ConfigurationDataSource<BasicHostConfiguration>(
               "world.wsj.com-BasicHostConfiguration",
               "BasicHostConfiguration",
               new BasicHostConfiguration
               {
                   Domain = "online.wsj.com",
                   MapType = MapType.Region,
                   Region = "world",
                   PerformanceZones = new[]
                            {
                                new PerformanceZone {From = 0, To = 8, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 8, To = 10, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 10, To = 30, ZoneType = PerformanceZoneType.Hot}
                            },
               }
               );*/
        }
    }
}