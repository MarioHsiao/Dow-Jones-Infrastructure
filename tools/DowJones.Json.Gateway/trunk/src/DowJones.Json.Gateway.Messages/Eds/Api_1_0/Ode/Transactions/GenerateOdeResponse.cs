using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode.Transactions
{
    [JsonObject(Title = "GenerateODEResponse")]
    [XmlRoot(ElementName = "GenerateODEResponse", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    public class GenerateOdeResponse : IJsonRestResponse
    {

        [JsonIgnore]
        [XmlElement(ElementName = "Status", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int __Status;

        // [JsonProperty]
        [JsonIgnore]
        [XmlIgnore]
        public int Status
        {
            get
            {
                return __Status;
            }
            set { __Status = value; }
        }

        [JsonIgnore]
        [XmlElement(ElementName = "DeliveryNumber", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __DeliveryNumber;

        [JsonProperty]
        [XmlIgnore]
        public string DeliveryNumber
        {
            get { return __DeliveryNumber; }
            set { __DeliveryNumber = value; }
        }

        [JsonIgnore]
        [XmlElement(Type = typeof(ToEmailAddress), ElementName = "ToEmailAddress", IsNullable = false, Form = XmlSchemaForm.Qualified)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ToEmailAddress __ToEmailAddress;

        [JsonIgnore]
        [XmlIgnore]
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