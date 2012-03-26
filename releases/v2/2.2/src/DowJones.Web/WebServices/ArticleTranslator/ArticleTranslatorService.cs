// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleTranslatorService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved. 	
// </copyright>
// <summary>
//   Article translator web service
// </summary>
// <author>
//   Pramod Sankar
// </author>
// <lastModified>
// </lastModified>
// --------------------------------------------------------------------------------------------------------------------
#region Namespace

using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Services;
using DowJones.Ajax.Security;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Globalization;
using DowJones.Loggers;
using DowJones.Managers.AutomatedTranslation;
using DowJones.Managers.AutomatedTranslation.Core;
using DowJones.Pages;
using DowJones.Session;
using DowJones.Web.Handlers;
using log4net;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Messages.ODE.V1_0;
using System.Globalization;
using System.Web.Script.Services;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using Settings = DowJones.Properties.Settings;
using TextInfo = DowJones.Managers.AutomatedTranslation.Core.TextInfo;

#endregion
namespace DowJones.Web.WebServices.ArticleTranslator
{
    /// <summary>
    /// ArticleTranslator Web Service
    /// </summary>
    [WebService(Namespace = "DowJones.Tools.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ArticleTranslatorService : WebService
    {
        #region ...::PrivateMethods::...
        /// <summary>
        /// The m log.
        /// </summary>
        private static readonly ILog MLog = LogManager.GetLogger(typeof(ArticleTranslatorService));
        #endregion

        #region ...::PublicMethods::...
        /// <summary>
        /// The TranslateArticle method.
        /// </summary>
        /// <param name="requestDelegate">ArticleTranslatorRequestDelegate.</param>
        /// <param name="credentials">proxy credentials</param>
        /// <returns>The ArticleTranslatorResponseDelegate Object</returns>
        [WebMethod]
        //[ScriptMethod(UseHttpGet = false)]
        public ArticleTranslatorResponseDelegate TranslateArticle(ArticleTranslatorRequestDelegate requestDelegate, ProxyCredentials credentials)
        {
            // var requestDelegate = new ArticleTranslatorRequestDelegate { AccessionNumber = accessionNumber, SourceLanguage = sourceLang,TargetLanguage=targetLang };
            //new SessionData(credentials.accessPointCode, credentials.interfaceLanguage, 0, false, credentials.productPrefix, string.Empty);
            var sourceLanguage = (ContentLanguage)Enum.Parse(typeof(ContentLanguage), requestDelegate.SourceLanguage, true);
            var targetLanguage = (ContentLanguage)Enum.Parse(typeof(ContentLanguage), requestDelegate.TargetLanguage, true);
            var inputFormat = (TextFormat)Enum.Parse(typeof(TextFormat), (string.IsNullOrEmpty(requestDelegate.InputFormat) ? "HTML" : requestDelegate.InputFormat), true);
            var sourceText = requestDelegate.SourceText;
            
            using (new TransactionLogger(MLog, MethodBase.GetCurrentMethod()))
            {
                var responseDelegate = new ArticleTranslatorResponseDelegate();
                try
                {
                    if ((string.IsNullOrEmpty(requestDelegate.SourceText)) || (string.IsNullOrEmpty(requestDelegate.SourceLanguage)) || (string.IsNullOrEmpty(requestDelegate.TargetLanguage)))
                    {
                        return null;
                    }
                    //responseDelegate.TranslatedText = "this is translated text";


                    //ITranslationManager manager = new TranslationManager(SessionData.Instance().SessionBasedControlData, SessionData.Instance().InterfaceLanguage);
                    var cData = (ControlData) Session["cData"];
                    ITranslationManager manager = new TranslationManager(ControlDataManager.Convert(cData), "en");
                    
                    var sourceTextInfo = new TextInfo(sourceText, inputFormat, sourceLanguage);
                    ITranslateRequest request = new TranslateTextRequest(sourceTextInfo, targetLanguage);
                    ITranslateTask task = manager.BeginTranslateTask(request);

                    //A client application can use the task reference to block until the translation is done
                    ITranslateResult result = manager.EndTranslateTask(task);

                    var translateItem = result.TranslatedItem as ITranslateText;
                    if (translateItem != null)
                        responseDelegate.TranslatedText = translateItem.Text.Body;
                    else
                        //TODO convert this into response Token
                        throw new Exception("Translation failed.");
                }
                catch (DowJonesUtilitiesException rEx)
                {
                    UpdateAjaxDelegate(rEx, responseDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), responseDelegate);
                }

                return responseDelegate;
            }
        }

        /// <summary>
        /// The UpdateRatings method.
        /// </summary>
        /// <param name="requestDelegate">UpdateRatingsRequestDelegate.</param>
        /// <param name="credentials">proxy credentials</param>
        /// <returns>The UpdateRatingsResponseDelegate Object</returns>
        [WebMethod]
        //[ScriptMethod(UseHttpGet = false)]
        public UpdateRatingsResponseDelegate UpdateRatings(UpdateRatingsRequestDelegate requestDelegate, ProxyCredentials credentials)
        {
            var accessionNo = requestDelegate.AccessionNumber;
            var sourceLanguageCode = requestDelegate.SourceLanguage;
            var targetLanguageCode = requestDelegate.TargetLanguage;
            var wordCount = requestDelegate.WordCount;
            var sourceName = requestDelegate.SourceName;
            var rating = requestDelegate.Rating;
            var sourceLanguageName = (ContentLanguage)Enum.Parse(typeof(ContentLanguage), sourceLanguageCode, true);
            var targetLanguageName = (ContentLanguage)Enum.Parse(typeof(ContentLanguage), targetLanguageCode, true);
            var objResponse = new UpdateRatingsResponseDelegate();
            // For emailing, we just need the english name of the languages
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            
            //TODO make the email addresses configurable.
            //var feedbackEmail = System.Configuration.ConfigurationManager.AppSettings["AUTO_TRANSLATION_FEEDBACK_EMAIL"];
            var feedbackEmail = Settings.Default.AUTO_TRANSLATION_FEEDBACK_EMAIL;
            try
            {
                // send email here, using something like following
                var request = new OnDemandEmailNoAuthRequest
                                  {
                                      formatType = EmailFormat.HTML,
                                      emailDevice = EmailDevice.DESKTOP,
                                      emailSource = EmailSource.EMBEDDEDMESSAGE,
                                      emailType = EmailType.ShareEmail,
                                      subject =
                                          string.Format("ATF-{0}:{1}-{2}", sourceLanguageCode.ToLower(),
                                                        targetLanguageCode.ToLower(), rating),
                                      freeText =
                                          string.Format(
                                          "Accession Number: {0}<br/>Source language: {1}<br/>Target language: {2}<br/>Source: {3}<br/>Word count: {4}<br/>Rating: {5} out of 5<br/>",
                                          accessionNo, sourceLanguageName, targetLanguageName, sourceName, wordCount,
                                          rating),
                                      replyToEmail = string.Empty,
                                      recipientEmail = feedbackEmail
                                  };
                var cData = (ControlData)Session["cData"];
                OnDemandEmailService.OnDemandEmailNoAuth(cData, request);
                objResponse.StatusMessage = "Success";
                
            }
            catch (Exception ex)
            {
                objResponse.ExceptionMessage = ex.Message;
                objResponse.StatusMessage = "Exception";
            }
            return objResponse;
        }
        #endregion

        [Inject("Cannot use constructor injection for web services")]
        protected IPageAssetsManagerFactory PageAssetsManagerFactory { get; set; }


        public ArticleTranslatorService()
        {
            ServiceLocator.Current.Inject(this);
        }

        protected virtual void ProcessRequest(MethodBase method, IAjaxResponseDelegate responseDelegate, ProxyCredentials credentials, Action<IAjaxResponseDelegate, IPageAssetsManager> processingFunction)
        {
            using (new TransactionLogger(MLog, method))
            {
                try
                {
                    var manager = PageAssetsManagerFactory.Create();
                    processingFunction.Invoke(responseDelegate, manager);
                }
                catch (DowJonesUtilitiesException rEx)
                {
                    UpdateAjaxDelegate(rEx, responseDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), responseDelegate);
                }
            }
        }
        protected static void UpdateAjaxDelegate(DowJonesUtilitiesException emgEx, IAjaxResponseDelegate ajaxResponseDelegate)
        {
            UpdateAjaxDelegate(emgEx, ajaxResponseDelegate, null);
        }

        protected static void UpdateAjaxDelegate(DowJonesUtilitiesException emgEx, IAjaxResponseDelegate ajaxResponseDelegate, TransactionLogger transactionLogger)
        {
            ajaxResponseDelegate.ReturnCode = emgEx.ReturnCode;
            ajaxResponseDelegate.StatusMessage = ResourceTextManager.Instance.GetErrorMessage(ajaxResponseDelegate.ReturnCode.ToString());
            if (transactionLogger != null)
            {
                ajaxResponseDelegate.ElapsedTime = transactionLogger.ElapsedTimeSinceInvocation;
            }
        }
    }
}
