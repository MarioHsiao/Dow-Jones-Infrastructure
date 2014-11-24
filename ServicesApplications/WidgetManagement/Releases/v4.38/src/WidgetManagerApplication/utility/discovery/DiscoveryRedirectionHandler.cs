using System;
using System.Collections.Specialized;
using System.Web;
using EMG.Utility.Exceptions;
using EMG.Utility.Handlers;
using EMG.widgets.ui.Properties;
using log4net;
using UrlBuilder=factiva.nextgen.ui.UrlBuilder;

namespace EMG.widgets.utility.discovery
{
    /// <summary>
    /// 
    /// </summary>
    public class DiscoveryRedirectionHandler : BaseHttpHandler
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(DiscoveryRedirectionHandler));

        public override void HandleRequest(HttpContext context)
        {
            try
            {
                var keyvalue = new NameValueCollection(context.Request.QueryString);
                var ub = new UrlBuilder
                             {
                                 OutputType = Utility.Uri.UrlBuilder.UrlOutputType.Absolute, 
                                 BaseUrl = Settings.Default.Cyclone_Redirection_URL
                             };

                if (!keyvalue.HasKeys()) return;
                for (var i = 0; i < keyvalue.Count; i++)
                {
                    switch (keyvalue.GetKey(i).ToLower())
                    {
                        case "erc":
                        case "eid4":
                            var temp = keyvalue.Get(i);
                            ub.Append(keyvalue.GetKey(i), temp.Replace(" ", "+"));
                            break;
                        default:
                            ub.Append(keyvalue.GetKey(i), keyvalue.Get(i));
                            break;
                    }
                }
                context.Response.Redirect(ub.ToString(null));
            }
            catch (EmgUtilitiesException exUtil)
            {
                m_Log.Error(exUtil);
            }
            catch (Exception ex)
            {
                m_Log.Error(ex);
            }
        }

        /// <summary>
        /// Validates the parameters.  Inheriting classes must
        /// implement this and return true if the parameters are
        /// valid, otherwise false.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns>
        /// 	<c>true</c> if the parameters are valid,
        /// otherwise <c>false</c>
        /// </returns>
        public override bool ValidateParameters(HttpContext context)
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this handler
        /// requires users to be authenticated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if authentication is required
        /// otherwise, <c>false</c>.
        /// </value>
        public override bool RequiresAuthentication
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the content MIME type.
        /// </summary>
        /// <value></value>
        public override string ContentMimeType
        {
            get { return "text/html"; }
        }
    }
}