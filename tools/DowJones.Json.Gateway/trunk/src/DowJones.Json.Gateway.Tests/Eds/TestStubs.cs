using DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Json.Gateway.Tests.Eds
{
    internal static class TestStubs
    {
        internal static TicketCollection GetTicketCollection()
        {
            var ticket = new Ticket()
            {
                Recepient = new Recepient()
                {
                    ToEmailCollection = new ToEmailCollection()
                    {
                        new ToEmailAddress()
                        {
                            Name = "Naresh",
                            Address = "djcicd@gmail.com"
                        }
                    },
                    CCEmailCollection = new CCEmailCollection()
                    {
                        new CCEmailAddress()
                        {
                            Name = "Li Sun 2",
                            Address = "sunliaa@Toyota.djip.com"
                        }
                    },
                    BCCEmailCollection = null,
                    FromEmailAddress = null,
                    ReplyToEmailAddress = new ReplyToEmailAddress()
                    {
                        Name = "Vivek K",
                        Address = "djcicd@gmail.com"
                    },
                    EmailDisplayFormat = EmailDisplayFormat.HTML,
                    Subject = "Blah blah"
                },
                FreeText = "This is free text 1",
                ProductType = "global",
                EmailContentType = EmailContentType.Headlines,
                FidsCollection = null,
                EmailDisplayLanguage = "en",
                ContentAsAttachment = true,
                ContentCollection = new ContentCollection()
                {
                    new ContentByID()
                    {
                        Source = Source.P,
                        EmailDisplayLanguage = "en",
                        ContentType = ContentTypeForContentByID.AN,
                        ContentData = "LBA0000020140307ea3700pxl"

                    }
                }
            };

            TicketCollection ticketCol = new TicketCollection();
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
    }
}
