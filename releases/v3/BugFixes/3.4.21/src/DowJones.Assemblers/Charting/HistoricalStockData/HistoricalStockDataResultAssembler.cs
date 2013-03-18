using System;
using System.Collections.Generic;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure;
using DowJones.Models.Charting.HistoricalStockData;
using DowJones.Models.Company;
using DowJones.Properties;
using Factiva.Gateway.Messages.MarketData.V1_0;
using DataPointFrequency = Factiva.Gateway.Messages.MarketData.V1_0.DataPointFrequency;
using Exchange = DowJones.Models.Company.Exchange;

namespace DowJones.Assemblers.Charting.HistoricalStockData
{
    public class HistoricalStockDataResultAssembler :
        IAssembler<HistoricalStockDataResult, GetHistoricalDataByTimePeriodExResponse>,
        IAssembler<HistoricalStockDataResult, HistoricalDataExResult>
    {
        private readonly DateTimeFormatter dateTimeFormatter;

        public HistoricalStockDataResultAssembler(DateTimeFormatter formatter)
        {
            dateTimeFormatter = formatter;
        }

        public HistoricalStockDataResult Convert(HistoricalDataExResult source, Factiva.Gateway.Messages.Symbology.Company.V1_0.Company company)
        {
            var result = new HistoricalStockDataResult
            {
                CurrencyCode = source.CurrencyCode,
                DjTicker = company != null ? company.PrimaryDowJonesTicker : source.RequestedSymbol,

                Frequency = MapDataPointFrequency(source.Frequency),
                FromDate = source.FromDateSpecified ? DateTimeFormatter.ConvertToUtc(source.FromDate) : (DateTime?)null,
                ToDate = source.ToDateSpecified ? DateTimeFormatter.ConvertToUtc(source.ToDate) : (DateTime?)null,
                InstrumentName = source.InstrumentName,
                RequestedSymbol = source.RequestedSymbol,
                Ric = company != null ? company.PrimaryRIC : source.Ric,
                DataPoints = Convert(source.DataPoints),
            };

            if (source.DataProvider != null && !string.IsNullOrWhiteSpace(source.DataProvider.name) && !string.IsNullOrWhiteSpace(source.DataProvider.refUrl))
            {
                result.Provider = new Provider
                {
                    Name = source.DataProvider.name,
                    ExternalUrl = source.DataProvider.refUrl
                };
            }
            else
            {
                result.Provider = new Provider
                {
                    Name = Settings.Default.MarketDataProvider,
                    ExternalUrl = Settings.Default.MarketDataProviderUrl
                };
            }

            if (source.Exchange != null)
            {
                result.Exchange = new Exchange
                {
                    Code = source.Exchange.Code,
                    Descriptor = source.Exchange.Value,
                    IsPrimary = true,
                };
            }
            return result;
        }

        #region Implementation of IAssembler<out HistoricalStockDataResult,in GetHistoricalDataByTimePeriodExResponse>

        public HistoricalStockDataResult Convert(GetHistoricalDataByTimePeriodExResponse response, Factiva.Gateway.Messages.Symbology.Company.V1_0.Company company)
        {
            Guard.IsNotNull(response, "response");
            Guard.IsNotNull(response.HistoricalDataResponse, "response");
            Guard.IsNotNull(response.HistoricalDataResponse.HistoricalDataResultSet, "response");

            return response.HistoricalDataResponse.HistoricalDataResultSet.count > 0 ? Convert(response.HistoricalDataResponse.HistoricalDataResultSet.HistoricalDataResult[0], company) : null;
        }

        #endregion

        protected internal StockDataPointCollection Convert(List<DataPointsEx> dataPoints)
        {
            if (dataPoints == null)
            {
                return null;
            }

            var stockDataPointCollection = new StockDataPointCollection();
            foreach (var dataPoint in dataPoints)
            {
                var stockDataPoint = new StockDataPoint
                {
                    ClosePrice = dataPoint.ClosePrice.HasValue ? new DoubleNumberStock(dataPoint.ClosePrice.Value) : null,
                    OpenPrice = dataPoint.OpenPrice.HasValue ? new DoubleNumberStock(dataPoint.OpenPrice.Value) : null,
                    DayHighPrice = dataPoint.DayHighPrice.HasValue ? new DoubleNumberStock(dataPoint.DayHighPrice.Value) : null,
                    DayLowPrice = dataPoint.DayLowPrice.HasValue ? new DoubleNumberStock(dataPoint.DayLowPrice.Value) : null,
                    Volume = dataPoint.Volume.HasValue ? new WholeNumber(dataPoint.Volume.Value) : null,
                    DataPoint = dataPoint.ClosePrice.HasValue ? new DoubleNumberStock(dataPoint.ClosePrice.Value) :
                                dataPoint.OpenPrice.HasValue ? new DoubleNumberStock(dataPoint.OpenPrice.Value) : null

                };

                // First get always returns false for some reason
                // first time accessing dataPoint.DateSpecified always give false for some reason
                // var temp = dataPoint.DateSpecified; - returns false
                // temp = dataPoint.DateSpecified; - returns true
                if (dataPoint.Date != default(DateTime))
                {
                    stockDataPoint.Date = DateTimeFormatter.ConvertToUtc(dataPoint.Date);
                    stockDataPoint.DateDisplay = dateTimeFormatter.FormatDate(dataPoint.Date);
                }
                stockDataPointCollection.Add(stockDataPoint);
            }
            return stockDataPointCollection;
        }

        private static Models.Charting.DataPointFrequency MapDataPointFrequency(DataPointFrequency? frequency)
        {
            switch (frequency)
            {
                default:
                    return Models.Charting.DataPointFrequency.Daily;
                case DataPointFrequency.Weekly:
                    return Models.Charting.DataPointFrequency.Weekly;
                case DataPointFrequency.Monthly:
                    return Models.Charting.DataPointFrequency.Monthly;
                case DataPointFrequency.Quarterly:
                    return Models.Charting.DataPointFrequency.Quarterly;
                case DataPointFrequency.Yearly:
                    return Models.Charting.DataPointFrequency.Yearly;
            }
        }

        #region Implementation of IAssembler<out HistoricalStockDataResult,in GetHistoricalDataByTimePeriodExResponse>

        public HistoricalStockDataResult Convert(GetHistoricalDataByTimePeriodExResponse source)
        {
            return Convert(source, null);
        }

        #endregion

        #region Implementation of IAssembler<out HistoricalStockDataResult,in HistoricalDataResult>

        public HistoricalStockDataResult Convert(HistoricalDataExResult source)
        {
            return Convert(source, null);
        }

        #endregion
    }
}
