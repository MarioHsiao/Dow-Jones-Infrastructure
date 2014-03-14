using System;

namespace DowJones.Json.Gateway.Interfaces
{
    public enum Environment
    {
        Production,
        Beta,
        Integration,
        Development,
    }

    public interface IRoutingData : IJsonSerializable
    {
        int ContentServerAddress { get; set; }

        string TransportType { get; set; }

        Environment Environment { get; set; }

        string ServerUri { get; set; }
    }
}