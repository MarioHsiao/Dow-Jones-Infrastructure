using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonConverter(typeof(AddItemsToWorkspaceDTO.MyCustomConverter))]
    [JsonObject(Title = "AddItemsToWorkspaceDTO")]
    [XmlType(TypeName = "AddItemsToWorkspaceDTO", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof(AddItemsToAutomaticWorkspaceDTO))]
    [XmlInclude(typeof(AddItemsToManualWorkspaceDTO))]
    [KnownType(typeof(AddItemsToAutomaticWorkspaceDTO))]
    [KnownType(typeof(AddItemsToManualWorkspaceDTO))]
    public abstract class AddItemsToWorkspaceDTO
    {
        [JsonProperty(PropertyName = "id")]
        [XmlElement(ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "id")]
        public long __id;

        [JsonIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __idSpecified;

        [JsonIgnore]
        [IgnoreDataMember]
        public long Id
        {
            get { return __id; }
            set { __id = value; __idSpecified = true; }
        }

        private class MyCustomConverter : JsonCreationConverter<AddItemsToWorkspaceDTO>
        {
            protected override AddItemsToWorkspaceDTO Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("AddItemsToAutomaticWorkspaceDTO".Equals(jObject.Value<string>("$type")))
                    return new AddItemsToAutomaticWorkspaceDTO();
                else if ("AddItemsToManualWorkspaceDTO".Equals(jObject.Value<string>("$type")))
                    return new AddItemsToManualWorkspaceDTO();
                else
                    return null;
            }
        }
    }
}
