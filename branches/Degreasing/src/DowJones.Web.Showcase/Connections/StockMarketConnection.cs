// -----------------------------------------------------------------------
// <copyright file="StockMarketConnection.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.Extensions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Session;
using Factiva.Gateway.Messages.RTQueue.V1_0;
using Factiva.Gateway.Services.V1_0;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignalR;
using Timer = System.Timers.Timer;

namespace DowJones.Web.Showcase.Connections.StockMarket
{
    public class StockMarketConnection : PersistentConnection
    {
        private static Timer timer;
       // private static int callNumber;
        private const int Interval = 200;
        private readonly static object SyncRootForUpdatingUsers = new object();
        private readonly static object SyncRootForMessage = new object();
        private static readonly Dictionary<string, User> Users = new Dictionary<string, User>();
        private static readonly JsonSerializer Serializer;

        static StockMarketConnection()
        {
            Serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                TypeNameHandling = TypeNameHandling.None,
            };

            Serializer.Converters.Add(new JavaScriptDateTimeConverter());
        }

        protected override Task OnConnectedAsync(SignalR.Hosting.IRequest request, IEnumerable<string> groups, string connectionId)
        {
            lock (SyncRootForUpdatingUsers)
            {
                Users.Add(connectionId, GetUser(connectionId));
            }
            return Connection.Broadcast(DateTime.Now + ": " + connectionId + " joined");
        }

        protected override Task OnDisconnectAsync(string connectionId)
        {
            lock (SyncRootForUpdatingUsers)
            {
                Users.Remove(connectionId);
            }
            return Connection.Broadcast(DateTime.Now + ": " + GetUser(connectionId) + " disconnected");
        }

        private static User GetUser(string clientId)
        {
            lock (SyncRootForUpdatingUsers)
            {
                User user;
                return !Users.TryGetValue(clientId, out user) ? new User { clientId = clientId } : user;
            }
        }

        protected override Task OnReceivedAsync(string connectionId, string data)
        {
            var message = Deserialize<Message>(data);

            switch (message.Type)
            {
                case MessageType.Suspend:
                    Suspend(connectionId);
                    return Send(connectionId, "--Suspending Timer");
                case MessageType.Resume:
                    Resume(connectionId);
                    return Send(connectionId, "--Suspending");
                case MessageType.Subscribe:
                    return SubscribeToChannel(connectionId, message.Value);
                case MessageType.Unsubscribe:
                default:
                    UnsubscribeFromChannel(connectionId, message.Value);
                    return Send(connectionId, "--UnsubscribeFromChannelr");
            }
        }

        public Task SubscribeToChannel(string clientId, string channel)
        {
            lock (SyncRootForUpdatingUsers)
            {
                var user = GetUser(clientId);
                user.symbols.Add(channel);
                AddToGroup(clientId, channel);
            }

            // ensure the start of the timer
            Resume(clientId);
            return Send(clientId, new IntitialDataPacket
            {
                Type = MessageType.InitialDataPacket,
                TypeDescriptor = MessageType.InitialDataPacket.ToString(),
                UserId = clientId,
                Data = null,
            });
        }

        public void UnsubscribeFromChannel(string clientId, string channel)
        {
            lock (SyncRootForUpdatingUsers)
            { 
                var user = GetUser(clientId);
                user.symbols.Remove(channel);
                RemoveFromGroup(clientId, channel);
            }
        }

        private static T Deserialize<T>(string json)
        {
            TextReader textReader = new StringReader(json);
            JsonReader jsonReader = new JsonTextReader(textReader);
            return Serializer.Deserialize<T>(jsonReader);
        }

        public void Process()
        {
            lock (SyncRootForMessage)
            {
                //IEnumerable<string> channels;
                lock (SyncRootForUpdatingUsers)
                {
                 
                   
                }
            }
        }

        private void Resume(string clientId)
        {
            if (timer == null)
            {
                lock (SyncRootForMessage)
                {
                    if (timer == null)
                    {
                        timer = new Timer
                        {
                            Interval = Interval
                        };
                        timer.Elapsed += (sender, e) => Process();
                        timer.Start();
                    }
                }
            }
            return;
        }

        private static void Suspend(string connectionId = null)
        {
            if (timer == null) return;
            lock (SyncRootForMessage)
            {
                timer.Stop();
                if (timer == null) return;
                timer.Dispose();
                timer = null;
            }
        }

        /// <summary>
        /// Returns Alert Context
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private static CreateSharedAlertResponse CreateSharedAlert(string topic)
        {
            var request = new CreateSharedAlertRequest
            {
                SharedTopic = topic,
                MaxHeadlinesToReturn = 10
            };

            var cData = ControlDataManager.GetLightWeightUserControlData("dacostad", "vader", "16");
            var serviceResponse = RTQueueAggregationService.CreateSharedAlert(ControlDataManager.Convert(cData), request);
            return serviceResponse.rc == 0 ? serviceResponse : null;
        }

        public static GetSharedAlertContentResponse GetSharedAlertContent(string alertContext)
        {
            var request = new GetSharedAlertContentRequest
            {
                AlertContext = alertContext,
                FetchOrder = QueueFetchOrder.FromTop,
                MaxHeadlinesToReturn = 100,
            };

            var cData = ControlDataManager.GetLightWeightUserControlData("dacostad", "vader", "16");
            var serviceResponse = RTQueueAggregationService.GetSharedAlertContent(ControlDataManager.Convert(cData), request);
            return serviceResponse.rc == 0 ? serviceResponse : null;
        }
    }

    enum MessageType
    {
        Subscribe,
        Unsubscribe,
        Resume,
        Suspend,
        DataPacket,
        InitialDataPacket,
    }

    internal class User
    {
        public string clientId { get; set; }

        public List<string> symbols { get; set; }
    }

    internal class Message
    {
        [DataMember(Name = "messageType")]
        [JsonProperty("messageType")]
        public MessageType Type
        {
            get;
            set;
        }


        [DataMember(Name = "messageTypeDescriptor")]
        [JsonProperty("messageTypeDescriptor")]
        public string TypeDescriptor
        {
            get;
            set;
        }

        [DataMember(Name = "value")]
        [JsonProperty("value")]
        public string Value
        {
            get;
            set;
        }
    }

    internal class UpdateDataPacket : Message
    {
        [JsonProperty("channel")]
        public string Channel
        {
            get;
            set;
        }

        [JsonProperty("alertContext")]
        public string AlertContext
        {
            get;
            set;
        }


        [JsonProperty("callNuber")]
        public int CallNumber
        {
            get;
            set;
        }

        [JsonProperty("data")]
        public PortalHeadlineListDataResult Data
        {
            get;
            set;
        }

    }

    [DataContract(Name = "intitialDataPacket", Namespace = "")]
    internal class IntitialDataPacket : Message
    {
        [DataMember(Name = "userId")]
        [JsonProperty("userId")]
        public string UserId
        {
            get;
            set;
        }

        [DataMember(Name = "data")]
        [JsonProperty("data")]
        public PortalHeadlineListDataResult Data
        {
            get;
            set;
        }
    }
}