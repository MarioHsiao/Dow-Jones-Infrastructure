using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DowJones.Pages
{
	public enum ShareScope
	{
		[EnumMember] Everyone,
		[EnumMember] Account,
		[EnumMember] AccountAdmin,
		[EnumMember] Personal,
	}
}
