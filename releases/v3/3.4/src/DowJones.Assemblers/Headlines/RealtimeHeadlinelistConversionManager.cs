using DowJones.Ajax.HeadlineList;
using DowJones.Formatters.Globalization.DateTime;
using Factiva.Gateway.Messages.RTQueue.V1_0;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    public class RealtimeHeadlinelistConversionManager
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(RealtimeHeadlinelistConversionManager));
        private readonly DateTimeFormatter _datetimeFormatter;
        private readonly string _interfaceLanguage = "en";

        public DateTimeFormatter DatetimeFormatter
        {
            get { return _datetimeFormatter; }
        }

        public string InterfaceLanguage
        {
            get { return _interfaceLanguage; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeHeadlinelistConversionManager"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        public RealtimeHeadlinelistConversionManager(string interfaceLanguage)
        {
            _interfaceLanguage = interfaceLanguage;
            _datetimeFormatter = new DateTimeFormatter(interfaceLanguage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeHeadlinelistConversionManager"/> class.
        /// </summary>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public RealtimeHeadlinelistConversionManager(DateTimeFormatter dateTimeFormatter)
        {
            _datetimeFormatter = dateTimeFormatter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeHeadlinelistConversionManager"/> class.
        /// </summary>
        public RealtimeHeadlinelistConversionManager()
        {
            _datetimeFormatter = new DateTimeFormatter(_interfaceLanguage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeHeadlinelistConversionManager"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="preference">The preference.</param>
        public RealtimeHeadlinelistConversionManager(string interfaceLanguage, string preference)
        {
            _interfaceLanguage = interfaceLanguage;
            _datetimeFormatter = new DateTimeFormatter(_interfaceLanguage, preference);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeHeadlinelistConversionManager"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="preference">The preference.</param>
        /// <param name="clockType">Type of the clock.</param>
        public RealtimeHeadlinelistConversionManager(string interfaceLanguage, string preference, ClockType clockType)
        {
             _interfaceLanguage = interfaceLanguage;
            _datetimeFormatter = new DateTimeFormatter(_interfaceLanguage, preference, clockType);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="generateExternalURLForHeadlineInfo">The generate external URL.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The update thumbnail.</param>
        /// <returns></returns>
        public HeadlineListDataResult Process(CreateSharedAlertResponse response, GenerateExternalUrlForHeadlineInfo generateExternalURLForHeadlineInfo, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo)
        {
            
            var converter = new CreateSharedAlertResponseConverter(response, -1, _datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalURLForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="generateExternalURLForHeadlineInfo">The generate external URL.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The update thumbnail.</param>
        /// <returns></returns>
        public HeadlineListDataResult Process(GetSharedAlertContentResponse response, GenerateExternalUrlForHeadlineInfo generateExternalURLForHeadlineInfo, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo)
        {
            var converter = new GetSharedAlertContentResponseConverter(response, -1, _datetimeFormatter);
            return (HeadlineListDataResult)converter.Process(generateExternalURLForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo);
        }
    }

}
