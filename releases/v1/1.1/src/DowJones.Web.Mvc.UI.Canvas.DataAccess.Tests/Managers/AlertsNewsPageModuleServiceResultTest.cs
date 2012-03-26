// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertsNewsPageModuleServiceResultTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Create;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Update;
using Factiva.Gateway.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class AlertsNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "23233";

        private const string PageForUpdate = "12819";
        private const string ModuleForUpdate = "22669";

        [TestMethod]
        public void PopulateTest()
        {
            var serviceResult = new AlertsNewsPageServiceResult();

            var request = new NewsPageHeadlineModuleDataRequest
            {
                PageId = PageForGet,
                ModuleId = ModuleForGet,
                MaxResultsToReturn = 5,
                FirstPartToReturn = 0,
                MaxPartsToReturn = 3
            };

            serviceResult.Populate(ControlData, request, Preferences);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
            SerializationUtility.SerializeObjectToStream(serviceResult);

            // Get alerts list
            var alertIdsOnTheModule = serviceResult.GetModule(request, ControlData, Preferences).AlertCollection;

            const int Iterations = 2;
            var sum = 0L;
            for (var i = 0; i < Iterations; i++)
            {
                serviceResult.Populate(ControlData, request, Preferences);
                if (serviceResult.ReturnCode != 0)
                {
                    continue;
                }

                Console.WriteLine(@"-Total Elapsed Time:{0}", serviceResult.ElapsedTime);
                sum = sum + serviceResult.ElapsedTime;

                var partIndex = 0;
                foreach (var partResult in serviceResult.PartResults)
                {
                    if (partResult.ReturnCode == 0)
                    {
                        // NN - validating order
                        Assert.AreEqual(alertIdsOnTheModule[partIndex].AlertID, partResult.Package.AlertId, "Output order is incorrect on iteration " + i);
                        Console.WriteLine(@"Part Identifier: {3} - Package Elapsed Time:{0} - Alert ID:{1} - Alert Name:{2}", partResult.ElapsedTime, partResult.Package.AlertId, partResult.Package.AlertName, partResult.Identifier);
                    }
                    else
                    {
                        Console.WriteLine(partResult.ReturnCode);
                    }
                    partIndex++;
                }
            }

            Console.WriteLine(@"-----Avg: {0} -----", sum / Iterations);
        }
        
        [TestMethod]
        public void UpdateModuleTest()
        {
            var updateResult = new AlertNewsPageModuleUpdateServiceResult();
            var updateRequest = new AlertsNewsPageModuleUpdateRequest();

           // {"title":"Test Alert","description":"","alerts":[{"alert":{"alertId":"300268242","name":"YahooAndMicrosoft","isPrivate":"false"}},{"alert":{"alertId":"300268241","name":"MyAlert","isPrivate":"true"}}],"moduleId":22816,"pageId":"13113"}

            var alert = new AlertID { AlertId = "300268242", IsPrivate = true,MakePublic = true};
            var alert1 = new AlertID { AlertId = "300268241", IsPrivate = true,MakePublic = false};
            updateRequest.AlertCollection.Add(alert);
            updateRequest.AlertCollection.Add(alert1);
            //updateRequest.AlertIdCollection.Add("300265658");
            //updateRequest.AlertIdCollection.Add("300233904");
            updateRequest.Description = "Alert Description";
            updateRequest.PageId = "13113";
            updateRequest.ModuleId = "22816";
            updateRequest.Title = "Alert Title";

           // var controlData = ControlDataManager.GetLightWeightUserControlData("snap1", "pwd", "16");
            var controlData = new ControlData { SessionID = "27139ZzZKJHEQUSCAAAGUAYAAAAAD6YYAAAAAABSGAYTCMBUGE2TCNRQHEYTSOBY" };
            updateResult.Update(controlData, updateRequest, Preferences);
            Assert.AreEqual(updateResult.ReturnCode, default(int));
        }

        [TestMethod]
        public void CreateModuleTest()
        {
            var testRequest = new AlertsNewsPageModuleCreateRequest
                                  {
                                      Description = "Test Data Description",
                                      Title = "Test Data title",
                                      PageId = "14067"
                                  };
            var alert = new AlertID { AlertId = "300268242", IsPrivate = true, MakePublic = true };
            var alert1 = new AlertID { AlertId = "300268241", IsPrivate = true, MakePublic = false };

            testRequest.AlertCollection.AddRange(new [] { alert,alert1 });

            //var controlData = new ControlData { SessionID = "27139ZzZKJHEQUSCAAAGUAYAAAAAD6YYAAAAAABSGAYTCMBUGE2TCNRQHEYTSOBY" };
            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            var res = new AlertNewsPageModuleCreateServiceResult();
            res.Create(controlData, testRequest, basePreferences);
            Assert.IsTrue(res.ReturnCode == 0);
            Assert.IsTrue(res.ModuleId > 0);
        }
    }
}
