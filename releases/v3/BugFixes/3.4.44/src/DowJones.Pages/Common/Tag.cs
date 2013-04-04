using System.Runtime.Serialization;

namespace DowJones.Pages
{
    /// <summary>
    /// Represents a single tag in the cloud
    /// </summary>
    [DataContract(Name = "tag", Namespace = "")]
    [KnownType(typeof(Tag))]
    public class Tag : AbstractTag
    {
    }
}