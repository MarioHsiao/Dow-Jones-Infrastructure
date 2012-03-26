// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageAssetsTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using DowJones.Web.Mvc.UI.Models.Common;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using DowJones.Web.Mvc.UI.Models.NewsPages.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    /// <summary>
    /// The page assets test.
    /// </summary>
    [TestClass]
    public class PageAssetsTest : AbstractUnitTest
    {
        /// <summary>
        /// The _preferences.
        /// </summary>
        private readonly IPreferences _preferences = new BasePreferences();

        /// <summary>
        /// The page id.
        /// </summary>
        private string pageId = 5106.ToString();

        /// <summary>
        /// The setsharepropertie s_ test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void SETSHAREPROPERTIES_TEST()
        {
            var pageM = new PageListManager(new ControlData
                                                {
                                                    AccessPointCode = "7", 
                                                    UserID = "london1", 
                                                    UserPassword = "bonsai", 
                                                    ProductID = "16"
                                                }, _preferences);
            var pageIdList = new List<int> {12616, 12619, 12614, 12612, 12623, 12621, 12617, 12618, 12638};
            foreach (int aPage in pageIdList)
            {
                pageM.UnPublishUserNewsPage(aPage.ToString());
            }
        }

        // 13053,13162
        /// <summary>
        /// The delet e_ page.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void DELETE_Page()
        {
            var pageM = new PageListManager(new ControlData
                                                {
                                                    AccessPointCode = "7", 
                                                    UserID = "block0162", 
                                                    UserPassword = "block0162", 
                                                    ProductID = "16"
                                                }, _preferences);
            var pageIdList = new List<int> {13083, 13082, 13081, 13011, }; // { 12616, 12619, 12614, 12612, 12623, 12621, 12617, 12618, 12638 };}
            foreach (int aPage in pageIdList)
            {
                pageM.DeleteUserNewsPage(aPage.ToString());
            }
        }

        [TestMethod]
        public void ReplaceModuleOnPageTest()
        {

            var controlData = new ControlData
            {
                AccessPointCode = "7",
                UserID = "apichecker",
                UserPassword = "apichecker",
                ProductID = "16"
            };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));

            NewsPageModule module = pageM.ReplaceModuleOnPage("13775", "23560", "23561");
        }

        /// <summary>
        /// The copy_ page.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void Copy_Page()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));


// List<int> pageIdList = new List<int>() { 13566 };//{ 12616, 12619, 12614, 12612, 12623, 12621, 12617, 12618, 12638 };}
            // foreach (var aPage in pageIdList)
            // {
            pageM.CopyPage(13556.ToString());


// }
        }

        /// <summary>
        /// The add modules to page_ test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void AddModulesToPage_Test()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));

            // pageM.AddModuleIdsToPage(12622,
            var moduleidcollection = new List<KeyValuePair<int, int>>();
            var moduleId = new KeyValuePair<int, int>(23559, 0);
            moduleidcollection.Add(moduleId);
            pageM.AddModuleIdsToPage(12622.ToString(), moduleidcollection);
        }

        // [TestMethod, TestCategory("Integration")] // TODO:  This needs a better name.  What is it testing?
        // public void GetPageServicePartResult_PageAssetsTest()
        // {
        // var serviceResult = new NewsPageServiceResult();

        // var request = new NewsPageRequest
        // {
        // PageId = 5106,
        // };

        // serviceResult.Populate(new ControlData() { AccessPointCode = "9", UserID = "lisaint", UserPassword = "lisaint", ProductID = "16" }, request, new BasePreferences());

        // SerializationUtility.SerializeObjectToStream(serviceResult);
        // Assert.IsTrue(serviceResult.ReturnCode == 0);
        // }


        // [TestMethod, TestCategory("Integration")] // TODO:  This needs a better name.  What is it testing?
        // public void GetPageListServicePartResult_PageAssetsTest()
        // {
        // var serviceResult = new NewsPagesListServiceResult();

        // var request = new NewsPagesListRequest
        // {
        // PageId = 5106,
        // };

        // serviceResult.Populate(new ControlData() { AccessPointCode = "9", UserID = "naresh", UserPassword = "naresh", ProductID = "16" }, request, new BasePreferences());

        // SerializationUtility.SerializeObjectToStream(serviceResult);
        // Assert.IsTrue(serviceResult.ReturnCode == 0);
        // }


        /// <summary>
        /// The get owner_ test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void GetOwner_Test()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            List<KeyValuePair<string, string>> response = pageM.GetAdminPageOwner(12639);

            if (response != null && response.Count > 0)
            {
                Assert.IsTrue(response[0].Value == "block0162");
                Assert.IsTrue(response[1].Value == "16");
            }
        }

        /// <summary>
        /// The get page withpage by i d_ page assets test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void GetPageWithpageByID_PageAssetsTest()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "apichecker", 
                                      UserPassword = "apichecker", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));

            NewsPage response = pageM.GetUserNewsPage("V1_14178_14178_010001");
            Assert.IsTrue(response.ID == "V1_14178_14178_010001");
        }


        /// <summary>
        /// The get page withpage_ page assets test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void GetPageWithpage_PageAssetsTest()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            NewsPageListWithNewsPage response = pageM.GetUserNewsPagesListWithAPage(12632.ToString());


