using DowJones.Managers.QueryUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using System;
using Factiva.Gateway.Managers;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Services.V2_0;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Infrastructure.Web.Search
{
    [TestClass]
    public class QueryUtilityTest : UnitTestFixture
    {
        #region PerformContentSearch
        //        [TestMethod]
        public void PerformContentSearchRequestXML_Test()
        {

            PerformContentSearchRequest request = GetPerformContentSearchRequest();


            Console.WriteLine("Request:");

            ControlData controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");



            Console.WriteLine(GeneralUtils.serialize(request));

            ServiceResponse serviceResponse = GetServiceResponse(controlData, request);

            object responseObj;
            CheckServiceResponse(serviceResponse, out responseObj);

            PerformContentSearchResponse response = (PerformContentSearchResponse)responseObj;
            Console.WriteLine("PerformContentSearchResponse");
            Console.WriteLine(GeneralUtils.serialize(response));
            //Console.WriteLine(GeneralUtils.serialize(response.ContentSearchResult.HitCount.ToString()));

        }

        private PerformContentSearchRequest GetPerformContentSearchRequest()
        {
            PerformContentSearchRequest request = new PerformContentSearchRequest();
            request.StructuredSearch = new StructuredSearch { Formatting = new ResultFormatting { MarkupType = Factiva.Gateway.Messages.Search.V2_0.MarkupType.All, SnippetType = SnippetType.Fixed, SortOrder = ResultSortOrder.Relevance, FreshnessDate = DateTime.Today, ClusterMode = ClusterMode.Off, DeduplicationMode = DeduplicationMode.Off }, Linguistics = new Linguistics { LemmatizationOn = true, SpellCheckMode = LinguisticsMode.Suggest, SynonymsOn = true, SymbolRecognitionMode = LinguisticsMode.Suggest, NameRecognitionMode = LinguisticsMode.Suggest } };
            request.StructuredSearch.Query = new StructuredQuery { Dates = new Dates { Format = Factiva.Gateway.Messages.Search.V2_0.DateFormat.MMDDCCYY, After = "-91" } };

            //request.StructuredSearch.Query.RankSearchStringCollection = new RankSearchStringCollection { new RankSearchString { Mode = SearchMode.Traditional, Data = "rst=djnwp or rst=bwr" } };

            request.StructuredSearch.Query.SearchStringCollection = new SearchStringCollection();
            request.StructuredSearch.Query.SearchStringCollection.Add(new SearchString { Id = "freetext", Mode = SearchMode.Traditional, Type = SearchType.Free, Value = "msft" });
            request.StructuredSearch.Query.SearchStringCollection.Add(new SearchString { Id = "freetext1", Mode = SearchMode.Simple, Type = SearchType.Free, Value = "google" });
            //request.StructuredSearch.Query.SearchStringCollection.Add(new SearchString { Id = "laAny", Mode = SearchMode.Any, Type = SearchType.Controlled, Scope = "la", Filter = true, Data = "en ru de" });


            request.DescriptorControl = new DescriptorControl { Mode = DescriptorControlMode.All, Language = "en" };

            request.NavigationControl = new NavigationControl { ReturnCollectionCounts = true, CollectionCountControl = new CollectionCountControl() };
            request.NavigationControl.CollectionCountControl.SearchCollection = new SearchCollectionCollection { SearchCollection.Publications, SearchCollection.WebSites, SearchCollection.Multimedia, SearchCollection.Pictures, SearchCollection.Blogs };
            request.NavigationControl.KeywordControl = new KeywordControl { ReturnKeywords = true, MaxKeywords = 20 };
            request.NavigationControl.CodeNavigatorControl = new CodeNavigatorControl { Mode = CodeNavigatorMode.All, MaxBuckets = 5, MinBucketValue = 0 };

            request.NavigationControl.TimeNavigatorMode = TimeNavigatorMode.PublicationDate;
            request.FirstResult = 0;
            request.MaxResults = 20;
            return request;
        }

        public static ServiceResponse GetServiceResponse(ControlData controlData, PerformContentSearchRequest performContentSearchRequest)
        {
            return SearchService.PerformContentSearch(ControlDataManager.Clone(controlData), performContentSearchRequest);
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

        #endregion

        [TestMethod]
        public void QueryManagerRequestXML_Test()
        {

            QueryManagerRequest request = GetRequest();
            QueryManager queryManager = new QueryManager();

            Console.WriteLine("Request:");

            ControlData controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");

            //ProximityRule rule = new ProximityRule { FilterTypes = new List<FilterType> { FilterType.ExecutiveScreeningFilter, FilterType.DJPersonCodeFilter }, RuleOperator = RuleOperator.Within, Data = 2 };
            //request.QueryRequests[0].Rules.Clear();
            //request.QueryRequests[0].Rules.Add(rule);
            Console.WriteLine(GeneralUtils.serialize(request));
            QueryManagerResponse response = queryManager.GetQueryManagerResponse(request, controlData, false, false);
            Console.WriteLine("Response");
            Console.WriteLine(GeneralUtils.serialize(response));

        }

        [TestMethod]
        public void QueryManagerRequest_Test()
        {
            QueryManager queryManager = new QueryManager();
            QueryManagerRequest request = new QueryManagerRequest();

            QueryRequest qreq1 = new QueryRequest { QueryId = 31348, CustomId = "1" };
            QueryRequest qreq2 = new QueryRequest { Query = GetQuery(), CustomId = "2" };
            QueryRequest qreq3 = new QueryRequest { QueryId = 31064, CustomId = "3" };


            //qreq3.Rules = qreq1.Rules;
            DaysFilter daysfilter = new DaysFilter { SinceDays = -91, Target = DateFilterTarget.PublicationDate };
            //qreq2.Query.Groups[0].FilterGroup.Filters.Add(daysfilter);

            SearchExclusionFilter searchExclusionFilter = new SearchExclusionFilter { AbstractsCaptionsAndHeadline = true, Transcripts = true, Crime = true };
            //qreq2.Query.Groups[0].FilterGroup.Filters.Add(searchExclusionFilter);

            SourceEntityFilter srcEntityFilter = new SourceEntityFilter { Operator = Factiva.Gateway.Messages.Assets.Queries.V1_0.Operator.Or, SourceEntitiesCollection = new SourceEntitiesCollection() };
            srcEntityFilter.SourceEntitiesCollection.Add(new SourceEntities { SourceEntityCollection = new SourceEntityCollection { new SourceEntity { Type = SourceEntityType.RST, Value = "africa" }, new SourceEntity { Type = SourceEntityType.PDF, Value = "sg_nmedia" } } });
            srcEntityFilter.SourceEntitiesCollection.Add(new SourceEntities { SourceEntityCollection = new SourceEntityCollection { new SourceEntity { Type = SourceEntityType.RST, Value = "africa" }, new SourceEntity { Type = SourceEntityType.PDF, Value = "SG_NMEDIA" } } });
            // qreq2.Query.Groups[0].FilterGroup.Filters.Add(srcEntityFilter);

            SourceEntityListIDFilter srcEntityListIdFilter = new SourceEntityListIDFilter { IdCollectionCollection = new IdCollectionCollection { "31111", "31123" }, Operator = Operator.Or };
            //qreq2.Query.Groups[0].FilterGroup.Filters.Add(srcEntityListIdFilter);

            //searchExclusionFilter = new SearchExclusionFilter {  EntertainmentAndLifestyle = true, RepublishedNews = true };
            //qreq2.Query.Groups[0].FilterGroup.Filters.Add(searchExclusionFilter);

            SearchAdditionalFilters filter = new SearchAdditionalFilters { Operator = Operator.And };
            filter.SourceFilters = new SearchAdditionalSourceFilterItems { SourceCollection = new SearchAdditionalSourceFilterItemCollection() };
            filter.SourceFilters.SourceCollection.Add(new SearchAdditionalSourceFilterItem { Code = "djtes", SearchFormatCategory = SearchFormatCategory.Publications });
            filter.CompanyFilters = new SearchAdditionalFilterItems { CodeCollection = new Factiva.Gateway.Messages.Screening.V1_1.codeCollection { "usg" } };

            //filter.SourceFilters.SourceCollection.Add(new SearchAdditionalSourceFilterItem { Code = "djdn", SearchFormatCategory = SearchFormatCategory.Publications });

            filter.IndustryFilters = new SearchAdditionalFilterItems();
            filter.IndustryFilters.CodeCollection = new Factiva.Gateway.Messages.Screening.V1_1.codeCollection { "ifinal", "i814" };
            filter.AuthorFilters = new SearchAdditionalFilterItems();
            filter.AuthorFilters.CodeCollection = new Factiva.Gateway.Messages.Screening.V1_1.codeCollection { "111222", "113332" };

            //filter.RegionFilters = new SearchAdditionalFilterItems { CodeCollection = new Factiva.Gateway.Messages.Screening.V1_1.codeCollection {"usa", "usdc" } };
            filter.SubjectFilters = new SearchAdditionalFilterItems { CodeCollection = new Factiva.Gateway.Messages.Screening.V1_1.codeCollection { "ntesi" } };
            //filter.Keywords = new SearchAdditionalKeywordsFilterItems { CodeCollection = new Factiva.Gateway.Messages.Assets.Queries.V1_0.KeywordCollection { "dow jones" } };
            //filter.ExecutiveFilters = new SearchAdditionalFilterItems { CodeCollection = new Factiva.Gateway.Messages.Screening.V1_1.codeCollection {"47729011", "41662693" } };
            //filter.DateFilter = new SearchAdditionalDateFilterItem { StartDate = new DateTime(2011, 7, 25), EndDate = new DateTime(2011, 7, 31) };
            qreq2.Query.Groups[0].FilterGroup.Filters.Add(filter);


            request.QueryRequests.Add(qreq1);
            //request.QueryRequests.Add(qreq2);
            //request.QueryRequests.Add(qreq3);

            //AuthorListFilter authors = new AuthorListFilter { Operator = Operator.Or, AuthorLists = new List<long> { 1234,4567 } };
            //Console.WriteLine("AuthorList");
            //Console.WriteLine(GeneralUtils.serialize(authors));
            //request.QueryRequests[1].Query.Groups[0].FilterGroup.Filters.Add(authors);

            Console.WriteLine("Request:");
            Console.WriteLine(GeneralUtils.serialize(request));
            ControlData controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");


            //request.CombineState = new CombineState { Enabled = false, CombineOperator = CombineOperator.Or, DaysFilter = new DaysFilter { SinceDays = -91, Operator = Operator.Or, Target = DateFilterTarget.PublicationDate } };
            QueryManagerResponse response = queryManager.GetQueryManagerResponse(request, controlData, false, false);

            Console.WriteLine("Response");
            Console.WriteLine(GeneralUtils.serialize(response));
            //Assert.IsFalse(response.StructuredSearch == null);
        }

        private Query GetQuery()
        {
            string queryXml = @"                                
<MediaMonitorQuery xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://{name7}/{name8}/{name9}"">
  <id>0</id>
  <version>1</version>
  <editIn>BRI</editIn>
  <contains>
    <alerts>false</alerts>
    <triggerTypes>false</triggerTypes>
    <freeText>false</freeText>
    <references>false</references>
  </contains>
  <group>
    <filterGroup xsi:type=""AndFilterGroup"">
<filter xsi:type=""FreeTextFilter"">
        <operator>Or</operator>
        <text>obama</text>
      </filter>
    </filterGroup>
    <operator>And</operator>
  </group>
  
  <properties>
    <description xsi:nil=""true"" />
    <creationDate>2011-06-02T13:15:59.6783868-04:00</creationDate>
    <lastModifiedDate>2011-06-02T13:15:59.6783868-04:00</lastModifiedDate>
    <lastAccessedDate>2011-06-02T13:15:59.6783868-04:00</lastAccessedDate>
    <setupScreen>Simple</setupScreen>
  </properties>
</MediaMonitorQuery>
                                ";

            MediaMonitorQuery query = (MediaMonitorQuery)GeneralUtils.deSerialize(queryXml, typeof(MediaMonitorQuery));

            return query;
        }

        private QueryManagerRequest GetRequest()
        {
            string xmlRequest = @"
<QueryManagerRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><queryRequests><queryRequest><queryId>0</queryId><query xsi:type=""MediaMonitorQuery""><id>0</id><version>1</version><editIn>BRI</editIn><contains><alerts>false</alerts><triggerTypes>false</triggerTypes><freeText>false</freeText><references>false</references></contains><group><filterGroup xsi:type=""AndFilterGroup""><filter xsi:type=""SourcesFilter""><operator>And</operator><isAllSources>false</isAllSources><allSourceCategories><isAllPublications>false</isAllPublications><isAllPictures>false</isAllPictures><isAllWebNews>false</isAllWebNews><isAllMultimedia>false</isAllMultimedia><isAllBlogs>false</isAllBlogs><isAllBoards>false</isAllBoards></allSourceCategories><sourceGroups><searchFormatCategory>P</searchFormatCategory><sourceCode>TDJW</sourceCode><sourceCode>D</sourceCode><sourceCode>J</sourceCode><sourceListId>0</sourceListId><isGroupList>false</isGroupList></sourceGroups></filter></filterGroup><operator>And</operator></group><segment>Unspecified</segment><product>Unspecified</product><properties><description xsi:nil=""true"" /><creationDate>2011-06-13T11:08:18.298529-04:00</creationDate><lastModifiedDate>2011-06-13T11:08:18.298529-04:00</lastModifiedDate><lastAccessedDate>2011-06-13T11:08:18.298529-04:00</lastAccessedDate><setupScreen>Simple</setupScreen></properties></query><additionalFilters /><rules /></queryRequest></queryRequests><combineState><enabled>false</enabled><operator>And</operator></combineState></QueryManagerRequest>
";
            xmlRequest = @"<QueryManagerRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><queryRequests><queryRequest><queryId>0</queryId><query xsi:type=""MediaMonitorQuery""><id>0</id><version>1</version><editIn>BRI</editIn><contains><alerts>false</alerts><triggerTypes>false</triggerTypes><freeText>false</freeText><references>false</references></contains><group><filterGroup xsi:type=""AndFilterGroup""><filter xsi:type=""FreeTextFilter""><operator>And</operator><text>asdf</text></filter></filterGroup><operator>And</operator></group><segment>Unspecified</segment><product>Unspecified</product><properties><description xsi:nil=""true"" /><creationDate>2011-09-30T12:36:32.9349408-04:00</creationDate><lastModifiedDate>2011-09-30T12:36:32.9349408-04:00</lastModifiedDate><lastAccessedDate>2011-09-30T12:36:32.9349408-04:00</lastAccessedDate><sortBy>m</sortBy><snippet>n</snippet><dateFormat>mdy</dateFormat><searchSetupScreen>AdvancedSearch</searchSetupScreen><currentTab>All</currentTab></properties></query><additionalFilters /><rules /></queryRequest></queryRequests><combineState><enabled>false</enabled><operator>And</operator></combineState></QueryManagerRequest>";

            QueryManagerRequest request = (QueryManagerRequest)GeneralUtils.deSerialize(xmlRequest, typeof(QueryManagerRequest));

            return request;
        }


    }
}
