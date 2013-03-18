using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Serialization;
using DowJones.Managers.Abstract;
using DowJones.Properties;
using DowJones.Thunderball.Library.Charting;

namespace DowJones.Managers.Charting.MarketData
{
    public class MarketDataChartingManager : IExternalService
    {
        [Obsolete]
        public static Dictionary<string, ChartDataResponse> GetChart(string[] symbols, TimePeriod timePeriod = TimePeriod.OneDay, Frequency frequency = Frequency.FifteenMinutes)
        {
            var chartRequests = new[]
                                                {
                                                    new Thunderball.Library.GetChartRequest
                                                        {
                                                            Time = GetXmlEnumName<TimePeriod>(timePeriod.ToString()),
                                                            Freq = GetXmlEnumName<Frequency>(frequency.ToString()),
                                                            Symbol = symbols,
                                                            EntitlementToken = Settings.Default.ThunderBallEntitlementToken,
                                                        }
                                                };


            var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            var endpointAddress = new EndpointAddress(Settings.Default.ThunderBallEndpointAddress);
            var chartServiceClient = new ChartServiceClient(basicHttpBinding, endpointAddress);
            return chartServiceClient.GetChart(chartRequests);
        }

        public static MarketChartDataServiceResult GetMarketChartData( string[] symbols, SymbolType symbolType = SymbolType.FCode, TimePeriod timePeriod = TimePeriod.OneDay, Frequency frequency = Frequency.FifteenMinutes)
        {
            var marketChartDataServiceResult = new MarketChartDataServiceResult();
            marketChartDataServiceResult.Populate(
                new MarketDataServiceRequest
                    {
                        Symbols = symbols, 
                        SymbolType = symbolType,
                        TimePeriod = timePeriod, 
                        Frequency = frequency
                    });
            return marketChartDataServiceResult;
        }

         /// <summary>
        /// Gets the name of the XML enum.
        /// </summary>
        /// <typeparam name="T">Any valid type.</typeparam>
        /// <param name="value">The string.</param>
        /// <returns>The string name associated with the enum</returns>
        internal static string GetXmlEnumName<T>(string value)
        {
            var fieldInfo = typeof(T).GetField(value);
            var enumAttribute = (XmlEnumAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(XmlEnumAttribute));
            return enumAttribute != null ? enumAttribute.Name : value;
        }
    }
}
