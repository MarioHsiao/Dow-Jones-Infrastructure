using System.Web.Mvc;
using DowJones.Pages;
using DowJones.Security;


namespace DowJones.Web.Mvc.UI.Canvas.ActionFilters
{
    public class PageListPopulationFilterAttribute : ActionFilterAttribute
    {
        private readonly IPageManager _pageManager;
        private readonly IUserContext _user;

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

        public PageListPopulationFilterAttribute(IPageManager pageManager = null, IUserContext user = null)
        {
            _pageManager = pageManager;
            _user = user;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!_user.IsAuthenticated())
                return;

            var pages = _pageManager.GetPages(SortBy, SortOrder);
            filterContext.Controller.ViewData["PageCollection"] = new PageCollection(pages);
        }
    }
}
