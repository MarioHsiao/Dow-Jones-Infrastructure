// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessionNumberSearchResponseConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Globalization.TimeZone;
using Factiva.Gateway.Messages.PCM.Search.V1_0;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    /// <summary>
    /// The accession number search response converter.
    /// </summary>
    internal class PCMAccessionNumberSearchResponseConverter : AbstractHeadlineListDataResultSetConverter, IExtendedListDataResultConverter
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof (AccessionNumberSearchResponseConverter));

        /// <summary>
        /// The _response.
        /// </summary>
        private readonly PCMGetCollectionHeadlinesByCodeResponse _response;

        /// <summary>
        /// The _result.
        /// </summary>
        private readonly HeadlineListDataResult _result = new HeadlineListDataResult();


        /// <summary>
        /// Initializes a new instance of the <see cref="AccessionNumberSearchResponseConverter"/> class. 
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="interfaceLanguage">
        /// The interface language.
        /// </param>
        public PCMAccessionNumberSearchResponseConverter(PCMGetCollectionHeadlinesByCodeResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            _response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessionNumberSearchResponseConverter"/> class. 
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public PCMAccessionNumberSearchResponseConverter(PCMGetCollectionHeadlinesByCodeResponse response)
            : this(response, "en")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessionNumberSearchResponseConverter"/> class. 
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="dateTimeFormatter">
        /// The date time formatter.
        /// </param>
        public PCMAccessionNumberSearchResponseConverter(PCMGetCollectionHeadlinesByCodeResponse response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            _response = response;
        }

        public bool IncludeInvalidHeadlines { get; set; }


        #region IExtendedListDataResultConverter Members

        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="generateExternalUrl">
        /// The generate external url.
        /// </param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">
        /// The generate snippet thumbnail for headline info.
        /// </param>
        /// <returns>
        /// </returns>
        public IListDataResult Process(Delegate generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo)
        {
            return Process((GenerateExternalUrlForHeadlineInfo) generateExternalUrl, generateSnippetThumbnailForHeadlineInfo);
        }

        #endregion

        /// <summary>
        /// The process.
        /// </summary>
        /// <returns>
        /// </returns>
        public override IListDataResult Process()
        {
            return Process(null, null);
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="generateExternalUrl">
        /// The generate external URL.
        /// </param>
        /// <param name="generateSnippetThumbnail">
        /// The update thumbnail.
        /// </param>
        /// <returns>
        /// </returns>
        public IListDataResult Process(GenerateExternalUrlForHeadlineInfo generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnail)
        {
            GenerateExternalUrlForHeadlineInfo = generateExternalUrl;
            GenerateSnippetThumbnailForHeadlineInfo = generateSnippetThumbnail;

            if (_response == null ||
                _response.AccessionNumberSearchResponse == null ||
                _response.AccessionNumberSearchResponse.AccessionNumberBasedContentItemSet == null ||
                _response.AccessionNumberSearchResponse.AccessionNumberBasedContentItemSet.Count <= 0)
                return _result;

            // Add the HitCount to the result set
            _result.hitCount = new WholeNumber(_response.AccessionNumberSearchResponse.AccessionNumberBasedContentItemSet.Count);

            // Format
            NumberFormatter.Format(_result.hitCount);

            if (_response.AccessionNumberSearchResponse.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection == null)
                return _result;

            _result.resultSet.first = new WholeNumber(0);

            if (_response.AccessionNumberSearchResponse.AccessionNumberBasedContentItemSet.Count <= 0)
                return _result;

            _result.resultSet.count = new WholeNumber(_response.AccessionNumberSearchResponse.AccessionNumberBasedContentItemSet.Count);
            _result.resultSet.duplicateCount = _result.resultSet.count;

            ProcessContentHeadlines(_response.AccessionNumberSearchResponse.AccessionNumberBasedContentItemSet);

            NumberFormatter.Format(_result.resultSet.first);
            NumberFormatter.Format(_result.resultSet.count);
            NumberFormatter.Format(_result.resultSet.duplicateCount);

            _result.isTimeInGMT = DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone;
            return _result;
        }


        /// <summary>
        /// The process content headlines.
        /// </summary>
        /// <param name="contentHeadlineResultSet">
        /// The content headline result set.
        /// </param>
        private void ProcessContentHeadlines(AccessionNumberBasedContentItemSet contentHeadlineResultSet)
        {
            var i = 0;
            foreach (var headline in contentHeadlineResultSet.AccessionNumberBasedContentItemCollection)
            {
                if (headline.HasBeenFound == false && IncludeInvalidHeadlines)
                {
                    _result.resultSet.headlines.Add(new HeadlineInfo(headline.AccessionNumber, ++i));
                    continue;
                }

                var headlineInfo = new HeadlineInfo();
                Convert(headlineInfo, headline.ContentHeadline, false, ++i);
                _result.resultSet.headlines.Add(headlineInfo);
            }
        }
    }
}