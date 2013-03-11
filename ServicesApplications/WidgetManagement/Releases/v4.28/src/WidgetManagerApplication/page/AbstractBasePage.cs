// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractBasePage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AbstractBasePage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Web.UI;
using factiva.nextgen;
using factiva.nextgen.ui.utility;
using NextGenAbstractBasePage = factiva.nextgen.ui.page.AbstractBasePage;

namespace EMG.widgets.ui.page
{
    /// <summary>
    /// </summary>
    public abstract class AbstractBasePage : NextGenAbstractBasePage
    {
        /// <summary>
        /// Redirects to site based on site user.
        /// </summary>
        /// <param name="siteUserType">Type of the site user.</param>
        public override void RedirectToSiteBasedOnSiteUser(SiteUserType siteUserType)
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);
            var host = NormalizeString(Request.Url.Host);
            string targetHost;

            switch (siteUserType)
            {
                case SiteUserType.IWorksPlus:
                    targetHost = new FactivaPackagesUtility().GetIWorksPlusHostUrl;
                    break;
                case SiteUserType.IWorksBasic:
                    targetHost = new FactivaPackagesUtility().GetIWorksBasicHostUrl;
                    break;
                case SiteUserType.FactivaReader:
                    targetHost = new FactivaPackagesUtility().GetFactivaReaderHostUrl;
                    break;
                case SiteUserType.UnregisterdUser:
                    targetHost = new FactivaPackagesUtility().GetIWorksBasicHostUrl;
                    break;
                case SiteUserType.FactivaExternalReader:
                    targetHost = new FactivaPackagesUtility().GetExternalReaderHostUrl;
                    break;
                default:
                    targetHost = new FactivaPackagesUtility().GetFactivaWidgetBuilderUserHostUrl;

                    break;
            }

            if (targetHost != null && host == targetHost)
            {
                return;
            }

            var action = string.Format("http://{0}{1}", NormalizeString(targetHost), Request.ServerVariables["SCRIPT_NAME"]);
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine(
                "<script type=\"text/javascript\">function fireOnLoad(){document.forms['login'].submit()}</script>");
            writer.WriteLine("</head>");
            writer.WriteLine(
                "<body onload=\"fireOnLoad()\"><form id=\"login\" name=\"login\" action=\"{0}\" method=\"post\">",
                action);

            foreach (string key in Request.QueryString.Keys)
            {
                if (key.ToLower() != "targetsite" &&
                    key.ToLower() != "landingpage" &&
                    key.ToLower() != "interfacelanguage")
                {
                    writer.WriteLine(
                        "<input type=\"hidden\" name=\"{0}\" value=\"{1}\"/>", 
                        key,
                        NormalizeString(Request.QueryString[key]));
                }
            }

            foreach (string key in Request.Form.Keys)
            {
                if (Request.QueryString[key] == null &&
                    key.ToLower() != "targetsite" &&
                    key.ToLower() != "landingpage" &&
                    key.ToLower() != "interfacelanguage")
                {
                    writer.WriteLine(
                        "<input type=\"hidden\" name=\"{0}\" value=\"{1}\"/>",
                        key,
                        NormalizeString(Request.Form[key]));
                }
            }

            writer.WriteLine("</form>");
            writer.WriteLine("</body></html>");

            Response.Write(sw.ToString());
            Response.End();
            return;
        }
    }
}

