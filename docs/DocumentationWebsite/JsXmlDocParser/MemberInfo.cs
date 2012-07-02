using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JsXmlDocParser
{
	public class MemberInfo
	{
	    protected internal const string Anonymous = "anonymous";

	    public IEnumerable<string> Lines { get; set; }

		public string Name { get; set; }

		public bool IsAnonymous
		{
			get { return Name.Equals(Anonymous); }
		}

		IEnumerable<string> _params;
		public IEnumerable<string> Params
		{
			get { return _params ?? (_params = ExtractParams(Lines)); }
		}

		string _signature;
		public string Signature
		{
			get
			{
				if (_signature == null)
					_signature = string.Format("{0}({1})", Name, string.Join(", ", Params));
				return _signature;
			}
		}

		string _docComments;
		public string DocComments
		{
			get
			{
				if (_docComments == null)
					_docComments = ExtractDocComments();
				return _docComments;
			}
		}

		private string ExtractDocComments()
		{
			return string.Join(Environment.NewLine, Lines.Skip(1).TakeWhile(l => l.Trim().StartsWith("///")).Select(l=>l.Substring(3)));
		}

		public MemberInfo(string block)
		{
			Lines = block.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			if (!Regex.IsMatch(Lines.First(), @"function\s*\(|function\s*(\w+)\s*\("))
				throw new ArgumentException("Given line is not a function definition", "block");

			Name = ExtractName(Lines.First());
		}

		private static string ExtractName(string line)
		{
			var patterns = new[] {	@"(\w+)\s*(?::|=)\s*function\s*\(", 
									@"function\s*(\w+)\s*\("};

			foreach (var pattern in patterns)
			{
				var m = Regex.Match(line, pattern);
				if (m.Success && m.Captures.Count > 0)
				{
					return m.Groups[1].Value;
				}
			}
			return Anonymous;
		}

		private static IEnumerable<string> ExtractParams(IEnumerable<string> lines)
		{
			var matches = Regex.Match(string.Concat(lines), @"\((?:(\w+\s*),?\s*)*\)");
			if (matches.Success && matches.Groups.Count > 0)
			{
				return matches.Groups[1].Captures.Cast<Capture>().Select(c => c.Value);
			}

			return Enumerable.Empty<string>();
		}



	}
}
