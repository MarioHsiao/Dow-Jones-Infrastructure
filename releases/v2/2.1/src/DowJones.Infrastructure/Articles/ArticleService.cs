using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Managers.Abstract;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Requests;
using DowJones.Preferences;
using DowJones.Session;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;
using ResponseDataSet = Factiva.Gateway.Messages.Archive.V2_0.ResponseDataSet;

namespace DowJones.Articles
{
    public class ArticleService : AbstractAggregationManager, IArticleService
    {
        private readonly ILog _log = LogManager.GetLogger( typeof( ArticleService ) );
        private readonly IPreferences _preferences;

        public ArticleService( IControlData controlData, IPreferences preferences )
            : base( controlData )
        {
            _preferences = preferences;
        }

        #region Overrides of AbstractAggregationManager

        protected override ILog Log
        {
            get
            {
                return _log;
            }
        }

        #endregion

        #region IArticleService2 Members

        public Article GetArticle(string accessionNumber, string canonicalSearchString = null)
        {
            var articleRequest = new GetArticleRequest
            {
                accessionNumbers = new[] { accessionNumber },
                responseDataSet = GetBaselinResponseDataSet(),
            };

            if (!canonicalSearchString.IsNullOrEmpty())
            {
                articleRequest.canonicalSearchString = canonicalSearchString;
            }

            var response = GetArticles(articleRequest);
            if (response != null && response.count > 0)
            {
                return response.article.FirstOrDefault();
            }
            return null;
        }

        public ArticleResponseSet GetArticles( GetArticleRequest request )
        {
            return GetArticlesWithLimit( request );
        }

        public string GetWebArticleUrl( string accessionNumber )
        {
            var request = new GetWebArticleUrlRequest
            {
                accessionNumbers = new[] { accessionNumber },
            };
            var response = Process<GetWebArticleUrlResponse>( request );

            if( response != null &&
                response.webArticleResultSet != null &&
                response.webArticleResultSet.webArticle != null &&
                response.webArticleResultSet.count > 0 )
            {
                var article = response.webArticleResultSet.webArticle.First();

                if( article.status == 0 )
                {
                    foreach( var articleProperty in article.properties
                            .Where( articleProperty => articleProperty.name == "url" ) )
                    {
                        return articleProperty.value;
                    }
                }
                else
                {
                    throw new DowJonesUtilitiesException( article.status );
                }
            }
            throw new DowJonesUtilitiesException( "GetWebArticleUri call failed" );
        }

        public string GetMultiMediaArticleUrl( string accessionNumber )
        {
            var request = new GetMultimediaArticleUrlRequest()
            {
                accessionNumbers = new[] { accessionNumber },
            };

            var response = Process<GetMultimediaArticleUrlResponse>( request );

            if( response != null &&
                response.multimediaArticleResultSet != null &&
                response.multimediaArticleResultSet.multimediaArticle != null &&
                response.multimediaArticleResultSet.count > 0 )
            {
                var article = response.multimediaArticleResultSet.multimediaArticle.First();

                if( article.status == 0 )
                {
                    foreach( var articleProperty in article.properties
                            .Where( articleProperty => articleProperty.name == "url" ) )
                    {
                        return articleProperty.value;
                    }
                }
                else
                {
                    throw new DowJonesUtilitiesException( article.status );
                }
            }
            throw new DowJonesUtilitiesException( "GetMultiMediaArticleUrl call failed" );
        }

        public ArticleResponseSet GetArticles( MixedContentArticleRequest request )
        {
            var response = new MixedContentArticleResponse();
            IList<string> archiveAns = new List<string>();
            IList<string> searchAns = new List<string>();

            if( request.ArticleReferences != null && request.ArticleReferences.Any() )
            {
                foreach( ArticleReference reference in request.ArticleReferences )
                {
                    if( IsArticleDocument( reference.ContentType ) )
                    {
                        archiveAns.Add( reference.AccessionNumber );
                    }
                    else
                    {
                        searchAns.Add( reference.AccessionNumber );
                    }
                }
            }

            /* -----> TODO: USE STA TASK SCHEDUALR FOR FORKING <----- */

            #region TASK 1
            if( archiveAns.Count > 0 )
            {
                var a = new GetArticleRequest
                {
                    accessionNumbers = archiveAns.ToArray(),
                    usageAggregator = request.UsageAggregator,
                    responseDataSet = request.ResponseDataSet,
                    canonicalSearchString = request.CanonicalSearchString
                };

                ArticleResponseSet ar = GetArticles( a );
                if( ar != null && ar.article != null )
                {
                    response.Articles = ar.article;
                }
            }
            #endregion

            #region TASK 2
            if( searchAns.Count > 0 )
            {
                var sm = new SearchManager( ControlData, _preferences );
                var dto = new AccessionNumberSearchRequestDTO
                {
                    SortBy = SortBy.FIFO,
                    MetaDataController =
                    {
                        Mode = CodeNavigatorMode.None
                    },
                    DescriptorControl =
                    {
                        Mode = DescriptorControlMode.None,
                        Language = "en"
                    },
                    AccessionNumbers = searchAns.ToArray()
                };
                dto.MetaDataController.ReturnCollectionCounts = false;
                dto.MetaDataController.ReturnKeywordsSet = false;
                dto.MetaDataController.TimeNavigatorMode = TimeNavigatorMode.None;
                dto.SearchCollectionCollection.AddRange( Enum.GetValues( typeof( SearchCollection ) ).Cast<SearchCollection>() );

                var sr = sm.GetPerformContentSearchResponse<PerformContentSearchRequest, PerformContentSearchResponse>(dto);
                if( sr != null &&
                       sr.ContentSearchResult != null &&
                       sr.ContentSearchResult.ContentHeadlineResultSet != null )
                {
                    response.ContentHeadlines =
                        sr.ContentSearchResult.ContentHeadlineResultSet.ContentHeadlineCollection;
                }
            }
            #endregion

            return AssembleResponse( request, response );
        }

