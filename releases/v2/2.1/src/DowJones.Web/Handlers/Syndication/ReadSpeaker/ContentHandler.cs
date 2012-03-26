using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using DowJones.Exceptions;
using DowJones.Generators;
using DowJones.Globalization;
using DowJones.OperationalData;
using DowJones.OperationalData.Article;
using DowJones.OperationalData.EntryPoint;
using DowJones.Properties;
using DowJones.Session;
using DowJones.Web.Handlers.Syndication.Podcast;
using DowJones.Web.Handlers.Syndication.ReadSpeaker.Core;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using log4net;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Handlers.Syndication.ReadSpeaker
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentHandler : Page, IHttpAsyncHandler
    {
        protected override void OnInit(EventArgs e)
        {
            // Request Hosting permissions
            new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();

            var handlerType = typeof(BaseContentHandler);
            IHttpHandler handler;
            try
            {
                // Create the handler by calling class abc or class xyz.
                handler = (IHttpHandler)Activator.CreateInstance(handlerType, true);
            }
            catch (Exception ex)
            {
                throw new HttpException("Unable to create handler", ex);
            }
            if (handler != null)
            {
                handler.ProcessRequest(Context);
            }
            Context.ApplicationInstance.CompleteRequest();
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extradata)
        {
            return AspCompatBeginProcessRequest(context, cb, extradata);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            AspCompatEndProcessRequest(result);
        }

    }

    public class BaseContentHandler : BaseHttpHandler
    {
        private const string CONTENT_MIME_TYPE = "text/html";
        private const string CONTENT_READER_INVALID_CONTENTTYPE = "520221";
        private const string CONTENT_READER_NOT_ALLOWED_ERRORCODE = "520220";
        private const string CONTENT_READER_XSLT = "ContentReader.xslt";
        private const string ERROR_HTML_BODY = "<html><body><h2>Error</h2><div>{0}</div></body></html>";
        private const bool REQUIRES_AUTHENTICATION = false;

        private static readonly ILog m_Log = LogManager.GetLogger(typeof (ContentHandler));
        private static readonly object m_SyncObject = new object();
        private static string _resourceData = string.Empty;
        private readonly int _maxWordCount = Settings.Default.ReadSpeaker_MaxWordCount;
        private readonly string _supportedLanguages = Settings.Default.ReadSpeaker_SupportedLanguages;
        private ServiceResponse _archiveResponse;


        /// <summary>
        /// Gets a value indicating whether this handler
        /// requires users to be authenticated.
        /// </summary>
        /// <value>
        ///    <c>true</c> if authentication is required
        ///    otherwise, <c>false</c>.
        /// </value>
        public override bool RequiresAuthentication
        {
            get { return REQUIRES_AUTHENTICATION; }
        }

        /// <summary>
        /// Gets the content MIME type.
        /// </summary>
        /// <value></value>
        public override string ContentMimeType
        {
            get { return CONTENT_MIME_TYPE; }
        }

        public override void HandleRequest(HttpContext context)
        {
            // Get the parameted and token data
            var token = context.Request[ContentUrlBuilder.QsParamToken];

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(token.Trim()))
            {
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Write(HandleError("-1"));
                context.Response.End();
            }
            else
            {
                token = token.Trim();

                try
                {
                    var podcastArticleToken = new PodcastArticleToken(token);
                    // Set the ui thread to ui
                    LanguageUtilityManager.SetThreadCulture(podcastArticleToken.ContentLanguage);
                    // Send out the response
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.Write(GetArticle(podcastArticleToken));
                }
                catch (Exception ex)
                {
                    if (m_Log.IsErrorEnabled)
                    {
                        new DowJonesUtilitiesException(ex, -1);
                    }
                    // this is the fall-through event
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.Write(HandleError("-1"));
                }
                context.Response.End();
            }
        }


        /// <summary>
        /// Validates the parameters.  Inheriting classes must
        /// implement this and return true if the parameters are
        /// valid, otherwise false.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns><c>true</c> if the parameters are valid,
        /// otherwise <c>false</c></returns>
        public override bool ValidateParameters(HttpContext context)
        {
            return true;
        }

        /// <summary>
        /// Gets the article.
        /// </summary>
        /// <param name="podcastArticleToken">The token properties.</param>
        /// <returns></returns>
        private string GetArticle(PodcastArticleToken podcastArticleToken)
        {
            var tempControlData = ControlDataManager.GetLightWeightUserControlData(Settings.Default.PodcastLightweightUser);
            tempControlData.ProxyUserId = podcastArticleToken.UserId;
            tempControlData.ProxyProductId = podcastArticleToken.NameSpace;

            var userControlData = ControlDataManager.Convert(tempControlData, true);

            // Access point Code)
            if (!string.IsNullOrEmpty(podcastArticleToken.AccessPointCode) && !string.IsNullOrEmpty(podcastArticleToken.AccessPointCode.Trim()))
            {
                userControlData.AccessPointCode = podcastArticleToken.AccessPointCode;
            }

            // Client type
            if (!string.IsNullOrEmpty(podcastArticleToken.ClientType) && !string.IsNullOrEmpty(podcastArticleToken.ClientType.Trim()))
            {
                userControlData.ClientType = podcastArticleToken.ClientType;
            }

            // Generate an ODS object to set
            var articlePostProcessingViewOperationalData = new ArticlePostProcessingViewOperationalData
                                                               {
                                                                   DisplayFormat = DisplayFormatType.FullArticles, 
                                                                   Origin = OriginType.PostProcessing, 
                                                                   Destination = ViewDestination.InProduct, 
                                                                   PostProcessing = PostProcessingType.TextToSpeach
                                                               };
            UserInterfaceAndDeviceOperationalData userInterfaceAndDeviceOperationalData;

            try
            {
                _archiveResponse = ArchiveService.GetArticle(userControlData, GetArticleRequestForContentReader(podcastArticleToken.AccessionNumber));
                //check if the object is valid
                if (_archiveResponse != null)
                {
                    object articleResponseObject;
                    _archiveResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out articleResponseObject);
                    var articleResponse = (GetArticleResponse) articleResponseObject;

                    if (articleResponse != null &&
                        articleResponse.articleResponseSet != null &&
                        articleResponse.articleResponseSet.article != null)
                    {
                        if (articleResponse.articleResponseSet.article[0] != null &&
                            (articleResponse.articleResponseSet.article[0].status == null ||
                             articleResponse.articleResponseSet.article[0].status.value == 0))
                        {
                            var article = articleResponse.articleResponseSet.article[0];
                            if (articleResponse.articleResponseSet.article[0].contentParts != null)
                            {
                                if (articleResponse.articleResponseSet.article[0].contentParts.contentType.ToLower() != "article")
                                {
                                    #region RecordOperationDataRequest

                                    userInterfaceAndDeviceOperationalData = SetUserInterfaceAndDeviceOperationalData(podcastArticleToken);
                                    articlePostProcessingViewOperationalData.IsSuccessful = new[] {false};
                                    if (article.status != null)
                                    {
                                        articlePostProcessingViewOperationalData.ErrorCode = new[] {article.status.value.ToString()};
                                    }
                                    articlePostProcessingViewOperationalData.SetComponent(userInterfaceAndDeviceOperationalData);

                                    FireRecordOperationalDataRequest(userControlData, articlePostProcessingViewOperationalData);

                                    #endregion

                                    return HandleError(CONTENT_READER_INVALID_CONTENTTYPE);
                                }
                            }

                            var lang = article.baseLanguage;
                            var wordCount = article.wordCount;

                            //check if the conditions are met..
                            if (!AllowContentReader(wordCount, lang))
                            {
                                #region RecordOperationalDataRequest

                                userInterfaceAndDeviceOperationalData = SetUserInterfaceAndDeviceOperationalData(podcastArticleToken);
                                articlePostProcessingViewOperationalData.IsSuccessful = new[] {false};
                                if (article.status != null)
                                {
                                    articlePostProcessingViewOperationalData.ErrorCode = new[] {article.status.value.ToString()};
                                }
                                articlePostProcessingViewOperationalData.SetComponent(userInterfaceAndDeviceOperationalData);

                                FireRecordOperationalDataRequest(userControlData, articlePostProcessingViewOperationalData);

                                #endregion

                                return HandleError(CONTENT_READER_NOT_ALLOWED_ERRORCODE);
                            }

                            _archiveResponse.GetResponse(ServiceResponse.ResponseFormat.String, out articleResponseObject);
                            var doc = new XmlDocument();
                            doc.LoadXml((string) articleResponseObject);

                            #region RecordOperationalDataRequest

                            userInterfaceAndDeviceOperationalData = SetUserInterfaceAndDeviceOperationalData(podcastArticleToken);
                            articlePostProcessingViewOperationalData.IsSuccessful = new[] {true};
                            articlePostProcessingViewOperationalData.SourceCode = new[] {article.sourceCode};
                            articlePostProcessingViewOperationalData.SetComponent(userInterfaceAndDeviceOperationalData);

                            FireRecordOperationalDataRequest(userControlData, articlePostProcessingViewOperationalData);

                            #endregion

                            return ApplyXslt(doc, podcastArticleToken.IncludeMarketingMessage);
                        }
                        if (articleResponse.articleResponseSet.article[0] != null &&
                            (articleResponse.articleResponseSet.article[0].status != null &&
                             articleResponse.articleResponseSet.article[0].status.value != 0))
                        {
                            #region RecordOperationalDataRequest

                            userInterfaceAndDeviceOperationalData = SetUserInterfaceAndDeviceOperationalData(podcastArticleToken);
                            articlePostProcessingViewOperationalData.IsSuccessful = new[] {false};
                            articlePostProcessingViewOperationalData.ErrorCode = new[] {articleResponse.articleResponseSet.article[0].status.value.ToString()};
                            articlePostProcessingViewOperationalData.SetComponent(userInterfaceAndDeviceOperationalData);

                            FireRecordOperationalDataRequest(userControlData, articlePostProcessingViewOperationalData);

                            #endregion

                            return HandleError(articleResponse.articleResponseSet.article[0].status.value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                #region RecordOperationalDataRequest

                userInterfaceAndDeviceOperationalData = SetUserInterfaceAndDeviceOperationalData(podcastArticleToken);
                articlePostProcessingViewOperationalData.IsSuccessful = new[] {false};
                articlePostProcessingViewOperationalData.SetComponent(userInterfaceAndDeviceOperationalData);

                FireRecordOperationalDataRequest(userControlData, articlePostProcessingViewOperationalData);

                #endregion

                if (m_Log.IsDebugEnabled)
                {
                    m_Log.DebugFormat("Could not GetArticle in ContentHandler: ", ex);
                }

                return HandleError("-1");
            }

            #region RecordOperationalDataRequest

            userInterfaceAndDeviceOperationalData = SetUserInterfaceAndDeviceOperationalData(podcastArticleToken);
            articlePostProcessingViewOperationalData.IsSuccessful = new[] {false};
            articlePostProcessingViewOperationalData.SetComponent(userInterfaceAndDeviceOperationalData);

            FireRecordOperationalDataRequest(userControlData, articlePostProcessingViewOperationalData);

            #endregion

            return HandleError("-1");
        }

        /// <summary>
        /// Takes an accession  number and returns the Article response. Uses FRSP object type for billing purposes
        /// </summary>
        /// <param name="accessionNumber"></param>
        /// <returns>ServiceResposne from Gateway</returns>
        private static GetArticleRequest GetArticleRequestForContentReader(string accessionNumber)
        {
            var articleRequest = new GetArticleRequest
                                     {
                                         accessionNumbers = new[] {accessionNumber}, 
                                         canonicalSearchString = string.Empty, 
                                         progressiveDisclosure = false
                                     };

            var responseDataSet = new ResponseDataSet
                                      {
                                          articleFormat = ArticleFormatType.FRSP, 
                                          fids = null
                                      };

            articleRequest.responseDataSet = responseDataSet;
            return articleRequest;
        }

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private static string HandleError(string code)
        {
            return string.Format(ERROR_HTML_BODY, ResourceTextManager.Instance.GetErrorMessage(code));
        }

        /// <summary>
        /// Allows the content reader.
        /// </summary>
        /// <param name="wordCount">The word count.</param>
        /// <param name="baseLanguage">The base language.</param>
        /// <returns></returns>
        private bool AllowContentReader(int wordCount, string baseLanguage)
        {
            var isValid = false;
            if (HttpContext.Current.Request["HTTPS"] != "off")
            {
                return false;
            }

            //_allowCR = false; //resetting the flag.
            char[] delimiterChars = {','};
            var langs = _supportedLanguages.Split(delimiterChars);


            if (m_Log.IsInfoEnabled)
                m_Log.InfoFormat("In AllowContentReader : {0} ", baseLanguage);
            if (m_Log.IsInfoEnabled)
                m_Log.InfoFormat("In AllowContentReader : lang supported {0}", _supportedLanguages);

            //sometimes the baselang can be null...
            if (baseLanguage != null)
            {
                if (langs.Any(t => t.ToLower().Equals(baseLanguage.ToLower())))
                {
                    isValid = true;
                }
            }
            if (isValid)
            {
                isValid = false; //resetting the flag.
                //if worcount is in the range of 0 to wordCountSupported, set this to tru..
                if (wordCount > 0 && wordCount <= _maxWordCount)
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        private string ApplyXslt(IXPathNavigable article, bool includeMarketingMessage)
        {
            var value = GetEmbeddedXslt(CONTENT_READER_XSLT);

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value.Trim()))
            {
                using (var reader = new XmlTextReader(new StringReader(value)))
                {
                    using (var sw = new StringWriter())
                    {
                        var trans = new XslCompiledTransform();

                        try
                        {
                            trans.Load(reader);

                            // Set up the argument list
                            var argumentList = new XsltArgumentList();
                            if (includeMarketingMessage && ResourceTextManager.Instance.IsResourceAssemblyLoaded)
                            {
                                argumentList.AddParam("marketingMessage", string.Empty, GetMarketingMessage());
                            }
                            trans.Transform(article, argumentList, sw);
                        }
                        catch (Exception ex)
                        {
                            sw.Write(ex.Message);
                        }
                        return sw.ToString();
                    }
                }
            }
            return string.Empty;
        }

        private static string GetMarketingMessage()
        {
            return ResourceTextManager.Instance.GetString(
                string.Concat(
                    "rsMktngMessage",
                    Convert.ToInt32(RandomKeyGenerator.GetRandomKey(1, "1234"))
                    )
                );
        }

        /// <summary>
        /// Gets the embedded support xml data file.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        private string GetEmbeddedXslt(string resourceName)
        {
            if (string.IsNullOrEmpty(_resourceData) || string.IsNullOrEmpty(_resourceData.Trim()))
            {
                lock (m_SyncObject)
                {
                    // Use double checked
                    if (string.IsNullOrEmpty(_resourceData) || string.IsNullOrEmpty(_resourceData.Trim()))
                    {
                        using (var stream = GetType().Assembly.GetManifestResourceStream(GetType(), resourceName))
                        {
                            if (stream != null)
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    _resourceData = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            return _resourceData;
        }

        /// <summary>
        /// Sets the user interface and device operational data.
        /// </summary>
        /// <param name="podcastArticleToken">The podcast article token.</param>
        /// <returns></returns>
        private static UserInterfaceAndDeviceOperationalData SetUserInterfaceAndDeviceOperationalData(PodcastArticleToken podcastArticleToken)
        {
            var userInterfaceAndDeviceOperationalData = new UserInterfaceAndDeviceOperationalData();

            if (!string.IsNullOrEmpty(podcastArticleToken.Device) && !string.IsNullOrEmpty(podcastArticleToken.Device.Trim()))
            {
                userInterfaceAndDeviceOperationalData.Device = podcastArticleToken.Device;
            }

            if (!string.IsNullOrEmpty(podcastArticleToken.ProductType) && !string.IsNullOrEmpty(podcastArticleToken.ProductType.Trim()))
            {
                userInterfaceAndDeviceOperationalData.DestinationProduct = podcastArticleToken.ProductType;
            }

            return userInterfaceAndDeviceOperationalData;
        }

        /// <summary>
        /// Fires the record operational data request.
        /// </summary>
        /// <param name="userControlData">The user control data.</param>
        /// <param name="articlePostProcessingViewOperationalData">The article post processing view operational data.</param>
        private static void FireRecordOperationalDataRequest(ControlData userControlData, IBaseOperationalData articlePostProcessingViewOperationalData)
        {
            try
            {
                var controlData = Factiva.Gateway.Managers.ControlDataManager.Clone(userControlData);
                controlData.AddRange(articlePostProcessingViewOperationalData.GetKeyValues);
                MetricsService.RecordOperationData(controlData);
            }
            catch (Exception ex)
            {
                if (m_Log.IsDebugEnabled)
                {
                    m_Log.DebugFormat("Could not FireRecordOperationalDataRequest: ", ex);
                }
            }
        }
    }
}