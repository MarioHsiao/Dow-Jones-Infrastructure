using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace DowJones.Documentation.DataAccess
{
	public class FileBasedContentRepository : IContentRepository
	{
		private readonly DirectoryInfo _baseDirectory;
		private readonly ContentSectionComparer _nameComparer;

		internal IEnumerable<DirectoryInfo> CategoryDirectories { get; private set; }

		public IEnumerable<string> SectionOrder { get; set; }


		public FileBasedContentRepository(string baseDirectory, IEnumerable<string> orderedSections = null)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(baseDirectory));
			_baseDirectory = new DirectoryInfo(baseDirectory);
			_nameComparer = new ContentSectionComparer(orderedSections ?? Enumerable.Empty<string>());


			if (!_baseDirectory.Exists) 
				CategoryDirectories = Enumerable.Empty<DirectoryInfo>();
			else
				CategoryDirectories = _baseDirectory
					.GetDirectories()
					.Where(n => !n.Name.Equals("images", StringComparison.OrdinalIgnoreCase));
		}


		public IEnumerable<ContentSection> GetCategories()
		{
			var categories = CategoryDirectories.Select(GetContentSection);
			return categories;
		}

		public ContentSection GetCategory(Name name)
		{
			return GetCategory(name, false);
		}

		public ContentSection GetCategory(Name name, bool ignoreOrdinal)
		{

			DirectoryInfo categoryDirectory;
			if (ignoreOrdinal)
				categoryDirectory = CategoryDirectories
									.FirstOrDefault(x => x.Name.WithoutOrdinal().Equals(name.Key, StringComparison.OrdinalIgnoreCase));
			else
				categoryDirectory = CategoryDirectories
									.FirstOrDefault(x => x.Name.Equals(name.Key, StringComparison.OrdinalIgnoreCase));

			return GetContentSection(categoryDirectory);
		}


		private ContentSection GetContentSection(DirectoryInfo contentDirectory)
		{
			if (contentDirectory == null)
				return null;

			var contentFiles = contentDirectory.GetFiles("*.md").Select(x => new ContentSection(x.Name));

			// take all directories except images
			var contentDirectories = contentDirectory
										.GetDirectories()
										.Select(GetContentSection);
			var relatedTopics = GetRelatedTopics(contentDirectory);

			var children =
				contentFiles
					.Union(contentDirectories)
					.Where(x => x != null)
					.OrderBy(x => x, _nameComparer);

			return new ContentSection(contentDirectory.Name, children: children, relatedTopics: relatedTopics);
		}

		private IEnumerable<RelatedTopic> GetRelatedTopics(DirectoryInfo contentDirectory)
		{
			var relatedTopicsFile = contentDirectory.GetFiles("RelatedTopics.json").SingleOrDefault();

			if (relatedTopicsFile == null)
				return Enumerable.Empty<RelatedTopic>();

			var json = File.ReadAllText(relatedTopicsFile.FullName);
			var relatedTopics = new JavaScriptSerializer().Deserialize<RelatedTopic[]>(json);

			return relatedTopics;

		}


		internal class ContentSectionComparer : IComparer<ContentSection>
		{
			private readonly string[] _orderedNames;

			public ContentSectionComparer(IEnumerable<string> orderedNames)
			{
				_orderedNames = (orderedNames ?? Enumerable.Empty<string>()).ToArray();
			}

			public int Compare(ContentSection x, ContentSection y)
			{
				string xKey = x.Name.Key, yKey = y.Name.Key;
				int xIndex = -1, yIndex = -1;

				for (int i = 0; i < _orderedNames.Length; i++)
				{
					if (string.Equals(_orderedNames[i], xKey, StringComparison.OrdinalIgnoreCase))
						xIndex = i;
					if (string.Equals(_orderedNames[i], yKey, StringComparison.OrdinalIgnoreCase))
						yIndex = i;
				}

				// If neither matched, just do the basic compare
				if (xIndex == -1 && yIndex == -1)
					return String.Compare(xKey, yKey);

				if (xIndex >= 0 && yIndex >= 0)
					return xIndex.CompareTo(yIndex);

				if (xIndex >= 0)
					return -1;

				return 1;
			}
		}
	}
}
