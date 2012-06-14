using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using JsXmlDocParser;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DowJones.Documentation.BuildTasks
{
	public class JsVsDocGenerator : Task
	{
		[Required]
		public string ProjectDir { get; set; }

		[Required]
		public string OutFile { get; set; }

		#region Task Members


		public override bool Execute()
		{
			return ProcessDirectory(ProjectDir);
		}


		#endregion

		private bool ProcessDirectory(string targetDirectory)
		{
			try
			{
				Log.LogMessage(MessageImportance.Low, "Looking for \"*.vsdoc.js\" and \"*.intellisense.js\" files under {0} folder...", targetDirectory);

				// Process the list of JavaScript files found in the directory.
				var fileEntries = Directory.GetFileSystemEntries(targetDirectory, "*.vsdoc.js", SearchOption.AllDirectories)
									.Union(Directory.GetFileSystemEntries(targetDirectory, "*-vsdoc.js", SearchOption.AllDirectories))
									.Union(Directory.GetFileSystemEntries(targetDirectory, "*.intellisense.js", SearchOption.AllDirectories))
									.Union(Directory.GetFileSystemEntries(targetDirectory, "*-intellisense.js", SearchOption.AllDirectories))
									.ToList();

				if (fileEntries.Count > 0)
				{
					InitializeOutputWriter();

					foreach (var fileEntry in fileEntries)
					{
						Log.LogMessage("Processing {0}...", fileEntry);
						var docXml = ProcessFile(fileEntry);

						Log.LogMessage("Loading VSDoc for {0}...", fileEntry);
						var xdoc = XElement.Parse(docXml); // make sure its valid xml

						Log.LogMessage("Successfully loaded. Adding {0} to output...", fileEntry);
						_writer.WriteNode(xdoc.CreateNavigator(), true);
					}
				}
				else
					Log.LogMessage(MessageImportance.Low, "No documentation files found");

				// build task was successful	
				return true;
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex, true, true, null);
				return false;	// build task failed
			}
			finally
			{
				FinalizeOutputWriter();
			}
		}

		private void FinalizeOutputWriter()
		{
			if (_writer != null && _writer.WriteState != WriteState.Closed)
			{
				_writer.WriteEndElement();
				_writer.WriteEndDocument();
				_writer.Flush();
				_writer.Close();
			}
		}

		XmlWriter _writer;
		private void InitializeOutputWriter()
		{
			if (_writer != null) return;

			_writer = XmlWriter.Create(OutFile, new XmlWriterSettings
					{
						Encoding = Encoding.UTF8,
						Indent = true,
						IndentChars = "\t",
						CloseOutput = true,
					});

			_writer.WriteStartDocument();
			_writer.WriteStartElement("doc");

		}
		
		private string ProcessFile(string path)
		{
			try
			{
				using (var reader = new StreamReader(path))
				{
					var fileName = Regex.Replace(path.Substring(path.LastIndexOf('\\') + 1), @"(?:(?:-|\.)(?:vsdoc|intellisense))*\.js", "");
					var results = JsParser.Parse(reader, fileName);

					if (Debug)
						Log.LogMessage("Parser results for {0} -------{1}{2}", fileName, Environment.NewLine, results);

					return results;
				}
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex, true, true, path);
			}

			return null;
		}

		public bool Debug { get; set; }
	}
}
