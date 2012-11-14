using DowJones.Mapping;
using DowJones.Pages.Modules.Snapshot;

namespace DowJones.Factiva.Currents.Website.Models
{
	public class PageListModel
	{
		public string Title { set; get; }

		public int Position { set; get; }

		public string Id { set; get; }

		public string FriendlyTitle { set; get; }
	}

	public class PageListResponseMapper : TypeMapper<NewsPage, PageListModel>
	{
		public override PageListModel Map(NewsPage source)
		{
			return new PageListModel
				{
					Id = source.ID,
					Position = source.Position,
					Title = source.Title,
					FriendlyTitle = GetFriendlyTitle(source.Title),
				};
		}

		private string GetFriendlyTitle(string title)
		{
			return title.Replace(" ", "-").ToLowerInvariant();
		}
	}
}