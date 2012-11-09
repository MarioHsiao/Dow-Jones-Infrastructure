using DowJones.Factiva.Currents.Website.Contracts;

namespace DowJones.Factiva.Currents.Website.Providers
{
	public class CachedPageAssetProvider : PageAssetProvider
	{
		public CachedPageAssetProvider(IPageServiceResponseParser pageServiceResponseParser) : base(pageServiceResponseParser)
		{
		}
	}
}