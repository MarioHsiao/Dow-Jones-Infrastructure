using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace DowJones.Utilities
{
    public static class Serialization
    {

        /// <summary>
        /// Deserializes the specified json.
        /// </summary>
        /// <typeparam name="T">The target type to be serialized to.</typeparam>
        /// <param name="json">The json string.</param>
        /// <returns>Deserialized instance of type T.</returns>
        public static T DeserializeJson<T>(this string json) where T : class
        {
            // Deserialize Example - myObject = DeserializeJsonToObject<Person>(input);
            //T obj = Activator.CreateInstance<T>();
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof (T));
            var obj = (T) serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }

        /// <summary>
        /// Serialize the object as XML
        /// </summary>
        /// <returns></returns>
        public static string Serialize(this object obj, Formatting formatting = Formatting.Indented, Encoding encoding = null)
        {
            var dcs = new DataContractSerializer(obj.GetType());
            var ms = new MemoryStream();
            string xml;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var xmlTextWriter = new XmlTextWriter(ms, encoding))
            {
                xmlTextWriter.Formatting = formatting;
                dcs.WriteObject(xmlTextWriter, obj);
                xmlTextWriter.Flush();
                ms = (MemoryStream) xmlTextWriter.BaseStream;
                ms.Flush();
                xml = Utf8ByteArrayToString(ms.ToArray());
            }
            return xml;
        }

        /// <summary>
        /// Deserialize the string into type T using DataContractSerializer.
        /// </summary>
        /// <typeparam name="T">The target type to be serialized to.</typeparam>
        /// <param name="str">The serialized XML.</param>
        /// <returns>Deserialized instance of type T.</returns>
        public static T Deserialize<T>(this string str) where T: class 
        {
            var dcs = new DataContractSerializer(typeof (T));
            var ms = new MemoryStream(StringToUtf8ByteArray(str));
            var xmlReader = XmlReader.Create(ms);
            return dcs.ReadObject(xmlReader, false) as T;
        }

        private static String Utf8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static Byte[] StringToUtf8ByteArray(String pXmlString)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}
