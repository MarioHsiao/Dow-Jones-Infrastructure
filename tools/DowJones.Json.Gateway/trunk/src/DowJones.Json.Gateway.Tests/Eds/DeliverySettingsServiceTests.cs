using System;
using System.Globalization;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = DowJones.Json.Gateway.Common.Environment;

namespace DowJones.Json.Gateway.Tests.Eds.DeliverySettings
{
    [TestClass]
    public class DeliverySettingsServiceTests : AbstractUnitTests
    {
        [TestMethod]
        public void VerifyPassingValidGetDeliverySettingsRequestToSerivceReturnsValidResponse()
        {
            RestResponse<GetDeliverySettingsResponse> response = GetSettings();
            if (response.ReturnCode == 0 && response.Data != null)
            {
                Assert.IsNotNull(response);
            }
            else
            {
                Console.WriteLine(response.Error.Message);
                Assert.Fail(string.Concat("failed w/rc:= ",response.Error.Message.ToString(CultureInfo.InvariantCulture)));
            }
        }
        

        [TestMethod]
        public void VerifyPassingValidDeleteDeliverySettingsRequestToSerivceReturnsValidResponse()
        {
            RestResponse<GetDeliverySettingsResponse> mySettings = GetSettings();
            if (mySettings != null && mySettings.Data != null)
            {
                foreach (Delivery del in mySettings.Data.Delivery)
                {
                    DeleteSetting(del.Id);
                }
            }
        }

        [TestMethod]
        public void VerifyPassingValidUpdateDeliverySettingsRequestToSerivceReturnsValidResponse()
        {
            var a = new UpdateDeliverySettingsRequest();
            RestResponse<GetDeliverySettingsResponse> mySettings = GetSettings();
            if (mySettings != null && mySettings.Data != null)
            {
                foreach (Delivery del in mySettings.Data.Delivery)
                {
                    del.Name = del.Name + "_Updated";
                    a.delivery.Add(del);
                }
            }
           
            var r = new RestRequest<UpdateDeliverySettingsRequest>
            {
                Request = a,
                ControlData = GetControlData()
            };

            UpdateRoutingDataForNonDevelopment(r.ControlData.RoutingData);

            RestResponse<UpdateDeliverySettingsResponse> response = Execute<UpdateDeliverySettingsRequest, UpdateDeliverySettingsResponse>(r);

            if (response.ReturnCode == 0 && response.Data != null)
            {
                Assert.IsNotNull(response);
            }
            else
            {
                Console.WriteLine(response.Error.Message);
                Assert.Fail(string.Concat("failed w/rc:= ", response.Error.Message.ToString(CultureInfo.InvariantCulture)));
            }
        }


        [TestMethod]
        public void VerifyPassingValidCreateDeliverySettingsRequestToSerivceReturnsValidResponse()
        {
            var r = new RestRequest<CreateDeliverySettingsRequest>
            {
                Request = TestStubs.GetCreateDeliverySettingsRequest("TestName123"),
                ControlData = GetControlData()
            };

            UpdateRoutingDataForNonDevelopment(r.ControlData.RoutingData);

            RestResponse<CreateDeliverySettingsResponse> response = Execute<CreateDeliverySettingsRequest, CreateDeliverySettingsResponse>(r);

            if (response.ReturnCode == 0 && response.Data != null)
            {
                Assert.IsNotNull(response);
            }
            else
            {
                Console.WriteLine(response.Error.Message);
                Assert.Fail(string.Concat("failed w/rc:= ", response.Error.Message.ToString(CultureInfo.InvariantCulture)));
            }
        }

