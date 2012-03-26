using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using EMG.Utility.Generators;
using EMG.Utility.Handlers.Syndication.Podcast.Core;
using EMG.Utility.Managers;
using EMG.Utility.Managers.Core;
using EMG.Utility.Properties;
using EMG.Utility.TokenEncryption;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using log4net;
 
namespace EMG.Utility.Handlers.Syndication.Podcast
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadSpeakerContent : IHttpHandler
    {
        private static readonly string CONTENT_READER_INVALID_CONTENTTYPE = "520221";
        private static readonly string CONTENT_READER_NOT_ALLOWED_ERRORCODE = "520220";
        private static readonly string m_ContentReader_XSLT = "ContentReader.xslt";
        private static readonly string m_ErrorHtmlBody = "<html><body><h2>Error</h2><div>{0}</div></body></html>";

        private static readonly ILog m_Log = LogManager.GetLogger(typeof (ReadSpeakerContent));
        private static readonly int m_MaxWordCount = 5000;
        private static readonly string m_SupportedLanguages = "en,es,fr,de,it";
        private static readonly object m_SyncObject = new object();
        private static string m_ResourceData = string.Empty;
        private readonly bool m_IsReusable = false;
        private ServiceResponse m_ArchiveResponse;

        #region IHttpHandler Members

        ///<summary>
        ///Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            // Get the parameted and token data
            string token = context.Request[ReadspeakerPodcastContentUrlBuilder.QS_PARAM_TOKEN];

            if (string.IsNullOrEmpty(token) || !string.IsNullOrEmpty(token.Trim()))
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
                    PodcastArticleToken podcastArticleToken = new PodcastArticleToken(token);
                    // Send out the response
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.Write(GetArticle(podcastArticleToken));
                    context.Response.End();
                    return;
                }
                catch
                {
                    // this is the fallthrough event
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.Write(HandleError("-1"));
                    context.Response.End();
                }
            }
        }

        ///<summary>
        ///Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.
        ///</returns>
        ///
        public bool IsReusable
        {
            get { return m_IsReusable; }
        }

        #endregion

        /// <summary>
        /// Gets the article.
        /// </summary>
        /// <param name="podcastArticleToken">The token properties.</param>
        /// <returns></returns>
        private string GetArticle(PodcastArticleToken podcastArticleToken)
        {
            ControlData userControlData = ControlDataManager.AddProxyCredentialsToControlData(
                ControlDataManager.GetLightWeightUserControlData(Settings.Default.Podcast_LightweightUser),
                podcastArticleToken.UserId,
                podcastArticleToken.NameSpace
                );
            try
            {
                m_ArchiveResponse = ArchiveService.GetArticle(userControlData, GetArticleRequestForContentReader(podcastArticleToken.AccessionNumber));
                //check if the object is valid
                if (m_ArchiveResponse != null)
                {
                    object articleResponseObject;
                    m_ArchiveResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out articleResponseObject);
                    GetArticleResponse articleResponse = (GetArticleResponse) articleResponseObject;

                    if (articleResponse != null &&
                        articleResponse.articleResponseSet != null &&
                        articleResponse.articleResponseSet.article != null)
                    {
                        if (articleResponse.articleResponseSet.article[0] != null &&
                            (articleResponse.articleResponseSet.article[0].status == null ||
                             articleResponse.articleResponseSet.article[0].status.value == 0))
                        {
                            if (articleResponse.articleResponseSet.article[0].contentParts != null)
                            {
                                if (articleResponse.articleResponseSet.article[0].contentParts.contentType.ToLower() != "article")
                                {
                                    return HandleError(CONTENT_READER_INVALID_CONTENTTYPE);
                                }
                            }

                            string lang = articleResponse.articleResponseSet.article[0].baseLanguage;
                            int wordCount = articleResponse.articleResponseSet.article[0].wordCount;

                            //check if the conditions are met..
                            if (!AllowContentReader(wordCount, lang))
                            {
                                return HandleError(CONTENT_READER_NOT_ALLOWED_ERRORCODE);
                            }

                            m_ArchiveResponse.GetResponse(ServiceResponse.ResponseFormat.String, out articleResponseObject);
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml((string) articleResponseObject);
                            return ApplyXSLT(doc, podcastArticleToken.IncludeMarketingMessage);
                        }
                        else if (articleResponse.articleResponseSet.article[0] != null &&
                                 (articleResponse.articleResponseSet.article[0].status != null ||
                                  articleResponse.articleResponseSet.article[0].status.value != 0))
                        {
                            return HandleError(articleResponse.articleResponseSet.article[0].status.value.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                return HandleError("-1");
            }
            return HandleError("-1");
        }

        /// <summary>
        /// Takes an accession  number and returns the Article response. Uses FRSP object type for billing purposes
        /// </summary>
        /// <param name="AccessionNumber"></param>
        /// <returns>ServiceResposne from Gateway</returns>
        private static GetArticleRequest GetArticleRequestForContentReader(string AccessionNumber)
        {
            GetArticleRequest articleRequest = new GetArticleRequest();
            articleRequest.accessionNumbers = new string[1] {AccessionNumber};
            articleRequest.canonicalSearchString = string.Empty;
            articleRequest.progressiveDisclosure = false;

            ResponseDataSet responseDataSet = new ResponseDataSet();
            responseDataSet.articleFormat = ArticleFormatType.FRSP;
            responseDataSet.fids = null;

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
            return string.Format(m_ErrorHtmlBody, ResourceTextManager.Instance.GetErrorMessage(code));
        }

        /// <summary>
        /// Allows the content reader.
        /// </summary>
        /// <param name="wordCount">The word count.</param>
        /// <param name="baseLanguage">The base language.</param>
        /// <returns></returns>
        private static bool AllowContentReader(int wordCount, string baseLanguage)
        {
            bool IsValid = false;
            if (WebUtilitiesManager.IsHttps())
            {
                return false;
            }

            //_allowCR = false; //resetting the flag.
            char[] delimiterChars = {','};
            string[] langs = m_SupportedLanguages.Split(delimiterChars);


            if (m_Log.IsInfoEnabled) m_Log.InfoFormat("In AllowContentReader : {0} ", baseLanguage);
            if (m_Log.IsInfoEnabled) m_Log.InfoFormat("In AllowContentReader : lang supported {0}", m_SupportedLanguages);

            //sometimes the baselang can be null...
            if (baseLanguage != null)
            {
                for (int iNum = 0; iNum < langs.Length; iNum++)
                {
                    if (langs[iNum].ToLower().Equals(baseLanguage.ToLower()))
                    {
                        IsValid = true;
                        break;
                    }
                }
            }
            if (IsValid)
            {
                IsValid = false; //resetting the flag.
                //if worcount is in the range of 0 to wordCountSupported, set this to tru..
                if (wordCount > 0 && wordCount <= m_MaxWordCount)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }

        private string ApplyXSLT(XmlDocument article, bool includeMarketingMessage)
        {
            string value = GetEmbeddedXslt(m_ContentReader_XSLT);

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value.Trim()))
            {
                using (XmlTextReader reader = new XmlTextReader(new StringReader(value)))
                {
                    using (StringWriter sw = new StringWriter())
                    {
                        XslCompiledTransform trans = new XslCompiledTransform();

                        try
                        {
                            trans.Load(reader);

                            // Set up the argument list
                            XsltArgumentList argumentList = new XsltArgumentList();
                            if (includeMarketingMessage)
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
            if (string.IsNullOrEmpty(m_ResourceData) || string.IsNullOrEmpty(m_ResourceData.Trim()))
            {
                lock (m_SyncObject)
                {
                    // Use double checked
                    if (string.IsNullOrEmpty(m_ResourceData) || string.IsNullOrEmpty(m_ResourceData.Trim()))
                    {
                        using (Stream stream = GetType().Assembly.GetManifestResourceStream(GetType(), resourceName))
                        {
                            if (stream != null)
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    m_ResourceData = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            return m_ResourceData;
        }
    }
}