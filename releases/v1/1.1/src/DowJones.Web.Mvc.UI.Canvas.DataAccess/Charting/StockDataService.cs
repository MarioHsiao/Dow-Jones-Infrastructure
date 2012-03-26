// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockDataService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml.Serialization;
using DowJones.Thunderball.Library.Charting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Charting
{
    public enum TimePeriod
    {
        /// <summary>
        /// One day time period.
        /// </summary>
        [XmlEnum("1dy")]
        OneDay,
    }

    public enum Frequency
    {
        /// <summary>
        /// fifteen minutes.
        /// </summary>
        [XmlEnum("15mi")]
        FifteenMinutes,
    }

    public static class StockDataService
    {
        /// <summary>
        /// Gets the chart.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <param name="frequency">The frequency.</param>
        /// <returns>A dictionary of chart data responses</returns>
        public static Dictionary<string, ChartDataResponse> GetChart(IEnumerable<string> symbol, TimePeriod timePeriod = TimePeriod.OneDay, Frequency frequency = Frequency.FifteenMinutes)
        {
            var request = new[]
                             {
                               new Thunderball.Library.GetChartRequest
                                   {
                                        Time = GetXmlEnumName<TimePeriod>(timePeriod.ToString()),
                                        Freq = GetXmlEnumName<Frequency>(frequency.ToString()),
                                        Symbol = symbol.ToArray(),
                                        EntitlementToken = Properties.Settings.Default.ThunderBallEntitlementToken,
                                   }         
                            };
            
            var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            var endpointAddress = new EndpointAddress(Properties.Settings.Default.ThunderBallEndpointAddress);

            var chartServiceClient = new ChartServiceClient(basicHttpBinding, endpointAddress);
            return chartServiceClient.GetChart(request);
        }

        /// <summary>
        /// Gets the name of the XML enum.
        /// </summary>
        /// <typeparam name="T">Any valid type.</typeparam>
        /// <param name="value">The string.</param>
        /// <returns>The string name associated with the enum</returns>
        public static string GetXmlEnumName<T>(string value)
        {
            var fieldInfo = typeof(T).GetField(value);
            var enumAttribute = (XmlEnumAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(XmlEnumAttribute));
            return enumAttribute != null ? enumAttribute.Name : value;
        }
    }
}
