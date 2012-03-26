// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoricalStockDataResultAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Web.Mvc.UI.Models.Company;
using Factiva.Gateway.Messages.MarketData.V1_0;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common
{
    public class HistoricalStockDataResultAssembler : 
        IAssembler<HistoricalStockDataResult, GetHistoricalDataByTimePeriodResponse>,
        IAssembler<HistoricalStockDataResult, HistoricalDataResult>
    {
        private readonly DateTimeFormatter dateTimeFormatter;

        public HistoricalStockDataResultAssembler(DateTimeFormatter formatter)
        {
            dateTimeFormatter = formatter;
        }

        public HistoricalStockDataResult Convert(HistoricalDataResult source, Company company)
        {
            var result = new HistoricalStockDataResult
                             {
                                 CurrencyCode = source.currencyCode,
                                 DjTicker = company != null ? company.PrimaryDowJonesTicker : source.djTicker,
                                 Exchange = new Exchange
                                                {
                                                    Code = source.exchange,
                                                    IsPrimary = true,
                                                 },


                                 Frequency = MapDataPointFrequency(source.frequency),
                                 FromDate = source.fromDateSpecified ? DateTimeFormatter.ConvertToUtc(source.fromDate) : (DateTime?)null,
                                 ToDate = source.toDateSpecified ? DateTimeFormatter.ConvertToUtc(source.toDate) : (DateTime?)null,
                                 InstrumentName = source.instrumentName,
                                 RequestedSymbol = source.requestedSymbol,
                                 Ric = company != null ? company.PrimaryRIC : source.ric,
                                 Provider = new Provider
                                                 {
                                                     Name = Properties.Settings.Default.MarketDataProvider,
                                                     ExternalUrl = Properties.Settings.Default.MarketDataProviderUrl
                                                 },
                                 DataPoints = Convert(source.dataPoints),
                             };
            return result;
        }

        #region Implementation of IAssembler<out HistoricalStockDataResult,in GetHistoricalDataByTimePeriodResponse>

        public HistoricalStockDataResult Convert(GetHistoricalDataByTimePeriodResponse response, Company company)
        {
            Guard.IsNotNull(response, "response");
            Guard.IsNotNull(response.historicalDataResponse, "response");
            Guard.IsNotNull(response.historicalDataResponse.historicalDataResultSet, "response");

            return response.historicalDataResponse.historicalDataResultSet.count > 0 ? Convert(response.historicalDataResponse.historicalDataResultSet.historicalDataResult[0], company) : null;
        }

        #endregion

        protected internal StockDataPointCollection Convert(DataPoints[] dataPoints)
        {
            if (dataPoints == null)
            {
                return null;
            }

            
            return new StockDataPointCollection(dataPoints.Select(dataPoint => new StockDataPoint
            {
                ClosePrice = dataPoint.closePriceSpecified ? new DoubleNumberStock(dataPoint.closePrice) : null,
                OpenPrice = dataPoint.openPriceSpecified ? new DoubleNumberStock(dataPoint.openPrice) : null,
                DayHighPrice = dataPoint.dayHighPriceSpecified ? new DoubleNumberStock(dataPoint.dayHighPrice) : null,
                DayLowPrice = dataPoint.dayLowPriceSpecified ? new DoubleNumberStock(dataPoint.dayLowPrice) : null,
                Date = dataPoint.dateSpecified ? DateTimeFormatter.ConvertToUtc(dataPoint.date) : (DateTime?)null,
                DateDisplay = dataPoint.dateSpecified ? dateTimeFormatter.FormatDate(dataPoint.date) : null,
                Volume = dataPoint.volumeSpecified ? new WholeNumber(dataPoint.volume) : null,
                DataPoint = dataPoint.closePriceSpecified ? new DoubleNumberStock(dataPoint.closePrice) : 
                            dataPoint.openPriceSpecified ? new DoubleNumberStock(dataPoint.openPrice) : null,
            }).ToList());
        }

        private static UI.Models.Core.DataPointFrequency MapDataPointFrequency(DataPointFrequency frequency)
        {
            switch (frequency)
            {
                default:
                    return UI.Models.Core.DataPointFrequency.Daily;
                case DataPointFrequency.Weekly:
                    return UI.Models.Core.DataPointFrequency.Weekly;
                case DataPointFrequency.Monthly:
                    return UI.Models.Core.DataPointFrequency.Monthly;
                case DataPointFrequency.Quarterly:
                    return UI.Models.Core.DataPointFrequency.Quarterly;
                case DataPointFrequency.Yearly:
                    return UI.Models.Core.DataPointFrequency.Yearly;
            }
        }

        #region Implementation of IAssembler<out HistoricalStockDataResult,in GetHistoricalDataByTimePeriodResponse>

        public HistoricalStockDataResult Convert(GetHistoricalDataByTimePeriodResponse source)
        {
            return Convert(source, null);
        }

        #endregion

        #region Implementation of IAssembler<out HistoricalStockDataResult,in HistoricalDataResult>

        public HistoricalStockDataResult Convert(HistoricalDataResult source)
        {
            return Convert(source, null);
        }

        #endregion
    }
}
