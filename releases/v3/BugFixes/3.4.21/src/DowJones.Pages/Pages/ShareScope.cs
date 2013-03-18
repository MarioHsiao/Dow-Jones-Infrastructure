using System.Runtime.Serialization;

namespace DowJones.Pages
{
	public enum ShareScope
	{
		[EnumMember] Everyone = 0,
		[EnumMember] Account = 2,
		[EnumMember] AccountAdmin = 1,
		[EnumMember] Personal = 3,
        [EnumMember] Group = 4
	}
}
