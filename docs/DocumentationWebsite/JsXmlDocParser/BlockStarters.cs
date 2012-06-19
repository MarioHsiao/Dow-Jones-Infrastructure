using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace JsXmlDocParser
{
	public class ClassBlockStarter : IBlockStarter
	{
		const string PatternRegex = @"([\w.]+)\s*=\s*([\w.]+).extend\s*\(\s*{";

		#region IBlockStarter Members

		public string Pattern { get { return PatternRegex; } }
		public PatternType PatternType { get { return PatternType.Class; } }

		#endregion

		public bool IsMatch(string line)
		{
			return Regex.IsMatch(line.Trim(), PatternRegex);
		}
	}

	public class FunctionBlockStarter : IBlockStarter
	{
		private const string PatternRegex = @"(?:([a-zA-Z0-9.]+)\s*(?::|=)\s*function|function\s*([a-zA-Z0-9]+))\s*\((?:(\w+\s*),?\s*)*\)\s*{";

		#region IBlockStarter Members

		public string Pattern { get { return PatternRegex; } }
		public PatternType PatternType { get { return PatternType.Function; } }

		#endregion

		public bool IsMatch(string line)
		{
			return Regex.IsMatch(line.Trim(), PatternRegex);
		}

	}

	public class EventsBlockStarter : IBlockStarter
	{
		private const string PatternRegex = @"events\s*:\s*{";

		#region IBlockStarter Members

		public string Pattern { get { return PatternRegex; } }
		public PatternType PatternType { get { return PatternType.Events; } }

		#endregion

		public bool IsMatch(string line)
		{
			return Regex.IsMatch(line.Trim(), PatternRegex);
		}

	}

	public class OptionsBlockStarter : IBlockStarter
	{
		private const string PatternRegex = @"options\s*:\s*{";

		#region IBlockStarter Members

		public string Pattern { get { return PatternRegex; } }
		public PatternType PatternType { get { return PatternType.Options; } }

		#endregion

		public bool IsMatch(string line)
		{
			return Regex.IsMatch(line.Trim(), PatternRegex);
		}

	}

	public class UnknownBlockStarter : IBlockStarter
	{
		private const string PatternRegex = @"{";

		#region IBlockStarter Members

		public string Pattern { get { return PatternRegex; } }
		public PatternType PatternType { get { return PatternType.Unknown; } }

		#endregion

		public bool IsMatch(string line)
		{
			return Regex.IsMatch(line.Trim(), PatternRegex);
		}
	}
}