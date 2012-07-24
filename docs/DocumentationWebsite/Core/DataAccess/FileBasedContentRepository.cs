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
	    private const string ImageFolderName = "images";
        private const string AssetsFolderName = "Assets";
	    private const string RelatedTopicsFolderName = "RelatedTopics";
	    private const string RelatedTopicsMetaFileName = "RelatedTopics.json";
        private static readonly IEnumerable<string> SpecialFiles =
            new[] { ImageFolderName, AssetsFolderName, RelatedTopicsFolderName, RelatedTopicsMetaFileName };

		private readonly DirectoryInfo _baseDirectory;
		private readonly ContentSectionComparer _nameComparer;
	    private IEnumerable<DirectoryInfo> _categoryDirectories;

	    internal IEnumerable<DirectoryInfo> CategoryDirectories
	    {
	        get
	        {
                if(_categoryDirectories == null)
                {
                    _categoryDirectories = 
                        (
                            from directory in _baseDirectory.GetDirectories()
                            where IsNotSpecialFile(directory.Name)
                            select directory
                        ).ToArray();
                }

	            return _categoryDirectories;
	        }
	        private set { _categoryDirectories = value; }
	    }

	    public IEnumerable<string> SectionOrder { get; set; }


		public FileBasedContentRepository(string baseDirectory, IEnumerable<string> orderedSections = null)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(baseDirectory));
			_baseDirectory = new DirectoryInfo(baseDirectory);
			_nameComparer = new ContentSectionComparer(orderedSections ?? Enumerable.Empty<string>());

			if (!_baseDirectory.Exists)
				CategoryDirectories = Enumerable.Empty<DirectoryInfo>();
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

	    public ContentSection GetPage(Name name, Name categoryName)
	    {
            var category = GetCategory(categoryName) ?? new ContentSection(string.Empty);
	        return category.Find(name);
	    }

	    private ContentSection GetContentSection(DirectoryInfo contentDirectory)
		{
			if (contentDirectory == null)
				return null;

			var contentFiles = 
                contentDirectory.GetFiles()
                    .Select(file => file.Name)
                    .Where(IsNotSpecialFile)
                    .Select(name => new ContentSection(name));

			// check meta file to see which folders are related topic folders
			var relatedTopics = GetRelatedTopics(contentDirectory).ToArray();

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

	            var relatedTopics =
	                from meta in relatedTopicMeta
	                let isLocal = (meta.Category == directory.Name)
	                select
	                    isLocal ? GetContentSection(directory.GetDirectories().FirstOrDefault(x => x.Name == meta.Page))
	                        : new ContentSection(meta.Page, new ContentSection(meta.Category));

	            return relatedTopics;
				
	        }
	        catch (Exception)
	        {
	            return Enumerable.Empty<ContentSection>();
	        }
	    }

	    private static bool IsNotSpecialFile(string filename)
	    {
	        return !SpecialFiles.Contains(filename, StringComparer.OrdinalIgnoreCase);
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
			/// <param name="x">The first object of type T to compare.</param><param name="y">The second object of type T to compare.</param>
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
