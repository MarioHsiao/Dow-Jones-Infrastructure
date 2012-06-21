using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace DowJones.Documentation.DataAccess
{
    public class FileBasedContentRepository : IContentRepository
    {
        private readonly DirectoryInfo _baseDirectory;

        public IEnumerable<string> SectionOrder { get; set; }

        public FileBasedContentRepository(string baseDirectory)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(baseDirectory));
            _baseDirectory = new DirectoryInfo(baseDirectory);
        }

        public IEnumerable<ContentSection> GetCategories()
        {
            var categories = GetCategoryDirectories().Select(GetCategory);
            return categories;
        }

        public ContentSection GetCategory(Name name)
        {
            var categoryDirectory = 
                GetCategoryDirectories()
                    .FirstOrDefault(x => x.Name.Equals(name.Key, StringComparison.OrdinalIgnoreCase));

            return GetCategory(categoryDirectory);
        }


        private ContentSection GetCategory(DirectoryInfo categoryDirectory)
        {
            if(categoryDirectory == null)
                return null;

            var pages = GetPages(categoryDirectory).ToArray();
            return new ContentSection(new Name(categoryDirectory.Name), children: pages);
        }

        private IEnumerable<DirectoryInfo> GetCategoryDirectories()
        {
            if (!_baseDirectory.Exists)
                return Enumerable.Empty<DirectoryInfo>();

            var directories = _baseDirectory.GetDirectories();
            return directories;
        }

        private IEnumerable<ContentSection> GetPages(DirectoryInfo categoryDirectory)
        {
            var pageFiles = categoryDirectory.GetFiles().Select(x => new ContentSection(x.Name));

            var pageDirectories =
                from directory in categoryDirectory.GetDirectories()
                let sections = GetSections(directory)
                select new ContentSection(directory.Name, children: sections);

            return pageFiles.Union(pageDirectories).OrderBy(x => x.Name.DisplayName);
        }

        private IEnumerable<ContentSection> GetSections(DirectoryInfo pageDirectory)
        {
            var sections = pageDirectory.GetFiles().Select(x => new ContentSection(x.Name)).ToArray();

            var orderedSections = new List<ContentSection>();

            foreach (var ordered in (SectionOrder ?? Enumerable.Empty<string>()).Select(x => x.ToLower()))
            {
                var section = sections.FirstOrDefault(x => x.Name.Value.Equals(ordered, StringComparison.OrdinalIgnoreCase));
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
