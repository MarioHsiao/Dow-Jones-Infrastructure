using System.Collections.Generic;
using DowJones.Json.Gateway.Core;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IUserAdminData
    {
         string AnalysisInformation { get; set; }

         string Hop { get; set; }

         string HopSpecificData { get; set; }

         string SupplementalData { get; set; }

         List<Token> Tokens { get; set; } 
    }
}