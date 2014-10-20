using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode.Transactions
{
    [JsonObject(Title = "GenerateODEResponse")]
    [XmlRoot(ElementName = "GenerateODEResponse", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    [DataContract(Name = "GenerateODEResponse", Namespace = "")]
    public class GenerateOdeResponse : IJsonRestResponse
    {

        [JsonProperty(PropertyName = "Status")]
        [XmlElement(ElementName = "Status", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "Status")]
        public int __Status;

        // [JsonProperty]
        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public int Status
        {
            get
            {
                return __Status;
            }
            set { __Status = value; }
        }

        [JsonProperty(PropertyName = "DeliveryNumber")]
        [XmlElement(ElementName = "DeliveryNumber", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "DeliveryNumber")]
        public string __DeliveryNumber;

        [JsonProperty]
        [XmlIgnore]
        [IgnoreDataMember]
        public string DeliveryNumber
        {
            get { return __DeliveryNumber; }
            set { __DeliveryNumber = value; }
        }

        [JsonProperty(PropertyName = "ToEmailAddress")]
        [XmlElement(Type = typeof(ToEmailAddress), ElementName = "ToEmailAddress", IsNullable = false, Form = XmlSchemaForm.Qualified)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "ToEmailAddress")]
        public ToEmailAddress __ToEmailAddress;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ToEmailAddress ToEmailAddress
        {
            get
            {
                if (__ToEmailAddress == null) __ToEmailAddress = new ToEmailAddress();
                return __ToEmailAddress;
            }
            set { __ToEmailAddress = value; }
        }

        private bool _canDeserialize;
      
        public GenerateOdeResponse()
        {
        }
    }
}