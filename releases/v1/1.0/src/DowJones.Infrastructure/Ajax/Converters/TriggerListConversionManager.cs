using DowJones.Tools.Ajax;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.TriggerList;
using DowJones.Utilities.Ajax.Converters.TriggerList;
using DowJones.Utilities.Ajax.TriggerList.Converters;
using DowJones.Utilities.Formatters.Globalization;
using Factiva.Gateway.Messages.Trigger.V1_1;
using Factiva.Gateway.Messages.Trigger.V2_0;
using log4net;

namespace DowJones.Utilities.Ajax.Converters
{
    public class TriggerListConversionManager
    {
        protected static readonly ILog _log = LogManager.GetLogger(typeof(TriggerListConversionManager));
        private readonly DateTimeFormatter _datetimeFormatter;
        private readonly string _interfaceLanguage = "en";
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerListConversionManager"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        public TriggerListConversionManager(string interfaceLanguage)
        {
            _interfaceLanguage = interfaceLanguage;
            _datetimeFormatter = new DateTimeFormatter(interfaceLanguage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerListConversionManager"/> class.
        /// </summary>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public TriggerListConversionManager(DateTimeFormatter dateTimeFormatter)
        {
            _datetimeFormatter = dateTimeFormatter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerListConversionManager"/> class.
        /// </summary>
        public TriggerListConversionManager()
        {
            _datetimeFormatter = new DateTimeFormatter(_interfaceLanguage);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public TriggerListDataResult Process(TriggerSearchResponse response)
        {
            IListDataResultConverter converter = new TriggerSearchResponseConverter(response, _datetimeFormatter);
            return (TriggerListDataResult)converter.Process();
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public TriggerListDataResult Process(SearchTriggersResponse response)
        {
            IListDataResultConverter converter = new SearchTriggersResponseConverter(response, _datetimeFormatter);
            return (TriggerListDataResult)converter.Process();
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="generateExternalURLForTriggerInfo">The generate external URL for trigger info.</param>
        /// <param name="generateExternalURLForHeadlineInfo">The generate external URL for headline info.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The generate snippet thumbnail for headline info.</param>
        /// <returns></returns>
        public TriggerListDataResult Process(TriggerSearchResponse response, GenerateExternalUrlForTriggerInfo generateExternalURLForTriggerInfo, GenerateExternalUrlForHeadlineInfo generateExternalURLForHeadlineInfo, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo, GenerateExternalUrlForPropertyInfo generateExternalURLForPropertyInfo)
        {
            var converter = new TriggerSearchResponseConverter(response, _datetimeFormatter);
            return (TriggerListDataResult)converter.Process(generateExternalURLForTriggerInfo, generateExternalURLForHeadlineInfo, generateSnippetThumbnailForHeadlineInfo, generateExternalURLForPropertyInfo);
        }
    }
}
