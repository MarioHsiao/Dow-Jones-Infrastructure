using System;
using factiva.nextgen.ui;
using EMG.widgets.ui.dto;
using EMG.widgets.ui.dto.request;
using WidgetType=EMG.widgets.ui.dto.WidgetType;
using SubscriberGetIntegrationCode = EMG.widgets.ui.syndication.subscriber.BaseGetIntegrationCode;

namespace EMG.widgets.ui.syndication.integration
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractWidgetPortalEndPoint : IWidgetPortalEndPoint
    {
        /// <summary>
        /// 
        /// </summary>
        protected static readonly string Badge = "<a class=\"portalLnk\" href=\"{0}\" title=\"{1}\" target=\"_blank\" ><img src=\"{2}\" border=\"0\" /><span class\"portalTitle\">{1}</span></a>";
        /// <summary>
        /// Gets the type of the MIME.
        /// </summary>
        /// <value>The type of the MIME.</value>
        public abstract string MimeType { get; }
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public abstract string Title { get; }
        /// <summary>
        /// Gets the virtual image icon path.
        /// </summary>
        /// <value>The virtual image icon path.</value>
        public abstract string IconVirtualPath { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has icon image.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has icon image; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasIconImage { get; }

        /// <summary>
        /// Gets the virtual bar image path.
        /// </summary>
        /// <value>The virtual bar path.</value>
        public abstract string BarImageVirtualPath { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has bar image.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has bar image; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasBarImage { get; }

        /// <summary>
        /// Gets the integration code.
        /// </summary>
        /// <returns></returns>
        public abstract string GetIntegrationCode();


        /// <summary>
        /// Gets the integration URL.
        /// </summary>
        /// <returns></returns>
        public abstract string GetIntegrationUrl();


        /// <summary>
        /// Gets the generic integration URL.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        protected static string GetGenericIntegrationUrl(string token, WidgetType type, IntegrationTarget target)
        {
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = "~/syndication/subscriber/GetIntegrationCode.ashx";
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof (RenderWidgetDTO), "token"), token);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof (RenderWidgetDTO), "type"), (int) type);
            urlBuilder.Append(UrlBuilder.GetParameterName(typeof (RenderWidgetDTO), "integrationTarget"), (int) target);
            urlBuilder.Append("t",DateTime.Now.Ticks);
            return urlBuilder.ToString();
        }


        /// <summary>
        /// Gets the generic encrypted integration URL.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        protected static string GetGenericEncryptedIntegrationUrl(string token, WidgetType type, IntegrationTarget target)
        {
            // Create temporary RenderWidgetDTO
            RenderWidgetDTO tRenderWidgetDTO = new RenderWidgetDTO();
            tRenderWidgetDTO.token = token;
            tRenderWidgetDTO.type = type;
            tRenderWidgetDTO.integrationTarget = target;

            //Serialize the token
            EncryptedRenderWidgetDTO tEncryptedRenderWidgetDTO = new EncryptedRenderWidgetDTO(tRenderWidgetDTO);

            // build the url
            UrlBuilder urlBuilder = new UrlBuilder();
            urlBuilder.OutputType = UrlBuilder.UrlOutputType.Absolute;
            urlBuilder.BaseUrl = "~/syndication/subscriber/GetIntegrationCode.ashx";
            urlBuilder.Append(tEncryptedRenderWidgetDTO.compositeToken);
            return urlBuilder.ToString();
        }

        /// <summary>
        /// Gets the portal integration URL.
        /// </summary>
        /// <returns></returns>
        public virtual string GetBadge()
        {
            return string.Format(Badge,
                                 GetIntegrationUrl(),
                                 Title,
                                 QuerystringManager.GetLocalUrl(IconVirtualPath)
                );
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        /// <param name="escape">if set to <c>true</c> [escape].</param>
        /// <returns></returns>
        protected static string GetWidgetTitle(RenderWidgetDTO renderWidgetDTO, bool escape)
        {
            return SubscriberGetIntegrationCode.GetWidgetTitle(renderWidgetDTO, escape);
        }
    }
}