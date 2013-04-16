using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Components.NewsRadar
{
    [DataContract(Name = "ownershipType")]
    public enum OwnershipType
    {
        Private,
        Public
    }
}
