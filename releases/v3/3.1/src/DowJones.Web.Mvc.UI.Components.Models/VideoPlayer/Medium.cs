using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Components.VideoPlayer
{
    [DataContract(Name = "medium", Namespace = "")]
    public enum Medium
    {
        Audio,
        Video,
    }
}