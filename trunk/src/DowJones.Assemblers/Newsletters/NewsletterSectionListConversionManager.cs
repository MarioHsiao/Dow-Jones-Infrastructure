using System;
using System.Collections.Generic;
using DowJones.Ajax.Newsletter;
using DowJones.Mapping;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;

namespace DowJones.Assemblers.Newsletters
{
    public class NewsletterSectionListConversionManager : ITypeMapper<SectionCollection, NewsletterSectionListDataResult>
    {
        public NewsletterSectionListDataResult Map(SectionCollection source)
        {
            return Process(source);
        }

        private static NewsletterSectionListDataResult Process(SectionCollection response)
        {
            var converter = new SectionConverter(response);
            return (NewsletterSectionListDataResult)converter.Process();
        }

        public object Map(object source)
        {
            var sectionCollection = source as SectionCollection;
            if (sectionCollection != null)
            {
                return Map(sectionCollection);
            }
            throw new NotSupportedException();
        }
    }

    public class NewsletterSubsectionListConversionManager : ITypeMapper<SubSectionCollection, NewsletterSectionListDataResult>
    {
        public NewsletterSectionListDataResult Map(SubSectionCollection source)
        {
            return Process(source);
        }

        private static NewsletterSectionListDataResult Process(SubSectionCollection response)
        {
            var converter = new SubSectionConverter(response);
            return converter.Process();
        }

        public object Map(object source)
        {
            var subSectionCollection = source as SubSectionCollection;
            if (subSectionCollection != null)
            {
                return Map(subSectionCollection);
            }
            throw new NotSupportedException();
        }
    }
}
