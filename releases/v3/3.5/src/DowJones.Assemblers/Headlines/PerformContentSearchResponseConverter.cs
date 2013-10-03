// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformContentSearchResponseConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Assemblers.Headlines
{
    internal class PerformContentSearchResponseConverter : AbstractHeadlineListDataResultSetConverter, IExtendedListDataResultConverter
    {
        protected static readonly ILog Log = LogManager.GetLogger( typeof( PerformContentSearchResponseConverter ) );
        private readonly IPerformContentSearchResponse _response;
        private readonly int _startIndex;
        private readonly HeadlineListDataResult _result = new HeadlineListDataResult();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public PerformContentSearchResponseConverter( IPerformContentSearchResponse response, int startIndex, string interfaceLanguage )
            : base( interfaceLanguage )
        {
            _response = response;
            _startIndex = startIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="startIndex">
        /// The start Index.
        /// </param>
        public PerformContentSearchResponseConverter( IPerformContentSearchResponse response, int startIndex )
            : this( response, startIndex, "en" )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformContentSearchResponseConverter"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="dateTimeFormatter">The date time formatter.</param>
        public PerformContentSearchResponseConverter( IPerformContentSearchResponse response, int startIndex, DateTimeFormatter dateTimeFormatter )
            : base( dateTimeFormatter )
        {
            _response = response;
            _startIndex = startIndex;
        }

        public override IListDataResult Process()
        {
            return Process( null, null );
        }

        public IListDataResult Process( Delegate generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnailForHeadlineInfoDelegate )
        {
            return Process( ( GenerateExternalUrlForHeadlineInfo ) generateExternalUrl, generateSnippetThumbnailForHeadlineInfoDelegate );
        }

        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="generateExternalUrl">The generate external URL.</param>
        /// <param name="generateSnippetThumbnail">The update thumbnail.</param>
        /// <returns> an IListDataResult object</returns>
        public IListDataResult Process( GenerateExternalUrlForHeadlineInfo generateExternalUrl, GenerateSnippetThumbnailForHeadlineInfo generateSnippetThumbnail )
        {
            GenerateExternalUrlForHeadlineInfo = generateExternalUrl;
            GenerateSnippetThumbnailForHeadlineInfo = generateSnippetThumbnail;

            if( _response == null || _response.ContentSearchResult == null || _response.ContentSearchResult.ContentHeadlineResultSet == null || _response.ContentSearchResult.HitCount <= 0 )
            {
                // Add the HitCount to the result set
                _result.hitCount = new WholeNumber( 0 );

                _result.resultSet.first = _result.hitCount;
                _result.resultSet.count = _result.hitCount;
                _result.resultSet.duplicateCount = _result.hitCount;
                return _result;
            }

            // Add the HitCount to the result set
            _result.hitCount = new WholeNumber( _response.ContentSearchResult.HitCount );

            var resultSet = _response.ContentSearchResult.ContentHeadlineResultSet;
            _result.resultSet.first = new WholeNumber( resultSet.First );

            if( resultSet.Count <= 0 )
            {
                return _result;
            }

            if( _response.ContentSearchResult.DeduplicatedHeadlineSet != null && _response.ContentSearchResult.DeduplicatedHeadlineSet.Count > 0 )
            {
                _result.resultSet.count = new WholeNumber( _response.ContentSearchResult.DeduplicatedHeadlineSet.Count );
                _result.resultSet.duplicateCount = new WholeNumber( resultSet.Count - _response.ContentSearchResult.DeduplicatedHeadlineSet.Count );

                // Format
                ProcessDeduplicatedHeadlines( resultSet, _response.ContentSearchResult.DeduplicatedHeadlineSet );
            }
            else
            {
                _result.resultSet.count = new WholeNumber( resultSet.Count );
                _result.resultSet.duplicateCount = new WholeNumber( 0 );
                ProcessContentHeadlines( resultSet );
            }

            return _result;
        }

        private void ProcessDeduplicatedHeadlines( ContentHeadlineResultSet contentHeadlineResultSet, DeduplicatedHeadlineSet deduplicatedHeadlineSet )
        {
            var headlineDictionary = contentHeadlineResultSet.ContentHeadlineCollection.ToDictionary( headline => headline.AccessionNo );
            var i = ( _startIndex > 0 ) ? _startIndex : contentHeadlineResultSet.First;

            foreach( var reference in deduplicatedHeadlineSet.HeadlineRefCollection )
            {
                var curHeadline = headlineDictionary[ reference.AccessionNo ];
                var curHeadlineInfo = new HeadlineInfo();
                Convert( curHeadlineInfo, curHeadline, false, ++i );
                if( reference.Duplicates.Count > 0 )
                {
                    var j = 0;
                    foreach( var headlineRef in reference.Duplicates.DuplicateRefCollection )
                    {
                        var curDedupHeadlineInfo = new DedupHeadlineInfo
                        {
                            ParentAccessionNo = reference.AccessionNo
                        };
                        var tempCurHeadline = headlineDictionary[ headlineRef.AccessionNo ];
                        Convert( curDedupHeadlineInfo, tempCurHeadline, true, ++j );
                        curHeadlineInfo.duplicateHeadlines.Add( curDedupHeadlineInfo );
                    }
                }

                _result.resultSet.headlines.Add( curHeadlineInfo );
            }
        }

        private void ProcessContentHeadlines( ContentHeadlineResultSet contentHeadlineResultSet )
        {
            var i = ( _startIndex > 0 ) ? _startIndex : contentHeadlineResultSet.First;
            foreach( var headline in contentHeadlineResultSet.ContentHeadlineCollection )
            {
                var headlineInfo = new HeadlineInfo();
                Convert( headlineInfo, headline, false, ++i );
                _result.resultSet.headlines.Add( headlineInfo );
            }
        }
    }
}