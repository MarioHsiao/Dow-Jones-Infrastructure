using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    [DataContract]
    public class PlatformAdminData : AbstractJsonSerializable, IPlatformAdminData
    {
        public PlatformAdminData()
        {
            Tokens = new List<Token>();
        }


        [DataMember]
        public string ArmRemainingPercentage { get; set; }

        [DataMember]
        public string ArmRemainingTime { get; set; }

        [DataMember]
        public string RequestFormat { get; set; }

        [DataMember]
        public string ResponseFormat { get; set; }

        [DataMember]
        public IEnumerable<Token> Tokens { get; set; }

        [DataMember]

        public short? TransactionTimeout { get; set; }
    }
}