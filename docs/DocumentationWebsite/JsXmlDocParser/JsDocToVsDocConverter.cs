using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace JsXmlDocParser
{
	public class JsDocToVsDocConverter
	{
		private readonly JsParser _parser;

		/// <summary>
		/// Indicates whether the converter should continue processing
		/// after encountering an error with one JsDoc, or abort the entire process
		/// </summary>
		public bool ContinueOnError { get; set; }

		public JsDocToVsDocConverter(JsParser parser = null)
		{
			_parser = parser ?? new JsParser();
			ContinueOnError = true;
		}

		public void Convert(Assembly assembly, XmlWriter output)
		{
			var resources =
				from resourceName in assembly.GetManifestResourceNames()
				where resourceName.EndsWith(".js")
				select assembly.GetManifestResourceStream(resourceName);

			Convert(resources, output, assembly.GetName().Name);
		}

		public void Convert(IEnumerable<string> filenames, XmlWriter output, string assemblyName = null)
		{
			var fileStreams =
				filenames
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Where(File.Exists)
					.Select(File.OpenRead);

			Convert(fileStreams, output, assemblyName);
		}

		public void Convert(IEnumerable<Stream> jsDocSources, XmlWriter output, string assemblyName = null, bool closeStream = true)
		{
			var writer = new MemberInfoWriter(output);

			writer.WriteStartDocument();

			if (!string.IsNullOrWhiteSpace(assemblyName))
				writer.WriteAssemblyName(assemblyName);

			foreach (var reader in jsDocSources.Select(x => new StreamReader(x)))
			{
				try
				{
					_parser.Parse(reader);
					writer.EnsureMembersElementOpened();
					foreach (var result in _parser.ParseResults)
					{
						result.ToVsDocXml(output);
						foreach (var child in result.Children)
						{
							output.WriteString(Environment.NewLine);
							child.ToVsDocXml(output);
							output.WriteString(Environment.NewLine);
						}
					}
				}
				catch (Exception)
				{
					if (ContinueOnError)
					{
						Trace.TraceWarning("Error parsing stream; continuing to next stream.");
					}
					else
					{
						throw;
					}
				}
				finally
				{
					if (closeStream)
						reader.Close();
				}
			}

			writer.WriteEndDocument();
		}
	}
}
