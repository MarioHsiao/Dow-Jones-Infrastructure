using System.Collections.Generic;

namespace JsXmlDocParser
{
	public interface IMemberInfo
	{
		MemberType MemberType { get; }
		IEnumerable<string> Lines { get; set; }
		string Name { get; set; }
	}
}