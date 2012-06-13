using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JsXmlDocParser
{
	public static class TextReaderExtensions
	{
		public static List<string> ReadFunctionBlocks(this TextReader tr)
		{
			return tr.ReadFunctionBlocks(null);
		}

		private static List<string> ReadFunctionBlocks(this TextReader tr, string startLine)
		{
			var functionBuilder = new StringBuilder();
			string line;
			var functionStarted = false;
			var openBraces = 0;
			var functionBlocks = new List<string>();

			if (!string.IsNullOrEmpty(startLine))
			{
				functionStarted = true;
				openBraces = startLine.CountBraces();
				functionBuilder.AppendLine(startLine);
			}

			while ((line = tr.ReadLine()) != null)
			{
				line = line.Trim();

				if (line.ContainsFunction())
				{
					if (functionStarted)
					{
						// function within function, read inner function first
						functionBlocks.AddRange(ReadFunctionBlocks(tr, line));
						continue;
					}
					
					functionStarted = line.Contains("{");
				}
				else if(!functionStarted) 
					continue;	// skip all comments, variable declaration at the beginning of file
				

				if (!string.IsNullOrEmpty(line))
					functionBuilder.AppendLine(line);

				if (!line.IsCommentLine())
				{
					openBraces += line.CountBraces();

					if (functionStarted && openBraces == 0)
					{
						functionBlocks.Add(functionBuilder.ToString());
						break;
					}
				}

			}

			return functionBlocks;
		}
	}

	public static class StringExtensions
	{
		public static bool ContainsFunction(this string line)
		{
			const string function = @"function(\s+\w+\s*|\s*)\(";
			return Regex.IsMatch(line, function);
		}

		public static int Occurs(this string haystack, char needle)
		{
			var count = 0;
			for (var index = 0; index < haystack.Length; index++)
			{
				if (haystack[index] == needle)
					count++;
			}

			return count;
		}

		public static int CountBraces(this string input)
		{
			return input.Occurs('{') - input.Occurs('}');
		}

		public static bool IsCommentLine(this string line)
		{
			return line.TrimStart().StartsWith("//");
		}
	}
}
