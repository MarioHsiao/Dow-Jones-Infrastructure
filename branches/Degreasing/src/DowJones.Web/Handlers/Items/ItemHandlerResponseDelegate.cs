using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace DowJones.Web.Handlers.Items
{
    [Serializable]
    public class ItemHandlerResponseDelegate : AbstractAjaxResponseDelegate
    {
        private long assetId;
        private string assetName;
        private FileContent fileContent = new FileContent();
        private string lastModifiedDate;

        [XmlElement("AssetId")]
        public long AssetId
        {
            get
            {
                return assetId;
            }
            set
            {
                assetId = value;
            }
        }

        [XmlElement("AssetName")]
        public string AssetName
        {
            get
            {
                return assetName;
            }
            set
            {
                assetName = value;
            }
        }

        [XmlElement("LastModifiedDate")]
        public string LastModifiedDate
        {
            get
            {
                return lastModifiedDate;
            }
            set
            {
                lastModifiedDate = value;
            }
        }

        [XmlElement("FileContent")]
        public FileContent FileContent
        {
            get
            {
                return fileContent;
            }
            set
            {
                fileContent = value;
            }
        }
     
        
        private string UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }


        public string ToXml()
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemHandlerResponseDelegate));
            xmlSerializer.Serialize(memoryStream, this);
            return UTF8ByteArrayToString(memoryStream.ToArray());
        }
        public string ToJson()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }
    [Serializable]
    public class FileContent
    {
        private string fileBinary;
        private string imageMimeType;

        [XmlElement("FileBinary")]
        public string FileBinary
        {
            get
            {
                return fileBinary;
            }
            set
            {
                fileBinary = value;
            }
        }
        [XmlElement("ImageMimeType")]
        public string ImageMimeType
        {
            get
            {
                return imageMimeType;
            }
            set
            {
                imageMimeType = value;
            }
        }

    }
}
