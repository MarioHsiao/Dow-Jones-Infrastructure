// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyndicationNewsPageModuleDataManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Xml.Serialization;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Create;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Update;
using Factiva.Gateway.Messages.PCM.Syndication.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlDataManager = Factiva.Gateway.Managers.ControlDataManager;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    /// <summary>
    /// The syndication news page module data manager.
    /// </summary>
    [TestClass]
    public class SyndicationNewsPageModuleDataManager : AbstractUnitTest //, IPopulateTest, IUpdateTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "23234";

        /// <summary>
        /// The populate method test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void PopulateTest()
        {
            var serviceResult = new SyndicationNewsPageModuleServiceResult();

            var request = new NewsPageHeadlineModuleDataRequest
                              {
                                  PageId = PageForGet,
                                  ModuleId = ModuleForGet, 
                                  MaxResultsToReturn = 1, 
                                  FirstPartToReturn = 0,
                                  MaxPartsToReturn = 5
                              };

            serviceResult.Populate(ControlData, request, Preferences);
            
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
            //feeds in the module
            //var feedsInModule = serviceResult.GetModule(request).SyndicationFeedCollection;

            //const int Iterations = 10;
            //var sum = 0L;
            //for (var i = 0; i < Iterations; i++)
            //{
            //    serviceResult.Populate(ControlData, request, Preferences);
            //    if (serviceResult.ReturnCode != 0)
            //    {
            //        continue;
            //    }

            //    Console.WriteLine(@"-{0}", serviceResult.ElapsedTime);
            //    sum = sum + serviceResult.ElapsedTime;

            //    if (serviceResult.PartResults == null)
            //    {
            //        continue;
            //    }

            //    var partIndex = 0;
            //    foreach (var partResult in serviceResult.PartResults)
            //    {
            //        // NN - validating order
            //        Assert.AreEqual(feedsInModule[partIndex++].SyndicationFeedUri, partResult.Package.FeedUri, "Output order is incorrect on iteration " + i);
            //        Console.WriteLine(@"-----{0} -- {1} -- {2}", partResult.ElapsedTime, partResult.Package.FeedUri, partResult.Package.FeedTitle);
            //    }
            //}

            //Console.WriteLine(@"-----Avg: {0} -----", sum / Iterations);
        }

        /// <summary>
        /// The syndication news page module update service result_ update_ test.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void UpdateTest()
        {
            var serviceResult = new SyndicationNewsPageModuleUpdateServiceResult();

            var syndicationidcol = new DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update.SyndicationIdCollection()
                              {
                               "112","113"
                              };

            var request = new SyndicationNewsPageModuleUpdateRequest
                              {
                                  PageId = 12622.ToString(),
                                  ModuleId = 22223.ToString(), 
                                  Description = "temp Des",
                                  SyndicationIdCollection = syndicationidcol, 
                                  Title = "Syndication Title", 
                              };

            SerializationUtility.SerializeObjectToStream(request);
            serviceResult.Update(ControlData, request, Preferences);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
            SerializationUtility.SerializeObjectToStream(serviceResult);
        }

        [TestMethod]
        public void CreateSyndicationNewspageModule()
        {
            SyndicationNewsPageModuleCreateRequest testRequest = new SyndicationNewsPageModuleCreateRequest();
            var syndicationidcol = new DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update.SyndicationIdCollection()
                              {
                               "111","112"
                              };

            #region Commented
            
            
            //SyndicationNewspageModule tempModule = new SyndicationNewspageModule();
            //tempModule.Description = "Test Data Description";
            //tempModule.Title = "Test Data title";
            //tempModule.Position = 10;
            //tempModule.LastModifiedDate = DateTime.Now;
            ////tempModule.ModuleProperties = new ModuleProperties
            ////                                  {
            ////                                      CategoryInfo = PublishStatusScope.Account,
            ////                                      ModuleMetaData = new MetaData
            ////                                                           {
            ////                                                               MetaDataCode = "test Code",
            ////                                                               MetaDataDescriptor = "Test Meta data Descriptor",
            ////                                                               MetaDataType = MetaDataType.Topic
            ////                                                           },
            ////                                      PublishStatusScope = PublishStatusScope.Global,
            ////                                  };
            ////tempModule.ModuleQualifier = AccessQualifier.Global;
            
            ////tempModule.SyndicationFeedUriCollection.AddRange(uriColl.ToArray());
            #endregion
            testRequest.PageId = 12622.ToString();
            testRequest.Description = "Test Data Description";
            testRequest.Title = "Test Data title";
            testRequest.SyndicationIdCollection = syndicationidcol;


            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };
            SyndicationNewsPageModuleCreateServiceResult res = new SyndicationNewsPageModuleCreateServiceResult();
            res.Create(controlData, testRequest, basePreferences);
            Assert.IsTrue(res.ReturnCode == 0);
            Assert.IsTrue(res.ModuleId > 0);
        }

        [TestMethod]
        public void GetSyndicationHeadlines()
        {
            var getSyndicationHeadlinesRequest = new GetSyndicationHeadlinesRequest();
            getSyndicationHeadlinesRequest.SyndicationIdCollection.AddRange(new string[] { "5392" });
            getSyndicationHeadlinesRequest.FirstResultToReturn = 0;
            getSyndicationHeadlinesRequest.MaxResultsToReturn = 10;
            getSyndicationHeadlinesRequest.SortBy = SortColumn.PublicationDate;
            getSyndicationHeadlinesRequest.SortOrder = SortOrder.Descending;
            getSyndicationHeadlinesRequest.Bookmark = "Somestring";
            getSyndicationHeadlinesRequest.FeedFormat = FeedFormatType.FactivaHeadlines;

            ServiceResponse serviceResponse = PCMSyndicationService.GetSyndicationHeadlines(ControlData, getSyndicationHeadlinesRequest);
            Assert.IsNotNull(serviceResponse);

            if (serviceResponse.rc != 0)
            {
                Console.WriteLine("Return Code: {0}", serviceResponse.rc);
                Assert.Fail("Return Code: {0}", serviceResponse.rc);
            }
            else
            {
                object responseObj;
                long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
                var getSyndicationHeadlinesResponse = (GetSyndicationHeadlinesResponse)responseObj;
                Assert.IsNotNull(getSyndicationHeadlinesResponse);
                try
                {

                    using (FileStream stream = GetFileStream(@"Temp", "GetSyndicationHeadlinesResponse_Serialized.xml"))
                    {
                        TextWriter writer = new StreamWriter(stream);
                        XmlSerializer serializer = new XmlSerializer(typeof(GetSyndicationHeadlinesResponse));
                        serializer.Serialize(writer, getSyndicationHeadlinesResponse);
                        writer.Close();
                    }
                }
                catch (Exception)
                {
                   // m_Log.Error(ex.Message);
                }
            }
        }

    }
}
