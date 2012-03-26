using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using DowJones.Tools.ServiceLayer.WebServices;
using DowJones.Utilities.Loggers;
using DowJones.Utilities.Managers.NewsPage;
using Factiva.Gateway.Managers;
using Factiva.Gateway.Utils.V1_0;
using log4net;

namespace DowJones.Tools.WebServices
{
    /// <summary>
    /// Family Tree Web Service
    /// </summary>
    [WebService(Namespace = "DowJones.Utilities.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class TabStripService : BaseWebService
    {
        /// <summary>
        /// The m log.
        /// </summary>
        private static readonly ILog MLog = LogManager.GetLogger(typeof (TabStripService));

        /// <summary>
        /// The _c data.
        /// </summary>
        private readonly ControlData _cData = ControlDataManager.GetLightWeightUserControlData("dacostad", "brian", "16");

        /// <summary>
        /// The process TabStrip.
        /// </summary>
        /// <param name="tabId">The tab id.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="productPrefix">The product prefix.</param>
        /// <returns>The Status</returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void DeleteTabPage(string tabId, string interfaceLanguage, string accessPointCode, string productPrefix)
        {
            using (new TransactionLogger(MLog, MethodBase.GetCurrentMethod()))
            {
                var newsPageManager = new NewsPageManager(_cData, "en");
                newsPageManager.DeleteNewsPage(tabId);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void MoveTabPage(ICollection<string> tabs, string interfaceLanguage, string accessPointCode,
                                string productPrefix)
        {
            using (new TransactionLogger(MLog, MethodBase.GetCurrentMethod()))
            {
                var newsPageManager = new NewsPageManager(_cData, "en");
                newsPageManager.UpdatePositions(tabs);
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void UpdateTabName(string tabId, string tabName, string interfaceLanguage, string accessPointCode,
                                  string productPrefix)
        {
            using (new TransactionLogger(MLog, MethodBase.GetCurrentMethod()))
            {
                var newsPageManager = new NewsPageManager(_cData, "en");
                newsPageManager.UpdateNewsPageName(tabId, tabName);
            }
        }
    }
}