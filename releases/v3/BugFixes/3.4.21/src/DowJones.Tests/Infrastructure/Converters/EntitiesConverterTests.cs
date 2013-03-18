using System.Collections.Generic;
using System.IO;
using DowJones.Models.Common;
using DowJones.Assemblers.Entities;
using DowJones.DependencyInjection;
using DowJones.DependencyInjection.Ninject;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Managers.QueryUtility;
using Factiva.Gateway.Managers;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Services.V2_0;
using Factiva.Gateway.Utils.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Newtonsoft.Json;

namespace DowJones.Infrastructure.Converters
{
    [TestClass]
    public class EntitiesConverterTests
    {
        private EntitiesConversionManager _converter;
        private DateTimeFormatter _dateTimeFormatter;

        [TestInitialize]
        public void TestInitialize()
        {
            var registry = AssemblyRegistry.CreateFromAssemblyReferences(Assembly.GetExecutingAssembly(), null);
            ServiceLocator.Initialize(new NinjectServiceLocatorFactory(registry));
            _dateTimeFormatter = new DateTimeFormatter("en");
            _converter = new EntitiesConversionManager(_dateTimeFormatter);
        }

        //[TestMethod]
        //public void ShouldConvertGatewayNavigatorSetMockTest()
        //{
        //    NavigatorSet nvs = new NavigatorSet();
        //    NavigatorCollection nvCollection = new NavigatorCollection();
        //    Navigator nv = new Navigator();
        //    BucketCollection bCollection = new BucketCollection();

        //    Bucket b1 = new Bucket();
        //    b1.HitCount = 123;
        //    b1.Id = "msft";
        //    b1.Type = "co";
        //    b1.Value = "Microsoft";
        //    bCollection.Add(b1);

        //    Bucket b2 = new Bucket();
        //    b2.HitCount = 321;
        //    b2.Id = "aapl";
        //    b2.Type = "co";
        //    b2.Value = "Apple";
        //    bCollection.Add(b2);

        //    nv.BucketCollection = bCollection;
        //    nv.Count = 2;
        //    nv.Id = "co";
        //    nvCollection.Add(nv);

        //    nvs.NavigatorCollection = nvCollection;
        //    nvs.Count = 5;

        //    Entities entities = _converter.Convert(nvs);
        //    Assert.IsNotNull(entities.CompanyNewsEntities);
        //    //Assert.IsNotNull(entities.IndustryNewsEntities);
        //}


        //[TestMethod]
        public void ShouldConvertGatewayNavigatorSetTest()
        {
            PerformContentSearchResponse performContentSearchResponse = PerformContentSearch();
            List<string> expandedChartList = new List<string>{"co", "in", "ns", "pd", "pw", "pm", "py"};
            performContentSearchResponse.ContentSearchResult.CodeNavigatorSet.NavigatorCollection[0].BucketCollection = null;
            Entities entities = _converter.Convert(performContentSearchResponse.ContentSearchResult.CodeNavigatorSet, expandedChartList);
            //Entities entities = _converter.Convert(performContentSearchResponse.ContentSearchResult.CodeNavigatorSet);
            Assert.IsNotNull(entities.CompanyNewsEntities);
            //Assert.IsNotNull(entities.IndustryNewsEntities);
            StringWriter stringWriter = new StringWriter();
            new JsonSerializer().Serialize(stringWriter, entities);
            //Console.WriteLine(stringWriter.ToString());
            
            foreach (Navigator nav in performContentSearchResponse.ContentSearchResult.TimeNavigatorSet.NavigatorCollection) {
                NavigatorSet nSet = new NavigatorSet();
                nSet.NavigatorCollection = new NavigatorCollection();
                nSet.NavigatorCollection.Add(nav);
                Entities timeEntities = _converter.Convert(nSet, expandedChartList);
                Assert.IsNotNull(timeEntities.DateNewsEntities);
                entities.DateNewsEntities = timeEntities.DateNewsEntities;
                stringWriter = new StringWriter();
                new JsonSerializer().Serialize(stringWriter, entities);
                //Console.WriteLine(stringWriter.ToString());
            }

            //Entities timeEntities = _converter.Convert(performContentSearchResponse.ContentSearchResult.TimeNavigatorSet, expandedChartList);
            //Assert.IsNotNull(timeEntities.DateNewsEntities);
            //entities.DateNewsEntities = timeEntities.DateNewsEntities;
            //new JsonSerializer().Serialize(stringWriter, entities);
            //Console.WriteLine(stringWriter.ToString());
            
        }

