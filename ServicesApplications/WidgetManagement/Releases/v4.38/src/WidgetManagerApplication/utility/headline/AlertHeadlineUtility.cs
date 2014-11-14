using System;
using System.Collections.Specialized;
using EMG.widgets.ui.delegates.core.alertHeadline;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.utility.headline
{
    /// <summary>
    /// </summary>
    public static class AlertHeadlineUtility
    {
        private const string AlertMetadataPublicKey = "ANJ8SM0Y1";
        private const string ExternalAccessTokenKeyName = "EAT";

        /// <summary>
        /// </summary>
        public const string AlertMetadataQuerystringName = "atmhk";


        /// <summary>
        /// </summary>
        /// <param name="alertInfo"></param>
        /// <returns></returns>
        public static string GetAlertMetadata(AlertInfo alertInfo)
        {
            if (alertInfo == null || String.IsNullOrEmpty(alertInfo.ExternalAccessToken))
            {
                return null;
            }
            var encryption = new Encryption();
            var nvp = new NameValueCollection(1) {{ExternalAccessTokenKeyName, alertInfo.ExternalAccessToken}};
            return encryption.encrypt(nvp, AlertMetadataPublicKey);
        }

        /// <summary>
        /// </summary>
        /// <param name="alerts"></param>
        public static void FilterSensitiveData(AlertInfo[] alerts)
        {
            if (alerts == null)
            {
                return;
            }
            foreach (AlertInfo info in alerts)
            {
                info.ExternalAccessToken = null;
            }
        }
    }
}