using System.Collections;
using System.Collections.Generic;
using DowJones.Json.Gateway.Common;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IPlatformAdminData : IJsonSerializable
    {
        short? TransactionTimeout { get; set; }

        string ArmRemainingPercentage { set; get; }

        string ArmRemainingTime { set; get; }

        string RequestFormat { set; get; }

        string ResponseFormat { set; get; }
        
        IEnumerable<Token> Tokens { get; set; }
    }
}