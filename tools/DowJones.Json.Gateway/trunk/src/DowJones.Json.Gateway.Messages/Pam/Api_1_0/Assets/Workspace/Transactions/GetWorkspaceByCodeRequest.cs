using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace.Transactions
{
    [ServicePath("2.0/Workspace")]
    [DataContract(Name = "GetWorkspaceByCode", Namespace = "")]
    [JsonObject(Title = "GetWorkspaceByCode")]
    public class GetWorkspaceByCodeRequest : IPostJsonRestRequest
    {
        [JsonProperty(PropertyName = "type")] 
        [XmlElement(ElementName = "type", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "type")]
        public WorkspaceType __type;

        [JsonIgnore]
        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __typeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        public WorkspaceType Type
        {
            get { return __type; }
            set { __type = value; __typeSpecified = true; }
        }

        [JsonProperty(PropertyName = "code")] 
        [XmlElement(Type = typeof(string), ElementName = "code", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "code")]
        public CodeCollection __codeCollection;

        [JsonIgnore]
        [XmlIgnore]
        public CodeCollection CodeCollection
        {
            get
            {
                if (__codeCollection == null) __codeCollection = new CodeCollection();
                return __codeCollection;
            }
            set { __codeCollection = value; }
        }

        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}
