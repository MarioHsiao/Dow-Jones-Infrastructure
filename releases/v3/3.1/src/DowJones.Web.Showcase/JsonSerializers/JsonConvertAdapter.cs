using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignalR;

namespace DowJones.Web.Showcase.JsonSerializers
{
    public class JsonConvertAdapter : IJsonSerializer
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;

        static JsonConvertAdapter()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            JsonSerializerSettings.Converters.Add( new IsoDateTimeConverter() );
        }

        public string Stringify( object obj )
        {
            return JsonConvert.SerializeObject( obj, Formatting.None, JsonSerializerSettings );
        }

        public object Parse( string json )
        {
            return JsonConvert.DeserializeObject( json, JsonSerializerSettings );
        }

        public object Parse( string json, Type targetType )
        {
            return JsonConvert.DeserializeObject( json, targetType , JsonSerializerSettings);
        }

        public T Parse<T>( string json )
        {
            return JsonConvert.DeserializeObject<T>( json, JsonSerializerSettings );
        }
    }
}