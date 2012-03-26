// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parallel.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlDataManager = Factiva.Gateway.Managers.ControlDataManager;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class Parallel
    {

        [TestMethod]
        public void TestParallelCompanyOverviewNewsPageModuleServiceResult()
        {
            var serviceResult = new CompanyOverviewNewsPageModuleServiceResult();
            int Index = 0;

            var request = new CompanyOverviewNewsPageModuleDataRequest
            {
                PageId = 12622.ToString(),
                ModuleId = 22226.ToString(),
                FirstResultToReturn = 0,
                MaxResultsToReturn = 5,
                Parts = new List<CompanyOverviewParts>(new[] { CompanyOverviewParts.SnapShot, CompanyOverviewParts.RecentArticles, CompanyOverviewParts.Trending, CompanyOverviewParts.Chart, })
            };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };

            serviceResult.Populate(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult, "JSON");
            SerializationUtility.SerializeObjectToStream(serviceResult, "XML");

            Assert.IsTrue(serviceResult.ReturnCode == 0);

            var tasks = new List<Task<CompanyOverviewNewsPageModuleServiceResult>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            const int Iterations = 3;
            for (var i = 0; i < Iterations; i++)
            {
                var tIndex = Interlocked.Increment(ref Index);
                tasks.Add(TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(() => GetCompanyOverviewNewsPageModuleServiceResult(tIndex.ToString(), controlData, request, new BasePreferences("en"))));
            }

            Task.WaitAll(tasks.ToArray(), 20 * 1000);
            stopwatch.Stop();
            Console.WriteLine(@"overall:{0}", stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void TestParallelSyndicationNewsPageModuleServiceResult()
        {
            int Index = 0;
            var serviceResult = new SyndicationNewsPageModuleServiceResult();

            var request = new NewsPageHeadlineModuleDataRequest
            {
                PageId = 12622.ToString(), 
                ModuleId = 22223.ToString(), 
                MaxResultsToReturn = 5, 
                FirstPartToReturn = 0, 
                MaxPartsToReturn = 6
            };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            serviceResult.Populate(controlData, request, new BasePreferences());
            Assert.IsTrue(serviceResult.ReturnCode == 0);
            SerializationUtility.SerializeObjectToStream(serviceResult);

            Thread.Sleep(500);

            Assert.IsTrue(serviceResult.ReturnCode == 0);

            var tasks = new List<Task<SyndicationNewsPageModuleServiceResult>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            const int Iterations = 10;
            for (var i = 0; i < Iterations; i++)
            {
                var tIndex = Interlocked.Increment(ref Index);
                tasks.Add(TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(
                    () => GetSyndicationNewsPageModuleServiceResult(
                        tIndex.ToString(), 
                        controlData, 
                        request, 
                        new BasePreferences("en")), 
                    TaskCreationOptions.None));
            }

            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            Console.WriteLine(@"overall:{0} ({1} Millisecond)", stopwatch.ElapsedTicks, stopwatch.ElapsedMilliseconds);

        }

        [TestMethod]
        public void TestParallelAlertNewsPageModuleServiceResult()
        {
            var serviceResult = new AlertsNewsPageServiceResult();
            int Index = 0;

            var request = new NewsPageHeadlineModuleDataRequest
            {
                PageId = 12622.ToString(),
                ModuleId = 22224.ToString(),
                MaxResultsToReturn = 5,
                FirstPartToReturn = 0,
                MaxPartsToReturn = 6
            };

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };

            serviceResult.Populate(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult, "JSON");
            SerializationUtility.SerializeObjectToStream(serviceResult, "XML");

            Assert.IsTrue(serviceResult.ReturnCode == 0);

            var tasks = new List<Task<AlertsNewsPageServiceResult>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            const int Iterations = 10;
            for (var i = 0; i < Iterations; i++)
            {
                var tIndex = Interlocked.Increment(ref Index);
                tasks.Add(TaskFactoryManager.Instance.GetDefaultTaskFactory().StartNew(() => GetAlertsNewsPageServiceResult(tIndex.ToString(), controlData, request, null)));
            }

            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            Console.WriteLine(@"overall:{0}", stopwatch.ElapsedMilliseconds);
        }

        private static AlertsNewsPageServiceResult GetAlertsNewsPageServiceResult(string index, Factiva.Gateway.Utils.V1_0.ControlData controlData, NewsPageHeadlineModuleDataRequest request, IPreferences preferences)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var serviceResult = new AlertsNewsPageServiceResult();
            serviceResult.Populate(controlData, request, preferences);
            foreach (var transation in serviceResult.Audit.TransactionCollection)
            {
                Console.WriteLine(@"{0}:{1}-{2}", transation.Name, transation.ReturnCode, transation.ElapsedTime);
            }

            stopwatch.Stop();
            Console.WriteLine(@"Index:{0}-->{1} ({2})", index, stopwatch.ElapsedTicks, stopwatch.ElapsedMilliseconds);
            return serviceResult;
        }

        private static SyndicationNewsPageModuleServiceResult  GetSyndicationNewsPageModuleServiceResult(string index, Factiva.Gateway.Utils.V1_0.ControlData controlData, NewsPageHeadlineModuleDataRequest request, IPreferences preferences)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var serviceResult = new SyndicationNewsPageModuleServiceResult();
            serviceResult.Populate(controlData, request, preferences);
            stopwatch.Stop();

            var a = System.DateTime.Now.Ticks;
            Console.WriteLine(@"-----Index:{0} Return code:{1}, Elapsed time:{2} ({3}) ", index, serviceResult.ReturnCode, stopwatch.ElapsedTicks, stopwatch.ElapsedMilliseconds);
            foreach (var transaction in serviceResult.Audit.TransactionCollection)
            {
                Console.WriteLine(
                        @"{5}-->{0}:{1}-{2}-{3}-Creation:{4}", 
                        transaction.Name, 
                        transaction.ReturnCode, 
                        transaction.ElapsedTime, 
                        transaction.Detail, 
                        transaction.CreationDateTime.ToString("MM/dd/yyyy HH:mm:ss::ffff"),
                        string.Concat(transaction.ApartmentState, "-", transaction.ManagedThreadId));
            }

            Console.WriteLine("Manual Ticks" + (System.DateTime.Now.Ticks - a));
            Console.WriteLine(@"-----End Index-----");
            return serviceResult;
        }
        
        private static CompanyOverviewNewsPageModuleServiceResult GetCompanyOverviewNewsPageModuleServiceResult(string index, Factiva.Gateway.Utils.V1_0.ControlData controlData, CompanyOverviewNewsPageModuleDataRequest request, IPreferences preferences)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var serviceResult = new CompanyOverviewNewsPageModuleServiceResult();
            serviceResult.Populate(controlData, request, preferences);
            foreach (var transation in serviceResult.Audit.TransactionCollection)
            {
                Console.WriteLine(@"{0}:{1}-{2}", transation.Name, transation.ReturnCode, transation.ElapsedTime);
            }

            stopwatch.Stop();
            Console.WriteLine(@"Index:{0}-->{1}", index, stopwatch.ElapsedMilliseconds);
            return serviceResult;
        }
    }
}
