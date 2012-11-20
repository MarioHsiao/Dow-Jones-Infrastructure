using System.Collections.Generic;
using DowJones.Extensions;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Factiva.Currents.Website.Models;

namespace DowJones.Factiva.Currents.Website.Providers
{
	public class CachedPageAssetProvider : PageAssetProvider
	{
		private readonly ICacheProvider<PageServiceResponse> _pageServiceResponseCacheProvider;
		private readonly ICacheProvider<IEnumerable<PageListModel>> _pagesCacheProvider;

		// cache duration in minutes
		private const int CacheDuration = 30;

		public CachedPageAssetProvider(IPageServiceResponseParser pageServiceResponseParser,
			ICacheProvider<PageServiceResponse> pageServiceResponseCacheProvider,
			ICacheProvider<IEnumerable<PageListModel>> pagesCacheProvider)
			: base(pageServiceResponseParser)
		{
			_pageServiceResponseCacheProvider = pageServiceResponseCacheProvider;
			_pagesCacheProvider = pagesCacheProvider;
		}

		public override PageServiceResponse GetPageByName(string pageName)
		{
			return _pageServiceResponseCacheProvider.GetItemFromCache("PageByName_{0}".FormatWith(pageName),
											CacheDuration, () => base.GetPageByName(pageName));
		}

		public override PageServiceResponse GetPageById(string pageId)
		{
			return _pageServiceResponseCacheProvider.GetItemFromCache("PageById_{0}".FormatWith(pageId),
											CacheDuration, () => base.GetPageById(pageId));
		}

		public override IEnumerable<PageListModel> GetPages()
		{
			return _pagesCacheProvider.GetItemFromCache("Pages",
											CacheDuration, () => base.GetPages());
		}
	}
}