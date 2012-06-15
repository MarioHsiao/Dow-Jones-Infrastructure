using System;
using System.Diagnostics.Contracts;
using System.Xml;

namespace JsXmlDocParser
{
	public class MemberInfoWriter : IDisposable
	{
		private readonly XmlWriter _writer;
		private bool _documentStarted;


	    internal bool IsWriterAvailable
	    {
	        get { return _writer == null || _writer.WriteState != WriteState.Closed; }
	    }


	    public MemberInfoWriter(XmlWriter writer)
	    {
            Contract.Requires(writer != null);
	        _writer = writer;
	    }


	    public void Close()
		{
            if (IsWriterAvailable)
                return;

	        _writer.Close();
		}

	    public void Dispose()
		{
			Close();
		}

	    public void Write(FunctionInfo functionInfo)
	    {
	        EnsureDocumentStarted();

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
	        EnsureDocumentStarted();

	        _writer.WriteStartElement("assembly");
	        _writer.WriteElementString("name", name);
	        _writer.WriteEndElement();
	    }

	    public void WriteEndDocument()
	    {
            if (!_documentStarted)
                return;

	        _writer.WriteEndElement();
	        _writer.WriteEndDocument();
	    }

	    public void WriteStartDocument()
	    {
	        if (_documentStarted) 
	            return;

	        _writer.WriteStartDocument();
	        _writer.WriteStartElement("doc");

	        _documentStarted = true;
	    }

	    internal void EnsureDocumentStarted()
	    {
	        WriteStartDocument();
	    }
	}
}