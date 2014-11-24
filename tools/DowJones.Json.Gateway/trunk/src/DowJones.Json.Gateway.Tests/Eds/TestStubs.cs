using DS = DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions;
using ODE = DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Json.Gateway.Tests.Eds
{
    internal static class TestStubs
    {
        internal static ODE.TicketCollection GetTicketCollection()
        {
            var ticket = new ODE.Ticket()
            {
                Recepient = new ODE.Recepient()
                {
                    ToEmailCollection = new ODE.ToEmailCollection()
                    {
                        new ODE.ToEmailAddress()
                        {
                            Name = "Naresh",
                            Address = "djcicd@gmail.com"
                        }
                    },
                    CCEmailCollection = new ODE.CCEmailCollection()
                    {
                        new ODE.CCEmailAddress()
                        {
                            Name = "Li Sun 2",
                            Address = "sunliaa@Toyota.djip.com"
                        }
                    },
                    BCCEmailCollection = null,
                    FromEmailAddress = null,
                    ReplyToEmailAddress = new ODE.ReplyToEmailAddress()
                    {
                        Name = "Vivek K",
                        Address = "djcicd@gmail.com"
                    },
                    EmailDisplayFormat = ODE.EmailDisplayFormat.HTML,
                    Subject = "Blah blah"
                },
                FreeText = "This is free text 1",
                ProductType = "global",
                EmailContentType = ODE.EmailContentType.Headlines,
                FidsCollection = null,
                EmailDisplayLanguage = "en",
                ContentAsAttachment = true,
                ContentCollection = new ODE.ContentCollection()
                {
                    new ODE.ContentByID()
                    {
                        Source = ODE.Source.P,
                        EmailDisplayLanguage = "en",
                        ContentType = ODE.ContentTypeForContentByID.AN,
                        ContentData = "LBA0000020140307ea3700pxl"

                    }
                }
            };

            ODE.TicketCollection ticketCol = new ODE.TicketCollection();
            ticketCol.Add(ticket);

            return ticketCol;
        }

        internal static string GetGenerateOdeRequestJson()
        {
            string rr = @"{  ""RequestMode"": ""Sync"",
                        ""Ticket"": [
                            {
                                ""Recepient"": {
                                    ""ToEmailAddress"": [
                                        {
                                            ""Name"": ""Naresh"",
                                            ""Address"": ""djcicd@gmail.com""
                                        }
                                    ],
                                    ""CCEmailAddress"": [
                                        {
                                            ""Name"": ""Li Sun 2"",
                                            ""Address"": ""sunliaa@Toyota.djip.com""
                                        }
                                    ],
                                    ""BCCEmailAddress"": null,
                                    ""FromEmailAddress"": null,
                                    ""ReplyToEmailAddress"": {
                                        ""Name"": ""Vivek K"",
                                        ""Address"": ""djcicd@gmail.com""
                                    },
                                    ""EmailDisplayFormat"": ""TEXT"",
                                    ""Subject"": null
                                },
                                ""FreeText"": ""This is free text 1"",
                                ""ProductType"": ""global"",
                                ""EmailContentType"": ""FullText"",
                                ""Fids"": null,
                                ""EmailDisplayLanguage"": ""en"",
                                ""ContentAsAttachment"": true,
                    ""Content"": [{""$type"": ""ContentByID"", ""Source"": ""P"",
                                        ""EmailDisplayLanguage"": ""en"",
                                        ""ContentType"": ""AN"",
                                        ""ContentData"": ""LBA0000020140307ea3700pxl""
                                    }
                                ]

                            }
                        ]
                    }";

            //var request = Newtonsoft.Json.JsonConvert.DeserializeObject<DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode.Transactions.GenerateOdeRequest>(rr);

            //var p = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            //var p2 = Newtonsoft.Json.JsonConvert.SerializeObject(r);

            //var p3 = r.Request.ToJson(new DataContractJsonConverter());

            return rr;
        }

        internal static CreateDeliverySettingsRequest GetCreateDeliverySettingsRequest(string deliveryName)
        {
            var deliveryList = new List<DS.Delivery>();

            DS.Delivery firstDelivery = GetCommonDeliverySetting();
            firstDelivery.Name = deliveryName + "_FirstDelivery";
            firstDelivery.DeliveryDayandTime.DeliveryTime = new List<string> { "8" };
            firstDelivery.Content[0].ContentID = "30029078";
            deliveryList.Add(firstDelivery);

            DS.Delivery secondDelivery = GetCommonDeliverySetting();
            secondDelivery.Name = deliveryName + "_SecondDelivery";
            secondDelivery.DeliveryDayandTime.DeliveryTime = new List<string> {"16"};
            secondDelivery.Content[0].ContentID = "30029079";
            deliveryList.Add(secondDelivery);

            var req = new CreateDeliverySettingsRequest
            {
                delivery = deliveryList
            };

            return req;
        }

        private static DS.Delivery GetCommonDeliverySetting()
        {
            var content = new DS.Content()
            {
                ContentID = "0",
                Position = 1,
                HeadlineSort = DS.HeadlineSort.ByDate,
                MaxHits = 10
            };

            var contentList = new List<DS.Content>();
            contentList.Add(content);
            var delivery = new DS.Delivery()
            {
                ToEmailAddress = "sunli@Toyota.djip.com",
                ProductType = DS.ProductType.global,
                DeliveryType = DS.DeliveryType.B,
                ContentAsAttachment = false,
                EmailDisplayFormat = DS.EmailDisplayFormat.TEXT,
                EmailDisplaylanguage = DS.Language.fr,
                EmailContentType = DS.EmailContentType.Fulltext,
                EnableDaylightSaving = true,
                TimeZone = "-04:00|1",
                ShowDuplicates = true,
                EnableHighlight = true,
                DeliveryDayandTime = new DS.DeliveryDayandTime()
                {
                    DeliveryDay = new List<DS.Day>() { DS.Day.Monday, DS.Day.Tuesday },
                    Repeat = DS.Repeat.Weekly,
                },
                Content = contentList
            };
            return delivery;
        }
    }
}
