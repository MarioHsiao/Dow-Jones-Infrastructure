using System;
using System.Collections.Generic;
using DowJones.Ajax;
using DowJones.Ajax.Newsletter;
using DowJones.Formatters;
using DowJones.Formatters.Numerical;
using Factiva.Gateway.Messages.Assets.Common.V2_0;

namespace DowJones.Assemblers.Newsletters
{
    public class SectionConverter
    {
        private readonly SectionCollection _response;
        private readonly NewsletterSectionListDataResult _result = new NewsletterSectionListDataResult();
        
        public SectionConverter(SectionCollection response)
        {
            _response = response;
        }

        public IListDataResult Process() 
        {
            if (_response == null)
            {
                return _result;
            }

            if (_response.Count == 0)
                return _result;

            var numberFormatter = new NumberFormatter();
            _result.ResultSet.Count = new WholeNumber(_response.Count);
            ProcessNewsletterSectionItems(_response);
            numberFormatter.Format(_result.ResultSet.Count);
            return _result;
        }

        private void ProcessNewsletterSectionItems(IEnumerable<Section> sectionCollection)
        {
            foreach (var newsletterSectionItem in sectionCollection)
            {
                var newsletterSectionInfo = new NewsletterSectionInfo();
                Convert(newsletterSectionInfo, newsletterSectionItem);
                _result.ResultSet.NewsletterSections.Add(newsletterSectionInfo);
            }
        }

        private static void Convert(NewsletterSectionInfo newsletterSectionInfo, Section newsletterSectionItem)
        {
            if (newsletterSectionItem == null)
                return;

            var cManager = new NewsletterSubsectionListConversionManager();
            newsletterSectionInfo.Name = newsletterSectionItem.Name;
            newsletterSectionInfo.Position = newsletterSectionItem.Position;
            newsletterSectionInfo.SubSections = cManager.Map(newsletterSectionItem.SubSectionCollection).ResultSet.NewsletterSections;
        }
    }

    public class SubSectionConverter
    {
        private readonly SubSectionCollection _response;
        private readonly NewsletterSectionListDataResult _result = new NewsletterSectionListDataResult();

        public SubSectionConverter(SubSectionCollection response)
        {
            _response = response;
        }

        public NewsletterSectionListDataResult Process()
        {
            if (_response == null)
            {
                return _result;
            }

            if (_response.Count == 0)
                return _result;

            var numberFormatter = new NumberFormatter();
            _result.ResultSet.Count = new WholeNumber(_response.Count);
            ProcessNewsletterSubSectionItems(_response);
            numberFormatter.Format(_result.ResultSet.Count);
            return _result;
        }

        private void ProcessNewsletterSubSectionItems(IEnumerable<Section> subSectionCollection)
        {
            foreach (var newsletterSubSectionItem in subSectionCollection)
            {
                var newsletterSectionInfo = new NewsletterSectionInfo();
                Convert(newsletterSectionInfo, newsletterSubSectionItem);
                _result.ResultSet.NewsletterSections.Add(newsletterSectionInfo);
            }
        }

        private static void Convert(NewsletterSectionInfo newsletterSubSectionInfo, Section newsletterSubSectionItem)
        {
            if (newsletterSubSectionItem == null)
                return;
            var cManager = new NewsletterSubsectionListConversionManager();
            newsletterSubSectionInfo.Name = newsletterSubSectionItem.Name;
            newsletterSubSectionInfo.Position = newsletterSubSectionItem.Position;
            newsletterSubSectionInfo.SubSections = cManager.Map(newsletterSubSectionItem.SubSectionCollection).ResultSet.NewsletterSections;
        }
    }
}
