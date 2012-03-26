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
using DowJones.Properties;
using DowJones.Tools.ServiceLayer.WebServices;
using DowJones.Tools.Session;
using DowJones.Utilities.Ajax.ArticleTranslator;
using DowJones.Utilities.Ajax.Security;
using DowJones.Utilities.Core;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using DowJones.Utilities.Managers.AutomatedTranslation;
using DowJones.Utilities.Managers.AutomatedTranslation.Core;
using log4net;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Messages.ODE.V1_0;
using System.Globalization;
using TextInfo = DowJones.Utilities.Managers.AutomatedTranslation.Core.TextInfo;
using Factiva.Gateway.Utils.V1_0;
using DowJones.Utilities.Managers;
using System.Web.Script.Services;

#endregion
namespace DowJones.Tools.WebServices
{
    /// <summary>
    /// ArticleTranslator Web Service
    /// </summary>
    [WebService(Namespace = "DowJones.Tools.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ArticleTranslatorService : BaseWebService
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
                    ITranslationManager manager = new TranslationManager(cData, "en");
                    
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
    }
}
