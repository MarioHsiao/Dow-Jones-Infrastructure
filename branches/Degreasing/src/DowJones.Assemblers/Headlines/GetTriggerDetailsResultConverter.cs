// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetTriggerDetailsResultConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Globalization.TimeZone;
using Factiva.Gateway.Messages.Trigger.V1_1;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    public class GetTriggerDetailsResultConverter : AbstractHeadlineListDataResultSetConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(GetTriggerDetailsResultConverter));       
        private readonly TriggerDetailResponse response;                               
        private readonly HeadlineListDataResult result = new HeadlineListDataResult();

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTriggerDetailsResultConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public GetTriggerDetailsResultConverter(TriggerDetailResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            this.response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTriggerDetailsResultConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        public GetTriggerDetailsResultConverter(TriggerDetailResponse response)
            : this(response, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTriggerDetailsResultConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public GetTriggerDetailsResultConverter(TriggerDetailResponse response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            this.response = response;
        }

        #region Overrides of AbstractHeadlineListDataResultSetConverter

        public override IListDataResult Process()
        {
            return Process(null);
        }

        #endregion

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="generateExternalURLForHeadlineInfo">The generate external URL.</param>
        /// <returns></returns>
        public IListDataResult Process(GenerateExternalUrlForHeadlineInfo generateExternalURLForHeadlineInfo)
        {
            GenerateExternalUrlForHeadlineInfo = generateExternalURLForHeadlineInfo;

            if (response == null || response.TriggerDetailResult == null || response.TriggerDetailResult.TriggerDetailResultSet == null || response.TriggerDetailResult.TriggerDetailResultSet.TriggerCollection.Count <= 0)
            {
                return result;
            }

            // Format
            NumberFormatter.Format(result.hitCount);

            var resultSet = response.TriggerDetailResult.TriggerDetailResultSet;
            result.resultSet.first = new WholeNumber(response.TriggerDetailResult.FirstResult + 1);

            var masterTrigger = new MasterTrigger();
            if (response.TriggerDetailResult.TriggerDetailResultSet.TriggerCollection[0].GetType().FullName == typeof(MasterTrigger).FullName)
            {
                masterTrigger = response.TriggerDetailResult.TriggerDetailResultSet.TriggerCollection[0] as MasterTrigger;
            }

            result.isTimeInGMT = DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone;

            // Add the HitCount and Requested Count to the result set
            if (masterTrigger != null && masterTrigger.Count > 0)
            {
                result.hitCount = new WholeNumber(masterTrigger.DocumentCount);
                result.resultSet.count = new WholeNumber(masterTrigger.Count);
                result.resultSet.duplicateCount = new WholeNumber(masterTrigger.Count);
            }
            else
            {
                return result;
            }

            // Format
            NumberFormatter.Format(result.resultSet.first);
            NumberFormatter.Format(result.resultSet.count);
            NumberFormatter.Format(result.resultSet.duplicateCount);
            ProcessTriggerDetailHeadlines(resultSet);

            return result;
        }

        private void ProcessTriggerDetailHeadlines(TriggerDetailResultSet contentHeadlineResultSet)
        {
            int i = response.TriggerDetailResult.FirstResult + 1;
            foreach (MasterTrigger tr in contentHeadlineResultSet.TriggerCollection)
            {
                foreach (var document in tr.DocumentCollection)
                {
                    var headlineInfo = new HeadlineInfo();
                    Convert(headlineInfo, document.ContentHeadline, false, ++i);
                    result.resultSet.headlines.Add(headlineInfo);
                }
            }
        }
    }
}
