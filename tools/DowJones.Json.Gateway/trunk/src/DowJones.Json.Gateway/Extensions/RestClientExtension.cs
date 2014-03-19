using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace DowJones.Json.Gateway.Extensions
{
    internal static class RestClientExtension
    {
        internal static void RemoveHandlers(this RestClient client)
        {
            client.RemoveHandler("application/xml");
            client.RemoveHandler("text/xml");
        }
    }
}
