using System;
using System.Globalization;
using System.Threading;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = DowJones.Json.Gateway.Common.Environment;

namespace DowJones.Json.Gateway.Tests.Pam
{
    [TestClass]
    public class DirectListServiceUnitTests : AbstractUnitTests
    {
        [TestMethod]
        public void GetListsDetailsListRequest()
        {
            var r = new RestRequest<GetListsDetailsListRequest>
                    {
                        Request = new GetListsDetailsListRequest
                                  {
                                      MaxResultsToReturn = 100,
                                      ListTypeCollection = new ListTypeCollection(new[] {ListType.AuthorList})
                                  },
                        ControlData = GetControlData(),
                    };

            // Update Routing Data
            UpdateRoutingData(r.ControlData.RoutingData);

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GetListsDetailsListRequest, GetListsDetailsListResponse>(r);

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        [TestMethod]
        public void GetListByIdRequest()
        {
            var id = CreateList();
            var r = new RestRequest<GetListByIdRequest>
                    {
                        Request = new GetListByIdRequest
                                  {
                                      Id = id,
                                  },
                        ControlData = GetControlData(),
                    };

            // Update Routing Data
            UpdateRoutingData(r.ControlData.RoutingData);

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GetListByIdRequest, GetListByIdResponse>(r);

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
                DeleteList(id);
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        [TestMethod]
        public void CreateAndDeleteListRequest()
        {
            DeleteList(CreateList());
        }

        [TestMethod]
        public void CrudListRequests()
        {
            var id = CreateList();

            UpdateListName(id);

            DeleteList(id);
        }


        [TestMethod]
        public void GetListsPropertiesList()
        {
            var id = CreateList();
            var r = new RestRequest<GetListsPropertiesListRequest>
            {
                Request = new GetListsPropertiesListRequest

                {
                    MaxResultsToReturn = 1,
                    ListTypeCollection = new ListTypeCollection(new[] { ListType.AuthorList }),

                    Paging = new Paging(),
                    FilterCollection = new FilterCollection(new[] {new IdSearchFilter { ListIdCollection = new ListIdCollection(new[] {id.ToString(CultureInfo.InvariantCulture)})}})
                },
                ControlData = GetControlData()
            };

            UpdateRoutingData(r.ControlData.RoutingData);

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GetListsPropertiesListRequest, GetListsPropertiesListResponse>(r);

                Console.Write(r.Request.ToJson(new DataContractJsonConverter()));

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
                DeleteList(id);
                return;
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            DeleteList(id);
            Assert.Fail("unable delete list");
        }

        [TestMethod]
        public void GetListsDetailsList()
        {
            var id = CreateList();
            var r = new RestRequest<GetListsDetailsListRequest>
                    {
                        Request = new GetListsDetailsListRequest

                                  {
                                      MaxResultsToReturn = 1,
                                      ListTypeCollection = new ListTypeCollection(new [] { ListType.AuthorList }),
                                      FilterCollection = new FilterCollection(new[]
                                                                              {
                                                                                  new IdSearchFilter
                                                                                  {
                                                                                      ListIdCollection = new ListIdCollection(new[] {id.ToString(CultureInfo.InvariantCulture)})
                                                                                  }
                                                                              })
                                  },
                        ControlData = GetControlData()
                    };

            UpdateRoutingData(r.ControlData.RoutingData);

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GetListsDetailsListRequest, GetListsDetailsListResponse>(r);

                Console.Write(r.Request.ToJson(new DataContractJsonConverter()));

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
                DeleteList(id);
                return;
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            DeleteList(id);
            Assert.Fail("unable delete list");
        }
        
       private void DeleteList(long id)
        {
            var r = new RestRequest<DeleteListRequest>
                {
                    Request = new DeleteListRequest
                    {
                       Id = id
                    },
                    ControlData = GetControlData(),
                };

            // Update Routing Data
            UpdateRoutingData(r.ControlData.RoutingData);

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<DeleteListRequest, DeleteListResponse>(r);

                Console.Write(r.Request.ToJson(new DataContractJsonConverter()));

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
                return;
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Assert.Fail("unable delete list");
        }

        private static void UpdateRoutingData(IRoutingData routingData)
        {
            // ReSharper disable StringLiteralTypo
            // ReSharper disable CommentTypo
            // routingData.HttpEndPoint = "http://sktfrtutil01.dev.us.factiva.net:9097";
            // ReSharper restore CommentTypo
            // ReSharper restore StringLiteralTypo

            // ReSharper disable StringLiteralTypo
            routingData.ServerUri = "http://fdevweb3.win.dowjones.net/serviceproxy2";
            routingData.HttpEndPoint = "http://pamapi.dev.dowjones.net/";
            // ReSharper restore StringLiteralTypo

            routingData.TransportType = "HTTP";
            routingData.Environment = Environment.Proxy;
            routingData.Serializer = JsonSerializer.DataContract;
        }

        private void UpdateListName(long id)
        {
            Thread.Sleep(100);
            var r = new RestRequest<UpdateListNameRequest>
            {
                Request = new UpdateListNameRequest
                {
                    Id = id,
                    Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture), 
                },
                ControlData = GetControlData(),
            };

            // update routing Data
            UpdateRoutingData(r.ControlData.RoutingData);

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<UpdateListNameRequest, UpdateListNameResponse>(r);

                Console.Write(r.Request.ToJson(new DataContractJsonConverter()));

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
                return;
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Assert.Fail("unable update name");
        }

        

        private long CreateList()
        {
             var r = new RestRequest<CreateListRequest>
                    {
                        Request = new CreateListRequest
                                  {
                                      List = GetAuthorList()
                                  },
                        ControlData = GetControlData(),
                    };

             UpdateRoutingData(r.ControlData.RoutingData);
            try
            {
                var rm = new RestManager();
                var t = rm.Execute<CreateListRequest, CreateListResponse>(r);

                Console.Write(r.Request.ToJson(new DataContractJsonConverter()));

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
                return t.Data.Id;
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Assert.Fail("unable to get id");
            return 0;
        }

        private static AuthorList GetAuthorList()
        {
            return new AuthorList
                   {
                       Id = 0,
                       Name = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture),
                       Properties = new AuthorListProperties
                                    {
                                        Description = "Author List"
                                    },
                       ItemGroupCollection = new ItemGroupCollection
                                             {
                                                 new ItemGroup
                                                 {
                                                     Id = 0,
                                                     GroupType = ItemGroupType.Default,
                                                     ItemCollection = new ItemCollection(new[]
                                                                                         {
                                                                                             new AuthorItem
                                                                                             {
                                                                                                 Id = 0,
                                                                                                 Properties = new AuthorItemProperties
                                                                                                              {
                                                                                                                  Code = "001"
                                                                                                              }
                                                                                             },
                                                                                             new AuthorItem
                                                                                             {
                                                                                                 Id = 0,
                                                                                                 Properties = new AuthorItemProperties
                                                                                                              {
                                                                                                                  Code = "002"
                                                                                                              }
                                                                                             }
                                                                                         })
                                                 }
                                             },
                       ShareProperties = new ShareProperties { 
                           AccessPermission = new Permission
                                              {
                                                  Scope = ShareScope.Personal, 
                                                 ShareRoleCollection = new ShareRoleCollection(), 
                                                 Groups = new GroupList(),
                                              },
                          
}

                   };
        }
    }
}