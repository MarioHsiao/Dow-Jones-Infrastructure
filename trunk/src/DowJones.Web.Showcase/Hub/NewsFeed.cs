using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.DependencyInjection;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Session;
using Factiva.Gateway.Messages.RTQueue.V1_0;
using Factiva.Gateway.Services.V1_0;
using log4net;
using Newtonsoft.Json;
using SignalR.Hubs;
using Timer = System.Timers.Timer;


namespace SignalR_Proto.Hubs
{
    [HubName( "newsFeed" )]
    public class NewsFeed : Hub
    {
        private static Timer timer;
        private static int callNumber;     
        private readonly static object SyncRoot = new object(); 
        private static readonly List<string> Channels = new List<string>();
        private static readonly Dictionary<string, string> ChannelsToAlertContext = new Dictionary<string,string>();
        
        public NewsFeed()
        {
            Start();
        }  
                                                                             
        [Inject("Injected to avoid a base constructor call")]
        protected ILog Log
        {
            // Provide a "sane default" so this is never the cause of NRE's
            get { return _log ?? LogManager.GetLogger(GetType()); }
            set { _log = value; }
        }

        private ILog _log;

        public IntitialDataPacket Connect(string channel)
        {
            // Set unique id for client.
            Caller.Id = Context.ConnectionId;                

            var portalHeadlineConversionManager = new PortalHeadlineConversionManager();
            var headlineConvertionManager = new RealtimeHeadlinelistConversionManager( new DateTimeFormatter( "en" ) );
            var response = CreateSharedAlert( channel );
            var headlines = headlineConvertionManager.Process( response, null, null );
            
            if( !Channels.Contains( channel ))
            {
                Channels.Add( channel );
            }

            if (!ChannelsToAlertContext.ContainsKey(channel) && response != null)
            {
                ChannelsToAlertContext.Add(channel, response.AlertContext);
            }

            this.AddToGroup( channel );
            return new IntitialDataPacket
                       {
                           UserId = Caller.Id,
                           Data = portalHeadlineConversionManager.Map( headlines ),
                        };
        }

        public bool Start()
        {
            if( timer == null )
            {
                lock( SyncRoot )
                {
                    if( timer == null )
                    {
                        timer = new Timer
                        {
                            Interval = 1000
                        };
                        timer.Elapsed += ( sender, e ) => Send();
                        timer.Start();
                    }
                }
            }
            return true;
        }

        public string Stop()
        {
            // Set unique id for client.
            Caller.Id = Context.ConnectionId;
            if( timer != null )
            {
                lock( SyncRoot )
                {
                    timer.Stop();
                    if( timer != null )
                    {
                        timer.Dispose();
                        timer = null;
                    }
                }
            }
            return Caller.Id;
        }

        public string Reset( string newChannel )
        {
            // Set unique id for client.
            Caller.Id = Context.ConnectionId;
            this.RemoveFromGroup( newChannel );
            return Caller.Id;
        }

        public void Send()
        {                                                  
            Interlocked.Increment( ref callNumber );
            foreach( var channel in Channels )
            {     
                                   
                    var alertContext = ChannelsToAlertContext[channel];
                    var portalHeadlineConversionManager = new PortalHeadlineConversionManager();
                    var headlineConvertionManager = new RealtimeHeadlinelistConversionManager(new DateTimeFormatter("en"));
                    var response = GetSharedAlertContent(alertContext);
                    var headlines = headlineConvertionManager.Process(response, null, null);

                    Clients[channel].addMessage(new UpdateDataPacket
                                                    {
                                                        CallNumber = callNumber,
                                                        Channel = channel,
                                                        AlertContext = alertContext,
                                                        Data = portalHeadlineConversionManager.Map(headlines),
                                                    });  
                  
            }
        }

        /// <summary>
        /// Returns Alert Context
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private static CreateSharedAlertResponse CreateSharedAlert( string topic )
        {
            var request = new CreateSharedAlertRequest
            {
                SharedTopic = topic,
                MaxHeadlinesToReturn = 10
            };

            var cData = ControlDataManager.GetLightWeightUserControlData( "dacostad", "vader", "16" );
            var serviceResponse = RTQueueAggregationService.CreateSharedAlert( ControlDataManager.Convert(cData), request );
            return serviceResponse.rc == 0 ? serviceResponse : null;
        }

        /// <summary>
        /// Gets the content of the shared alert.
        /// </summary>
        /// <param name="alertContext">The alert context.</param>
        /// <returns></returns>
        public static GetSharedAlertContentResponse GetSharedAlertContent(string alertContext)
        {
            var request = new GetSharedAlertContentRequest
                              {
                                  AlertContext = alertContext,
                                  FetchOrder = QueueFetchOrder.FromTop,
                                  MaxHeadlinesToReturn = 100,
                              };

            var cData = ControlDataManager.GetLightWeightUserControlData( "dacostad", "brian", "16" );
            var serviceResponse = RTQueueAggregationService.GetSharedAlertContent( ControlDataManager.Convert( cData ), request );
            return serviceResponse.rc == 0 ? serviceResponse : null;
        }
    }

    public class UpdateDataPacket
    {
        [JsonProperty( "channel" )]
        public string Channel { get; set; }

        [JsonProperty( "alertContext" )]
        public string AlertContext { get; set; }


        [JsonProperty( "callNuber" )]
        public int CallNumber
        {
            get;
            set;
        }

        [JsonProperty( "data" )]   
        public PortalHeadlineListDataResult Data { get; set; }

    }

    [DataContract( Name = "intitialDataPacket", Namespace = "" )]
    public class IntitialDataPacket
    {
        [DataMember( Name="userId" )]
        [JsonProperty( "userId" )]
        public string UserId { get; set; }

        [DataMember( Name = "data" )]
        [JsonProperty( "data" )]
        public PortalHeadlineListDataResult Data { get; set; }
    }
}