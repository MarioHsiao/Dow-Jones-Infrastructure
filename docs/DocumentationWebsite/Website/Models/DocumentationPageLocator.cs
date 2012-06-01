using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace DowJones.Documentation.Website.Models
{
    public interface IDocumentationPageLocator
    {
        DocumentationPages LocateDocumentationPages();
    }

    public class DocumentationPageLocator : IDocumentationPageLocator
    {
        private readonly string _baseDirectory;

        public IEnumerable<string> SectionOrder { get; set; }

        public DocumentationPageLocator(string baseDirectory)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(baseDirectory));
            _baseDirectory = baseDirectory;
        }

        public DocumentationPages LocateDocumentationPages()
        {
            Contract.Requires(Directory.Exists(_baseDirectory));

            var baseDirectory = new DirectoryInfo(_baseDirectory);

            if(!baseDirectory.Exists)
                return new DocumentationPages();

            var categories = GetCategories(baseDirectory);

            return new DocumentationPages(categories);
        }

        private IEnumerable<DocumentationCategory> GetCategories(DirectoryInfo baseDirectory)
        {
            foreach (var categoryDirectory in baseDirectory.GetDirectories())
            {
                var pages = GetPages(categoryDirectory).OrderBy(x => x.Name);
                yield return new DocumentationCategory(new Name(categoryDirectory.Name), pages);
            }
        }

        private IEnumerable<DocumentationPage> GetPages(DirectoryInfo category)
        {
            var pageFiles = category.GetFiles().Select(x => new DocumentationPage(x.Name));

            var pageDirectories =
                from directory in category.GetDirectories()
                let sections = GetSections(directory)
                select new DocumentationPage(directory.Name, sections);

            return pageFiles.Union(pageDirectories).OrderBy(x => x.Name);
        }

        private IEnumerable<DocumentationPageSection> GetSections(DirectoryInfo directory)
        {
            var sections = directory.GetFiles().Select(x => new DocumentationPageSection(x.Name)).ToArray();

            var orderedSections = new List<DocumentationPageSection>();

            foreach (var ordered in (SectionOrder ?? Enumerable.Empty<string>()).Select(x => x.ToLower()))
            {
                var section = sections.FirstOrDefault(x => x.Name.Equals(ordered, StringComparison.OrdinalIgnoreCase));
                if (section != null)
                {
                    orderedSections.Add(section);
                }
            }

            orderedSections.AddRange(sections.Except(orderedSections));

            return orderedSections;
        }
    }
}
