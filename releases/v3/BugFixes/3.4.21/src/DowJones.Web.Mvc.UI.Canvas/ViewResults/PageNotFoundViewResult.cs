using System.Web.Mvc;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class PageNotFoundViewResult : ViewResult
    {
        public string PageId { get; private set; }

        public PageNotFoundViewResult(string pageID = null)
        {
            PageId = pageID;
            ViewName = "PageNotFound";
            ViewData.Model = pageID;
        }
    }
}