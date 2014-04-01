using System.Collections.Generic;
using DowJones.Json.Gateway.Common;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IAuthorizationData
    {
        List<AuthComponent> AuthComponents { get; set; }
        
        string CountryCode { get; set; }
        
        string CustomerType { get; set; }
        
        string RuleCode { get; set; }
        
        string TimeZone { get; set; }

         IEnumerable<Token>Tokens { get; set; }
    }
}