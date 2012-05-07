using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    [DataContract(Name = "medium", Namespace = "")]
    public enum Medium
    {
        Audio,
        Video,
    }
}