using System.Collections.Generic;

namespace DowJones.Json.Gateway.Core
{
    public class ClientCode
    {
        public string CostCodeDelimiter { get; set; }

        public string CostCodeDescriptor { get; set; }

        public List<string> CostCodes { get; set; }

        public string CostCodeType { get; set; }
    }
}