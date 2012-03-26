using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using log4net;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    public class SerializationUtility
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SerializationUtility));

        public static string SerializeObjectToStream(object obj)
        {
            using (var stream = new MemoryStream())
            {
                // Data Contract Serialization
                new DataContractSerializer(obj.GetType()).WriteObject(stream, obj);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    if (!Logger.IsDebugEnabled)
                    {
                        return "";
                    }

                    var text = reader.ReadToEnd();
                    Logger.Debug(text);
                    return text;
                }
            }
        }
        
        public static string SerializeObjectToStream<T>(T obj, string format)
        {
            using (var stream = new MemoryStream())
            {
                // Data Contract Serialization
                switch (format.ToUpper())
                {
                    case "JSON":
                        new DataContractJsonSerializer(obj.GetType()).WriteObject(stream, obj);
                        break;
                    case "XML":
                        new DataContractSerializer(obj.GetType()).WriteObject(stream, obj);
                        break;
                }

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    if (!Logger.IsDebugEnabled)
                    {
                        return "";
                    }

                    var text = reader.ReadToEnd();
                    Logger.Debug(text);
                    return text;
                }
            }
        }
     
    }
}
