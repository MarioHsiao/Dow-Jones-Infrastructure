using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using Factiva.BusinessLayerLogic.Exceptions;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using factiva.nextgen;
using factiva.nextgen.ui;
using factiva.nextgen.ui.formstate;
using EMG.widgets.Managers;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.encryption;
using log4net;

namespace EMG.widgets.ui.syndication.podcast
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadSpeakerContentHandler : IHttpHandler
    {
        private static readonly string CONTENT_READER_INVALID_CONTENTTYPE = "520221";
        private static readonly string CONTENT_READER_NOT_ALLOWED_ERRORCODE = "520220";
        private static readonly string m_ErrorHtmlBody = "<html><body><h2>Error</h2><div>{0}</div></body></html>";

        private static readonly ILog m_Log = LogManager.GetLogger(typeof (ReadSpeakerContentHandler));
        private static readonly int m_MaxWordCount = 5000;
        private static readonly string m_SupportedLanguages = "en,es,fr,de,it";
        private static readonly string m_ContentReader_XSLT = "ContentReader.xslt";
        private static string m_ResourceData = string.Empty;
        private static readonly object m_SyncObject = new object();
        private readonly bool isReusable = false;
        private ServiceResponse _archiveResponse;

        #region IHttpHandler Members

        ///<summary>
        ///Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            // Get the parameted and token data
            FormState formState = new FormState(string.Empty);
            ReadspeakerContentDTO contentDTO = (ReadspeakerContentDTO) formState.Accept(typeof (ReadspeakerContentDTO), false);
            ArticleTokenProperties tokenProperties = new ArticleTokenProperties(contentDTO.token);

            if (contentDTO.IsValid())
            {
                 // Set up Session
                new SessionData("b", tokenProperties.ContentLanguage.ToString(), 0, false);

                // Send out the response
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Write(GetArticle(tokenProperties));
                context.Response.End();
                return;
            }

            // this is the fallthrough event
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(HandleError("-1"));
            context.Response.End();
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
            get { return isReusable; }
        }

        #endregion

        /// <summary>
        /// Gets the article.
        /// </summary>
        /// <param name="tokenProperties">The token properties.</param>
        /// <returns></returns>
        private string GetArticle(ArticleTokenProperties tokenProperties)
        {
            ControlData userControlData = ControlDataManager.AddProxyCredentialsToControlData(ControlDataManagerEx.GetPodcastLightWeightUser(), tokenProperties.UserId, tokenProperties.NameSpace);
            ArchiveManager archiveManager = new ArchiveManager(userControlData, tokenProperties.ContentLanguage.ToString());

            try
            {
                _archiveResponse = archiveManager.GetArticleForContentReader(tokenProperties.AccessionNumber);
            }
            catch (FactivaBusinessLogicException fbe)
            {
                return HandleError(fbe.ReturnCodeFromFactivaService.ToString());
            }
            catch (Exception)
            {
                return HandleError("-1");
            }

            //check if the object is valid
            if (_archiveResponse != null)
            {
                object articleResponseObject;
                _archiveResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out articleResponseObject);
                GetArticleResponse articleResponse = (GetArticleResponse)articleResponseObject;

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

                        _archiveResponse.GetResponse(ServiceResponse.ResponseFormat.String, out articleResponseObject);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml((string)articleResponseObject);
                        return ApplyXSLT(doc);
                    }
                    else if (articleResponse.articleResponseSet.article[0] != null &&
                             (articleResponse.articleResponseSet.article[0].status != null ||
                              articleResponse.articleResponseSet.article[0].status.value != 0))
                    {
                        return HandleError(articleResponse.articleResponseSet.article[0].status.value.ToString());
                    }
                }
            }
            return HandleError("-1");
        }

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private static string HandleError(string code)
        {

            return string.Format(m_ErrorHtmlBody, ResourceText.GetInstance.GetErrorMessage(code));
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
            if (WebUtility.IsHttps())
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

        private string ApplyXSLT(XmlDocument article)
        {
            string value = GetEmbeddedXslt(m_ContentReader_XSLT);

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value.Trim()))
            {
                using (XmlTextReader reader = new XmlTextReader(new StringReader(value)))
                {
                    using (StringWriter sw = new StringWriter())
                    {
                        XslCompiledTransform trans = new XslCompiledTransform();;
                        try
                        {
                            trans.Load(reader);
                            trans.Transform(article, null, sw);
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