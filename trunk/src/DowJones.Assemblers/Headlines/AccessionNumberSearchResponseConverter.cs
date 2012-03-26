﻿// --------------------------------------------------------------------------------------------------------------------
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
using DowJones.Managers.Search.Responses;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    /// <summary>
    /// The accession number search response converter.
    /// </summary>
    internal class AccessionNumberSearchResponseConverter : AbstractHeadlineListDataResultSetConverter, IExtendedListDataResultConverter
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(AccessionNumberSearchResponseConverter));

        /// <summary>
        /// The _response.
        /// </summary>
        private readonly AccessionNumberSearchResponse response;

        /// <summary>
        /// The _result.
        /// </summary>
        private readonly HeadlineListDataResult result = new HeadlineListDataResult();


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
        public AccessionNumberSearchResponseConverter(AccessionNumberSearchResponse response, string interfaceLanguage)
            : base(interfaceLanguage)
        {
            this.response = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessionNumberSearchResponseConverter"/> class. 
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public AccessionNumberSearchResponseConverter(AccessionNumberSearchResponse response)
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
        public AccessionNumberSearchResponseConverter(AccessionNumberSearchResponse response, DateTimeFormatter dateTimeFormatter)
            : base(dateTimeFormatter)
        {
            this.response = response;
        }

        #region IExtendedListDataResultConverter Members

        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="generateExternalUrl">The generate external url.</param>
        /// <param name="generateSnippetThumbnailForHeadlineInfo">The generate snippet thumbnail for headline info.</param>
        /// <returns></returns>
        public IListDataResult Process(Delegate generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfo)
        {
            return Process((GenerateExternalUrlForHeadlineInfo)generateExternalUrl, generateSnippetThumbnailForHeadlineInfo);
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

            if (response == null || response.AccessionNumberBasedContentItemSet == null || response.AccessionNumberBasedContentItemSet.Count <= 0)
                return result;

            // Add the HitCount to the result set
            result.hitCount = new WholeNumber(response.AccessionNumberBasedContentItemSet.Count);

// Format
            NumberFormatter.Format(result.hitCount);

            if (response.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection == null)
                return result;

            result.resultSet.first = new WholeNumber(0);

            if (response.AccessionNumberBasedContentItemSet.Count <= 0)
                return result;

            result.resultSet.count = new WholeNumber(response.AccessionNumberBasedContentItemSet.Count);
            result.resultSet.duplicateCount = result.resultSet.count;
            ProcessContentHeadlines(response.AccessionNumberBasedContentItemSet);

            NumberFormatter.Format(result.resultSet.first);
            NumberFormatter.Format(result.resultSet.count);
            NumberFormatter.Format(result.resultSet.duplicateCount);

            result.isTimeInGMT = DateTimeFormatter.CurrentTimeZone == TimeZoneManager.GmtTimeZone;
            return result;
        }


        /// <summary>
        /// The process content headlines.
        /// </summary>
        /// <param name="contentHeadlineResultSet">
        /// The content headline result set.
        /// </param>
        private void ProcessContentHeadlines(AccessionNumberBasedContentItemSet contentHeadlineResultSet)
        {
            int i = 0;
            foreach (var headline in contentHeadlineResultSet.AccessionNumberBasedContentItemCollection)
            {
                if (!headline.HasBeenFound)
                {
                    result.resultSet.headlines.Add(new HeadlineInfo(headline.AccessionNumber, ++i));
                    continue;
                }

                var headlineInfo = new HeadlineInfo();
                Convert(headlineInfo, headline.ContentHeadline, false, ++i);
                result.resultSet.headlines.Add(headlineInfo);
            }
        }
    }
}