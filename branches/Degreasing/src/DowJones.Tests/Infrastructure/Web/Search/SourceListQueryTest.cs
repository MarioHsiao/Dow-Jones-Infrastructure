using System.Collections.Generic;
using System.Diagnostics;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Web
{
    [TestClass]
    public class SourceListQueryTest : UnitTestFixture
    {
        private const string NAME = "UnitTest_Infras_Test_";
        private IControlData _controlData = new ControlData {UserID = "joyful", UserPassword = "joyful", ProductID = "16"};

        //[TestMethod]
        public void CreateMultipleList()
        {
            for (int i = 0; i < 10; i++)
            {
                CreateSourceList(NAME + i.ToString("000"));
            }
        }

        //[TestMethod]
        private void CreateSourceList(string name)
        {
            var sourceListQuery = new SourceListQuery();
            sourceListQuery.Properties = new SourceListQueryProperties();
            sourceListQuery.Properties.Name = name;
            sourceListQuery.Properties.Description = "All posible values in one list from naresh's unit test..";
            sourceListQuery.EditIn = EditIn.CommunicatorSources;
            sourceListQuery.Version = 1;

            var mainGroup = new Group();
            mainGroup.Operator = GroupOperator.Or;
            mainGroup.FilterGroup = new OrFilterGroup();
            mainGroup.FilterGroup.Filters = new QueryFilterCollection();

            //SC
            mainGroup.FilterGroup.Filters.Add(
                new SourceEntityFilter
                    {
                        SourceEntitiesCollection = new SourceEntitiesCollection
                        {
                            new SourceEntities
                            {
                                SourceEntityCollection = new SourceEntityCollection
                                {
                                    new SourceEntity {Type = SourceEntityType.SC, Value = "j"}
                                }
                            }
                        }
                    });

            //RTS
            mainGroup.FilterGroup.Filters.Add(
                new SourceEntityFilter
                    {
                        SourceEntitiesCollection = new SourceEntitiesCollection
                        {
                            new SourceEntities
                            {
                                SourceEntityCollection = new SourceEntityCollection
                                {
                                    new SourceEntity {Type = SourceEntityType.RST, Value = "tdjw"}
                                }
                            }
                        }
                    });

            //PST
            mainGroup.FilterGroup.Filters.Add(
                new SourceEntityFilter
                    {
                        SourceEntitiesCollection = new SourceEntitiesCollection
                        {
                            new SourceEntities
                            {
                                SourceEntityCollection = new SourceEntityCollection
                                {
                                    new SourceEntity {Type = SourceEntityType.PDF, Value = "SG_COEXIN"}
                                }
                            }
                        }
                    });

            //Uncoded content
            mainGroup.FilterGroup.Filters.Add(
                new SourceEntityFilter
                    {
                        SourceEntitiesCollection = new SourceEntitiesCollection
                        {
                            new SourceEntities
                            {
                                SourceEntityCollection = new SourceEntityCollection
                                {
                                    new SourceEntity {Type = SourceEntityType.SN, Value = "Naresh Blog"},
                                    new SourceEntity {Type = SourceEntityType.BY, Value = "Naresh Patel"}
                                }
                            }
                        }
                    });

            //PST +  RST
            mainGroup.FilterGroup.Filters.Add(
                new SourceEntityFilter
                    {
                        SourceEntitiesCollection = new SourceEntitiesCollection
                        {
                            new SourceEntities
                            {
                                SourceEntityCollection = new SourceEntityCollection
                                {
                                                         new SourceEntity {Type = SourceEntityType.PDF, Value = "SG_PRREL_DISC"},
                                                         new SourceEntity {Type = SourceEntityType.RST, Value = "iacc"}
                                }
                            }
                        }
                    });

            sourceListQuery.Groups.Add(mainGroup);

            var createQueryRequest = new CreateQueryRequest();
            createQueryRequest.Query = sourceListQuery;

            ServiceResponse createQueryResponse = QueryService.CreateQuery(ControlDataManager.Convert(_controlData), createQueryRequest);
            EnsureResponse(createQueryResponse);

           
        }

//        [TestMethod]
        public void GetAllSourceListItems()
        {
            var listRequest = new GetQueriesDetailsListRequest();
            listRequest.QueryTypes = new List<QueryType>();
            listRequest.QueryTypes.Add(QueryType.SourceListQuery);
            ServiceResponse listResponse = QueryService.GetQueriesDetailsList(ControlDataManager.Convert(_controlData), listRequest);
            EnsureResponse(listResponse);

            object responseObj;
            listResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
            var myList = (GetQueriesDetailsListResponse)responseObj;


            foreach (QueryDetailsItem queryDetailsItem in myList.QueryDetailsItems)
            {
                var byIDRequest = new GetQueryByIDRequest();
                byIDRequest.Id = queryDetailsItem.Id;
                ServiceResponse byIdResponse = QueryService.GetQueryByID(ControlDataManager.Convert(_controlData), byIDRequest);
                EnsureResponse(byIdResponse);
            }
        }

        //[TestMethod]
        public void DeleteAllSourceListItems()
        {
            var listRequest = new GetQueriesDetailsListRequest();
            listRequest.QueryTypes = new List<QueryType>();
            listRequest.QueryTypes.Add(QueryType.SourceListQuery);
            ServiceResponse listResponse = QueryService.GetQueriesDetailsList(ControlDataManager.Convert(_controlData), listRequest);
            EnsureResponse(listResponse);

            object responseObj;
            listResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
            var myList = (GetQueriesDetailsListResponse)responseObj;


            foreach (QueryDetailsItem queryDetailsItem in myList.QueryDetailsItems)
            {
                var byIDRequest = new GetQueryByIDRequest();
                byIDRequest.Id = queryDetailsItem.Id;
                ServiceResponse byIdResponse = QueryService.GetQueryByID(ControlDataManager.Convert(_controlData), byIDRequest);
                EnsureResponse(byIdResponse);

                var delete = new DeleteQueryRequest();
                delete.Id = queryDetailsItem.Id;
                ServiceResponse deleteResponse = QueryService.DeleteQuery(ControlDataManager.Convert(_controlData), delete);
                EnsureResponse(deleteResponse);
            }
        }

        private static void EnsureResponse(ServiceResponse serviceResponse)
        {
            Debug.WriteLine(serviceResponse.RawResponse);
            Assert.IsTrue(serviceResponse.rc == 0);
        }
    }
}