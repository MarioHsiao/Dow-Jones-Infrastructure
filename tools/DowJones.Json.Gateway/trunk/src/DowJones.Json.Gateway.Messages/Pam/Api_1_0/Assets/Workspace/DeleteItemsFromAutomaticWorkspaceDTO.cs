using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "DeleteItemsFromAutomaticWorkspaceDTO")]
    [XmlType(TypeName = "DeleteItemsFromAutomaticWorkspaceDTO", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class DeleteItemsFromAutomaticWorkspaceDTO : DeleteItemsFromWorkspaceDTO
    {
        [JsonProperty(PropertyName = "itemId")]
        [XmlElement(Type = typeof(long), ElementName = "itemId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "itemId")]
        public ItemIdCollection __itemIdCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ItemIdCollection ItemIdCollection
        {
            get
            {
                if (__itemIdCollection == null) __itemIdCollection = new ItemIdCollection();
                return __itemIdCollection;
            }
            set { __itemIdCollection = value; }
        }
    }
}
