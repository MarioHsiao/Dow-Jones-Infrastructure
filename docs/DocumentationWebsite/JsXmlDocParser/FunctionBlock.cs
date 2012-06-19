using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace JsXmlDocParser
{
	public class FunctionBlock : AbstractBlock
	{
		public string NameSpace { get; private set; }
		public string Name { get; private set; }
		public string Signature { get; private set; }
		public IEnumerable<string> Parameters { get; private set; }

		string _vsDocNameFormat;
		protected override string VsDocNameFormat
		{
			get { return _vsDocNameFormat; }
			set { _vsDocNameFormat = value; }
		}

		public FunctionBlock(string line):base(line)
		{
			_vsDocNameFormat = "M:{0}";
			BlockStarter = new FunctionBlockStarter();
			ParseDeclaration(line);
		}

		private void ParseDeclaration(string line)
		{
			var match = Regex.Match(line, BlockStarter.Pattern);
			Name = ExtractName(match);
			NameSpace = ExtractNameSpace(match);
			Parameters = ExtractParamemters(match);
			Signature = CreateSignature(Name, Parameters);
		}

		private IEnumerable<string> ExtractParamemters(Match match)
		{
			return Enumerable.Empty<string>();
		}

		private string CreateSignature(string name, IEnumerable<string> parameters)
		{
			return name + "()";
		}

		private string ExtractNameSpace(Match match)
		{
			return "DJ.UI";
		}

		private string ExtractName(Match match)
		{
			return "PortalHeadlineList";
		}

		public override string ToString()
		{
			return string.Format("{0}.{1}", NameSpace, Signature);
		}

	}
}