// SerializationUtility.SerializeObjectToStream(aPage);
            Assert.IsTrue(response.RequestedNewsPage.ID == 5388.ToString());

            // var obj = DeserializeToObject("serialize.xml") as RegionalMapNewsPageServiceResult<RegionalMapNewsPageServicePartResult<RegionalMapPackage>, RegionalMapPackage>;

            // System.Diagnostics.Debug.WriteLine(obj.PartResults.First().Package.GetType().Name);
        }

        /// <summary>
        /// The get page list_ page assets test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void GetPageList_PageAssetsTest()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dacostad", 
                                      UserPassword = "brian", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            List<NewsPage> aPages = pageM.GetUserNewsPagesList();

            Assert.IsTrue(aPages.Count > 0);
        }

        /// <summary>
        /// The publish userpage.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void PublishUserpage()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            pageM.PublishUserNewsPage(13489.ToString(), new List<int>());
        }

        /// <summary>
        /// The un publish userpage.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void UnPublishUserpage()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            pageM.UnPublishUserNewsPage(12.ToString());
        }

        /// <summary>
        /// The creat user pages.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void CreatUserPages()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));

            var aPage = new NewsPage
                            {
                                AccessQualifier = AccessQualifier.User, 
                                Description = "MRM5070 test page", 
                                Position = 1, 
                                Title = "page-3-DONOT MESS WITH IT!!", 
                                ModuleCollection = new List<NewsPageModule>
                                                       {
                                                           new AlertsNewspageModule
                                                               {
                                                                   AlertIDCollection = new AlertIDCollection
                                                                                           {
                                                                                               "300237541", "300265550", "300265550", "300251278", "300237057", "300237781", "300239643", "300239898"
                                                                                           }, 
                                                                   Description = "Alert Module for Venu testing! Do not mess with it", 
                                                                   Title = "My alerts...."
                                                               }
                                                       }
                            };

            pageId = pageM.CreateCustomPage(aPage);

            Assert.IsTrue(pageId.IsNotEmpty());
        }

        /// <summary>
        /// The create admin pages.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void CreateAdminPages()
        {
            // factivanp
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));

            var aPage = new AdminNewsPage();


            for (int i = 81; i < 90; i++)
            {
                aPage = new AdminNewsPage();
                aPage.AccessQualifier = AccessQualifier.Account;
                aPage.Description = "Admin Page 1 created by Dev team for testing the pages-owner:joyful"; // TODO : SM_TODO: need the right description do not see it in the master requirements
                aPage.Position = 1;
                aPage.Title = "Admin Page " + i;
                aPage.ModuleCollection = new List<NewsPageModule>();
                aPage.ModuleCollection.Add(
                    new CompanyOverviewNewspageModule
                        {
                            Fcode = new FcodeCollection {"MCROST"}, 
                            Description = "Company overview module for Microsoft", 
                            Title = "CompanyOverview"
                        });

                pageId = pageM.CreateAdminPage(aPage);

                Assert.IsTrue(pageId.IsNotEmpty());
            }
        }


        /// <summary>
        /// The un publish admin page.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void UnPublishAdminpage()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            pageM.UnPublishAdminPage(13162.ToString());
        }


        /// <summary>
        /// The get admin pages.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void GetAdminpages()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            List<AdminNewsPage> pagelist = pageM.GetAdminNewsPagesList();

            Assert.IsNotNull(pagelist.Count > 0);
        }

        /// <summary>
        /// The get admin shared sub scribable news pages_ test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void GetAdminSharedSubScribableNewsPages_TEST()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "block0024", 
                                      UserPassword = "block0024", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            List<AdminNewsPage> pagelist = pageM.GetAdminSubscribableNewsPagesList();

            Assert.IsNotNull(pagelist.Count > 0);
        }


        /// <summary>
        /// The copy page_ test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void CopyPage_TEST()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "london1", 
                                      UserPassword = "bonsai", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            var id = pageM.CopyAsAdminPage(13002.ToString());

            Assert.IsTrue(id.IsNotEmpty());
        }


        /// <summary>
        /// The create a factiva page.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void CreateAFactivaPage()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            var aPage = new NewsPage();

            aPage = new NewsPage();
            aPage.AccessQualifier = AccessQualifier.Global;
            aPage.Description = "This is Accounting Page description"; // TODO : SM_TODO: need the right description do not see it in the master requirements
            aPage.Position = 1;
            aPage.Title = "Accounting/Consulting";
            aPage.MetaData = new MetaData {MetaDataCode = "iacc", MetaDataDescriptor = "Accounting/Consulting", MetaDataType = MetaDataType.Industry};
            aPage.CategoryInfo = new CategoryInfo
                                     {
                                         CategoryCode = "ifin", 
                                         CategoryDescriptor = "Financial"
                                     };
            aPage.ModuleCollection = new List<NewsPageModule>
                                         {
                                             new CompanyOverviewNewspageModule
                                                 {
                                                     Fcode = new FcodeCollection
                                                                 {
                                                                     "MCROST"
                                                                 }, 
                                                     Description = "Company overview module for Microsoft", 
                                                     Title = "CompanyOverview"
                                                 }
                                         };

            pageId = pageM.CreateAFactivaPage(aPage);

            Assert.IsTrue(pageId.IsNotEmpty());


            aPage = new NewsPage();
            aPage.AccessQualifier = AccessQualifier.Global;
            aPage.Description = "This is Banking Page description"; // TODO : SM_TODO: need the right description do not see it in the master requirements
            aPage.Position = 1;
            aPage.Title = "Banking/Credit";
            aPage.MetaData = new MetaData {MetaDataCode = "ibnk", MetaDataDescriptor = "Banking/Credit", MetaDataType = MetaDataType.Industry};
            aPage.CategoryInfo = new CategoryInfo {CategoryCode = "ifin", CategoryDescriptor = "Financial"};
            aPage.ModuleCollection = new List<NewsPageModule>();
            aPage.ModuleCollection.Add(
                new CompanyOverviewNewspageModule
                    {
                        Fcode = new FcodeCollection {"MCROST"}, 
                        Description = "Company overview module for Microsoft", 
                        Title = "CompanyOverview"
                    });

            pageId = pageM.CreateAFactivaPage(aPage);

            Assert.IsTrue(pageId.IsNotEmpty());

            #region Investing Securities

            aPage = new NewsPage();
            aPage.AccessQualifier = AccessQualifier.Global;
            aPage.Description = "This is Investing Securities Page description"; // TODO : SM_TODO: need the right description do not see it in the master requirements
            aPage.Position = 1;
            aPage.Title = "Investing Securities";
            aPage.MetaData = new MetaData {MetaDataCode = "iinv", MetaDataDescriptor = "Investing Securities", MetaDataType = MetaDataType.Industry};
            aPage.CategoryInfo = new CategoryInfo {CategoryCode = "ifin", CategoryDescriptor = "Financial"};
            aPage.ModuleCollection = new List<NewsPageModule>();
            aPage.ModuleCollection.Add(
                new CompanyOverviewNewspageModule
                    {
                        Fcode = new FcodeCollection {"MCROST"}, 
                        Description = "Company overview module for Microsoft", 
                        Title = "CompanyOverview"
                    });

            pageId = pageM.CreateAFactivaPage(aPage);

            Assert.IsTrue(pageId.IsNotEmpty());

            #endregion

            #region Advertising/PR/Marketing

            aPage = new NewsPage();
            aPage.AccessQualifier = AccessQualifier.Global;
            aPage.Description = "This is Advertising/PR/Marketing Page description"; // TODO : SM_TODO: need the right description do not see it in the master requirements
            aPage.Position = 1;
            aPage.Title = "Advertising/PR/Marketing";
            aPage.MetaData = new MetaData {MetaDataCode = "iadv", MetaDataDescriptor = "Advertising/PR/Marketing", MetaDataType = MetaDataType.Industry};
            aPage.CategoryInfo = new CategoryInfo {CategoryCode = "imda", CategoryDescriptor = "Financial"};
            aPage.ModuleCollection = new List<NewsPageModule>();
            aPage.ModuleCollection.Add(
                new CompanyOverviewNewspageModule
                    {
                        Fcode = new FcodeCollection {"MCROST"}, 
                        Description = "Company overview module for Microsoft", 
                        Title = "CompanyOverview", 
                    });

            pageId = pageM.CreateAFactivaPage(aPage);

            Assert.IsTrue(pageId.IsNotEmpty());

            #endregion

            #region Transportation/Shipping

            aPage = new NewsPage
                        {
                            AccessQualifier = AccessQualifier.Global, 
                            Description = "This is Transportation/Shipping Page description", 
                            Position = 1, 
                            Title = "Advertising/PR/Marketing", 
                            MetaData = new MetaData
                                           {
                                               MetaDataCode = "itsp", 
                                               MetaDataDescriptor = "Transportation/Shipping", 
                                               MetaDataType = MetaDataType.Industry
                                           }, 
                                           CategoryInfo = new CategoryInfo
                                                              {
                                                                  CategoryCode = "itra", CategoryDescriptor = "Transportation"
                                                              }, 
                                                              ModuleCollection = new List<NewsPageModule>
                                                                                                                                                                                                                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                                                                                                                                                                                                                            new CompanyOverviewNewspageModule
                                                                                                                                                                                                                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                    Fcode = new FcodeCollection {"MCROST"},
                                                                                                                                                                                                                                                                                                                                                                                                                                                                    Description = "Company overview module for Microsoft",
                                                                                                                                                                                                                                                                                                                                                                                                                                                                    Title = "CompanyOverview"
                                                                                                                                                                                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                                                                                                                                                                        }
                        };

            pageId = pageM.CreateAFactivaPage(aPage);
            Assert.IsTrue(pageId.IsNotEmpty());

            #endregion

            #region Retail/Wholesale

            aPage = new NewsPage();
            aPage.AccessQualifier = AccessQualifier.Global;
            aPage.Description = "This is Retail/Wholesale Page description"; // TODO : SM_TODO: need the right description do not see it in the master requirements
            aPage.Position = 1;
            aPage.Title = "Retail/Wholesale";
            aPage.MetaData = new MetaData {MetaDataCode = "i64", MetaDataDescriptor = "Retail/Wholesale", MetaDataType = MetaDataType.Industry};
            aPage.CategoryInfo = new CategoryInfo {CategoryCode = "iret", CategoryDescriptor = "Retail"};
            aPage.ModuleCollection = new List<NewsPageModule>();
            aPage.ModuleCollection.Add(
                new CompanyOverviewNewspageModule
                    {
                        Fcode = new FcodeCollection {"MCROST"}, 
                        Description = "Company overview module for Microsoft", 
                        Title = "CompanyOverview"
                    });

            pageId = pageM.CreateAFactivaPage(aPage);

            Assert.IsTrue(pageId.IsNotEmpty());

            #endregion

            #region Health Care

            aPage = new NewsPage();
            aPage.AccessQualifier = AccessQualifier.Global;
            aPage.Description = "This is Health Care Page description"; // TODO : SM_TODO: need the right description do not see it in the master requirements
            aPage.Position = 1;
            aPage.Title = "Retail/Wholesale";
            aPage.MetaData = new MetaData {MetaDataCode = "i951", MetaDataDescriptor = "Health Care", MetaDataType = MetaDataType.Industry};
            aPage.CategoryInfo = new CategoryInfo {CategoryCode = "ihea", CategoryDescriptor = "Health Care"};
            aPage.ModuleCollection = new List<NewsPageModule>();
            aPage.ModuleCollection.Add(
                new CompanyOverviewNewspageModule
                    {
                        Fcode = new FcodeCollection {"MCROST"}, 
                        Description = "Company overview module for Microsoft", 
                        Title = "CompanyOverview"
                    });

            pageId = pageM.CreateAFactivaPage(aPage);

            Assert.IsTrue(pageId.IsNotEmpty());
            #endregion
        }

        /*
        [TestMethod, TestCategory("Integration")]
        // TODO:  This needs a better name.  What is it testing?
        public void CreateAPage_PageAssetsTest()
        {
            var pageM = new PageListManager(new ControlData() { AccessPointCode = "9", UserID = "lisaint", UserPassword = "lisaint", ProductID = "16" }, "en");
            NewsPage aPage = new NewsPage();
            aPage.AccessQualifier = AccessQualifier.User;
            aPage.Description = "This is a test page from the unit test";
            aPage.Position = 1;
            aPage.Title = "SURYA TEST PAGE1";
            aPage.ModuleCollection = new List<NewsPageModule>();
            aPage.ModuleCollection.Add(
                new CompanyOverviewNewspageModule ()
                    {
                        Fcode ="MCROST",
                        Description = "Company overview module for Microsoft",
                        Title="CompanyOverview",
                        //ModuleState = ModuleState.Maximized,
                    });
            aPage.ModuleCollection.Add(
                new AlertsNewspageModule()
                {
                    AlertIDCollection = new AlertIDCollection() {"130120"},
                    Description = "Alert module ",
                    Title = "Alert Module",
                    //ModuleState = ModuleState.Maximized,
                });


            pageId = pageM.CreateCustomPage(aPage);

            Assert.IsTrue(pageId  > 0);
            //AddModulesToPage();
            //AddModuleIdsToPage();

            //var obj = DeserializeToObject("serialize.xml") as RegionalMapNewsPageServiceResult<RegionalMapNewsPageServicePartResult<RegionalMapPackage>, RegionalMapPackage>;

            //System.Diagnostics.Debug.WriteLine(obj.PartResults.First().Package.GetType().Name);


        }

        [TestMethod, TestCategory("Integration")]
        // TODO:  This needs a better name.  What is it testing?
        public void AddModulesToPage_PageAssetsTest()
        {
            var pageM = new PageListManager(new ControlData() { AccessPointCode = "9", UserID = "lisaint", UserPassword = "lisaint", ProductID = "16" }, "en");


           List<NewsPageModule> moduleCollection = new List<NewsPageModule>();
            moduleCollection.Add(
                new CompanyOverviewNewspageModule ()
                    {
                        Fcode ="MCROST",
                        Description = "Company overview module for Microsoft",
                        Title="CompanyOverview",
                       // ModuleState = ModuleState.Maximized,
                    });
            moduleCollection.Add(
                new CompanyOverviewNewspageModule()
                {
                    Fcode = "IBM",
                    Description = "Company overview module for IBM",
                    Title = "CompanyOverview",
                   // ModuleState = ModuleState.Maximized,
                });

            pageM.AddModulesToPage(pageId, moduleCollection);


        }

        [TestMethod, TestCategory("Integration")]
        // TODO:  This needs a better name.  What is it testing?
        public void AddModuleIdsToPage_PageAssetsTest()
        {
            var pageM = new PageListManager(new ControlData() { AccessPointCode = "9", UserID = "lisaint", UserPassword = "lisaint", ProductID = "16" }, "en");

            pageM.AddModuleIdsToPage( pageId, new int[]{10,11});


        }
*/

        /// <summary>
        /// The get modules list by type_ page assets test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void GetModulesListByType_PageAssetsTest()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dacostad", 
                                      UserPassword = "brian", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            var modulesList = pageM.GetModulesByModuleType(ModuleType.SummaryNewspageModule);

            Console.Write(modulesList.Count);

            Assert.IsNotNull(modulesList);
        }

        // [TestMethod, TestCategory("Integration")]
        //// TODO:  This needs a better name.  What is it testing?
        // public void SetPageShareProperties_PageAssetsTest()
        // {
        // var pageM = new PageListManager(new ControlData() { AccessPointCode = "9", UserID = "block0162", UserPassword = "block0162", ProductID = "16" }, "en");
        // ShareProperties _sp = new ShareProperties()
        // {
        // AccessControlScope = AccessControlScope.Everyone,
        // ListingScope = ShareScope.Everyone, AssignedScope = ShareScope.Personal

        // };
        // pageM.SetPageShareProperties(13002, _sp);


        // }


        // [TestMethod, TestCategory("Integration")]
        // TODO:  This needs a better name.  What is it testing?
        // public void GetSubscribablePageList_PageAssetsTest()
        // {
        // var pageM = new PageListManager(new ControlData() { AccessPointCode = "7", UserID = "joyful", UserPassword = "joyful", ProductID = "16" }, "en");

        // MetadataFilter f;
        // f.Filter.
        // var pagelist = pageM.GetSubscribableNewsPagesList(new List<AccessQualifier> { AccessQualifier.Account, AccessQualifier.Global, AccessQualifier });

        // Assert.IsNotNull(pagelist.Count > 0);
        // }

        /// <summary>
        /// The get industry sub scribable pages_ page assets test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void GetIndustrySubScribablePages_PageAssetsTest()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            List<NewsPage> pagelist = pageM.GetIndustrySubscribableNewsPagesList();

            Assert.IsNotNull(pagelist.Count > 0);
        }

        /// <summary>
        /// The subscribe to page_ page assets test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void SubscribeToPage_PageAssetsTest()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "mrm5070", 
                                      UserPassword = "changeme", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            pageM.SubscribeToPage(13002.ToString());
        }

        /// <summary>
        /// The get page list wit ppage_ page assets test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void GetPageListWitPpage_PageAssetsTest()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            NewsPageListWithNewsPage nnp = pageM.GetUserNewsPagesListWithAPage(0.ToString(), 1);

            Assert.IsTrue(nnp.RequestedNewsPage.ID.IsNotEmpty());

            // var obj = DeserializeToObject("serialize.xml") as RegionalMapNewsPageServiceResult<RegionalMapNewsPageServicePartResult<RegionalMapPackage>, RegionalMapPackage>;

            // System.Diagnostics.Debug.WriteLine(obj.PartResults.First().Package.GetType().Name);
        }

        // [TestMethod, TestCategory("Integration")]
        //// TODO:  This needs a better name.  What is it testing?
        // public void GetCategoryInfo_TEST()
        // {
        // var pageM = new PageListManager(new ControlData() { AccessPointCode = "9", UserID = "lisaint", UserPassword = "lisaint", ProductID = "16" }, "fr");
        // var categoryInfo= pageM.GetCategoryInfoCollection(MetaDataType.Industry, new List<string>() {"ifin","imda"});

        // Assert.IsTrue(categoryInfo.Count > 0 );

        // //var obj = DeserializeToObject("serialize.xml") as RegionalMapNewsPageServiceResult<RegionalMapNewsPageServicePartResult<RegionalMapPackage>, RegionalMapPackage>;

        // //System.Diagnostics.Debug.WriteLine(obj.PartResults.First().Package.GetType().Name);


        // }

        /// <summary>
        /// The get module meta data_ test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
// TODO:  This needs a better name.  What is it testing?
        public void GetModuleMetaData_TEST()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };
            BasePreferences preferences = PreferencesUtilites.GetBasePreferences(controlData);
            var pageM = new PageListManager(controlData, preferences);

            // var obj = DeserializeToObject("serialize.xml") as RegionalMapNewsPageServiceResult<RegionalMapNewsPageServicePartResult<RegionalMapPackage>, RegionalMapPackage>;

            // System.Diagnostics.Debug.WriteLine(obj.PartResults.First().Package.GetType().Name);
        }

        /// <summary>
        /// The remove assigned page.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void RemoveAssignedPage()
        {
            var controlData = new ControlData
                                  {
                                      AccessPointCode = "7", 
                                      UserID = "dhangehoi", 
                                      UserPassword = "passwd", 
                                      ProductID = "16"
                                  };

            var pageM = new PageListManager(controlData, PreferencesUtilites.GetBasePreferences(controlData));
            bool removed = pageM.RemoveAsssigedPage(13002.ToString());
            Assert.IsTrue(removed);
        }
    }
}
