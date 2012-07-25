using System.Runtime.Serialization;

namespace VSDocPreprocessor
{
    [DataContract(Namespace="", Name = "parameter")]
    public class Parameter : DocumentEntity
    {
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        public Parameter() : base(null)
        {
        }
    }
}