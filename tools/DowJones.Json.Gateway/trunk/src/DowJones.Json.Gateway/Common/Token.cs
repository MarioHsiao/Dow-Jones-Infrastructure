using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Common
{
    public class Token : IToken
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
