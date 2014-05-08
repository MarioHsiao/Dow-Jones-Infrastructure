using System.Runtime.Serialization;

namespace DowJones.Pages
{
    /// <summary>
    /// The meta data type.
    /// </summary>
    public enum MetaDataType
    {
        [EnumMember] Industry,
        [EnumMember] Geographic,
        [EnumMember] Topic,
        [EnumMember] Custom,
        [EnumMember] SymbologyCode
    }
}