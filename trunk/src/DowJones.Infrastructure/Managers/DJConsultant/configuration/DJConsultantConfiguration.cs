using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using DowJones.Properties;
using log4net;

namespace DowJones.Managers.DJConsultant.configuration
{
    /// <summary>
    /// Gets the user's add Content and dashobard configuration
    /// </summary>
    public class DJConsultantConfiguration
    {
        //START: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
        public const string MYPAGE_PageName = "MYPAGE";
        public const string ALERTSRESEARCH_PageName = "ALERTSRESEARCH";
        //END: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
        public const int NUMBER_OF_DEFAULT_HEADLINES = 5;
        private static readonly string CACHE_KEY_IIS = "DJCContentConfiguration";
        public const int CACHE_EXPIRATION_TIME_FOR_QUERIES = 1440;
        public const int CACHE_EXPIRATION_TIME_FOR_DJC = 44;
        public const int CACHE_REFRESH_INTERVAL_TIME_FOR_DJC = 15;

        //        public const string PLATFORM_QUERIES_CACHE_KEY = "PLATFORM_QUERIES_CACHE_KEY";

        private static readonly ILog _log = LogManager.GetLogger(typeof(DJConsultantConfiguration));

        private static readonly DJConsultantConfiguration instance = new DJConsultantConfiguration();

        private DJConsultantConfiguration()
        {
        }

        public static DJConsultantConfiguration GetInstance
        {
            get { return instance; }
        }


        public static XDocument GetConsultantConfiguration()
        {
            //try fetching the list of cats from cache..
            return (XDocument) HttpContext.Current.Cache.Get(CACHE_KEY_IIS) ?? LoadContentConfigurationXml();
        }

        /// <summary>
        /// Gets the dashboard content options.
        /// </summary>
        /// <param name="pageType">Type of the page.BUILD_FO_PageName or ADVISE_PageName etc.,use the constants of this class</param>
        /// <returns>Returns the dashboard</returns>
        public IEnumerable<XElement> GetDashboardContentOptions(string pageType)
        {
            XDocument ContentItems = GetConsultantConfiguration();
            return from dashboard in ContentItems.Elements("DJCDashboardCanvasQueries").Elements("dashboard")
                   where
                       dashboard.Attribute("id") != null &&
                       dashboard.Attribute("id").Value != null &&
                       dashboard.Attribute("id").Value.ToUpper() == pageType.ToUpper()
                   select dashboard;
        }

        /// <summary>
        /// Gets the default advise modules.
        /// </summary>
        /// <returns>IEnumerable of XElement</returns>
        public IEnumerable<XElement> GetDefaultModules(string pageType)
        {
            // if found return, else, try loading it
            //find the Dashboard...
            IEnumerable<XElement> theDashboard = GetDashboardContentOptions(pageType);

            //get the default modules...
            IEnumerable<XElement> theDefaultModules = from moduleTypes in theDashboard.Elements("moduleType")
                                                      where
                                                          moduleTypes.Attribute("isDefaultOnDashboard") != null &&
                                                          (bool)moduleTypes.Attribute("isDefaultOnDashboard")
                                                      select moduleTypes;
            return theDefaultModules;
        }

        /// <summary>
        /// Gets the default advise modules.
        /// </summary>
        /// <returns>IEnumerable of XElement</returns>
        public IEnumerable<XElement> GetAllModules(string pageType)
        {
            // if found return, else, try loading it
            //find the Dashboard...
            IEnumerable<XElement> theDashboard = GetDashboardContentOptions(pageType);

            //get the default modules...
            IEnumerable<XElement> theDefaultModules = from moduleTypes in theDashboard.Elements("moduleType")
                                                      select moduleTypes;
            return theDefaultModules;
        }

        //public IEnumerable<XElement> GetDefaultAdviseModules()
        //{
        //    //START: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
        //    return GetDefaultModules(MYPAGE_PageName);
        //    //END: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
        //}

        //START: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page
        //public IEnumerable<XElement> GetDefaultBuildFindOpportunitiesModules()
        //{
        //    return GetDefaultModules(BUILD_FO_PageName);
        //}
        //END: Infosys 09/02/2010 - Changes for combining Build and Advice Dashboards to My Page

