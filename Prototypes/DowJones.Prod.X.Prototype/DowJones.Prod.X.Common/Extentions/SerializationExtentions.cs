using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DowJones.Extensions;

namespace DowJones.Prod.X.Common.Extentions
{
    public static class SerializationExtensions
    {
        public static TResponse DataContractConvert<TRequest, TResponse>(this TRequest value)
            where TResponse : class
            where TRequest : class
        {
            return DataContractDeserialize<TResponse>(SerializeWithNoXmlDeclaration(value));
        }

        public static string XmlSerialize<T>(this T value)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false, false),
                Indent = false,
                OmitXmlDeclaration = false
            };

            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        public static string SerializeWithNoXmlDeclaration<T>(this T value)
        {
            var serializer = new XmlSerializer(typeof(T));
            var swBlob = new StringWriter();
            var xw = new XmlTextWriter(swBlob)
            {
                Formatting = Formatting.None
            };

            xw.WriteRaw("");

            serializer.Serialize(xw, value);
            return swBlob.ToString();
        }

        public static string DataContractSerialize<T>(this T value)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false, false),
                Indent = false,
                OmitXmlDeclaration = false
            };

            var dcs = new DataContractSerializer(typeof(T));

            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    dcs.WriteObject(xmlWriter, value);
                }

                return textWriter.ToString();
            }
        }

        public static T XmlDeserialize<T>(this string str) where T : class
        {
            if (string.IsNullOrEmpty(str))
            {
                return default(T);
            }

            var serializer = new XmlSerializer(typeof(T));
            using (var xmlReader = XmlReader.Create(new MemoryStream(new UTF8Encoding().GetBytes(str))))
            {
                return (T)serializer.Deserialize(xmlReader);
            }
        }

        public static T DataContractDeserialize<T>(this string str) where T : class
        {
            if (str.IsNotEmpty())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(str);

                var dcs = new DataContractSerializer(typeof(T));
                if (xmlDoc.DocumentElement != null)
                {
                    using (var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement))
                    {
                        return dcs.ReadObject(xmlReader, false) as T;
                    }
                }
            }
            return default(T);
        }


        public static T ParseEnum<T>(this string value, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
            if (string.IsNullOrEmpty(value)) return defaultValue;

            T temp;
            return Enum.TryParse(value, out temp) ? temp : defaultValue;
        }

        public static T ParseEnum<T>(this Enum value, T defaultValue)
            where T : struct, IConvertible
        {
            return ParseEnum(value.ToString(), defaultValue);
        }
    }
}
