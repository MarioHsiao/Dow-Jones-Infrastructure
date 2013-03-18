using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace DowJones.Web.Mvc.ActionResults
{
    public class XmlResult : ActionResult
    {
        public object Model { get; set; }

        public XmlWriterSettings Settings
        {
            get { return _settings; }
        }
        private readonly XmlWriterSettings _settings;

        public Encoding ContentEncoding { get; set; }

        public XmlResult(object model = null)
        {
            _settings = new XmlWriterSettings { OmitXmlDeclaration = true };
            Model = model;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Model != null)
                Serialize(response.Output);
        }

        protected internal void Serialize(TextWriter writer)
        {
            var serializer = new XmlSerializer(Model.GetType());

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlWriter xmlWriter = XmlWriter.Create(writer, _settings);
            serializer.Serialize(xmlWriter, Model, namespaces);
        }
    }
}
