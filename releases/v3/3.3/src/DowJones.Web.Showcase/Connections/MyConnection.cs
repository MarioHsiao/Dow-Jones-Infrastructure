
using System;
using System.Web.Script.Serialization;
using SignalR;
using System.Threading.Tasks;

namespace DowJones.Web.Showcase.Connections
{
    public class MyConnection : PersistentConnection
    {
        protected override Task OnReceivedAsync(string clientId, string jsonData)
        {
            // Deserialize json string
            var data = new JavaScriptSerializer()
                .Deserialize(jsonData, typeof(EventData)) as EventData;

            // Broadcast data to all clients
            return Connection.Broadcast(string.Format("{0} {1}: {2}",
                DateTime.Now.ToShortTimeString(), data.ClientName, data.Message));
        }
    }

    public class EventData
    {
        public string ClientName { get; set; }
        public string Message { get; set; }
    }
}