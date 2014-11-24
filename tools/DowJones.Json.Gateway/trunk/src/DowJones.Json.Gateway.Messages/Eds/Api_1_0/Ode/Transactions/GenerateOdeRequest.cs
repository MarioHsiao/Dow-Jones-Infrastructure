using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode.Transactions
{
    [ServicePath("1.0/ODE")]
    [DataContract(Name = "GenerateODE", Namespace = "")]
    [JsonObject(Title = "GenerateODE")]
    [XmlRoot(ElementName = "GenerateODE", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    public class GenerateOdeRequest : IPostJsonRestRequest
    {
        [JsonProperty(PropertyName = "Ticket")] 
        [XmlElement(Type = typeof (Ticket), ElementName = "Ticket", IsNullable = false, Form = XmlSchemaForm.Qualified)] 
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "Ticket", IsRequired = true)]
        public TicketCollection __TicketCollection;
        
        [JsonProperty(PropertyName = "RequestMode")] 
        [XmlAttribute(AttributeName = "RequestMode", Form = XmlSchemaForm.Unqualified)] 
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "RequestMode", IsRequired = true)]
        public RequestMode __requestMode;

        [JsonIgnore]
        [XmlIgnore]
        public RequestMode requestMode
        {
            get { return __requestMode; }
            set { __requestMode = value; }
        }


        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public TicketCollection TicketCollection
        {
            get
            {
                if (__TicketCollection == null) __TicketCollection = new TicketCollection();
                return __TicketCollection;
            }
            set { __TicketCollection = value; }
        }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}