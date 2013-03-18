using System.Runtime.Serialization;

namespace DowJones.Pages
{
    public enum PublishStatusScope
    {
        [EnumMember] Personal,
        [EnumMember] Account,
        [EnumMember] Global,
    }
}