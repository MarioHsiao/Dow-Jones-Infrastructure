using System.Runtime.Serialization;

namespace DowJones
{
    [DataContract]
    public class NameValuePair
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
