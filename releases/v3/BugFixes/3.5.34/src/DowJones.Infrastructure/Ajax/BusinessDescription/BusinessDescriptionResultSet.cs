using System.Collections.Generic;
using Factiva.Gateway.Messages.FCE.Assets.V1_0;
using System.Xml.Serialization;

namespace DowJones.Ajax.BusinessDescription
{
    public class BusinessDescriptionResultSet
    {
        [XmlElement("paragraph")]
        public string[] Paragraphs;

        [XmlElement("assetLink")]
        public List<AssetLink> AssetLinks;
    }

    public class AssetLink
    {
        public AssetReferenceType AssetType;
        public string Reference;
        public string FCode;
        public string ProviderCode;
        public string Text;
        public string Href;
        public string Data;
    }
}
