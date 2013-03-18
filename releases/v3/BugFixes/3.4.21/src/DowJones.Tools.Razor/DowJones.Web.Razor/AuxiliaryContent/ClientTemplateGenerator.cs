using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using DowJones.Web.Razor.Keywords.ClientTemplate;

namespace DowJones.Web.Razor.AuxiliaryContent
{
    public class ClientTemplateGenerator : IAuxiliaryContentGenerator
    {
        private readonly ClientTemplateParser _templateParser;

        public ClientTemplateGenerator(ClientTemplateParser templateParser = null)
        {
            _templateParser = templateParser ?? new ClientTemplateParser();
        }

        public IEnumerable<AuxiliaryContent> GenerateAuxiliaryContent(GeneratorResults generatorResults)
        {
            var clientTemplates = GenerateClientTemplates(generatorResults);
            return clientTemplates;
        }


        private IEnumerable<AuxiliaryContent> GenerateClientTemplates(GeneratorResults generatorResults)
        {
            var documentSpans = generatorResults.Document.Flatten();
            var clientTemplateSpans = documentSpans.OfType<ClientTemplateResourceSpan>();
            var sectionSpans = documentSpans.OfType<SectionHeaderSpan>();

            var clientTemplateSections =
                from span in clientTemplateSpans
                let sectionName = span.SectionName
                where !string.IsNullOrWhiteSpace(sectionName)
                let section = sectionSpans.SingleOrDefault(x => x.SectionName.Equals(sectionName, StringComparison.OrdinalIgnoreCase))
                select new Tuple<ClientTemplateResourceSpan, SectionHeaderSpan>(span, section);

            var clientTemplates = clientTemplateSections.Select(GenerateClientTemplate);

            return clientTemplates;
        }

        private ClientTemplateContent GenerateClientTemplate(Tuple<ClientTemplateResourceSpan, SectionHeaderSpan> sectionInfo)
        {
            return GenerateClientTemplate(sectionInfo.Item1, sectionInfo.Item2);
        }

        private ClientTemplateContent GenerateClientTemplate(ClientTemplateResourceSpan meta, SectionHeaderSpan sectionHeader)
        {
            if (sectionHeader == null)
                throw new ClientTemplateSectionNotImplementedException(meta.SectionName);

            var contentBuilder = new StringBuilder();

            Span nextSpan = sectionHeader;
            while (ShouldContinueParsingDocument(nextSpan = nextSpan.Next))
                ParseSpan(nextSpan, contentBuilder);

            var template = _templateParser.Parse(meta.TemplateId, contentBuilder.ToString());

            return new ClientTemplateContent
                       {
                           Content = template,
                           Name = meta.TemplateId,
                       };
        }

        private static bool ShouldContinueParsingDocument(Span span)
        {
            if(span == null)
                return false;

            var isTransitionalSpan = span is TransitionSpan;
            var isClosingTag = span is MetaCodeSpan && span.Content == "}";

            return isTransitionalSpan == false
                && isClosingTag == false;
        }

        private void ParseSpan(Span span, StringBuilder builder)
        {
            builder.AppendLine(span.Content);
        }
    }

    internal class ClientTemplateSectionNotImplementedException : Exception
    {
        public string SectionName { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format(
                    "Client Template section {0} was referenced but no implementation was found", 
                    SectionName
                );
            }
        }

        public ClientTemplateSectionNotImplementedException(string sectionName)
        {
            SectionName = sectionName;
        }
    }

    public class ClientTemplateContent : AuxiliaryContent
    {
        
    }
}