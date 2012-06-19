using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace JsXmlDocParser
{
	public class OptionsBlock : AbstractBlock
	{
		public OptionsBlock(string line) : base(line)
		{
			_vsDocNameFormat = "P:{0}";
			BlockStarter = new OptionsBlockStarter();
		}

		string _vsDocNameFormat;
		protected override string VsDocNameFormat
		{
			get { return _vsDocNameFormat; }
			set { _vsDocNameFormat = value; }
		}
	}
}