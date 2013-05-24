//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="StockDataService.cs" company="Dow Jones">
////   © 2010 Dow Jones, Inc. All rights reserved.
//// </copyright>
//// --------------------------------------------------------------------------------------------------------------------

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel;
//using System.Xml.Serialization;
//using DowJones.Thunderball.Library.Charting;
//using ConfigurationSettings = DowJones.Web.Showcase.Properties.Settings;

//namespace DowJones.Web.Shows.Services.Charting
//{
//    public enum TimePeriod
//    {
//        /// <summary>
//        /// One hour.
//        /// </summary>
//        [XmlEnum("1hr")]
//        OneHour,
        
//        /// <summary>
//        /// One day.
//        /// </summary>
//        [XmlEnum( "1dy" )]
//        OneDay,

//        /// <summary>
//        /// Two days.
//        /// </summary>
//        [XmlEnum( "2dy" )]
//        TwoDays,

//        /// <summary>
//        /// Five days.
//        /// </summary>
//        [XmlEnum( "5dy" )]
//        FiveDays,

//        /// <summary>
//        /// Ten days.
//        /// </summary>
//        [XmlEnum( "10dy" )]
//        TenDays,

//        /// <summary>
//        /// One Month
//        /// </summary>
//        [XmlEnum( "1mo" )]
//        OneMonth,

//        /// <summary>
//        /// Two months.
//        /// </summary>
//        [XmlEnum( "2mo" )]
//        TwoMonths,

//        /// <summary>
//        /// Three months 
//        /// </summary>
//        [XmlEnum( "3mo" )]
//        ThreeMonths,

//        /// <summary>
//        /// Six months.
//        /// </summary>
//        [XmlEnum( "6mo" )]
//        SixMonths,

//        /// <summary>
//        /// One year.
//        /// </summary>
//        [XmlEnum( "1yr" )]
//        OneYear,

//        /// <summary>
//        /// Two years.
//        /// </summary>
//        [XmlEnum( "2yr" )]
//        TwoYears,

//        /// <summary>
//        /// Three years.
//        /// </summary>
//        [XmlEnum( "3yr" )]
//        ThreeYears,

//        /// <summary>
//        /// Four years
//        /// </summary>
//        [XmlEnum( "4yr" )]
//        FourYears,

//        /// <summary>
//        /// Five years
//        /// </summary>
//        [XmlEnum( "5yr" )]
//        FiveYears,

//        /// <summary>
//        /// Ten years
//        /// </summary>
//        [XmlEnum( "10yr" )]
//        TenYears,

//        /// <summary>
//        /// Year to date.
//        /// </summary>
//        [XmlEnum( "Ytd" )]
//        YearToDate,

//        /// <summary>
//        /// All available data
//        /// </summary>
//        [XmlEnum( "All" )]
//        All
//    }

//    public enum Frequency
//    {
//        /// <summary>
//        /// One hour
//        /// </summary>
//        [XmlEnum( "1hr" )]
//        OneMinute,
//        /// <summary>
//        /// Five minutes
//        /// </summary>
//        [XmlEnum( "1hr" )]
//        FiveMinutes,

//        /// <summary>
//        /// fifteen minutes.
//        /// </summary>
//        [XmlEnum( "15mi" )]
//        FifteenMinutes,

//        /// <summary>
//        /// One hour
//        /// </summary>
//        [XmlEnum( "1hr" )]
//        OneHour,

//        /// <summary>
//        /// One day
//        /// </summary>
//        [XmlEnum( "1dy" )]
//        OneDay,

//        /// <summary>
//        /// One Month
//        /// </summary>
//        [XmlEnum( "1mo" )]
//        OneMonth,

//        /// <summary>
//        /// Three months
//        /// </summary>
//        [XmlEnum( "3mo" )]
//        ThreeMonths,

//        /// <summary>
//        /// One year
//        /// </summary>
//        [XmlEnum( "1yr" )]
//        OneYear,
//    }

//    public static class StockDataService
//    {
//        /// <summary>
//        /// Gets the chart.
//        /// </summary>
//        /// <param name="symbol">The symbol.</param>
//        /// <param name="timePeriod">The time period.</param>
//        /// <param name="frequency">The frequency.</param>
//        /// <returns>A dictionary of chart data responses</returns>
//        public static Dictionary<string, ChartDataResponse> GetChart( IEnumerable<string> symbol, TimePeriod timePeriod = TimePeriod.OneDay, Frequency frequency = Frequency.FifteenMinutes )
//        {
//            var request = new[]
//                             {
//                               new Thunderball.Library.GetChartRequest
//                                   {
//                                        Time = GetXmlEnumName<TimePeriod>(timePeriod.ToString()),
//                                        Freq = GetXmlEnumName<Frequency>(frequency.ToString()),
//                                        Symbol = symbol.ToArray(),
//                                        EntitlementToken = ConfigurationSettings.Default.ThunderBallEntitlementToken,
//                                   }         
//                            };

//            var basicHttpBinding = new BasicHttpBinding( BasicHttpSecurityMode.None );
//            var endpointAddress = new EndpointAddress( ConfigurationSettings.Default.ThunderBallEndpointAddress );

//            var chartServiceClient = new ChartServiceClient( basicHttpBinding, endpointAddress );
//            return chartServiceClient.GetChart( request );
//        }

//        /// <summary>
//        /// Gets the name of the XML enum.
//        /// </summary>
//        /// <typeparam name="T">Any valid type.</typeparam>
//        /// <param name="value">The string.</param>
//        /// <returns>The string name associated with the enum</returns>
//        public static string GetXmlEnumName<T>( string value )
//        {
//            var fieldInfo = typeof( T ).GetField( value );
//            var enumAttribute = ( XmlEnumAttribute ) Attribute.GetCustomAttribute( fieldInfo, typeof( XmlEnumAttribute ) );
//            return enumAttribute != null ? enumAttribute.Name : value;
//        }
//    }
//}
