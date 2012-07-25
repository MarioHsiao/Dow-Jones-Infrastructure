using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Documentation.Website.Extensions;

namespace DowJones.Documentation.Website.Models
{
	public class CategoryViewModel : ContentSectionViewModel
	{
		private readonly string _currentPage;

		public IEnumerable<PageViewModel> Pages
		{
			get { return Children.OfType<PageViewModel>(); }
		}

		public IEnumerable<PageViewModel> RelatedTopicsPages
		{
			get { return RelatedTopics.OfType<PageViewModel>(); }
		}


		public bool HasRelatedTopics
		{
			get { return RelatedTopics.Any(); }
		}


		public CategoryViewModel(ContentSection section, string currentPage = null)
			: base(section)
		{
			_currentPage = currentPage;
		}

		public IEnumerable<IEnumerable<PageViewModel>> GetPageGroups(int groupSize = 5)
		{
			var pages = Pages.ToArray();

			IEnumerable<PageViewModel> group;
			int skip = 0;

			do
			{
				@group = pages.Skip(skip).Take(groupSize).ToArray();
				skip += groupSize;
				yield return @group;
			} while (@group.Any());
		}

		public IEnumerable<IEnumerable<PageViewModel>> GetRelatedTopicGroups(int groupSize = 5)
		{
			var pages = RelatedTopicsPages.ToList();

			IEnumerable<PageViewModel> group;
			int skip = 0;

			do
			{
				@group = pages.Skip(skip).Take(groupSize).ToArray();
				skip += groupSize;
				yield return @group;
			} while (@group.Any());
		}

		protected override void SetDefaultSelection(IEnumerable<ContentSectionViewModel> contentSectionViewModels)
		{
			var children = contentSectionViewModels.ToArray();
			var selectedPage =
				children.FirstOrDefault(child => child.Key.Equals(_currentPage, StringComparison.OrdinalIgnoreCase))
					?? RelatedTopics.FirstOrDefault(x => x.Key == _currentPage)
					?? children.First();

			selectedPage.Selected = true;
		}

		protected override IEnumerable<ContentSectionViewModel> MapRelatedTopics()
		{
			if (ContentSection.RelatedTopics == null || !ContentSection.RelatedTopics.Any())
				return Enumerable.Empty<ContentSectionViewModel>();

			var children = ContentSection
							.RelatedTopics
							.Where(c=> c!=null && c.Name != null)
							.Select(MapRelatedTopicChild);
				
			return children;
		}

		protected  ContentSectionViewModel MapRelatedTopicChild(ContentSection child)
		{
			var category = this;

			if (child.Parent != null && child.Parent.Name != ContentSection.Name)
				category = new CategoryViewModel(child.Parent);

			return new PageViewModel(child, category);
		}

		protected override ContentSectionViewModel MapChild(ContentSection child)
		{
			return new PageViewModel(child, this);
		}

	}
}