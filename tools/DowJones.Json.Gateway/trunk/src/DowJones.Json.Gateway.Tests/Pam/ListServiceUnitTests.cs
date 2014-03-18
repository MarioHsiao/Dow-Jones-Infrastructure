using System;
using System.Globalization;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = DowJones.Json.Gateway.Interfaces.Environment;

namespace DowJones.Json.Gateway.Tests.Pam
{
    [TestClass]
    public class ListServiceUnitTests : AbstractUnitTests
    {
        [TestMethod]
        public void GetListByIdRequest()
        {
            var r = new RestRequest<GetListByIdRequest>
                    {
                        Request = new GetListByIdRequest
                                  {
                                      Id = 10,
                                  },
                        ControlData = GetControlData(),
                    };

            r.ControlData.RoutingData.TransportType = "HTTP";
            // ReSharper disable StringLiteralTypo
            r.ControlData.RoutingData.ServerUri = "http://pamapi.dev.dowjones.net/";
            // ReSharper restore StringLiteralTypo
            r.ControlData.RoutingData.Environment = Environment.Development;
            r.ControlData.RoutingData.Environment = Environment.Development;
            r.ControlData.RoutingData.Serializer = JsonSerializer.DataContract;

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
                    Assert.Fail(string.Concat("failed w/rc:= ",t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
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
        public void CreateList()
        {

            AuthorItem item = new AuthorItem();
            AuthorItem item_1 = new AuthorItem();
            var authList = new AuthorList();
            Permission per = new Permission();
            ShareRoleCollection role = new ShareRoleCollection();
            ItemCollection itemColl = new ItemCollection();
            ItemGroup group = new ItemGroup();
            GroupList grpList = new GroupList();
            ShareProperties share = new ShareProperties();
            ItemGroupCollection grpCollection = new ItemGroupCollection();
            grpList.Add(new GroupIdList { Type = GroupListType.Group, IdCollection = new IdCollection { "test-1", "test-2" } });
            role.Add(ShareRole.Admin);
            per.Scope = ShareScope.Account;
            per.ShareRoleCollection = role;
            per.Groups = grpList;
            share.AccessPermission = per;
            share.AssignPermission = new Permission { Groups = grpList, Scope = ShareScope.Personal, ShareRoleCollection = role };
            share.DeletePermission = new Permission { Groups = grpList, Scope = ShareScope.AccountAdmin, ShareRoleCollection = role };
            item.Id = 1;
            item.Properties = new AuthorItemProperties { Code = "001" };
            item_1.Id = 2;
            item_1.Properties = new AuthorItemProperties { Code = "002" };
            itemColl.Add(item);
            itemColl.Add(item_1);
            group.Id = 10;
            group.GroupType = ItemGroupType.Default;
            group.ItemCollection = itemColl;

            authList.Id = 100;
            authList.Name = "Dow Jones Author List";
            authList.CustomCode = "Arunald";
            grpCollection.Add(group);
            authList.ItemGroupCollection = grpCollection;
            authList.Properties = new AuthorListProperties { Description = "Ahthor List" };
            authList.ShareProperties = share;

            Console.Write(authList.ToJson(true));

            var r = new RestRequest<CreateListRequest>
            {
                Request = new CreateListRequest
                {
                    List = authList
                },
                ControlData = GetControlData(),
            };

            // ReSharper disable StringLiteralTypo
            r.ControlData.RoutingData.ServerUri = "http://pamapi.dev.dowjones.net/";
            // ReSharper restore StringLiteralTypo
            r.ControlData.RoutingData.TransportType = "RTS";
            r.ControlData.RoutingData.Environment = Environment.Development;

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<CreateListRequest, CreateListResponse>(r);

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
    }
}