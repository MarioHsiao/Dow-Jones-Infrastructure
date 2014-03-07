using DowJones.Json.Gateway.Core;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IToken
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}