using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.API.Common
{
    [DataContract(Name = "AuditLog", Namespace = "")]
    [XmlRootAttribute(ElementName = "AuditLog")]
    public class AuditLog
    {
        public AuditLog()
        {
            AuditTransactionTrace = new List<AuditTransaction>();
        }

        //[DataMember(Name = "AuditTransaction", EmitDefaultValue = false)]
        //[XmlElement(ElementName = "AuditTransaction")]
        //[JsonProperty("AuditTransaction")]
        //public AuditTransaction AuditTransaction;

        [DataMember(Name = "transactions", EmitDefaultValue = false)]
        [XmlArray(ElementName = "transactions")]
        [XmlArrayItem(ElementName = "transaction")]
        [JsonProperty("transactions")]
        public List<AuditTransaction> AuditTransactionTrace;


    }

    [DataContract(Name = "ARM", Namespace = "")]
    [XmlRootAttribute(ElementName = "ARM")]
    public class ARMValues
    {
        [DataMember(Name = "RemainingPercentage", EmitDefaultValue = false)]
        [XmlElement(ElementName = "RemainingPercentage")]
        [JsonProperty("RemainingPercentage")]
        public string ArmRemainingPercentage;

        [DataMember(Name = "RemainingTime", EmitDefaultValue = false)]
        [XmlElement(ElementName = "RemainingTime")]
        [JsonProperty("RemainingTime")]
        public string ArmRemainingTime;
    }

    [DataContract(Name = "AuditTransaction", Namespace = "")]
    [XmlRootAttribute(ElementName = "AuditTransaction")]
    public class AuditTransaction
    {
        [DataMember(Name = "Name", EmitDefaultValue = false)]
        [XmlElement(ElementName = "Name")]
        [JsonProperty("Name")]
        public string Name;

        [DataMember(Name = "Type", EmitDefaultValue = false)]
        [XmlElement(ElementName = "Type")]
        [JsonProperty("Type")]
        public ApiTransactionType Type;

        [DataMember(Name = "ReturnCode", EmitDefaultValue = false)]
        [XmlElement(ElementName = "ReturnCode")]
        [JsonProperty("ReturnCode")]
        public long? ReturnCode;

        [DataMember(Name = "RequestDateTime", EmitDefaultValue = false)]
        [XmlElement(ElementName = "RequestDateTime")]
        [JsonProperty("RequestDateTime")]
        public DateTime? RequestDateTime;

        [DataMember(Name = "RawRequest", EmitDefaultValue = false)]
        [XmlElement(ElementName = "RawRequest")]
        [JsonProperty("RawRequest")]
        public string RawRequest;

        [DataMember(Name = "ResponseDateTime", EmitDefaultValue = false)]
        [XmlElement(ElementName = "ResponseDateTime")]
        [JsonProperty("ResponseDateTime")]
        public DateTime? ResponseDateTime;

        [DataMember(Name = "RawResponse", EmitDefaultValue = false)]
        [XmlElement(ElementName = "RawResponse")]
        [JsonProperty("RawResponse")]
        public string RawResponse;

        [DataMember(Name = "ElapsedTime", EmitDefaultValue = false)]
        [XmlElement(ElementName = "ElapsedTime")]
        [JsonProperty("ElapsedTime")]
        public int? ElapsedTime;

        [DataMember(Name = "ControlData", EmitDefaultValue = false)]
        [XmlElement(ElementName = "ControlData")]
        [JsonProperty("ControlData")]
        public string ControlData;

        [DataMember(Name = "Details", EmitDefaultValue = false)]
        [XmlElement(ElementName = "Details")]
        [JsonProperty("Details")]
        public string Details;
    }

    [DataContract(Name = "TransactionType", Namespace = "")]
    [XmlRootAttribute(ElementName = "TransactionType")]
    public enum ApiTransactionType
    {
        General,
        Login,
        Logout
    }
}
