using System.Collections.Generic;
using System.Linq;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Session;
using Factiva.Gateway.Messages.MarketData.V1_0;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;
using Factiva.Gateway.Messages.Symbology.Util.V1_0;
using Factiva.Gateway.Messages.Symbology.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;

namespace DowJones.Managers.Sparkline
{
    public interface ISparklineService
    {
        IEnumerable<SparklineDataSet> GetData(SparklineServiceRequest request);
    }

    public class SparklineService : ISparklineService
    {
        private readonly IControlData _controlData;
        private GetHistoricalDataByDataPointsResponse _historicalDataResponse;


        public SparklineService(IControlData controlData)
        {
            _controlData = controlData;
        }

        public IEnumerable<SparklineDataSet> GetData(SparklineServiceRequest request)
        {
            //1. Validate
            ValidateRequest(request);

            //2. Get ticker from symbology
            var companyRequest = new GetCompaniesRequest();
            companyRequest.CodeCollection.AddRange(request.CompanyCodes);
            companyRequest.CodeType = CompanyCodeType.FactivaCompany;
            companyRequest.ElementsToReturnCollection.AddRange(new[]
                                                                   {
                                                                       CompanyElements.Status,
                                                                       CompanyElements.ThirdPartyInstrumentCodes
                                                                   });

            ServiceResponse serviceResponse =
                SymbologyCompanyService.GetCompanies(ControlDataManager.Convert(_controlData), companyRequest);
            var companyResponse = serviceResponse.GetObject<GetCompaniesResponse>();

            if (companyResponse == null || companyResponse.CompanyResultSet == null ||
                companyResponse.CompanyResultSet.CompanyResultCollection.Count == 0)
            {
                return Enumerable.Empty<SparklineDataSet>();
            }

            CompanyResultCollection companyList = companyResponse.CompanyResultSet.CompanyResultCollection;

            var symbols = new List<Symbol>();
            for (int i = 0; i < companyList.Count; i++)
            {
                CompanyResult t = companyList[i];
                Company result = t.ResultCompany;
                if (result != null)
                {
                    var symbol = new Symbol();
                    if (result.CompanyStatus.ListingStatus == ListingStatus.Listed && result.PrimaryDowJonesTicker != null)
                    {
                        symbol.Code = result.PrimaryDowJonesTicker;
                        symbol.CodeScheme = CodeScheme.DJ;
                        symbols.Add(symbol);
                    }
                }
            }


            //3. Get quotes
//                var quoteRequest = new GetQuoteExRequest {QuickQuote = true};
//                quoteRequest.SymbolCollection.AddRange(ricCodes);
//                GetQuote(quoteRequest);

            //4. Get data points
            if (symbols.Count > 0)
            {
                var dataPointRequest = new GetHistoricalDataByDataPointsRequest
                                           {
                                               numberOfDataPoints = 5,
                                               frequency = DataPointFrequency.Daily,
                                               adjustForCapitalChanges = true,
                                               adjustForCapitalChangesSpecified = true,
                                               symbols = symbols.Select(a => a.Code).ToArray(),
                                               CodeScheme = CodeScheme.DJ
                                           };

                GetDataPoint(dataPointRequest);
            }


            //Assemble final result set
//            IEnumerable<Quote> quotes = Enumerable.Empty<Quote>();
//            if (_quoteExResponse != null && _quoteExResponse.QuoteResponse != null &&
//                _quoteExResponse.QuoteResponse.quoteResultSet != null &&
//                _quoteExResponse.QuoteResponse.quoteResultSet.quote != null)
//            {
//                quotes = _quoteExResponse.QuoteResponse.quoteResultSet.quote;
//            }

            IEnumerable<HistoricalDataResult> dataPoints = Enumerable.Empty<HistoricalDataResult>();
            if (_historicalDataResponse != null && _historicalDataResponse.historicalDataResponse != null
                && _historicalDataResponse.historicalDataResponse.historicalDataResultSet != null
                && _historicalDataResponse.historicalDataResponse.historicalDataResultSet.historicalDataResult != null)
            {
                dataPoints = _historicalDataResponse.historicalDataResponse.historicalDataResultSet.historicalDataResult;
            }

            var list = new List<SparklineDataSet>();
            foreach (CompanyResult result in companyList)
            {
                if (result.ResultCompany != null)
                {
                    var set = new SparklineDataSet {Company = result.ResultCompany};
                    string ticker = set.Company.PrimaryDowJonesTicker;
                    set.HistoricalDataResult =
                        dataPoints.SingleOrDefault(data => data.requestedSymbol != null && data.requestedSymbol.Equals(ticker));
                    list.Add(set);
                }
            }
            return list;
        }

        private void GetQuote(GetQuoteExRequest request)
        {
            ServiceResponse serviceResponse = MarketDataService.GetQuoteEx(ControlDataManager.Convert(_controlData),
                                                                           request);
            serviceResponse.GetObject<GetQuoteExResponse>();
        }

        private void GetDataPoint(GetHistoricalDataByDataPointsRequest request)
        {
            ServiceResponse serviceResponse =
                MarketDataService.GetHistoricalDataByDataPoints(ControlDataManager.Convert(_controlData), request);
            _historicalDataResponse = serviceResponse.GetObject<GetHistoricalDataByDataPointsResponse>();
        }

        private static void ValidateRequest(SparklineServiceRequest request)
        {
            if (request == null || request.CompanyCodes == null || !request.CompanyCodes.Any())
            {
                throw new DowJonesUtilitiesException("Invalid request");
            }
        }
    }
}