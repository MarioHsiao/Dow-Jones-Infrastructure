using System.Runtime.Serialization;

namespace DowJones.Pages
{
    public enum AccessQualifier
    {
        [EnumMember] User,
        [EnumMember] Account,
        [EnumMember] Global, 
    }
}