        [TestMethod]
        public void Verify_CreateDeliverySettingsRequest_Property_Names_Are_Properly_Serialized()
        {
            CreateDeliverySettingsRequest request = TestStubs.GetCreateDeliverySettingsRequest("Test12345");
            string serializedObject = request.ToJson(new JsonDotNetJsonConverter());
            if (serializedObject == null) throw new ArgumentNullException("serializedObject");

            Assert.AreNotEqual(serializedObject.IndexOf("\"delivery\""), -1, "Delivery property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"name\""), -1, "Name property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"toEmailAddress\""), -1, "ToEmailAddress property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"productType\""), -1, "ProductType property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"deliveryType\""), -1,"DeliveryType property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"contentAsAttachment\""), -1,"ContentAsAttachment property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"emailDisplayFormat\""), -1,"EmailDisplayFormat property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"emailDisplaylanguage\""), -1,"EmailDisplaylanguage property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"emailContentType\""), -1,"EmailContentType property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"enableDaylightSaving\""), -1,"EnableDaylightSaving property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"timeZone\""), -1, "TimeZone property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"showDuplicates\""), -1,"ShowDuplicates property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"enableHighlight\""), -1,"EnableHighlight property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"deliveryDayandTime\""), -1,"DeliveryDayandTime property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"deliveryDay\""), -1,"DeliveryDay property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"repeat\""), -1, "Repeat property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"deliveryTime\""), -1,"DeliveryTime property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"content\""), -1, "Content property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"contentID\""), -1,"ContentID property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"position\""), -1, "Position property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"headlineSort\""), -1,"HeadlineSort property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"maxHits\""), -1, "MaxHits property incorrectly serialized");
        }

        [TestMethod]
        public void Verify_GetDeliverySettingsRequest_Property_Names_Are_Properly_Serialized()
        {
            var request = new GetDeliverySettingsRequest
            {
                DeliveryId = "30029078",
                DeliveryType = DeliveryType.B,
                ReturnType = ReturnType.Full
            };

            string serializedObject = request.ToJson(new JsonDotNetJsonConverter());
            if (serializedObject == null) throw new ArgumentNullException("serializedObject");

            Assert.AreNotEqual(serializedObject.IndexOf("returnType"), -1);
            Assert.AreNotEqual(serializedObject.IndexOf("deliveryId"), -1);
            Assert.AreNotEqual(serializedObject.IndexOf("deliveryType"), -1);
        }

        [TestMethod]
        public void Verify_DeleteDeliverySettingsRequest_Property_Names_Are_Properly_Serialized()
        {
            var request = new DeleteDeliverySettingsRequest
            {
                DeliveryId = "30029078"
            };

            string serializedObject = request.ToJson(new JsonDotNetJsonConverter());
            if (serializedObject == null) throw new ArgumentNullException("serializedObject");

            Assert.AreNotEqual(serializedObject.IndexOf("deliveryId"), -1);
        }

        private RestResponse<GetDeliverySettingsResponse> GetSettings()
        {
            var r = new RestRequest<GetDeliverySettingsRequest>
            {
                Request = new GetDeliverySettingsRequest { DeliveryType = DeliveryType.B },
                ControlData = GetControlData()
            };

            UpdateRoutingDataForNonDevelopment(r.ControlData.RoutingData);
            //UpdateRoutingData(r.ControlData.RoutingData);

            RestResponse<GetDeliverySettingsResponse> response =Execute<GetDeliverySettingsRequest, GetDeliverySettingsResponse>(r);

            return response;
        }

        private void DeleteSetting(string id)
        {
            var r = new RestRequest<DeleteDeliverySettingsRequest>
            {
                Request = new DeleteDeliverySettingsRequest { DeliveryId = id },
                ControlData = GetControlData()
            };

            UpdateRoutingDataForNonDevelopment(r.ControlData.RoutingData);

            RestResponse<DeleteDeliverySettingsResponse> response =Execute<DeleteDeliverySettingsRequest, DeleteDeliverySettingsResponse>(r);

            if (response.ReturnCode == 0 && response.Data != null)
            {
                Assert.IsNotNull(response);
            }
            else
            {
                Console.WriteLine(response.Error.Message);
                Assert.Fail(string.Concat("failed w/rc:= ",response.Error.Message.ToString(CultureInfo.InvariantCulture)));
            }
        }

        private static void UpdateRoutingData(IRoutingData routingData)
        {
            routingData.ServiceUrl = "http://edsapi.int.dowjones.net/";
            routingData.TransportType = "HTTP";
            routingData.Environment = Environment.Direct;
            routingData.Serializer = JsonSerializer.JsonDotNet;
        }

        private static void UpdateRoutingDataForNonDevelopment(IRoutingData routingData)
        {
            routingData.ServerUri = "http://utilities.int.dowjones.com/restserviceproxy";
            routingData.TransportType = "RTS";
            routingData.Environment = Environment.Proxy;
            routingData.Serializer = JsonSerializer.JsonDotNet;
        }
    }
}