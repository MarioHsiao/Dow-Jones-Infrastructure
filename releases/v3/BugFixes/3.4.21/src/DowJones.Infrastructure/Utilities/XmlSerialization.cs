using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DowJones.Utilities
{
    /// <summary>
    /// Serialization utility using XmlSerializer
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        public static string XmlSerialize(this object obj, Formatting formatting = Formatting.Indented)
        {
            // Serialization
            var serializer = new XmlSerializer(obj.GetType());
            var swBlob = new StringWriter();
            //create an xmlwriter and then write nothing to this to fake and remove xml decl
            var xw = new XmlTextWriter(swBlob) { Formatting = formatting };

            // serialize the request data.
            serializer.Serialize(xw, obj);
            return swBlob.ToString();
        }

        /// <summary>
        /// Serializes the with no XML declaration.
        /// </summary>
        public static string XmlSerializeWithNoXmlDeclaration(this object obj, Formatting formatting = Formatting.None)
        {
            var serializer = new XmlSerializer(obj.GetType());
            var swBlob = new StringWriter();
            //create an xmlwriter and then write nothing to this to fake and remove xml decl
            var xw = new XmlTextWriter(swBlob) { Formatting = formatting };

            xw.WriteRaw("");

            // serialize the request data.
            serializer.Serialize(xw, obj);
            return swBlob.ToString();
        }

        /// <summary>
        /// Deserializes the specified XML request.
        /// </summary>
        /// <param name="xmlRequest">The XML request.</param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(this string xmlRequest)
        {
            //System.Type objectType = null;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequest);
            var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);

            var xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(xmlReader);
        }
    }
}
