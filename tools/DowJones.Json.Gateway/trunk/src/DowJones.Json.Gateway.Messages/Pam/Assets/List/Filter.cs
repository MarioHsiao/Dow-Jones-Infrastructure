using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "Filter", Namespace = "")]
    [KnownType(typeof(IdSearchFilter))]
    public class Filter
    {
    }
}