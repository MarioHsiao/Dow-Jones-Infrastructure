using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Pages
{
    public class GetPageByName: IGetJsonRestRequest
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    public class GetPageByNameResponse: IJsonRestResponse
    {
    }
}
