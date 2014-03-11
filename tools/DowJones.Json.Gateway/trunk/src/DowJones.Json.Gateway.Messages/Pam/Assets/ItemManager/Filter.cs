using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [XmlInclude(typeof(UserFileSearchFilter))]
    [DataContract(Name = "Filter")]
    public abstract class Filter
    {
    }
}