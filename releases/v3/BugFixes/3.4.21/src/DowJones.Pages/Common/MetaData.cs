using System.Runtime.Serialization;

namespace DowJones.Pages
{
    /// <summary>
    /// The meta data.
    /// </summary>
    [DataContract(Name = "metaData", Namespace = "")]
    public class MetaData
    {
        [DataMember(Name = "metaDataType")]
        public MetaDataType MetaDataType { get; set; }

        [DataMember(Name = "metaDataCode")]
        public string MetaDataCode { get; set; }

        [DataMember(Name = "metaDataDescriptor")]
        public string MetaDataDescriptor { get; set; }

        [DataMember(Name = "metaDataDescription")]
        public string MetaDataDescription { get; set; }
    }
}