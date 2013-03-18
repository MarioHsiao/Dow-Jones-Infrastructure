using DowJones.Ajax.AddressContact;
using Factiva.Gateway.Messages.FCE.Assets.V1_0;
using Factiva.Gateway.Messages.Screening.V1_1;
using log4net;

namespace DowJones.Assemblers.Assets
{
    /// <summary>
    /// 
    /// </summary>
    public class AddressContactConversionManager
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof (AddressContactConversionManager));
        //private readonly DateTimeFormatter _datetimeFormatter;
        //private readonly string _interfaceLanguage = "en";

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="getReportListExResponse">The get report list ex response.</param>
        /// <param name="getAssetsResponse">The get assets response.</param>
        /// <returns></returns>
        public AddressContactData Process(GetReportListExResponse getReportListExResponse,
                                          GetAssetsResponse getAssetsResponse)
        {
            var converter = new AddressContactCoverter(getReportListExResponse, getAssetsResponse);
            return converter.Process();
        }
    }
}