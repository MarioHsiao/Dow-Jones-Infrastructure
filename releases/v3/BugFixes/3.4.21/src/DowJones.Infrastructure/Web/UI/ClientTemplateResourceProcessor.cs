using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Exceptions;
using DowJones.Infrastructure;

namespace DowJones.Web.UI
{
    public class ClientTemplateResourceProcessor : IClientResourceProcessor
    {
        public const string JavaScriptClassNamespace = "DJ.UI";
        public const string ClientComponentTemplateProperty = "prototype.baseTemplates";

        private readonly IClientTemplateParser _parser;

        public HttpContextBase HttpContext { get; set; }

        public ClientResourceProcessorOrder? Order
        {
            get { return ClientResourceProcessorOrder.First; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Postprocessor; }
        }


        public ClientTemplateResourceProcessor(IClientTemplateParser parser)
        {
            _parser = parser;
        }


        public void Process(ProcessedClientResource resource)
        {
            Guard.IsNotNull(resource, "resource");

            if (!resource.HasContent) 
                return;

            Guard.IsNotNull(resource.ClientResource, "resource.ClientResource");
            var clientResource = resource.ClientResource;

            if (clientResource.ResourceKind != ClientResourceKind.ClientTemplate)
                return;

            // Returns an anonymous function containing the template logic
            // that needs to be assigned to something
            var parsedTemplate = _parser.Parse(resource.Content);

            var clientTemplate = new ClientTemplateInfo(clientResource);

            resource.Content = DecorateParsedTemplate(parsedTemplate, clientTemplate);
            resource.MimeType = KnownMimeTypes.JavaScript;
        }

        protected internal string DecorateParsedTemplate(string templateFunction, ClientTemplateInfo clientTemplate)
        {
            Guard.IsNotNullOrEmpty(templateFunction, "templateFunction");
            Guard.IsNotNull(clientTemplate, "clientTemplate");

            if (clientTemplate.IsFullyQualifiedTemplateId)
            {
                return string.Format("{0} = {1};\r\n", 
                                     clientTemplate.TemplateId, 
                                     templateFunction);
            }

            return 
                string.Format(
                    "DJ.jQuery.extend({0}, {{ {1}: {2} }});\r\n", 
                        clientTemplate.ClientComponentTemplatesPath,
                        clientTemplate.TemplateId,
                        templateFunction
                );
        }

        public class ClientTemplateInfo
        {
            private readonly IEnumerable<string> _resourceNameParts;

            public ClientResource ClientResource { get; set; }

            public string ClientComponentName
            {
                get
                {
                    return string.Format("{0}.{1}", JavaScriptClassNamespace, ComponentName);
                }
            }

            public string ComponentName
            {
                get
                {
                    if (_componentName == null && !IsFullyQualifiedTemplateId)
                    {
                        if (_resourceNameParts.Count() < 3)
                            return null;

                        _componentName =
                            _resourceNameParts
                                .Reverse()
                                // Ignore the "ClientTemplates" subfolder if it exists
                                .Where(x => !x.Equals("ClientTemplates", StringComparison.OrdinalIgnoreCase))
                                .Skip(2)
                                .First();
                    }

                    return _componentName;
                }
            }
            private string _componentName;

            public string FullTemplatePath
            {
                get
                {
                    if (IsFullyQualifiedTemplateId)
                        return TemplateId;

                    return string.Format("{0}.{1}", ClientComponentTemplatesPath, TemplateId);
                }
            }
            
            public bool IsFullyQualifiedTemplateId
            {
                get { return (TemplateId ?? string.Empty).Contains('.'); }
            }

            public string TemplateId { get; set; }

            public string ClientComponentTemplatesPath
            {
                get
                {
                    return string.Format("{0}.{1}", 
                                         ClientComponentName,
                                         ClientComponentTemplateProperty);
                }
            }


            public ClientTemplateInfo(ClientResource resource)
            {
                Guard.IsNotNull(resource, "resource");
                Guard.IsNotNull(resource.Name, "resource.Name");

                if (string.IsNullOrWhiteSpace(resource.TemplateId))
                {
                    string message = string.Format("Unspecified Template ID for Client Template {0}", resource);
                    throw new DowJonesUtilitiesException(message);
                }

                ClientResource = resource;
                TemplateId = resource.TemplateId;
                _resourceNameParts = resource.Name.Split('.');
            }
        }
    }
}
