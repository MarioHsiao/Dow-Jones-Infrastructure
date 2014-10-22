using System;
using System.Globalization;
using System.Threading;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = DowJones.Json.Gateway.Common.Environment;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode.Transactions;
using System.Collections.Generic;

namespace DowJones.Json.Gateway.Tests.Eds
{
    [TestClass]
    public class DirectListServiceUnitTests : AbstractUnitTests
    {
        [TestMethod]
        public void GenerateODE()
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
                EmailContentType = EmailContentType.FullText,
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

            var r = new RestRequest<GenerateOdeRequest>
            {
                Request = new GenerateOdeRequest()
                {
                  TicketCollection  = ticketCol,
                  requestMode = RequestMode.ASync
                },
                ControlData = GetControlData()
            };

            //var p2 = Newtonsoft.Json.JsonConvert.SerializeObject(r);

            //var p3 = r.Request.ToJson(new DataContractJsonConverter());

            //UpdateRoutingData(r.ControlData.RoutingData);

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GenerateOdeRequest, GenerateOdeResponse>(r);

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
            Assert.Fail("unable to generate ODE");
        }

        private static void UpdateRoutingData(IRoutingData routingData)
        {
            // ReSharper disable StringLiteralTypo
            // ReSharper disable CommentTypo
            // routingData.HttpEndPoint = "http://sktfrtutil01.dev.us.factiva.net:9097";
            // ReSharper restore CommentTypo
            // ReSharper restore StringLiteralTypo

            // ReSharper disable StringLiteralTypo
            //routingData.ServerUri = "http://utilities.int.dowjones.com/restserviceproxy";
            // ReSharper restore StringLiteralTypo
            routingData.ServiceUrl = "http://edsapi.int.dowjones.net/";
            routingData.TransportType = "HTTP";
            routingData.Environment = Environment.Proxy;
            routingData.Serializer = JsonSerializer.DataContract;
        }
    }
}
