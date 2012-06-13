using System;
using System.Collections.Generic;

namespace JsXmlDocParser
{
	/// <summary>
	/// Hold Function description
	/// </summary>
	public class MemberDescription
	{
		public MemberDescription()
		{
			Parameters = new List<string>();
			InternalDescription = String.Empty;
			Name = String.Empty;
			Description = String.Empty;
		}

		public string Description { get; set; }

		public string Name { get; set; }

		public string InternalDescription { get; set; }

		public int StartingAt { get; set; }

		public int Complexity { get; set; }

		public IList<string> Parameters { get; set; }
	}
}