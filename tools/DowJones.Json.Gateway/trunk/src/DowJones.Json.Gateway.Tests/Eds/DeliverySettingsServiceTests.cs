using System;
using System.Globalization;
using System.Threading;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Environment = DowJones.Json.Gateway.Common.Environment;
using System.Collections.Generic;
using Formatting = Newtonsoft.Json.Formatting;
using JsonSerializer = DowJones.Json.Gateway.Common.JsonSerializer;

namespace DowJones.Json.Gateway.Tests.Eds.DeliverySettings
{
    [TestClass]
    public class DeliverySettingsServiceTests : AbstractUnitTests
    {
        [TestMethod]
        public void Verify_Passing_Valid_CreateDeliverySettingsRequest_To_Serivce_Returns_Valid_Response()
        {


            var r = new RestRequest<CreateDeliverySettingsRequest>
            {
                Request = TestStubs.GetCreateDeliverySettingsRequest(),
                ControlData = GetControlData()
            };

            UpdateRoutingData(r.ControlData.RoutingData);

            var response = Execute<CreateDeliverySettingsRequest, CreateDeliverySettingsResponse>(r);

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
            CreateDeliverySettingsRequest request = TestStubs.GetCreateDeliverySettingsRequest();
            string serializedObject = request.ToJson(new JsonDotNetJsonConverter());
            if (serializedObject == null) throw new ArgumentNullException("serializedObject");

            Assert.AreNotEqual(serializedObject.IndexOf("\"delivery\""), -1, "Delivery property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"name\""), -1, "Name property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"toEmailAddress\""), -1, "ToEmailAddress property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"productType\""), -1, "ProductType property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"deliveryType\""), -1, "DeliveryType property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"contentAsAttachment\""), -1, "ContentAsAttachment property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"emailDisplayFormat\""), -1, "EmailDisplayFormat property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"emailDisplaylanguage\""), -1, "EmailDisplaylanguage property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"emailContentType\""), -1, "EmailContentType property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"enableDaylightSaving\""), -1, "EnableDaylightSaving property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"timeZone\""), -1, "TimeZone property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"showDuplicates\""), -1, "ShowDuplicates property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"enableHighlight\""), -1, "EnableHighlight property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"deliveryDayandTime\""), -1, "DeliveryDayandTime property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"deliveryDay\""), -1, "DeliveryDay property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"repeat\""), -1, "Repeat property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"deliveryTime\""), -1, "DeliveryTime property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"content\""), -1, "Content property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"contentID\""), -1, "ContentID property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"contentName\""), -1, "ContentName property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"position\""), -1, "Position property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"headlineSort\""), -1, "HeadlineSort property incorrectly serialized");
            Assert.AreNotEqual(serializedObject.IndexOf("\"maxHits\""), -1, "MaxHits property incorrectly serialized");
        }

        [TestMethod]
        public void Verify_GetDeliverySettingsRequest_Property_Names_Are_Properly_Serialized()
        {
            GetDeliverySettingsRequest request = new GetDeliverySettingsRequest()
            {
                DeliveryId = "30029078",
                DeliveryType = DeliveryType.C,
                ReturnType = ReturnType.Full
            };

            string serializedObject = request.ToJson(new JsonDotNetJsonConverter());
            if (serializedObject == null) throw new ArgumentNullException("serializedObject");

            Assert.AreNotEqual(serializedObject.IndexOf("returnType"), -1);
            Assert.AreNotEqual(serializedObject.IndexOf("deliveryId"), -1);
            Assert.AreNotEqual(serializedObject.IndexOf("deliveryType"), -1);

        }

        private static void UpdateRoutingData(IRoutingData routingData)
        {
            routingData.ServiceUrl = "http://edsapi.int.dowjones.net/";
            routingData.TransportType = "HTTP";
            routingData.Environment = Environment.Proxy;
            routingData.Serializer = JsonSerializer.JsonDotNet;
        }
    }
}