        public XElement GetNewsOpinionAnalysisModuleDetails(string moduleId, string industryCode)
        {
            XElement theDefaultModule = null;
            try
            {
                IEnumerable<XElement> items = from modules in GetConsultantConfiguration().Root.Descendants("module")
                                              where
                                                  modules.Attribute("id").Value == moduleId &&
                                                  modules.Parent.Attribute("id").Value == industryCode
                                              select modules;
                if (items.Count() > 0)
                {
                    theDefaultModule = items.First();
                }
            }
            catch(Exception ex)
            {
                _log.Debug(ex);
            }
            return theDefaultModule;            
            
        }

        public XElement GetBusinessOpportunitiesModuleDetails(string practiceArea)
        {
            XElement businessOpbyPA = null;
            try
            {
                IEnumerable<XElement> items = from businessEventType in GetConsultantConfiguration().Root.Descendants("businessEventType")
                                              where
                                                  businessEventType.Attribute("practiceArea").Value == practiceArea
                                              select businessEventType;
                if (items.Count() > 0)
                {
                    businessOpbyPA = items.First();
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
            return businessOpbyPA;

        }
        public XElement GetAnalysisAndProfilesModuleDetails(string industryCode)
        {
            XElement aAndPByInd = null;
            try
            {
                IEnumerable<XElement> items = from industry in GetConsultantConfiguration().Root.Descendants("industry")
                                              where
                                                  industry.Attribute("consultantIndustry").Value == industryCode
                                              select industry;
                if (items.Count() > 0)
                {
                    aAndPByInd = items.First();
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
            return aAndPByInd;

        }
        public XElement GetFISQuery (string fisType)
        {
            XElement theDefaultModule = null;
            try
            {
                IEnumerable<XElement> items = from modules in GetConsultantConfiguration().Root.Descendants("fisType")
                                              where
                                                  modules.Attribute("id").Value == fisType 
                                              select modules;
                if (items.Count() > 0)
                {
                    theDefaultModule = items.First();
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
            return theDefaultModule;
        }
        /// THIS METHOD WILL GET THE LIST EITHER FROM THE XML OR BY  PERFORMING A SYMBOLOGY TXS..
        /// </summary>
        /// <returns></returns>
        private static XDocument LoadContentConfigurationXml()
        {
            XDocument objXmlList = null;
            string DJCContentConfiguration = null;
            try
            {
                DJCContentConfiguration =
                    HttpContext.Current.Server.MapPath(Settings.Default.DjConsultantConfigurationXmlRelativePath);
            }
                //looks like HttpContext  is null.by product of the thread spawns???
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
            if (!string.IsNullOrEmpty(DJCContentConfiguration))
            {
                //get the xml loaded into the DOM...
                try
                {
                    objXmlList = XDocument.Load(DJCContentConfiguration);
                    UpdateCache(objXmlList, CACHE_KEY_IIS);
                }
                catch (Exception ex)
                {
                    _log.Debug(
                        "error while updating cache with contentconfiguration " + DJCContentConfiguration, ex);
                }
            }
            return objXmlList;
        }

        private static void UpdateCache(object _aDoc, string key)
        {
            string DJCContentConfiguration =
                HttpContext.Current.Server.MapPath("~/configuration/data/ConsultantContentqueries_v1.0.xml");

            //depends on the file changes....
            var fileDependency = new CacheDependency(DJCContentConfiguration);

            _log.Debug("FileDependency on: " + DJCContentConfiguration);
            //if not a null object add this to cache..
            if (_aDoc != null)
            {
                HttpContext.Current.Cache.Add(key, _aDoc, fileDependency, Cache.NoAbsoluteExpiration,
                                              Cache.NoSlidingExpiration,
                                              CacheItemPriority.NotRemovable, UpdateListCallBack);
            }
        }

        public static void UpdateListCallBack(string s, object val, CacheItemRemovedReason r)
        {
            _log.Debug("Entering UpdateListCallBack");
            XDocument objXmlList = LoadContentConfigurationXml();

            if (objXmlList == null)
            {
                _log.Debug("Unable to configuration Cache @" + DateTime.Now.ToLongDateString());
            }
            _log.Debug("Exiting UpdateListCallBack");
        }

        public static Dictionary<string, string> GetIndustryIdCodePairs()
        {
            Dictionary<string, string> idCodePairs = new Dictionary<string, string>();
            var industries = from inds in GetConsultantConfiguration().Root.Descendants("industry")
                                          select inds;
            foreach (var industry in industries)
            {
                var id = industry.Attribute("consultantIndustry").Value;
                if (!idCodePairs.ContainsKey(id))
                    idCodePairs.Add(id, industry.Descendants().First().Attribute("codeName").Value);
            }
            return idCodePairs;
        }


        #region Briefing Book Modules for NOAA and BO Methods


        /// <summary>
        /// Gets the briefing book's Opinion And Analysis module's details.
        /// This returns an XElement enumeration that has module Elements. use codename Attribute to get the token for display
        /// And also, query under each for the searches. This should be used for the dropdown list items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XElement> GetBriefingBookNOAAModuleDetails()
        {
            //find the Dashboard...
            IEnumerable<XElement> theDashboard = GetDashboardContentOptions("BRIEFINGBOOK");

            //get the default modules...
            IEnumerable<XElement> aModuleType = from moduleTypes in theDashboard.Elements("moduleType")
                                                      where
                                                          moduleTypes.Attribute("id") != null &&
                                                          moduleTypes.Attribute("id").Value == "BB_NOAA"
                                                      select moduleTypes;

            IEnumerable<XElement> allModules = from modules in aModuleType.Elements("module")
                                               select modules;
            return allModules;
        }

        /// <summary>
        /// Gets the briefing book NOAA query.
        /// </summary>
        /// <param name="NOAAType">Type of the NOAA. One of the drop down items.</param>
        /// <returns>An XElement module; the Query can be retrieved off it.</returns>
        public XElement GetBriefingBookNOAAQuery(string NOAAType)
        {
            IEnumerable<XElement> theDashboard = GetDashboardContentOptions("BRIEFINGBOOK");
            XElement theDefaultModule = null;
            try
            {
                IEnumerable<XElement> items = from modules in theDashboard.Descendants("module")
                                              where
                                                  modules.Attribute("id").Value == NOAAType
                                              select modules;
                if (items.Count() > 0)
                {
                    theDefaultModule = items.First().Element("query");
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
            return theDefaultModule;
        }

        //public IEnumerable<XElement> GetBriefingBookBusinessOpModuleDetails()
        //{
        //    //find the Dashboard...
        //    IEnumerable<XElement> theDashboard = GetDashboardContentOptions("BRIEFINGBOOK");

        //    //get the default modules...
        //    IEnumerable<XElement> aModuleType = from moduleTypes in theDashboard.Elements("businessEventType")
        //                                        where
        //                                            moduleTypes.Attribute("id") != null &&
        //                                            moduleTypes.Attribute("id").Value == "BB_BO"
        //                                        select moduleTypes;

        //    IEnumerable<XElement> allModules = from modules in aModuleType.Elements("module")
        //                                       select modules;
        //    return allModules;
        //}

        /// <summary>
        /// Gets the briefing book business op query.
        /// </summary>
        /// <param name="practiceAreaCode">The practice area code.</param>
        /// <returns>An XElement module; the Query can be retrieved off it.</returns>
        public XElement GetBriefingBookBusinessOpQuery(string practiceAreaCode)
        {
            IEnumerable<XElement> theDashboard = GetDashboardContentOptions("BRIEFINGBOOK");
            XElement theDefaultModule = null;
            try
            {
                IEnumerable<XElement> items = from modules in theDashboard.Descendants("businessEventType")
                                              where
                                                  modules.Attribute("practiceArea").Value == practiceAreaCode
                                              select modules;
                if (items.Count() > 0)
                {
                    theDefaultModule = items.First().Element("query");
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
            return theDefaultModule;
        }

        #endregion Briefing Book Modules for NOAA and BO Methods


    }
}
