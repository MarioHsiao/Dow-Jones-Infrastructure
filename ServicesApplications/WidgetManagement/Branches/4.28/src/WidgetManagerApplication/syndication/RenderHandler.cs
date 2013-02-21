using System.Web;
using EMG.widgets.ui.delegates.interfaces;
using EMG.widgets.ui.delegates.output.syndication;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.utility;
using factiva.nextgen;
using factiva.nextgen.ui.formstate;
using EMGUtility = EMG.widgets.ui.utility.Utility;

namespace EMG.widgets.ui.syndication
{
    /// <summary>
    /// 
    /// </summary>
    public class RenderHandler : IHttpHandler
    {
        #region IHttpHandler Members

        ///<summary>
        ///Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var formState = new FormState(string.Empty);
            var renderWidgetDTO = (RenderWidgetDTO)formState.Accept(typeof(RenderWidgetDTO), false);
            new SessionData("b", renderWidgetDTO.language, 0, false);

            var response = HttpContext.Current.Response;
            response.Buffer = true;
            var wDelegate = GetDelegate(renderWidgetDTO);
            if (wDelegate == null) return;
            switch (renderWidgetDTO.responseFormat)
            {
                case ResponseFormat.JSON:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.JSON);
                    response.Write(wDelegate.ToJSON());
                    response.End();
                    break;
                case ResponseFormat.JSONP:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.JSON);
                    if (string.IsNullOrEmpty(renderWidgetDTO.callBackParam) || string.IsNullOrEmpty(renderWidgetDTO.callBackParam.Trim()))
                    {
                        response.Write(wDelegate.ToJSONP(renderWidgetDTO.callBackFunction));
                    }
                    else
                    {
                        response.Write(wDelegate.ToJSONP(renderWidgetDTO.callBackFunction, renderWidgetDTO.callBackParam));
                    }
                    response.End();
                    break;
                case ResponseFormat.XML:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.XML);
                    response.Write(wDelegate.ToXML());
                    response.End();
                    break;
                case ResponseFormat.ATOM:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.ATOM);
                    response.Write(wDelegate.ToATOM());
                    response.End();
                    break;
                case ResponseFormat.RSS:
                    response.ContentType = EMGUtility.GetMimeType(MimeType.RSS);
                    response.Write(wDelegate.ToRSS());
                    response.End();
                    break;
                case ResponseFormat.PODCAST_RSS:
                    var podcastDelegate = wDelegate as IWidgetPodcastDelegate;
                    if (podcastDelegate != null)
                    {
                        response.ContentType = EMGUtility.GetMimeType(MimeType.XML);
                        response.Write(podcastDelegate.ToPodcastRSS());
                        response.End();
                    }
                    break;
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
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        #endregion

        /// <summary>
        /// Gets the delegate.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        /// <returns></returns>
        public IWidgetSyndicationDelegate GetDelegate(RenderWidgetDTO renderWidgetDTO)
        {
            // Instantiate the correct delegate
            switch (renderWidgetDTO.type)
            {
                case WidgetType.AlertHeadlineWidget:
                    var alertHeadlineWidgetDelegate = new AlertHeadlineWidgetDelegate();
                    alertHeadlineWidgetDelegate.Fill(renderWidgetDTO.token, renderWidgetDTO.integrationTarget);

                    return alertHeadlineWidgetDelegate;

                case WidgetType.AutomaticWorkspaceWidget:
                    var automaticWorkspaceWidgetDelegate = new AutomaticWorkspaceWidgetDelegate();
                    automaticWorkspaceWidgetDelegate.Fill(renderWidgetDTO.token, renderWidgetDTO.integrationTarget);
                    return automaticWorkspaceWidgetDelegate;

                case WidgetType.ManualNewsletterWorkspaceWidget:
                    var manualNewsletterWorkspaceWidgetDelegate = new ManualNewsletterWorkspaceWidgetDelegate();
                    manualNewsletterWorkspaceWidgetDelegate.Fill(renderWidgetDTO.token, renderWidgetDTO.integrationTarget);
                    return manualNewsletterWorkspaceWidgetDelegate;
            }
            return null;
        }
    }
}