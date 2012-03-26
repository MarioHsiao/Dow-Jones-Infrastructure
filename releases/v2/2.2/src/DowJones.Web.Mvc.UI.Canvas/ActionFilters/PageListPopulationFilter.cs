using System.Linq;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Pages;
using DowJones.Security;
using Page = DowJones.Pages.Page;
using SortOrder = Factiva.Gateway.Messages.Assets.Common.V2_0.SortOrder;
using GatewayNewsPage = Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage;
using PageCollection = DowJones.Pages.PageCollection;
using PageType = Factiva.Gateway.Messages.Assets.Pages.V1_0.PageType;
using SortBy = Factiva.Gateway.Messages.Assets.Pages.V1_0.SortBy;

namespace DowJones.Web.Mvc.UI.Canvas.ActionFilters
{
    public class PageListPopulationFilterAttribute : ActionFilterAttribute
    {
        private readonly PageType _pageType;

        [Inject("Can't use constructor injection in Attributes")]
        public IPageAssetsManager PageAssetsManager { get; set; }

        [Inject("Can't use constructor injection in Attributes")]
        public IUserContext User { get; set; }

        public SortOrder SortOrder
        {
            get { return _sortOrder.GetValueOrDefault(SortOrder.Ascending); }
            set { _sortOrder = value; }
        }
        private SortOrder? _sortOrder;

        public SortBy SortBy
        {
            get { return _sortBy.GetValueOrDefault(SortBy.Position); }
            set { _sortBy = value; }
        }
        private SortBy? _sortBy;

        public PageListPopulationFilterAttribute(PageType pageType)
        {
            _pageType = pageType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!User.IsAuthenticated())
                return;

            var pageInfoCollection =
                PageAssetsManager.GetPageListInfoCollection(new[] { _pageType }, SortOrder, SortBy);

            var pages = pageInfoCollection.Select(Mapper.Map<Page>);

            filterContext.Controller.ViewData["PageCollection"] = new PageCollection(pages);
        }
    }
}
