using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JsXmlDocParser
{
	public class JsParser
	{
		/// <summary>
		/// Parse a JS block.
		/// </summary>
		/// <param name="reader">Text Stream containing valid JavaScript</param>
		/// <returns>Parse Results as XML string</returns>
		public static string Parse(TextReader reader, string assemblyName = null)
		{
			IEnumerable<string> functions;

			using (reader)
			{
				functions = reader.ReadFunctionBlocks();
			}

			var sb = new StringBuilder();
			using (var memberInfoWriter = new MemberInfoWriter(sb))
			{
				if(!string.IsNullOrWhiteSpace(assemblyName)) 
					memberInfoWriter.WriteAssemblyName(assemblyName);

				foreach (var functionInfo in functions.Select(function => new FunctionInfo(function)))
				{
					memberInfoWriter.Write(functionInfo);
				}
			}

			return sb.ToString();
		}

	}
}