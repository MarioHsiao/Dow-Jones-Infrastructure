﻿using System.Collections.Generic;
using DowJones.Dash.DataSources;
using DowJones.Dash.Extentions;
using DowJones.DependencyInjection;

namespace DowJones.Dash.Website
{
    public class DataSources : DependencyInjectionModule
    {
        #region GomezCountryCode enum

        public enum GomezCountryCode
        {
            Us = 223,
            Germany = 57,
            China = 49,
            Japan = 111,
            Indonesia = 100,
            Asia = 2,
            Korea = 119
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
            WsjAsia = 8,
            WsjIndonesia = 9,
            WsjKorea = 10
        }

        #endregion

        protected override void OnLoad()
        {
            var dataSourceConfigurations = new[]
                {
                    new DataSourceConfig
                        {
                            Site = Site.WsjUs,
                            Host = "online.wsj.com",
                            Domain = "online.wsj.com",
                            Path = string.Empty,
                            GomezCountryCode = GomezCountryCode.Us
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjGermany,
                            Host = "wallstreetjournal.de",
                            Domain = "wallstreetjournal.de",
                            Path = string.Empty,
                            GomezCountryCode = GomezCountryCode.Germany
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjJapan,
                            Host = "jp.wsj.com",
                            Domain = "jp.wsj.com",
                            Path = string.Empty,
                            GomezCountryCode = GomezCountryCode.Japan
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjChina,
                            Host = "cn.wsj.com",
                            Domain = "cn.wsj.com",
                            Path = string.Empty,
                            GomezCountryCode = GomezCountryCode.China
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjAsia,
                            Host = "online.wsj.com",
                            Domain = "asia.wsj.com",
                            Path = "asia.wsj.com/home-page",
                            GomezCountryCode = GomezCountryCode.Asia
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjIndonesia,
                            Host = "online.wsj.com",
                            Domain = "indo.wsj.com",
                            Path = "indo.wsj.com/home-page",
                            GomezCountryCode = GomezCountryCode.Indonesia
                        },
                    new DataSourceConfig
                        {
                            Site = Site.WsjKorea,
                            Host = "online.wsj.com",
                            Domain = "kr.wsj.com",
                            Path = "kr.wsj.com/home-page",
                            GomezCountryCode = GomezCountryCode.Korea
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

            yield return new GomezDataSource("{0}-PageLoadDetailsBySubCountryforCountry".FormatWith(dataSourceConfig.Domain),
                                             "PageLoadDetailsBySubCountryforCountry", 
                                             "[SplunkExport].[dbo].[GetPageLoadDetailsBySubCountryforCountryNew]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"country", dataSourceConfig.GomezCountryCode},
                                                     {"seconds", 3600},
                                                     {"site", (int) dataSourceConfig.Site},
                                                 });

            yield return new GomezDataSource("{0}-PageLoadDetailsByCountryForRegion".FormatWith(dataSourceConfig.Domain),
                                             "PageLoadDetailsByCountryForRegion", 
                                             "[SplunkExport].[dbo].[GetPageLoadDetailsByCountryForRegion]",
                                             new Dictionary<string, object>
                                                 {
                                                     {"region", 0},
                                                     {"seconds", 3600},
                                                     {"site", (int) dataSourceConfig.Site},
                                                 });
        }


        public IEnumerable<IDataSource> GetHostConfigurations()
        {
            // TODO:Dry it up
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
                                new PerformanceZone {From = 7, To = 15, ZoneType = PerformanceZoneType.Hot}
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
                                new PerformanceZone {From = 10, To = 15, ZoneType = PerformanceZoneType.Hot}
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
                                new PerformanceZone {From = 10, To = 15, ZoneType = PerformanceZoneType.Hot}
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
                                new PerformanceZone {From = 10, To = 15, ZoneType = PerformanceZoneType.Hot}
                            },
                    });

            yield return new ConfigurationDataSource<BasicHostConfiguration>(
                "asia.wsj.com-BasicHostConfiguration",
                "BasicHostConfiguration",
                new BasicHostConfiguration
                    {
                        Domain = "asia.wsj.com",
                        MapType = MapType.Country,
                        Region = "asia",
                        PerformanceZones = new[]
                            {
                                new PerformanceZone {From = 0, To = 5, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 5, To = 7, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 7, To = 15, ZoneType = PerformanceZoneType.Hot}
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
                                new PerformanceZone {From = 0, To = 5, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 5, To = 7, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 7, To = 15, ZoneType = PerformanceZoneType.Hot}
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
                                new PerformanceZone {From = 0, To = 5, ZoneType = PerformanceZoneType.Cool},
                                new PerformanceZone {From = 5, To = 7, ZoneType = PerformanceZoneType.Neutral},
                                new PerformanceZone {From = 7, To = 15, ZoneType = PerformanceZoneType.Hot}
                            },
                    }
                );
        }
    }
}