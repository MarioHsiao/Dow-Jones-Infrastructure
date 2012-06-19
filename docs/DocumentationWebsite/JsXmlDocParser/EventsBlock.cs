using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JsXmlDocParser
{
	public class EventsBlock : AbstractBlock
	{
		public EventsBlock(string line):base(line)
		{
			_vsDocNameFormat = "E:{0}";
			BlockStarter = new EventsBlockStarter();
		}


		string _vsDocNameFormat;

		protected override string VsDocNameFormat
		{
			get { return _vsDocNameFormat; }
			set { _vsDocNameFormat = value; }
		}
	}
}