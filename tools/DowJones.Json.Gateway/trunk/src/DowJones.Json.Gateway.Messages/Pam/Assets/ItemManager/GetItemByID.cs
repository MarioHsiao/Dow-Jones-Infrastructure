using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetItemByID")]
    [ServicePath("pamapi/1.0/ItemManager.svc")]
    public class GetItemByID : IGetJsonRestRequest
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
    }
}