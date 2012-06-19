using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsXmlDocParser
{
	public class JavaScriptReaderAdapter
	{
		private StreamReader Underlying;
		private Queue<string> BufferedLines;

		public JavaScriptReaderAdapter(StreamReader underlying)
		{
			Underlying = underlying;
			BufferedLines = new Queue<string>();
		}

		public string PeekLine()
		{
			var line = ReadLine();
			if (line == null)
				return null;
			BufferedLines.Enqueue(line);
			return line;
		}

		public string ReadLine()
		{
			return BufferedLines.Count > 0 ? BufferedLines.Dequeue() : ReadCodeLine();
		}

		private string ReadCodeLine()
		{
			var line = Underlying.ReadLine();
			if (line != null && line.Contains("function"))
				if (line.Contains("{"))
					return line;
				else
				{
					var buffer = new StringBuilder(line);
					while (true)
					{
						var character = Underlying.Read();
						if (character == -1)
							break;

						buffer.Append((char)character);

						if ((char)character == '{')
							break;
					}

					return buffer.ToString();
				}

			return line;
		}

		public IEnumerable<string> ReadAllLines()
		{
			string line;
			var lines = new List<string>();
			while ((line = ReadCodeLine()) != null)
				lines.Add(line);

			return lines;
		}
	}
}