using Factiva.BusinessLayerLogic.Configuration;
using Factiva.BusinessLayerLogic.Managers.V2_0;
using Factiva.Gateway.Utils.V1_0;

namespace EMG.widgets.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlDataManagerEx : ControlDataManager
    {
        private const string DefaultClientCode = "D";
        private const string RssLightweightUserAccessPointCode = "s";
        private const string PodcastLightweightUserAccessPointCode = "AA";

        /// <summary>
        /// Gets the RSS UER lightweight user.
        /// </summary>
        /// <returns></returns>
        public static ControlData GetRssFeed1LightWeightUser()
        {
            LightWeightUser lightWeightUser = ConfigurationManager.GetLightWeightUser("RssFeed1LightWeightUser");
            return GetLightWeightUserControlData(lightWeightUser.userId, 
                lightWeightUser.userPassword, 
                lightWeightUser.productId, 
                string.IsNullOrEmpty(lightWeightUser.clientCodeType) ? DefaultClientCode : lightWeightUser.clientCodeType,
                string.IsNullOrEmpty(lightWeightUser.accessPointCode) ? RssLightweightUserAccessPointCode : lightWeightUser.accessPointCode,
                lightWeightUser.contentServerAddress,
                lightWeightUser.ipAddress);
        }

        /// <summary>
        /// Gets the Podcast UER lightweight user.
        /// </summary>
        /// <returns></returns>
        public static ControlData GetPodcastLightWeightUser()
        {
            LightWeightUser lightWeightUser = ConfigurationManager.GetLightWeightUser("PodcastLightWeightUser");
            return GetLightWeightUserControlData(lightWeightUser.userId, 
                lightWeightUser.userPassword, 
                lightWeightUser.productId,
                string.IsNullOrEmpty(lightWeightUser.clientCodeType) ? DefaultClientCode : lightWeightUser.clientCodeType,
                string.IsNullOrEmpty(lightWeightUser.accessPointCode) ? PodcastLightweightUserAccessPointCode : lightWeightUser.accessPointCode,
                lightWeightUser.contentServerAddress, 
                lightWeightUser.ipAddress);
        }
    }
}