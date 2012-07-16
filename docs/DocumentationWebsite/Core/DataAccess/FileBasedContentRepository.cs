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
			{
				var directories = _baseDirectory
					.GetDirectories()
					.Where(n => !n.Name.Equals("images", StringComparison.OrdinalIgnoreCase)).ToList();

				CategoryDirectories = directories.Where(dir => dir.Name.ToLower() != "relatedtopics");
			}
		}


		public IEnumerable<ContentSection> GetCategories()
		{
			var categories = CategoryDirectories.Select(GetContentSection);
			return categories;
		}

		public ContentSection GetCategory(Name name)
		{
			var categoryDirectory = CategoryDirectories
									.FirstOrDefault(x => x.Name.Equals(name.Key, StringComparison.OrdinalIgnoreCase));

            // If the exact name wasn't found, try it without the ordinal
            if (categoryDirectory == null)
				categoryDirectory = CategoryDirectories
									.FirstOrDefault(x => x.Name.WithoutOrdinal().Equals(name.Key, StringComparison.OrdinalIgnoreCase));

			return GetContentSection(categoryDirectory);
		}


		private ContentSection GetContentSection(DirectoryInfo contentDirectory)
		{
			if (contentDirectory == null)
				return null;

			var contentFiles = contentDirectory.GetFiles("*.md").Select(x => new ContentSection(x.Name));



			//IEnumerable<DirectoryInfo> relatedTopicDirectories;
			//ContentSection[] relatedTopics;

			// check meta file to see which folders are related topic folders
			var relatedTopics = GetRelatedTopics(contentDirectory);

			// all directories
			var contentDirectories = contentDirectory
										.GetDirectories()
										.Select(GetContentSection);

			// now get the actual "content" directories
			var children =
				contentFiles
					.Union(contentDirectories.Except(relatedTopics, new ContentSectionEqualityComparer()))
					.Where(x => x != null)
					.OrderBy(x => x, _nameComparer);

			// roll it all up in a nice package!
			return new ContentSection(contentDirectory.Name, children: children, relatedTopics: relatedTopics);

		}

		private IEnumerable<ContentSection> GetRelatedTopics(DirectoryInfo directory)
		{
			var relatedTopicsMetaFilePath = Path.Combine(directory.FullName, "RelatedTopics.json");

			if (!File.Exists(relatedTopicsMetaFilePath)) return Enumerable.Empty<ContentSection>();

			try
			{
				var json = File.ReadAllText(relatedTopicsMetaFilePath);
				var relatedTopicMeta = new JavaScriptSerializer().Deserialize<RelatedTopic[]>(json);
				
				var localTopics = relatedTopicMeta
									.Where(x => x.Category == directory.Name)
									.Select(x => new DirectoryInfo(Path.Combine(directory.FullName, x.Page)))
									.Where(d => d.Exists)
									.Select(GetContentSection);

				var externalTopics =  relatedTopicMeta
										.Where(x => x.Category != directory.Name)
										.Select(x => new ContentSection(x.Page, new ContentSection(x.Category)));
				
				return localTopics.Union(externalTopics);
				
			}
			catch (Exception)
			{
				return Enumerable.Empty<ContentSection>();
			}

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

		internal class ContentSectionEqualityComparer : IEqualityComparer<ContentSection>
		{

			#region Implementation of IEqualityComparer<in ContentSection>

			/// <summary>
			/// Determines whether the specified objects are equal.
			/// </summary>
			/// <returns>
			/// true if the specified objects are equal; otherwise, false.
			/// </returns>
			/// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
			public bool Equals(ContentSection x, ContentSection y)
			{
				if (x == null || y == null)
					return false;

				// see if ref equal
				if (x.Equals(y))
					return true;

				// see if name matches
				return x.Name.Equals(y.Name)
						|| x.Name.DisplayKey.Equals(y.Name.DisplayKey); // weak: see if display key matches
			}

			/// <summary>
			/// Returns a hash code for the specified object.
			/// </summary>
			/// <returns>
			/// A hash code for the specified object.
			/// </returns>
			/// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
			public int GetHashCode(ContentSection obj)
			{
				return obj.Name.DisplayKey.GetHashCode();
			}

			#endregion
		}
	}
}
