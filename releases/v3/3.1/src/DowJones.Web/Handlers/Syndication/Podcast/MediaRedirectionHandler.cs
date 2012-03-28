using System;
using System.Web;
using System.Web.UI;
using DowJones.Properties;
using DowJones.Web.Handlers.Syndication.ReadSpeaker.Core;
using log4net;

namespace DowJones.Web.Handlers.Syndication.Podcast
{
    /// <summary>
    /// 
    /// </summary>
    public class MediaRedirectionHandler : Page, IHttpAsyncHandler
    {
        protected override void OnInit(EventArgs e)
        {
            // Request Hosting permissions
            new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal).Demand();
            IHttpHandler handler;
            try
            {
                // Create the handler by calling class abc or class xyz.
                handler = (IHttpHandler)Activator.CreateInstance(typeof(BaseMediaRedirectionHandler), true);
            }
            catch (Exception ex)
            {
                throw new HttpException("Unable to create handler", ex);
            }
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

    public class BaseMediaRedirectionHandler : BaseHttpHandler
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MediaRedirectionHandler));
        private const string CONTENT_READER_VOICE_DE = "klaus22k";
        private const string CONTENT_READER_VOICE_ES = "";
        private const string CONTENT_READER_VOICE_FR = "bruno22k";
        private const string CONTENT_READER_VOICE_IT = "Silvia";
        private const string CONTENT_READER_VOICE_US = "Paul";
        public const string TOKEN_NAME_VALUE_PAIR = AudioMediaUrlBuilder.TOKEN_NAME_VALUE_PAIR;
        private const string CONTENT_MIME_TYPE = "text/html";
        private const bool REQUIRES_AUTHENTICATION = false;

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
            string trueToken = string.Empty;
            try
            {
                string tempToken = (context.Request[TOKEN_NAME_VALUE_PAIR]) ??  GetToken(context.Request.Url);
                trueToken = AudioMediaUrlBuilder.Decode(tempToken);
            }
            catch(System.Exception ex)
            {
                _log.Error(ex);
            }

            if (string.IsNullOrEmpty(trueToken) || string.IsNullOrEmpty(trueToken.Trim()))
                return;
            
            PodcastArticleToken podcastArticleToken = new PodcastArticleToken(trueToken);

            // this is a quick redirection handler to ReadSpeaker
            MediaRedirectionUrlBuilder redirectionUrlBuilder = new MediaRedirectionUrlBuilder(
                podcastArticleToken.MediaRedirectionType,
                podcastArticleToken.AccessionNumber,
                trueToken,
                MapToReadSpeakerContentLanguage(podcastArticleToken.ContentLanguage),
                MapToReadSpeakerVoice(podcastArticleToken.ContentLanguage),
                podcastArticleToken.AccessPointCode
                );
            context.Response.Redirect(redirectionUrlBuilder.ToString());
        }

        private static string MapToReadSpeakerVoice(string language)
        {
            /*<add key="Content_Reader_Voice_us" value="Paul"/><!-- us is en..they know it as us-->
	        <add key="Content_Reader_Voice_fr" value="bruno22k"/>
	        <add key="Content_Reader_Voice_de" value="klaus22k"/>
            <add key="Content_Reader_Voice_es" value=""/><!-- go for default-->
            <add key="Content_Reader_Voice_it" value="Silvia"/><!-- go for default-->*/

            switch (language.ToLower())
            {
                case "en":
                    return CONTENT_READER_VOICE_US;
                case "fr":
                    return CONTENT_READER_VOICE_FR;
                case "de":
                    return CONTENT_READER_VOICE_DE;
                case "it":
                    return CONTENT_READER_VOICE_IT;
                case "es":
                    return CONTENT_READER_VOICE_ES;
                default:
                    return string.Empty;
            }
        }

        private static string MapToReadSpeakerContentLanguage(string language)
        {
            switch (language.ToLower())
            {
                case "en":
                    return Settings.Default.UseReadSpeakerEnterprise ? "en_us" : "us";
                default:
                    return Settings.Default.UseReadSpeakerEnterprise ? string.Concat(language.ToLower(),"_",language.ToLower()) : language.ToLower();
            }
        }

        private static string GetToken(System.Uri url)
        {
            string requestedFile = url.Segments[url.Segments.Length - 1];
            string temp = requestedFile.Substring(0, requestedFile.IndexOf("."));

            //Guarantee that there are no spaces in the token and turn them into 'plus'.
            return temp.Replace(' ', '+');
        }
    }
}