using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Reflection;



namespace DowJones.Managers.QueryUtility
{
    public static class GeneralUtils
    {
        private static string _serialize(object obj, Type type, bool omitDeclaration, bool enableIndenting)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(type);
            XmlWriterSettings xws = new XmlWriterSettings();
            //xws.Encoding = Encoding.Unicode;
            xws.OmitXmlDeclaration = omitDeclaration;
            xws.Indent = enableIndenting;
            using (XmlWriter xw = XmlWriter.Create(ms, xws))
            {
                xs.Serialize(xw, obj);
                return UTF8ByteArrayToString(ms.ToArray());
            }            
        }

        public static string serialize(object obj)
        {
            return _serialize(obj, obj.GetType(), false, false);
        }

        public static string serialize(object obj, Type type)
        {
            return _serialize(obj, type, false, false);
        }

        /// <summary>
        /// Returns the serialized xml of the given object. Also, can enable/diable the xml declaration and indenting.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="omitDeclaration"></param>
        /// <param name="enableIndenting"></param>
        /// <returns></returns>
        public static string serialize(object obj, bool omitDeclaration, bool enableIndenting)
        {
            return _serialize(obj, obj.GetType(), omitDeclaration, enableIndenting);
        }

        /// <summary>
        /// Returns the serialized xml of the given object. Also, can enable/diable the xml declaration and indenting.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="omitDeclaration"></param>
        /// <param name="enableIndenting"></param>
        /// <returns></returns>
        public static string serialize(object obj, Type type, bool omitDeclaration, bool enableIndenting)
        {
            return _serialize(obj, type, omitDeclaration, enableIndenting);
        }

        /// <summary>
        /// Serializes the with no XML declaration.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string SerializeWithNoXmlDeclaration(object obj)
        {
            return _serialize(obj, obj.GetType(), true, false);
        }

        /// <summary>
        /// Serializes the with no XML declaration.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string SerializeWithNoXmlDeclaration(object obj, Type type)
        {
            return _serialize(obj, type, true, false);
        }

        
            public static string FixXml(this string xmlStr)
            {
                if (xmlStr == null)
                    return null;

                var indexOfLt = xmlStr.IndexOf('<');
                return indexOfLt != 0 ? xmlStr.Substring(indexOfLt) : xmlStr;
            }
       

        public static object deSerialize(string xmlRequest, Type objectType)
        {
            using (StringReader sr = new StringReader(xmlRequest))
            {
                XmlTextReader xmlReader = new XmlTextReader(sr);
                XmlSerializer xs = new XmlSerializer(objectType);
                object obj = xs.Deserialize(xmlReader);
                return obj;
            }
        }

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        public static string formatDBParam(string data)
        {
            try
            {
                if (data != null)
                    return data.Replace("'", "''");
                else
                    return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int enumNameToValue(Enum enumName)
        {
            try
            {
                XmlDocument _xmlConfig = new XmlDocument();
                string strGetManagerConfigFile = System.Configuration.ConfigurationManager.AppSettings["ConfigApiManager"];
                _xmlConfig.Load(string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, strGetManagerConfigFile));
                string _xmlPath = string.Format(_xmlConfig.SelectSingleNode("//Workspace/EnumList/XmlPath[@name='NameToValue']").InnerText, enumName.GetType().Name, enumName);
                int _rtnValue = int.Parse(_xmlConfig.SelectSingleNode(_xmlPath).Attributes.GetNamedItem("value").Value);
                return _rtnValue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        #region XML utility functions

        /// <summary>
        /// Gets the name of the XML enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetXmlEnumName<T>(Enum value)
        {
            Type type = typeof(T);
            return GetXmlEnumName(type, value);
        }

        public static string GetXmlEnumName(Type type, Enum value)
        {
            FieldInfo fieldInfo = type.GetField(value.ToString());
            XmlEnumAttribute enumAttribute = (XmlEnumAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(XmlEnumAttribute));
            return enumAttribute != null ? enumAttribute.Name : value.ToString();
        }

        #endregion
    }
}