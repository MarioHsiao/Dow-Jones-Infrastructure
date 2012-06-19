using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace JsXmlDocParser
{
	public class ClassBlock : AbstractBlock
	{
		private string _vsDocNameFormat;
		protected override string VsDocNameFormat
		{
			get { return _vsDocNameFormat; }
			set { _vsDocNameFormat = value; }
		}

		public ClassBlock(string line) : base(line)
		{
			_vsDocNameFormat = "T:{0}";
			BlockStarter = new ClassBlockStarter();
			ParseLine(line);
		}

		private void ParseLine(string line)
		{
			var match = Regex.Match(line, BlockStarter.Pattern);
			Name = "AClass";
			NameSpace = "DJ.UI";
		}

		public override string ToString()
		{
			return string.Format("{0}.{1}", NameSpace, Name);
		}
		
		public string NameSpace { get; private set; }
		public string Name { get; private set; }
		
	}
}