using System;

namespace DowJones.Json.Gateway.Interfaces
{
    public interface IJsonSerializable : ICloneable
    {
        string ToJson();
    }
}