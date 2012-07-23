using System.Runtime.Serialization;

namespace DowJones.Models.Company
{
    [DataContract(Name = "provider", Namespace = "")]
    public class Provider
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name = "externalUrl")]
        public string ExternalUrl { get; set; }
    }
}