 using System;
using System.IO;
 using System.Runtime.Serialization;
 using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DowJones.Utilities.Managers.Core
{
    public class XmlUtilitiesManager
    {
        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            // Serialization
            var serializer = new XmlSerializer(obj.GetType());
            var swBlob = new StringWriter();
            //create an xmlwriter and then write nothing to this to fake and remove xml decl
            var xw = new XmlTextWriter(swBlob)
                         {
                             Formatting = Formatting.None
                         };

            // serialize the request data.
            serializer.Serialize(xw, obj);
            return swBlob.ToString();
        }


        /// <summary>
        /// Serializes the with no XML declaration.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string SerializeWithNoXmlDeclaration(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            var swBlob = new StringWriter();
            //create an xmlwriter and then write nothing to this to fake and remove xml decl
            var xw = new XmlTextWriter(swBlob)
                         {
                             Formatting = Formatting.None
                         };

            xw.WriteRaw("");
            // serialize the request data.
            serializer.Serialize(xw, obj);
            return swBlob.ToString();
        }

        /// <summary>
        /// Deserializes the specified XML request.
        /// </summary>
        /// <param name="xmlRequest">The XML request.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public static object Deserialize(string xmlRequest, Type objectType)
        {
            //System.Type objectType = null;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequest);
            if (xmlDoc.DocumentElement != null)
            {
                var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);

                var xs = new XmlSerializer(objectType);
                var obj = xs.Deserialize(xmlReader);
                return obj;
            }
            return null;
        }

        /// <summary>
        /// Deserializes the specified XML request.
        /// </summary>
        /// <param name="xmlRequest">The XML request.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            //System.Type objectType = null;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            if (xmlDoc.DocumentElement == null)
            {
                throw new SerializationException("Unable to deserialize xml");
            }
            var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);
            var xs = new XmlSerializer(typeof (T));
            var obj = xs.Deserialize(xmlReader);
            return (T) obj;
        }
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        public static String UTF8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        public static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}
