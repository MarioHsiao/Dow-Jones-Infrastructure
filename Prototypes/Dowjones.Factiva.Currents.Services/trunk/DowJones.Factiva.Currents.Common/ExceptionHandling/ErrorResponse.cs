using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Xml.Serialization;
using DowJones.Globalization;
using DowJones.DependencyInjection;

namespace DowJones.API.Common.ExceptionHandling
{
    [DataContract(Name = "ErrorResponse", Namespace = "")]
    public class ErrorResponse : IServiceResponse
    {
        [DataMember(Name = "Error", EmitDefaultValue = false)]
        [JsonProperty("Error")]
        public Error Error;

        [DataMember(Name = "AuditLog", EmitDefaultValue = false)]
        [XmlElement(ElementName = "AuditLog")]
        [JsonProperty("AuditLog")]
        AuditLog IServiceResponse.AuditLog { get; set; }

        [DataMember(Name = "ArmValues", EmitDefaultValue = false)]
        [XmlElement(ElementName = "ArmValues")]
        [JsonProperty("ArmValues")]
        ARMValues IServiceResponse.ARMValues { get; set; }

        #region CSTR
        public ErrorResponse() { }

        public ErrorResponse(Error error)
        {
            Error = error ?? new Error();
        }
        #endregion
    }

    [DataContract(Name = "Error", Namespace = "")]
    public class Error
    {
        [DataMember(Name = "Code", EmitDefaultValue = false)]
        [JsonProperty("Code")]
        public long Code;

        [DataMember(Name = "Message", EmitDefaultValue = false)]
        [JsonProperty("Message")]
        public string Message;

        private IResourceTextManager resources = ServiceLocator.Resolve<IResourceTextManager>();

        #region CSTR
        public Error() { }

        public Error(long code, string message)
        {
            Code = code;
            Message = message;
        }

        public Error(long code)
        {
            Code = code;
            //Message = ResourceTextManager.Instance.GetErrorMessage(code.ToString());
            //Message = Resources.GetErrorMessage(code.ToString());
            resources = resources ?? ServiceLocator.Resolve<IResourceTextManager>();
            Message = resources.GetErrorMessage(code.ToString());
        }

        #endregion
    }
}
