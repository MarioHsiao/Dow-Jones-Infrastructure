using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using EMG.widgets.ui.encryption;
using EMG.widgets.ui.services.web;
using Encryption = FactivaEncryption.encryption;

namespace EMG.widgets.ui.services.ajax
{
    /// <summary>
    /// 
    /// </summary>
    public partial class WidgetDesignerManager
    {
        /// <summary>
        /// Dycrypts the widget token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public TokenItem[] DycryptWidgetToken2(string token)
        {
            Encryption encryption = new Encryption();
            NameValueCollection collection = encryption.decrypt(token, RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
            if (collection == null || collection.Count == 0)
            {
                token = HttpUtility.UrlDecode(token);
                collection = encryption.decrypt(token, RenderWidgetEncryptionConfiguration.ENCRYPTION_KEY);
            }
            List<TokenItem> list = new List<TokenItem>(collection.Keys.Count);
            for (int i = 0; i < collection.Keys.Count; i++)
            {
                list.Add(new TokenItem(collection.Keys[i], collection[collection.Keys[i]]));
            }
            return list.ToArray();
        }
    }
}
