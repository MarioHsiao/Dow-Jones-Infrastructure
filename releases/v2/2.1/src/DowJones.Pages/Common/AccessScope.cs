using System.Runtime.Serialization;

namespace DowJones.Pages
{
    public enum AccessScope
    {
        [EnumMember] OwnedByUser,
        [EnumMember] AssignedToUser,
        [EnumMember] SubscribedByUser,
        [EnumMember] UnSpecified,
    }
}