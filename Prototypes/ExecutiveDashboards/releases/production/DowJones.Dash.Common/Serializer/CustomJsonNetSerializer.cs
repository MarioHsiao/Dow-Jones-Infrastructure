using System;
using Newtonsoft.Json;
using SignalR;

namespace DowJones.Dash.Serializer
{
    public class CustomJsonNetSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public CustomJsonNetSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public string Stringify(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public object Parse(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        public object Parse(string json, Type targetType)
        {
            return JsonConvert.DeserializeObject(json, targetType);
        }

        public T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}