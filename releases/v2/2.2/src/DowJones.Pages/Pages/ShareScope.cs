using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DowJones.Pages
{
	public enum ShareScope
	{
		[EnumMember] Everyone = 0,
		[EnumMember] Account = 2,
		[EnumMember] AccountAdmin = 1,
		[EnumMember] Personal = 3,
	}
}
