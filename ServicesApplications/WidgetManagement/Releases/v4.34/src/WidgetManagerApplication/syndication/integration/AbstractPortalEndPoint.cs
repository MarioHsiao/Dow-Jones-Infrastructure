using System;
using Factiva.BusinessLayerLogic.Configuration;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.Gateway.Messages.Assets.WebWidgets.V1_0;
using Factiva.Gateway.Utils.V1_0;
using factiva.nextgen.ui;
using factiva.widgets.ui.dto;
using factiva.widgets.ui.dto.request;
using factiva.widgets.ui.encryption;
using WidgetType=factiva.widgets.ui.dto.WidgetType;

namespace factiva.widgets.ui.syndication.integration
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractPortalEndPoint : IPortalEndPoint
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
        /// Generates the integration URL.
        /// </summary>
        /// <returns></returns>
        public abstract string GenerateIntegrationUrl();

        /// <summary>
        /// Generates the page flakes add flake URL.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        protected static string GenericGadgetLinkBuilder(string token, WidgetType type, IntegrationTarget target)
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
        /// Generics the gadget encrypted link builder.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        protected static string GenericGadgetEncryptedLinkBuilder(string token, WidgetType type, IntegrationTarget target)
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
                                 GenerateIntegrationUrl(),
                                 Title,
                                 QuerystringManager.GetLocalUrl(IconVirtualPath)
                );
        }

        /// <summary>
        /// Gets the premium content light weight user.
        /// </summary>
        /// <returns></returns>
        protected static ControlData GetWidgetProxyLightWeightUser()
        {
            LightWeightUser lightWeightUser = ConfigurationManager.GetLightWeightUser("RssFeed1LightWeightUser");
            return ControlDataManager.GetLightWeightUserControlData(lightWeightUser.userId, lightWeightUser.userPassword, lightWeightUser.productId);
        }


        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="renderWidgetDTO">The render widget DTO.</param>
        /// <param name="escape">if set to <c>true</c> [escape].</param>
        /// <returns></returns>
        protected static string GetTitle(RenderWidgetDTO renderWidgetDTO, bool escape)
        {
            WidgetTokenProperties tokenProperties = new WidgetTokenProperties(renderWidgetDTO.token);
            ControlData userControlData = ControlDataManager.AddProxyCredentialsToControlData(GetWidgetProxyLightWeightUser(), tokenProperties.UserId, tokenProperties.NameSpace);
            
            WidgetManager m_WidgetManager = new WidgetManager(userControlData, renderWidgetDTO.language);
            string widgetName = ResourceText.GetInstance.GetString("factivaWidgetName");
            try
            {
                Widget widget = m_WidgetManager.GetWidgetById(tokenProperties.WidgetId);
                if (widget != null)
                {
                    widgetName = widget.properties.name;
                }
            }
            catch { }
            if (escape)
            {
                return escapeTitle(widgetName);
            }
            return widgetName;
        }

        private static string escapeTitle(string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str.Trim()))
                return string.Empty;
            return str.Replace("&", "&amp;").Replace("\"", "&quot;");
        }
    }
}