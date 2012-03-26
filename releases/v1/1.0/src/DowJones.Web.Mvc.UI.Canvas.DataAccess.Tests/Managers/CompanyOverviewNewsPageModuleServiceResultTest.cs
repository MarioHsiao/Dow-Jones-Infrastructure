// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewNewsPageModuleServiceResultTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.CompanyOverview;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Create;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Update;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    /// <summary>
    /// The company overview news page module service result test.
    /// </summary>
    [TestClass]
    public class CompanyOverviewNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "14120";
        private const string ModuleForGet = "23576";

        private const string PageForUpdate = "13233";
        private const string ModuleForUpdate = "23150";

        [TestMethod]
        public void PopulateTest()
        {
            var serviceResult = new CompanyOverviewNewsPageModuleServiceResult();
            var request = new CompanyOverviewNewsPageModuleDataRequest
                              {
                                  PageId = PageForGet,
                                  ModuleId = ModuleForGet,
                                  FirstResultToReturn = 0,
                                  MaxResultsToReturn = 5,
                                  UseCustomDateRange = false,
                                  StartDate = "01032011",
                                  EndDate = "01162011",
                                  Parts = new List<CompanyOverviewParts>(new[]
                                                                             {
                                                                                 CompanyOverviewParts.SnapShot,
                                                                                 /*
                                                                                 CompanyOverviewParts.Chart,
                                                                                 CompanyOverviewParts.Trending,
                                                                                 CompanyOverviewParts.RecentArticles*/
                                                                             }
                                      )
                              };

            serviceResult.Populate(ControlData, request, Preferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);

            // Assert.IsTrue(serviceResult.ReturnCode == 0);

            const int Iterations = 1;
            var sum = 0L;
            for (var i = 0; i < Iterations; i++)
            {
                serviceResult.Populate(ControlData, request, Preferences);
                SerializationUtility.SerializeObjectToStream(serviceResult, "XML");

                foreach (var transaction in serviceResult.Audit.TransactionCollection)
                {
                    Console.WriteLine(@"{0}::{1}::{2}", transaction.ElapsedTime, transaction.Name, transaction.Detail);
                }

                Console.WriteLine(@"-{0}", serviceResult.ElapsedTime);
                sum = sum + serviceResult.ElapsedTime;

                foreach (var partResult in serviceResult.PartResults)
                {
                    var snanpshotPackage = partResult.Package as CompanySnapshotPackage;
                    var chartPackage = partResult.Package as CompanyChartPackage;
                    var trendingPackage = partResult.Package as CompanyTrendingPackage;
                    var headlinesPackage = partResult.Package as CompanyRecentArticlesPackage;

                    Console.WriteLine(@"-Service Part RC: {0} -- {1}", partResult.ReturnCode, partResult.ElapsedTime);

                    if (snanpshotPackage != null)
                    {
                        Console.WriteLine(@"{0}", snanpshotPackage.GetType().Name);
                    }
                    else if (chartPackage != null)
                    {
                        Console.WriteLine(@"{0}", chartPackage.GetType().Name);
                    }
                    else if (trendingPackage != null)
                    {
                        Console.WriteLine(@"{0}", trendingPackage.GetType().Name);
                    }
                    else if (headlinesPackage != null)
                    {
                        Console.WriteLine(@"{0}", headlinesPackage.GetType().Name);
                        Console.WriteLine(@"--- Hit Count: {0}", headlinesPackage.Result.ResultSet.DuplicateCount.Text.Value);
                    }
                }
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            var updateResult = new CompanyOverviewNewsPageModuleUpdateServiceResult();
            var updateRequest = new CompanyOverviewNewsPageModuleUpdateRequest();

            updateRequest.FCodeCollection.Add("MCROST");
            updateRequest.Description = "Company overview Description";

            updateRequest.ModuleId = ModuleForUpdate;
            updateRequest.PageId = PageForUpdate; 

            updateRequest.Title = "Company overview Title";

            updateResult.Update(ControlData, updateRequest, Preferences);
            Assert.AreEqual(updateResult.ReturnCode, default(int));
        }

        [TestMethod]
        public void CreateCompanyOverviewNewsPageModule()
        {
            var testRequest = new CompanyOverviewNewsPageModuleCreateRequest {PageId = PageForUpdate};
            //var tempModule = new CompanyOverviewNewspageModule
            //                     {
            //                         Description = "Test Data Description", 
            //                         Title = "Test Data title", Position = 10, 
            //                         LastModifiedDate = DateTime.Now, 
            //                         Fcode = new FcodeCollection { "MCROST" }
            //                     };

            // tempModule.ModuleProperties = new ModuleProperties
            //                                  {
            //                                      CategoryInfo = PublishStatusScope.Account,
            //                                      ModuleMetaData = new MetaData
            //                                                           {
            //                                                               MetaDataCode = "test Code",
            //                                                               MetaDataDescriptor = "Test Meta data Descriptor",
            //                                                               MetaDataType = MetaDataType.Topic
            //                                                           },
            //                                      PublishStatusScope = PublishStatusScope.Global,
            //                                  };
            // tempModule.ModuleQualifier = AccessQualifier.Global;

            var res = new CompanyOverviewNewsPageModuleCreateServiceResult();
            res.Create(ControlData, testRequest, Preferences);
            Assert.IsTrue(res.ReturnCode == 0);
            Assert.IsTrue(res.ModuleId > 0);

            // testRequest.
        }
    }
}
