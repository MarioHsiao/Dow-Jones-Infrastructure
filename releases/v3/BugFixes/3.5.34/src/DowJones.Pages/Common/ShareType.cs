using System.Runtime.Serialization;

namespace DowJones.Pages
{
    public enum ShareType
    {
        [EnumMember] OwnedByUser,
        [EnumMember] AssignedToUser,
        [EnumMember] SubscribedByUser,
        [EnumMember] UnSpecified,
    }
}