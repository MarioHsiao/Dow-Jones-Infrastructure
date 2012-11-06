using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using DowJones.API.Common.Logging;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace DowJones.API.Common.Utilities
{
    public static class Serialization
    {
        //public static object Deserialize(string xmlRequest, Type objectType)
        //{
        //    var ser = new DataContractSerializer(objectType);

        //    var enc = new UTF8Encoding();
        //    Byte[] b = enc.GetBytes(xmlRequest);
        //    XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(b, new XmlDictionaryReaderQuotas());
        //    object obj = ser.ReadObject(reader);
        //    return obj;
        //}

        /// <summary>
        /// Deserializes the specified json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T DeserializeJson<T>(this string json) where T : class
        {
            // Deserialize Example - myObject = DeserializeJsonToObject<Person>(input);
            //T obj = Activator.CreateInstance<T>();
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            var obj = (T)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }

        public static string Serialize(this object obj)
        {
            string xml = string.Empty;

            try
            {
                var dcs = new DataContractSerializer(obj.GetType());
                var ms = new MemoryStream();
                using (var xmlTextWriter = new XmlTextWriter(ms, Encoding.Default))
                {
                    xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
                    dcs.WriteObject(xmlTextWriter, obj);
                    xmlTextWriter.Flush();
                    ms = (MemoryStream)xmlTextWriter.BaseStream;
                    ms.Flush();
                    xml = Utf8ByteArrayToString(ms.ToArray());
                }
            }
            catch (Exception Ex)
            {
                ApiLog.Logger.ErrorFormat(ApiLog.LogPrefix.Error, Ex.Message);
            }

            return xml;
        }

        public static string SerializeObjectToString(Object obj)
        {
            string serialized = string.Empty;

            try
            {
                MemoryStream stream = new MemoryStream();
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                xs.Serialize(stream, obj);
                stream.Position = 0;
                using (StreamReader sr = new StreamReader(stream))
                {
                    serialized = sr.ReadToEnd();
                }
            }
            catch (Exception Ex)
            {
                ApiLog.Logger.ErrorFormat(ApiLog.LogPrefix.Error, Ex.Message);
                serialized = Serialize(obj);
            }

            return serialized;
        }

        public static Stream SerializeObjectToStream(object obj, RequestFormat format, bool wrapCallback = true)
        {
            string OPNAME = "SerializeObjectToStream";
            ApiLog.Logger.InfoFormat(ApiLog.LogPrefix.Value, string.Format("Start:{0} - format:{1}, obj:{2}", OPNAME, System.Enum.GetName(format.GetType(), format), obj.GetType().ToString()));

            MemoryStream stream = new MemoryStream();

            ////Add ARM values to the response object if returnARM=true
            //ARMUtility.GetARMValuesFromContext(obj);

            switch (format)
            {
                case RequestFormat.Json:
                    JsonSerializer(stream, obj);

                    break;
                case RequestFormat.Xml:
                    new DataContractSerializer(obj.GetType()).WriteObject(stream, obj);
                    break;
                default:
                    new DataContractSerializer(obj.GetType()).WriteObject(stream, obj);
                    break;
            }

            stream.Position = 0;
            ApiLog.Logger.InfoFormat(ApiLog.LogPrefix.Value, string.Format("End:{0} - format:{1}, obj:{2}", OPNAME, System.Enum.GetName(format.GetType(), format), obj.GetType().ToString()));

            // Handling Callback
            if (format == RequestFormat.Json && wrapCallback)
            {
                string callback = Web.GetRequest("callback");
                if (!string.IsNullOrEmpty(callback))
                    stream = WrapStreamWithCallback(stream, callback);
            }
            return stream;
        }

        public static MemoryStream WrapStreamWithCallback(MemoryStream stream, string callback)
        {
            MemoryStream ms = new MemoryStream();
            ApiLog.Logger.Info(ApiLog.LogPrefix.Start);

            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            string data = sr.ReadToEnd();
            string callbackString = string.Format("{0}({1});", callback, data);
            UTF8Encoding enc = new UTF8Encoding();
            Byte[] b = enc.GetBytes(callbackString);
            ms.Write(b, 0, b.Length);
            ms.Position = 0;
            ApiLog.Logger.Info(ApiLog.LogPrefix.End);

            return ms;
        }

        public static T Deserialize<T>(this string str) where T : class
        {
            var dcs = new DataContractSerializer(typeof(T));
            var ms = new MemoryStream(StringToUtf8ByteArray(str));
            var xmlReader = XmlReader.Create(ms);
            return dcs.ReadObject(xmlReader, false) as T;
        }

        public static String Utf8ByteArrayToString(Byte[] characters)
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

        /// <summary>
        /// Deserializes the XML file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static T DeserializeXmlFile<T>(this string filePath) where T : class
        {
            ApiLog.Logger.Info(ApiLog.LogPrefix.Start);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNodeReader xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);

            var xs = new DataContractSerializer(typeof(T));
            return xs.ReadObject(xmlReader, false) as T;
        }

        public static T DeserializeXmlFileWithXmlSerializer<T>(this string filePath) where T : class
        {
            ApiLog.Logger.Info(ApiLog.LogPrefix.Start);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNodeReader xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);

            var xs = new XmlSerializer(typeof(T));
            return xs.Deserialize(xmlReader) as T;
        }

        /// <summary>
        /// JSON serializer
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        public static void JsonSerializer(Stream stream, object obj)
        {
            /*Newtonsoft.Json.JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //settings.Converters.Add(new IsoDateTimeConverter());
            //settings.Converters.Add(new JavaScriptDateTimeConverter());
            string output = Newtonsoft.Json.JavaScriptConvert.SerializeObject(obj, Formatting.None, settings);

            MemoryStream ms = new MemoryStream();
            ms = new MemoryStream(System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(output));
            ms.Position = 0;
            stream.Position = 0;
            ms.WriteTo(stream);
            ms.Close();*/

            Newtonsoft.Json.JsonSerializer json = new JsonSerializer();
            json.NullValueHandling = NullValueHandling.Ignore;
            json.DefaultValueHandling = DefaultValueHandling.Ignore;
            json.ObjectCreationHandling = ObjectCreationHandling.Replace;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //json.TypeNameHandling = TypeNameHandling.Objects;  //Not Needed
            //TODO: Check, Dave (Decorate infrastructure with attributes and remove following line) 
            json.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            //Serialize Dates in ISO Format
            json.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());

            //json.Converters.Add(new JavaScriptDateTimeConverter());
            StringWriter sw = new StringWriter();
            Newtonsoft.Json.JsonTextWriter writer = new JsonTextWriter(sw);
            writer.QuoteChar = '"';
            json.Serialize(writer, obj);
            string output = sw.ToString();
            writer.Close();
            sw.Close();

            // Changing encoding to fix serialization issue 
            //MemoryStream ms = new MemoryStream(ASCIIEncoding.GetEncoding(0).GetBytes(output));
            MemoryStream ms = new MemoryStream(ASCIIEncoding.GetEncoding("utf-8").GetBytes(output));
            ms.Position = 0;
            stream.Position = 0;
            ms.WriteTo(stream);
            ms.Close();
        }
    }
}
