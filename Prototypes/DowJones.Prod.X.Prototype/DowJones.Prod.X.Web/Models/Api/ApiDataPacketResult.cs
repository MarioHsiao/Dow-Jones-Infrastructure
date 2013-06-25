using DowJones.Newsletters.App.Web.Models.Api;
using Newtonsoft.Json;

namespace DowJones.Prod.X.Web.Models.Api
{
    public class ApiDataPacketResult<T> : BasicApiResult
    {
        public ApiDataPacketResult()
        {
        }

        public ApiDataPacketResult(T packet)
        {
            Packet = packet;
        }

        [JsonProperty("packet")]
        public T Packet { get; set; }
    }
}