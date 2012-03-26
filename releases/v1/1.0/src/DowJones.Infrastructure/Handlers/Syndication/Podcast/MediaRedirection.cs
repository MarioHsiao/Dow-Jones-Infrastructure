using System.Web;
using EMG.Utility.Handlers.Syndication.Podcast.Core;
using EMG.Utility.TokenEncryption;

namespace EMG.Utility.Handlers.Syndication.Podcast
{
    public class MediaRedirection: IHttpHandler
    {
        private static readonly bool m_IsReusable = false;
        private static readonly string m_CONTENT_READER_VOICE_US = "Paul";
        private static readonly string m_CONTENT_READER_VOICE_FR = "bruno22k";
        private static readonly string m_CONTENT_READER_VOICE_DE = "klaus22k";
        private static readonly string m_CONTENT_READER_VOICE_ES = string.Empty; // Setting for default voice.
        private static readonly string m_CONTENT_READER_VOICE_IT = "Silvia";

        #region IHttpHandler Members

        ///<summary>
        ///Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            string trueToken = PodcastMediaUrlBuilder.Decode(GetToken(context.Request.Url));
            PodcastArticleToken podcastArticleToken = new PodcastArticleToken(trueToken);

            // this is a quick redirection handler to ReadSpeaker
            ReadspeakerPodcastRedirectionUrlBuilder readspeakerUrl = new ReadspeakerPodcastRedirectionUrlBuilder(podcastArticleToken.AccessionNumber,
                                                                                                                 trueToken,
                                                                                                                 MapToReadSpeakerContentLanguage(podcastArticleToken.ContentLanguage),
                                                                                                                 MapToReadSpeakerVoice(podcastArticleToken.ContentLanguage)
                );
            context.Response.Redirect(readspeakerUrl.ToString());
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
                    return m_CONTENT_READER_VOICE_US;
                case "fr":
                    return m_CONTENT_READER_VOICE_FR;
                case "de":
                    return m_CONTENT_READER_VOICE_DE;
                case "it":
                    return m_CONTENT_READER_VOICE_IT;
                case "es":
                    return m_CONTENT_READER_VOICE_ES; 
                default:
                    return string.Empty;
            }
        }

        private static string MapToReadSpeakerContentLanguage(string language)
        {
            switch (language)
            {
                case "en":
                    return "us";
                default:
                    return language;
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
