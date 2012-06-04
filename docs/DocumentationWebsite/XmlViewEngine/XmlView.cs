using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace XmlViewEngine
{
    public class XmlView : IView
    {
        private readonly Lazy<XDocument> _xml;
        private readonly XslCompiledTransform _transform;

        public XmlView(XDocument xml, XslCompiledTransform transform)
            : this(() => xml, transform)
        {
        }

        public XmlView(XmlView view, XslCompiledTransform transform)
            : this(view.ToXDocument, transform)
        {
        }

        internal XmlView(Func<XDocument> factory, XslCompiledTransform transform)
        {
            Contract.Requires(factory != null);

            _xml = new Lazy<XDocument>(factory);
            _transform = transform;
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            Render(new XmlTextWriter(writer));
        }

        private void Render(XmlWriter writer)
        {
            var xml = _xml.Value;

            if(_transform != null)
                _transform.Transform(xml.CreateReader(), writer);
            else
                xml.WriteTo(writer);

            writer.Flush();
        }

        internal XDocument ToXDocument()
        {
            var document = new XDocument();
            Render(document.CreateWriter());
            return document;
        }
    }
}
