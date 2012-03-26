// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPagesTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page;
using Factiva.Gateway.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using AccessQualifier = DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page.AccessQualifier;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class NewsPagesTest : AbstractUnitTest
    {
        [TestMethod]
        public void GetSubscribableNewsPagesListTest()
        {
            var serviceResult = new SubscribableNewsPagesListServiceResult();

            var request = new SubscribableNewsPagesListRequest
            {
                AccessQualifiers = new List<AccessQualifier> { AccessQualifier.Factiva, AccessQualifier.Account, AccessQualifier.User },
                FilterType = FilterType.Industry,
            };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            serviceResult.Populate(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void GetUserNewsPagesList()
        {
            var serviceResult = new NewsPagesListServiceResult();

            var request = new NewsPagesListRequest
            {
            };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            serviceResult.Populate(controlData, request, basePreferences);

            foreach (var page in serviceResult.Package.NewsPages)
            {
                Console.WriteLine(page.ID);
                GetUserNewsPagebyIdTest(page.ID);
            }
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void GetUserNewsPagebyIdTest()
        {
            //ControlData = ControlDataManager.GetLightWeightUserControlData("snapshot5", "passwd", "16");
            var serviceResult = GetPageById("13216");
            //var taskA = new []
            //                {
            //                    Task<NewsPageServiceResult>.Factory.StartNew(() => GetPageById("12774")),
            //                    Task<NewsPageServiceResult>.Factory.StartNew(() => GetPageById("12774")),
            //                    Task<NewsPageServiceResult>.Factory.StartNew(() => GetPageById("12774")),
            //                    //Task<NewsPageServiceResult>.Factory.StartNew(() => { GetPageById("12774"); }),
            //                    //Task<NewsPageServiceResult>.Factory.StartNew(() => { GetPageById("12774"); }),
            //                    //Task<NewsPageServiceResult>.Factory.StartNew(() => { GetPageById("12774"); }),
            //                    //Task<NewsPageServiceResult>.Factory.StartNew(() => { GetPageById("12774"); }),
            //                };

            //Task.WaitAll(taskA);
            //foreach (var task in taskA)
            //{
            //    Console.WriteLine(SerializationUtility.SerializeObjectToStream(task.Result));
            //}

            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        private NewsPageServiceResult GetPageById(string pageId)
        {
            var serviceResult = new NewsPageServiceResult();

            var request = new NewsPageRequest
            {
                PageId = pageId
            };
            serviceResult.Populate(ControlData, request, Preferences);
            return serviceResult;
        }

        private void GetUserNewsPagebyIdTest(string pageId)
        {
            var serviceResult = new NewsPageServiceResult();

            var request = new NewsPageRequest
                              {
                                  PageId = pageId,
                              };

            serviceResult.Populate(ControlData, request, Preferences);
            //serviceResult.Populate(ControlData, request, new BasePreferences("fr"));
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void SubscribeToPage()
        {
            var serviceResult = new SubscribePageServiceResult();

            var request = new PageSubscriptionRequest
                              {
                                  PageId = 12702.ToString(),
                                  Position = 4,
                              };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            serviceResult.Subscribe(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void CopyPageTest()
        {
            var serviceResult = new CopyPageServiceResult();

            var request = new CopyPageRequest
            {
                PageId = 12622.ToString(),
                Position = 1,
            };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            serviceResult.CopyPage(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void AddModuleIdsToPageTest()
        {
            var serviceResult = new AddModuleIdToPageServiceResult();

            var request = new AddModuleToPageRequest
            {
                PageId = 12622.ToString(),
            };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            serviceResult.AddModuleToPage(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void DeletePage()
        {
            var serviceResult = new DeletePageServiceResult();

            var request = new DeletePageRequest
            {
                PageId = 13258.ToString(),
                PageAccessScope = AccessScope.UnSpecified
            };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            serviceResult.Delete(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }
        [TestMethod]
        public void AddPageById()
        {
            var serviceResult = new AddPageByIdServiceResult();

            var request = new AddPageByIdRequest
                              {
                                  PageId = 12673.ToString(),
                                  Position = 2, 
                              };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            serviceResult.AddPageById(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }
    }
}
