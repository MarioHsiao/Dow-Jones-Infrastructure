using DowJones.Web.Mvc.UI.Components.HeadlineList;

namespace DowJones.Web.Mvc.UI.Components.HeadlineListCarousel
{
    public class HeadlineListCarouselModel : ViewComponentModel
    {
        
        public HeadlineListModel HeadlineList { get; set; }
        
        public int NumberOfHeadlinesToScrollBy { get; set; }
        
        public string SelectedAccessionNo { get; set; }
        
        public string AutoScrollSpeed { get; set; }
        
    }
}
