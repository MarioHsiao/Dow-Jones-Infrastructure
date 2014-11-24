using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "IdSearchFilter")]
    [XmlType(TypeName = "IdSearchFilter", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class IdSearchFilter : Filter
    {
        [JsonProperty(PropertyName = "id")]
        [XmlElement(Type = typeof(string), ElementName = "id", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "id")]
        public IdCollection __idCollection;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public IdCollection IdCollection
        {
            get
            {
                if (__idCollection == null) __idCollection = new IdCollection();
                return __idCollection;
            }
            set { __idCollection = value; }
        }
    }
}
