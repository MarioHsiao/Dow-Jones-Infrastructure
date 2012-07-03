using System;
using System.Collections.Generic;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Models.Company;
using DowJones.Properties;

namespace DowJones.Assemblers.Quotes
{
    public class QuoteAssembler : IAssembler<Quote, Factiva.Gateway.Messages.MarketData.V1_0.Quote>
    {
        private readonly DateTimeFormatter dateTimeFormatter;

        public QuoteAssembler(DateTimeFormatter formatter)
        {
            dateTimeFormatter = formatter;
        }

        #region Implementation of IAssembler<out Quote,in Quote>

        public Quote Convert(Factiva.Gateway.Messages.MarketData.V1_0.Quote source, Factiva.Gateway.Messages.Symbology.Company.V1_0.Company company)
        {
            var quote = new Quote
            {
                AskPrice = source.askPriceSpecified ? new DoubleNumberStock(source.askPrice) : null,
                BidPrice = source.bidPriceSpecified ? new DoubleNumberStock(source.bidPrice) : null,
                Change = source.changeSpecified ? new DoubleNumber(source.change) : null,
                ClosePrice = source.closePriceSpecified ? new DoubleNumber(source.closePrice) : null,
                PercentageChange = source.percentageChangeSpecified ? new Percent(source.percentageChange) : null,
                High = source.dayHighPriceSpecified ? new DoubleNumberStock(source.dayHighPrice) : null,
                Low = source.dayLowPriceSpecified ? new DoubleNumberStock(source.dayLowPrice) : null,
                Open = source.openPriceSpecified ? new DoubleNumberStock(source.openPrice) : null,
                Last = source.lastTradePriceSpecified ? new DoubleNumberStock(source.lastTradePrice) : null,
                CloseDate = source.closeDateSpecified ? DateTimeFormatter.ConvertToUtc(source.closeDate) : (DateTime?)null,
                CloseDateDisplay = source.closeDateSpecified ? dateTimeFormatter.FormatDate(source.closeDate) : null,
                DjTicker = (company != null) ? company.PrimaryDowJonesTicker : source.djTicker,
                Ric = (company != null) ? company.PrimaryRIC : source.ric,
                Currency = source.currency,
                FCode = string.Empty,
                CompanyName = source.companyName,
                ListedExchanges = new List<Exchange>(new[]
                                                                         {
                                                                             new Exchange
                                                                                 {
                                                                                     Code = source.exchange,
                                                                                     IsPrimary = true,
                                                                                 }
                                                                         }),

                Volume = source.volumeSpecified ? new WholeNumber(source.volume) : null,

                LastTradeDateTime = source.lastTradePriceSpecified ? DateTimeFormatter.ConvertToUtc(source.lastTradeDateTime) : (DateTime?)null,
                LastTradeDateTimeDisplay = source.lastTradePriceSpecified ? dateTimeFormatter.FormatLongDateTime(DateTimeFormatter.ConvertToUtc(source.lastTradeDateTime)) : null,

                LastTradePrice = source.lastTradePriceSpecified ? new DoubleNumberStock(source.lastTradePrice) : null,

                FiftyTwoWeekHighDate = source.last52WeekHighDateSpecified ? DateTimeFormatter.ConvertToUtc(source.last52WeekHighDate) : (DateTime?)null,
                FiftyTwoWeekHighDateDisplay = source.last52WeekHighDateSpecified ? dateTimeFormatter.FormatDate(source.last52WeekHighDate) : null,
                FiftyTwoWeekHigh = source.last52WeekHighDateSpecified ? new DoubleNumberStock(source.last52WeekHighPrice) : null,

                FiftyTwoWeekLowDate = source.last52WeekLowDateSpecified ? DateTimeFormatter.ConvertToUtc(source.last52WeekLowDate) : (DateTime?)null,
                FiftyTwoWeekLowDateDisplay = source.last52WeekLowDateSpecified ? dateTimeFormatter.FormatDate(source.last52WeekLowDate) : null,
                FiftyTwoWeekLow = source.last52WeekLowDateSpecified ? new DoubleNumberStock(source.last52WeekLowPrice) : null,
            };

            if (source.dataProvider != null && !string.IsNullOrWhiteSpace(source.dataProvider.name) && !string.IsNullOrWhiteSpace(source.dataProvider.refUrl))
            {
                quote.Provider = new Provider
                {
                    Name = source.dataProvider.name,
                    ExternalUrl = source.dataProvider.refUrl
                };
            }
            else
            {
                quote.Provider = new Provider
                {
                    Name = Settings.Default.MarketDataProvider,
                    ExternalUrl = Settings.Default.MarketDataProviderUrl
                };
            }

            return quote;
        }

        #endregion

        #region Implementation of IAssembler<out Quote,in Quote>

        public Quote Convert(Factiva.Gateway.Messages.MarketData.V1_0.Quote source)
        {
            return Convert(source, null);
        }

        #endregion
    }
}
