using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    public class PlatformAdminData : AbstractJsonSerializable, IPlatformAdminData
    {
        [DataMember]
        public int? TransactionTimeout { get; set; }
    }
}
