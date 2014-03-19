using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RestSharp;

namespace DowJones.Json.Gateway.Converters
{
    public class DataContractJsonConverter : ISerialize
    {
        public string Serialize<T>(T obj, Formatting formatting)
        {
            return Serialize(obj);
        }

        public string Serialize<T>(T obj)
        {
            //Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(ms, obj);
            var json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public T Deserialize<T>(string str)
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));
            var serializer = new DataContractJsonSerializer(typeof(T));
            var target = (T)serializer.ReadObject(ms);
            ms.Close();
            return target;
        }

        public T Deserialize<T>(IRestResponse response) 
        {
            return Deserialize<T>(response.Content);
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
        public string Serialize(object obj, Formatting formatting)
        {
            return Serialize(obj);
        }
    }
}