        public PerformContentSearchResponse PerformContentSearch()
        {
            PerformContentSearchRequest request = GetPerformContentSearchRequest();


            //ControlData controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            ControlData controlData = ControlDataManager.GetLightWeightUserControlData("joshimoi", "passwd", "16");

            Console.WriteLine(GeneralUtils.serialize(request));

            ServiceResponse serviceResponse = GetServiceResponse(controlData, request);

            object responseObj;
            CheckServiceResponse(serviceResponse, out responseObj);

            PerformContentSearchResponse response = (PerformContentSearchResponse)responseObj;


            Console.WriteLine("PerformContentSearchResponse");
            Console.WriteLine(GeneralUtils.serialize(response));
            //Console.WriteLine(GeneralUtils.serialize(response.ContentSearchResult.HitCount.ToString()));
            return response;
        }

        public static ServiceResponse GetServiceResponse(ControlData controlData, PerformContentSearchRequest performContentSearchRequest)
        {
            return SearchService.PerformContentSearch(ControlDataManager.Clone(controlData), performContentSearchRequest);
        }

        private PerformContentSearchRequest GetPerformContentSearchRequest()
        {
            PerformContentSearchRequest request = new PerformContentSearchRequest();
            request.StructuredSearch = new StructuredSearch { Formatting = new ResultFormatting { MarkupType = Factiva.Gateway.Messages.Search.V2_0.MarkupType.All, SnippetType = SnippetType.Fixed, SortOrder = ResultSortOrder.Relevance, FreshnessDate = DateTime.Today, ClusterMode = ClusterMode.Off, DeduplicationMode = DeduplicationMode.Off }, Linguistics = new Linguistics { LemmatizationOn = true, SpellCheckMode = LinguisticsMode.Suggest, SynonymsOn = true, SymbolRecognitionMode = LinguisticsMode.Suggest, NameRecognitionMode = LinguisticsMode.Suggest } };
            request.StructuredSearch.Query = new StructuredQuery { Dates = new Dates { Format = Factiva.Gateway.Messages.Search.V2_0.DateFormat.MMDDCCYY, After = "-91" } };

            //request.StructuredSearch.Query.RankSearchStringCollection = new RankSearchStringCollection { new RankSearchString { Mode = SearchMode.Traditional, Data = "rst=djnwp or rst=bwr" } };

            request.StructuredSearch.Query.SearchStringCollection = new SearchStringCollection();
            request.StructuredSearch.Query.SearchStringCollection.Add(new SearchString { Id = "freetext", Mode = SearchMode.Traditional, Type = SearchType.Free, Value = "sandy" });
            //request.StructuredSearch.Query.SearchStringCollection.Add(new SearchString { Id = "freetext1", Mode = SearchMode.Simple, Type = SearchType.Free, Value = "google" });
            //request.StructuredSearch.Query.SearchStringCollection.Add(new SearchString { Id = "laAny", Mode = SearchMode.Any, Type = SearchType.Controlled, Scope = "la", Filter = true, Data = "en ru de" });


            request.DescriptorControl = new DescriptorControl { Mode = DescriptorControlMode.All, Language = "en" };

            request.NavigationControl = new NavigationControl { ReturnCollectionCounts = true, CollectionCountControl = new CollectionCountControl() };
            request.NavigationControl.CollectionCountControl.SearchCollection = new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites, SearchCollection.Multimedia, SearchCollection.Pictures, SearchCollection.Blogs };
            request.NavigationControl.KeywordControl = new KeywordControl { ReturnKeywords = true, MaxKeywords = 20 };
            request.NavigationControl.CodeNavigatorControl = new CodeNavigatorControl { Mode = CodeNavigatorMode.All, MaxBuckets = 5, MinBucketValue = 0 };

            request.NavigationControl.TimeNavigatorMode = TimeNavigatorMode.All;
            request.FirstResult = 0;
            request.MaxResults = 52;
            return request;
        }

        private void CheckServiceResponse(ServiceResponse serviceResponse, out object responseObject)
        {
            if (serviceResponse.rc != 0)
            {
                throw new Exception(serviceResponse.RawResponse.ToString());
            }
            else
            {
                long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObject);
                if (responseObjRC != 0)
                {
                    throw new Exception(responseObjRC.ToString());
                }
            }
        }
    }
}
