using System;
using System.IO;
using System.Text;
using System.Xml;

namespace JsXmlDocParser
{
	public class MemberInfoWriter : IDisposable
	{
		readonly XmlWriter _writer;
		private bool initializedDocument;

		public MemberInfoWriter(Stream w, Encoding encoding)
		{
			_writer = XmlWriter.Create(w, DefaultXmlWriterSettings(encoding));
			InitializeDocument();
		}

		public MemberInfoWriter(string filename, Encoding encoding)
			: this(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read), encoding)
		{
		}


		public MemberInfoWriter(StringBuilder output)
		{
			_writer = XmlWriter.Create(output, DefaultXmlWriterSettings());
			InitializeDocument();

		}


		public MemberInfoWriter(TextWriter w)
		{
			_writer = XmlWriter.Create(w, DefaultXmlWriterSettings(w.Encoding));
			InitializeDocument();
		}


		internal XmlWriterSettings DefaultXmlWriterSettings(Encoding encoding = null)
		{
			return new XmlWriterSettings
					{
						Encoding = encoding ?? Encoding.UTF8,
						Indent = true,
						IndentChars = "\t",
					};
		}

		private void InitializeDocument()
		{
			if (initializedDocument) return;

			_writer.WriteStartDocument();
			_writer.WriteStartElement("doc");
			initializedDocument = true;
		}


		public void Write(FunctionInfo functionInfo)
		{
			_writer.WriteStartElement("member");
			_writer.WriteAttributeString("name", string.Format("M:{0}", functionInfo.Signature));
			if (!string.IsNullOrWhiteSpace(functionInfo.DocComments))
			{
				_writer.WriteString(Environment.NewLine);
				_writer.WriteRaw(functionInfo.DocComments);
				_writer.WriteString(Environment.NewLine);
			}
			_writer.WriteEndElement();
		}

		public void WriteAssemblyName(string name)
		{
			_writer.WriteStartElement("assembly");
			_writer.WriteElementString("name", name);
			_writer.WriteEndElement();
		}

		public void Flush()
		{
			_writer.Flush();
		}

		public void Close()
		{
			_writer.WriteEndElement();
			_writer.WriteEndDocument();
			_writer.Flush();
			_writer.Close();
		}

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion
	}
}