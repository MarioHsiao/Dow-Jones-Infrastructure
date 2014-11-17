using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(DeleteItemsFromWorkspaceDTO.MyCustomConverter))]
    [JsonObject(Title = "DeleteItemsFromWorkspaceDTO")]
    [XmlType(TypeName = "DeleteItemsFromWorkspaceDTO", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof(DeleteItemsFromAutomaticWorkspaceDTO))]
    [KnownType(typeof(DeleteItemsFromAutomaticWorkspaceDTO))]
    public abstract class DeleteItemsFromWorkspaceDTO
    {
        [JsonProperty(PropertyName = "workspaceId")]
        [XmlElement(ElementName = "workspaceId", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "workspaceId")]
        public long __workspaceId;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __workspaceIdSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public long Id
        {
            get { return __workspaceId; }
            set { __workspaceId = value; __workspaceIdSpecified = true; }
        }

        private class MyCustomConverter : JsonCreationConverter<DeleteItemsFromWorkspaceDTO>
        {
            protected override DeleteItemsFromWorkspaceDTO Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("DeleteItemsFromAutomaticWorkspaceDTO".Equals(jObject.Value<string>("$type")))
                    return new DeleteItemsFromAutomaticWorkspaceDTO();
                else
                    return null;
            }
        }
    }
}
