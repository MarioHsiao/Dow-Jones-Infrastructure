using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using DowJones.Tools.Ajax;

namespace DowJones.Utilities.Handlers.DJInsider
{
    [Serializable]
    public class DJInsiderResponseDelegate : AbstractAjaxResponseDelegate
    {
        private string statusCode;
        private int djiStatus;
        private string eMailId;
        private string djiPrefItemStatus;

        [XmlElement("StatusCode")]
        public string StatusCode
        {
            get
            {
                return statusCode;
            }
            set
            {
                statusCode = value;
            }
        }
        [XmlElement("DJIStatus")]
        public int DJIStatus
        {
            get
            {
                return djiStatus;
            }
            set
            {
                djiStatus = value;
            }
        }
        [XmlElement("DJIPrefItemStatus")]
        public string DJIPrefItemStatus
        {
            get
            {
                return djiPrefItemStatus;
            }
            set
            {
                djiPrefItemStatus = value;
            }
        }

        [XmlElement("EMailID")]
        public string EMailID
        {
            get
            {
                return eMailId;
            }
            set
            {
                eMailId = value;
            }
        }
         public string ToJson()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
         public string ToXml()
         {
             MemoryStream memoryStream = new MemoryStream();
             XmlSerializer xmlSerializer = new XmlSerializer(typeof(DJInsiderResponseDelegate));
             xmlSerializer.Serialize(memoryStream, this);
             return UTF8ByteArrayToString(memoryStream.ToArray());
         }
         private string UTF8ByteArrayToString(Byte[] characters)
         {
             UTF8Encoding encoding = new UTF8Encoding();
             string constructedString = encoding.GetString(characters);
             return (constructedString);
         }
    }
}