        /// <summary>
        /// Assemble articles in the same sequence as it is requested, also convert headline object into Article object for conetent type
        /// other than publication and picture document
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private static ArticleResponseSet AssembleResponse( MixedContentArticleRequest request, MixedContentArticleResponse response )
        {
            IList<Article> list = new List<Article>();

            foreach( ArticleReference articleReference in request.ArticleReferences )
            {
                string an = articleReference.AccessionNumber;
                if( IsArticleDocument( articleReference.ContentType ) )
                {
                    Article art =
                        response.Articles.FirstOrDefault(
                            d => d.accessionNo != null && d.accessionNo.Equals( an, StringComparison.InvariantCultureIgnoreCase ) );
                    if( art != null )
                    {
                        list.Add( art );
                    }
                }
                else
                {
                    var headline =
                        response.ContentHeadlines.FirstOrDefault(
                            d => d.AccessionNo.Equals( an, StringComparison.InvariantCultureIgnoreCase ) );

                    if( headline != null )
                    {
                        list.Add( Mapper.Map<Article>( headline ) );
                    }
                }
            }

            return new ArticleResponseSet
            {
                article = list.ToArray(),
                count = list.Count,
                countSpecified = true
            };
        }

        #endregion

        private static bool IsArticleDocument( string contentType )
        {
            switch( contentType.ToLower() )
            {
                case "file":
                case "article":
                case "articlewithgraphics":
                case "publication":
                case "picture":
                    return true;
                /*
                case "pdf":
                case "analyst":
                case "html":
                case "multimedia":
                case "internal":
                case "summary":
                case "board":
                case "blog":
                case "customerdoc": 
                 */
                default:
                    return false;
            }
        }

        private GetArticleWithLimitResponse GetArticlesWithLimit( GetArticleWithLimitRequest request )
        {
            return Process<GetArticleWithLimitResponse>( request );
        }

        private ArticleResponseSet GetArticlesWithLimit( GetArticleRequest request )
        {
            var limitRequest = Map( request );
            var response = this.GetArticlesWithLimit( limitRequest );
            if( response == null )
            {
                return null;
            }
            var continuationContext = response.continuationContext;

            var articles = new List<Article>();
            articles.AddRange( response.articleResponseSet.article );
            while( !string.IsNullOrEmpty( continuationContext ) )
            {
                limitRequest = new GetArticleWithLimitRequest
                {
                    continuationContext = continuationContext
                };
                var eachResponse = this.GetArticlesWithLimit( limitRequest );
                if( eachResponse.articleResponseSet != null && eachResponse.articleResponseSet.count > 0 )
                {
                    articles.AddRange( eachResponse.articleResponseSet.article );
                }
                continuationContext = eachResponse.continuationContext;
            }
            response.articleResponseSet = new ArticleResponseSet
            {
                count = articles.Count,
                article = articles.ToArray()
            };
            return response.articleResponseSet;
        }

        private static GetArticleWithLimitRequest Map( GetArticleRequest request )
        {
            var limitRequest = new GetArticleWithLimitRequest
            {
                accessionNumbers = request.accessionNumbers,
                auxiliaryResponseDataSet = request.auxiliaryResponseDataSet,
                canonicalSearchString = request.canonicalSearchString,
                progressiveDisclosure = request.progressiveDisclosure,
                responseDataSet = request.responseDataSet,
                segmentIDs = request.segmentIDs,
                usageAggregator = request.usageAggregator
            };
            return limitRequest;
        }

        public static ResponseDataSet GetBaselinResponseDataSet()
        {
            var responseDataSet = new ResponseDataSet
            {
                articleFormat = ArticleFormatType.FULL,
            };
            return responseDataSet;
        }

        public GetBinaryResponse GetBinary( GetBinaryRequest request )
        {
            return Invoke<GetBinaryResponse>( request ).ObjectResponse;
        }
    }
}