using System.Runtime.Serialization;

namespace DowJones.Pages.Multimedia
{
    [DataContract(Name = "mustPlayFromSource", Namespace = "")]
    public class MustPlayFromSource
    {

        [DataMember(Name = "status")]
        public bool Status;

        [DataMember(Name = "url")]
        public string Url;
    }
}